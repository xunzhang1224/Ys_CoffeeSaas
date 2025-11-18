using Aop.Api.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.BasicDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Services.OrderInfoServices
{
    /// <summary>
    /// 支付通用创建订单服务接口
    /// </summary>
    public class CreateOrderService(CoffeeMachineDbContext context, IMapper mapper) : ICreateOrderService
    {
        /// <summary>
        /// 创建订单信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderNO"></param>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrderInfo(CreateOrderBaseInput input, string orderNO, string payOrderId)
        {
            // 获取支付平台配置
            //var paymentPlatform = await context.PaymentConfig.AsQueryable()
            //    .Where(a => a.P_PaymentConfigId == input.MerchantId)
            //    .FirstOrDefaultAsync();
            //if (paymentPlatform == null)
            //{
            //    throw ExceptionHelper.AppFriendly("未找到对应的支付平台配置");
            //}

            // 验证订单金额
            if (input.PayAmount <= 0)
            {
                throw ExceptionHelper.AppFriendly("支付金额必须大于0");
            }

            // 验证订单详情
            if (input.OrderDetails == null || input.OrderDetails.Count == 0)
            {
                throw ExceptionHelper.AppFriendly("订单详情不能为空");
            }

            // 获取设备基础信息
            var deviceinfo = await context.DeviceInfo.Include(i => i.DeviceModel).AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.DeviceBaseId == input.DeviceBaseId);

            // 获取对应的饮品信息
            var beverageInfos = await context.BeverageInfo.AsQueryable().AsNoTracking().IgnoreQueryFilters()
                .Include(i => i.FormulaInfos)
                .Where(b => input.OrderDetails.Select(d => d.ItemCode).Contains(b.Code) && b.DeviceId == deviceinfo!.Id)
                .ToListAsync();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            // 当前饮品的物料盒信息
            var materialBoxs = await context.DeviceMaterialInfo.Where(w => w.DeviceBaseId == deviceinfo.DeviceBaseId && w.Type == MaterialTypeEnum.Cassette).ToListAsync();
            var orderDetails = new List<OrderDetails>();
            foreach (var detail in input.OrderDetails)
            {
                if (/*string.IsNullOrWhiteSpace(detail.ItemCode) || */string.IsNullOrWhiteSpace(detail.BeverageName) || detail.Price <= 0)
                {
                    throw ExceptionHelper.AppFriendly("订单详情中的商品信息不完整或价格无效");
                }
                var beverageInfoJson = string.Empty;
                // 获取当前的饮品信息
                var beverageInfo = beverageInfos.FirstOrDefault(b => b.Code == detail.ItemCode);
                if (beverageInfo != null)
                {
                    var beverageInfoDto = mapper.Map<OrderBeverageInfoDto>(beverageInfo);

                    var categorys = await context.ProductCategory.AsNoTracking()
                        .Where(w => beverageInfoDto.CategoryIds != null && beverageInfoDto.CategoryIds.Contains(w.Id))
                        .ToListAsync();

                    if (categorys != null && categorys.Any())
                        beverageInfoDto.CategoryName = categorys.Select(s => s.Name).ToList();

                    // 设置物料盒名称
                    if (beverageInfoDto.FormulaInfos != null && beverageInfoDto.FormulaInfos.Any())
                    {
                        foreach (var formula in beverageInfoDto.FormulaInfos.Where(w => w.FormulaType == FormulaTypeEnum.Lh))
                        {
                            // 当前饮品的物料盒信息
                            formula.MaterialBoxName = materialBoxs.FirstOrDefault(w => w.Index == formula.MaterialBoxId)?.Name ?? string.Empty;
                        }
                    }
                    beverageInfoJson = beverageInfoDto == null ? string.Empty : JsonConvert.SerializeObject(beverageInfoDto, settings);
                }

                orderDetails.Add(new OrderDetails(detail.CounterNo, detail.SlotNo, detail.ItemCode, detail.BeverageName, detail.Price, detail.Quantity, 0, 0, string.Empty,
                    beverageInfo == null ? string.Empty : beverageInfo.BeverageIcon, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), beverageInfoJson, new List<OrderDetaliMaterial>()));
            }

            // 获取货币信息
            var currencyInfo = await context.Currency.FirstOrDefaultAsync(w => w.Code == input.CurrencyCode);
            var currencySymbol = currencyInfo == null ? string.Empty : currencyInfo.CurrencySymbol;

            var orderInfo = new OrderInfo(OrderTypeEnum.OnlineOrder, input.DeviceBaseId, input.BizNo, input.PayAmount, input.Provider, input.PayTimeSp, input.CurrencyCode, currencySymbol, input.EnterpriseinfoId, orderNO, orderDetails);
            orderInfo.SetSaleResult(OrderSaleResult.NotPay);
            orderInfo.SetShipmentResult(OrderShipmentResult.NotShipped);
            orderInfo.SetThirdOrderId(payOrderId);

            // 设置当时的设备及企业信息
            if (deviceinfo != null)
            {
                if (deviceinfo != null)
                {
                    var enterpriseInfo = await context.EnterpriseInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == deviceinfo.EnterpriseinfoId);
                    orderInfo.SetEDBaseInfo(enterpriseInfo.Id, enterpriseInfo.Name, deviceinfo.Id, deviceinfo.Name, deviceinfo.DeviceModel == null ? 0 : deviceinfo.DeviceModel.Id, deviceinfo.DeviceModel == null ? "" : deviceinfo.DeviceModel.Name);
                }
            }
            if (input.OrderPaymentType != null)// 暂时只支持微信和支付宝支付
            {
                orderInfo.SetOrderStatus(OrderStatusEnum.PaymentInProgress);
                orderInfo.SetSaleResult(OrderSaleResult.NotPay);
            }

            // 线上支付，添加当前支付信息
            if (!string.IsNullOrWhiteSpace(input.PaymentMerchantId))
                orderInfo.SetPaymentInfo(input.PaymentMerchantId, input.SystemPaymentMethodId, input.SystemPaymentServiceProviderId);

            // 微信支付宝支付时需要传该参数
            if (input.OrderPaymentType != null)
                orderInfo.SetOrderPaymentType(input.OrderPaymentType);

            // 保存订单信息到数据库
            await context.OrderInfo.AddAsync(orderInfo);

            return true;
        }
    }
}