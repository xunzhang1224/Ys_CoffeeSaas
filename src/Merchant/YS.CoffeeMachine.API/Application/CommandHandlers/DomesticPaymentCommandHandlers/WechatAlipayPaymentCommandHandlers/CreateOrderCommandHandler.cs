using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V3;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.WechatAlipayPaymentCommand;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers.WechatAlipayPaymentCommandHandlers
{
    /// <summary>
    /// 验证商户Id是否存在命令
    /// </summary>
    /// <param name="context"></param>
    /// <param name="createOrderService"></param>
    /// <param name="_paymentPlatformUtil"></param>
    public class VerifyMerchantIdCommandHandler(CoffeeMachineDbContext context, ICreateOrderService createOrderService, PaymentPlatformUtil _paymentPlatformUtil) : IRequestHandler<VerifyMerchantIdCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(VerifyMerchantIdCommand request, CancellationToken cancellationToken)
        {
            var systemPaymentMethod = await context.SystemPaymentMethod.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.SystemPaymentMethodId);

            if (systemPaymentMethod == null)
                return false;

            return request.OrderPaymentType switch
            {
                OrderPaymentTypeEnum.WxNativePay => await _paymentPlatformUtil.VerifyWxNative(request.MerchantId, systemPaymentMethod.PaymentPlatformId),
                OrderPaymentTypeEnum.AlipayJsApi => await _paymentPlatformUtil.VerifyAlipayNative(request.MerchantId, systemPaymentMethod.PaymentPlatformId),
                _ => throw ExceptionHelper.AppFriendly("不支持的支付方式"),
            };
        }

        /// <summary>
        /// 发起微信支付宝Native二维码扫码支付命令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="createOrderService"></param>
        /// <param name="_paymentPlatformUtil"></param>
        /// <param name="_logger"></param>
        public class CreateOrderCommandHandler(CoffeeMachineDbContext context,
            ICreateOrderService createOrderService,
            PaymentPlatformUtil _paymentPlatformUtil,
            ILogger<CreateOrderCommandHandler> _logger) : IRequestHandler<CreateOrderCommand, CreateNativeOrderOutput>
        {
            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<CreateNativeOrderOutput> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                // 获取设备信息
                var deviceBaseInfo = context.DeviceBaseInfo.AsNoTracking().FirstOrDefault(x => x.MachineStickerCode == request.Input.Mid);
                if (deviceBaseInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

                var deviceInfo = await (from a in context.DeviceInfo.IgnoreQueryFilters()
                                        join b in context.DeviceBaseInfo on a.DeviceBaseId equals b.Id
                                        where b.MachineStickerCode == request.Input.Mid
                                        select new MiniDeviceInfoDto
                                        {
                                            Id = a.Id,
                                            DeviceBaseId = b.Id,
                                            TenantId = a.EnterpriseinfoId
                                        }).FirstOrDefaultAsync();
                if (deviceInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

                // 获取支付信息
                var systemPaymentMethodId = request.Input.OrderPaymentType == OrderPaymentTypeEnum.WxNativePay ? CommonConst.WechatPaymentId : CommonConst.AlipaPaymenteId;
                var m_paymentMenthod = await _paymentPlatformUtil.GetDevicePaymentMethod(deviceInfo.DeviceBaseId, deviceInfo.TenantId, systemPaymentMethodId);

                // 生成订单号
                string orderNo = BasicUtils.GenerateOrderNo();

                // 组装订单信息
                var orderBaseInput = new CreateOrderBaseInput
                {
                    DeviceBaseId = deviceBaseInfo.Id,
                    OutTradeNo = orderNo,
                    BizNo = request.Input.BizNo,
                    EnterpriseinfoId = deviceInfo.TenantId,
                    MerchantId = request.Input.MerchantId,
                    PayAmount = request.Input.PayAmount,
                    Provider = request.Input.Provider == null ? request.Input.OrderPaymentType.GetEnumDefaultValue() : request.Input.Provider,
                    PayTimeSp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), // request.Input.PayTimeSp,
                    CurrencyCode = "CNY",
                    CustomContent = request.Input.CustomContent,
                    OrderPaymentType = request.Input.OrderPaymentType,
                    OrderDetails = request.Input.OrderDetails,
                    PaymentMerchantId = m_paymentMenthod.MerchantId,
                    SystemPaymentMethodId = m_paymentMenthod.SystemPaymentMethodId,
                    SystemPaymentServiceProviderId = m_paymentMenthod.SystemPaymentServiceProviderId
                };

                // 创建订单信息
                await createOrderService.CreateOrderInfo(orderBaseInput, orderNo, null);

                return request.Input.OrderPaymentType switch
                {
                    OrderPaymentTypeEnum.WxNativePay => await CreateNativeOrderToWxNative(orderBaseInput),
                    OrderPaymentTypeEnum.AlipayJsApi => await CreateNativeOrderToAlipayNative(orderBaseInput),
                    _ => throw ExceptionHelper.AppFriendly("不支持的支付方式"),
                };
            }

            /// <summary>
            /// 微信扫码支付-创建订单
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public async Task<CreateNativeOrderOutput> CreateNativeOrderToWxNative(CreateOrderBaseInput input)
            {
                // 商品名称
                var productName = string.Join("|", input.OrderDetails.Select(s => s.BeverageName));
                if (productName.Length > 40)
                    productName = productName.Substring(0, 40);

                // 订单商品详情
                var goods = input.OrderDetails.Select(a => new WxPromotionGoodsDetail
                {
                    GoodsName = a.BeverageName,
                    MerchantGoodsId = a.ItemCode,
                    Quantity = a.Quantity,
                    UnitPrice = (int)(a.Price * 100)
                }).ToList();

                var result = await _paymentPlatformUtil.CreateWechatNative(new CreateWxpayOrderInput
                {
                    SystemPaymentServiceProviderId = input.SystemPaymentServiceProviderId ?? 0,
                    Amount = input.PayAmount,
                    Description = productName,
                    //OpenId = input.EnterpriseinfoId.ToString(),
                    OutTradeNo = input.OutTradeNo,
                    MerchantId = input.PaymentMerchantId,
                    GoodsDetail = goods
                });

                if (!result.Succeeded)
                {
                    _logger.LogInformation($"发起在线支付失败,订单号;{input.OutTradeNo},失败原因：" + JsonConvert.SerializeObject(result));

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    // 更新订单
                    await context.OrderInfo
                        .Where(a => a.Code == input.OutTradeNo && a.OrderStatus == OrderStatusEnum.PaymentInProgress)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(a => a.OrderStatus, OrderStatusEnum.Fail)
                            .SetProperty(a => a.PayTimeSp, timestamp));

                    throw ExceptionHelper.AppFriendly(result?.Data?.Message ?? result?.Message);
                }

                var payRes = result.Data.Adapt<WxTransactionResponse>();
                return new CreateNativeOrderOutput
                {
                    Code = input.OutTradeNo,
                    Content = payRes,
                    QrCodeData = payRes?.CodeUrl
                };
            }

            /// <summary>
            /// 支付宝扫码支付-创建订单
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public async Task<CreateNativeOrderOutput> CreateNativeOrderToAlipayNative(CreateOrderBaseInput input)
            {
                // 商品名称
                var productName = string.Join("|", input.OrderDetails.Select(s => s.BeverageName));
                if (productName.Length > 40)
                    productName = productName.Substring(0, 40);

                // 订单商品详情
                var goodDetail = input.OrderDetails.Select(a => new Alipay_GoodsDetail
                {
                    GoodsId = a.ItemCode,
                    GoodsName = a.BeverageName,
                    Price = (a.Price / a.Quantity).ToString("F2"),
                    Quantity = a.Quantity
                }).ToList();

                var result = await _paymentPlatformUtil.CreateAlipayJsapi(new CreateAlipayOrderInput
                {
                    SystemPaymentServiceProviderId = input.SystemPaymentServiceProviderId ?? 0,
                    Amount = input.PayAmount,
                    OutTradeNo = input.OutTradeNo,
                    Subject = productName,
                    OpenId = input.EnterpriseinfoId.ToString(),
                    MerchantId = input.PaymentMerchantId,
                    GoodsDetail = goodDetail,
                    PaymentMethodId = string.IsNullOrEmpty(input.PaymentMerchantId) ? 0 : Convert.ToInt64(input.PaymentMerchantId)
                });

                if (!result.Success)
                {
                    _logger.LogInformation($"发起支付在线支付失败,订单号;{input.OutTradeNo},失败原因：" + JsonConvert.SerializeObject(result));

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    // 更新订单
                    await context.OrderInfo
                        .Where(a => a.Code == input.OutTradeNo && a.OrderStatus == OrderStatusEnum.PaymentInProgress)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(a => a.OrderStatus, OrderStatusEnum.Fail)
                            .SetProperty(a => a.PayTimeSp, timestamp));
                    throw ExceptionHelper.AppFriendly(result.Data?.SubMsg ?? result.Msg);
                }

                var payRes = result.Data.Adapt<Alipay_TradePrecreateResponse>();
                return new CreateNativeOrderOutput
                {
                    Code = input.OutTradeNo,
                    Content = payRes,
                    QrCodeData = payRes?.QrCode
                };
            }
        }
    }
}