using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers
{
    /// <summary>
    /// 订单退款订阅
    /// </summary>
    /// <param name="context"></param>
    /// <param name="paymentPlatformUtil"></param>
    /// <param name="publish"></param>
    /// <param name="logger"></param>
    public class OrderRefundSubscriber(CoffeeMachineDbContext context, PaymentPlatformUtil paymentPlatformUtil, IPublishService publish, ILogger<OrderRefundSubscriber> logger) : ICapSubscribe
    {
        /// <summary>
        /// 执行订单退款订阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.DomesticPaymentOrderRefund)]
        public async Task Handle(OrderRefundSubscriberDto input)
        {
            logger.LogInformation("接收到订单退款消息，内容：{Input}", JsonConvert.SerializeObject(input));

            #region 参数准备

            // 需要插入的订单退款表的数据
            List<OrderRefund> addOrderRefunds = new List<OrderRefund>();

            // 发起退款所需参数
            var refundGoods = new List<OrderRefundGoodsDto>();

            // 退款金额
            decimal refundAmount = 0;

            // 主订单状态（枚举类型：OrderStatusEnum）
            OrderStatusEnum orderStatus = OrderStatusEnum.FullRefund;

            // 当前utc时间
            DateTime currUtcTime = DateTime.UtcNow;

            // 根据订单创建时间精准定位订单表
            string orderTableName = string.Empty;

            // 自定义退款号
            string outRefundNo = YitIdHelper.NextId().ToString();

            #endregion

            try
            {
                #region (入参验证)

                if (input.OrderId <= 0)
                    throw ExceptionHelper.AppFriendly("主订单号不能为空");

                if (!string.IsNullOrWhiteSpace(input.RefundReason))
                {
                    input.RefundReason = input.RefundReason.Trim();
                    if (input.RefundReason.Length > 50)
                        input.RefundReason = input.RefundReason.Substring(0, 50);
                }

                if (input.RefundOrderProducts == null || input.RefundOrderProducts.Count == 0)
                    throw ExceptionHelper.AppFriendly("请选择要退款的子订单");

                foreach (var item in input.RefundOrderProducts)
                {
                    if (item.RefundAmount < 0 && item.RefundAmount != -1)
                        throw ExceptionHelper.AppFriendly("请选择要退款的子订单");
                }

                #endregion

                #region 获取订单、子订单表信息，并验证订单状态

                var orderInfo = await context.OrderInfo.Include(i => i.OrderDetails)
                    .AsQueryable().AsNoTracking().IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.Id == input.OrderId);
                if (orderInfo == null)
                    throw ExceptionHelper.AppFriendly("未找到对应的订单信息");

                if (orderInfo.OrderDetails == null || orderInfo.OrderDetails.Count == 0)
                    throw ExceptionHelper.AppFriendly("未找到对应的子订单信息");

                if (orderInfo.OrderStatus == OrderStatusEnum.PaymentInProgress
                    || orderInfo.OrderStatus == OrderStatusEnum.Fail
                    || orderInfo.OrderStatus == OrderStatusEnum.CancelPayment)
                    throw ExceptionHelper.AppFriendly("当前订单未付款不能退款");

                if (orderInfo.OrderStatus == OrderStatusEnum.FullRefund)
                    throw ExceptionHelper.AppFriendly("没有可退款的子订单");

                if (orderInfo.SystemPaymentMethodId == CommonConst.OtherPaymentId)
                    throw ExceptionHelper.AppFriendly("现金支付的订单不支持退款");

                // 获取饮品信息,TODO：后续增加副柜需要调整
                var skus = orderInfo.OrderDetails.Select(s => s.ItemCode).ToList();
                var deviceInfo = await context.DeviceInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.DeviceBaseId == orderInfo.DeviceBaseId && !w.IsDelete);
                //var beverages = await context.BeverageInfo.IgnoreQueryFilters().Where(w => skus.Contains(w.Code) && w.DeviceId == deviceInfo.Id && !w.IsDelete).Distinct().ToListAsync();
                //var beverageDics = beverages.ToDictionary(k => k.Code, v => v.BeverageIcon);

                // 组装订单商品信息,TODO：子订单后续可能需要新增金额及实付金额
                var orderProducts = orderInfo.OrderDetails.Select(p => new RefundOrderProductDto()
                {
                    OrderProductId = p.Id.ToString(),
                    CounterNo = p.CounterNo,
                    SlotNo = p.SlotNo,
                    Sku = p.ItemCode,
                    BarCode = p.ItemCode,
                    Name = p.BeverageName,
                    MainImage = p.Url,// beverageDics.Any(w => w.Key == p.ItemCode) ? beverageDics[p.ItemCode] : string.Empty,
                    PayAmount = p.Price,
                    ShipmentQuantity = p.Quantity,
                    PurchaseQuantity = p.Quantity,
                    RefundableAmount = p.Price,
                    ItemPricing = p.Price
                }).ToList();

                #endregion

                #region 获取退款订单表信息

                var orderRefunds = await context.OrderRefund.AsQueryable().AsNoTracking().IgnoreQueryFilters()
                    .Where(w => w.OrderId == input.OrderId.ToString() && w.RefundStatus != RefundStatusEnum.Fail)
                    .Select(p => new
                    {
                        p.OrderDetailId,
                        p.RefundAmount
                    })
                    .ToListAsync();

                if (orderRefunds.Count > 0)
                {
                    orderProducts.ForEach(item =>
                    {
                        // 可退金额
                        item.RefundableAmount -= orderRefunds.Where(p => p.OrderDetailId == item.OrderProductId).Sum(p => p.RefundAmount);
                    });
                }

                #endregion

                #region (组装订单退款表的数据)

                // 退款的处理方式
                HandlingMethodEnum handlingMethod = HandlingMethodEnum.FullRefund;

                // 订单商品表的键值对
                // key：子订单号(OrderDetailId)
                // value：订单商品信息
                Dictionary<string, RefundOrderProductDto> dicOrderProductId = orderProducts.ToDictionary(p => p.OrderProductId, p => p);

                // 订单商品信息
                RefundOrderProductDto? temp_OrderProductDto;
                foreach (var item in input.RefundOrderProducts)
                {
                    dicOrderProductId.TryGetValue(item.OrderProductId, out temp_OrderProductDto);
                    if (temp_OrderProductDto == null)
                    {
                        throw ExceptionHelper.AppFriendly("退款的子订单不存在");
                    }

                    #region (退款金额)

                    if (item.RefundAmount == -1)// 当前子订单全部退款
                    {
                        item.RefundAmount = temp_OrderProductDto.RefundableAmount;
                    }
                    else if (item.RefundAmount > 0 && temp_OrderProductDto.RefundableAmount < item.RefundAmount)
                    {
                        // 子订单的可退款金额少于用户输入的退款金额
                        throw ExceptionHelper.AppFriendly("退款金额不能超过可退金额");
                    }

                    #endregion

                    #region (退款的处理方式)

                    if (item.RefundAmount == 0)
                        handlingMethod = HandlingMethodEnum.UpdateInventory;
                    else if (temp_OrderProductDto.RefundableAmount == item.RefundAmount)
                        handlingMethod = HandlingMethodEnum.FullRefund;
                    else
                        handlingMethod = HandlingMethodEnum.PartialRefund;

                    #endregion

                    // 需要插入的订单退款表的数据
                    addOrderRefunds.Add(new OrderRefund()
                    {
                        OrderId = orderInfo.Id.ToString(),
                        OrderDetailId = item.OrderProductId,
                        RefundOrderNo = outRefundNo,
                        ProductId = temp_OrderProductDto.ProductId,
                        BarCode = temp_OrderProductDto.BarCode,
                        Name = temp_OrderProductDto.Name,
                        MainImage = temp_OrderProductDto.MainImage,
                        RefundAmount = item.RefundAmount,
                        RefundReason = input.RefundReason,
                        RefundStatus = RefundStatusEnum.UnRefund,
                        HandlingMethod = handlingMethod,
                        OrderCreatedOnUtc = orderInfo.CreateTime,
                        InitiationTime = currUtcTime,
                        CreateUserId = input.OperateUserId,
                        EnterpriseinfoId = orderInfo.EnterpriseinfoId
                    });

                    // 发起退款所需参数
                    refundGoods.Add(new OrderRefundGoodsDto
                    {
                        Name = temp_OrderProductDto.Name,
                        Price = temp_OrderProductDto.ItemPricing,
                        Code = temp_OrderProductDto.Sku,
                        Quantity = temp_OrderProductDto.PurchaseQuantity,
                        RefundAmount = item.RefundAmount
                    });
                }

                #endregion

                // 退款金额
                refundAmount = input.RefundOrderProducts.Sum(p => p.RefundAmount);
                if (orderProducts.Sum(p => p.RefundableAmount) > refundAmount)
                    orderStatus = OrderStatusEnum.PartialRefund;

                #region Db操作，更新订单及持久化退款订单信息

                // 更新订单状态，TODO：后续优化，订单表加上乐观锁避免重复更新
                var orderInfoUpdate = await context.OrderInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == input.OrderId);
                if (orderInfoUpdate != null)
                {
                    orderInfoUpdate.SetOrderStatus(OrderStatusEnum.Refunding);
                    orderInfoUpdate.SetSaleResult(OrderSaleResult.Refunding);
                    context.OrderInfo.Update(orderInfoUpdate);
                    // 如果是设备等待超时退款，则更新订单出货状态为未出货
                    if (input.RefundReason == "设备等待超时退款")
                        orderInfoUpdate.SetShipmentResult(OrderShipmentResult.NotShipped);
                }

                // 插入和更新退款记录信息
                await context.OrderRefund.AddRangeAsync(addOrderRefunds);

                await context.SaveChangesAsync();

                #endregion

                #region 支付退款操作

                // 提交退款到支付平台(微信支付宝)
                if (refundAmount > 0)
                {
                    var refundIds = addOrderRefunds.Select(a => a.Id);
                    try
                    {
                        // 订单退款
                        var refundRes = await paymentPlatformUtil.OrderRefund(orderInfoUpdate, refundGoods, refundAmount, input.RefundReason, outRefundNo);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "执行订单退款失败,调用退款发送异常");
                    }

                    // 更新为退款中状态
                    await context.OrderRefund.IgnoreQueryFilters().Where(w => refundIds.Contains(w.Id) && w.RefundStatus == RefundStatusEnum.UnRefund)
                        .ExecuteUpdateAsync(s => s.
                    SetProperty(u => u.RefundStatus, RefundStatusEnum.Refunding));

                    await context.SaveChangesAsync();

                    // 延迟15s主动查询一次状态(这里不管成功失败，都主动去查询一次，以防网络异常请求失败等原因，导致退款状态不一致")

                    // 发起时间超过10分钟则每10分钟查询一次，没超过10分钟就1分钟查询一次
                    var second = 15;
                    await publish.SendDelayMessage(CapConst.DomesticPaymentOrderRefundSyncStatus, new OrderRefundSyncStatusDto
                    {
                        OrderId = orderInfo.Code,
                        OutRefundNo = outRefundNo
                    }, second);
                    logger.LogInformation($"[订单退款状态同步]{JsonConvert.SerializeObject(input)}退款中延迟{second}s再次查询");
                }

            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, $"执行订单退款操作，具体原因：{ex.Message}，请求内容：{JsonConvert.SerializeObject(input)} ");

                // 自动退款，就吞掉业务类型的异常（业务类型的异常（比如退款金额不够，订单不存在），不需要cap重试）
                if (input.OrderRefundType)
                    return;
            }
            #endregion
        }
    }
}