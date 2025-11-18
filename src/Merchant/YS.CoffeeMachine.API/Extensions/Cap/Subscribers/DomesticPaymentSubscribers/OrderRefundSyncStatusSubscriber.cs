using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.DomesticPaymentSubscribers
{
    /// <summary>
    /// 退款订单同步状态
    /// </summary>
    /// <param name="context"></param>
    /// <param name="publish"></param>
    /// <param name="paymentPlatformUtil"></param>
    /// <param name="logger"></param>
    public class OrderRefundSyncStatusSubscriber(CoffeeMachineDbContext context, IPublishService publish, PaymentPlatformUtil paymentPlatformUtil, ILogger<OrderRefundSyncStatusSubscriber> logger) : ICapSubscribe
    {
        /// <summary>
        /// 执行退款订单同步状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.DomesticPaymentOrderRefundSyncStatus)]
        public async Task Handle(OrderRefundSyncStatusDto input)
        {
            logger.LogError("开始执行退款订单同步状态");

            #region 参数验证

            if (input == null || string.IsNullOrEmpty(input.OrderId) || string.IsNullOrEmpty(input.OutRefundNo))
            {
                logger.LogError("[订单退款状态同步]参数错误");
                return;
            }
            #endregion

            #region 订单信息确认

            // 查询主订单
            var order = await context.OrderInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Code == input.OrderId);
            if (order == null)
                return;

            var unRefundStatus = new RefundStatusEnum[] { RefundStatusEnum.Fail, RefundStatusEnum.UnRefund };
            var orderId = order.Id.ToString();
            var orderRefunds = await context.OrderRefund.IgnoreQueryFilters()
                .Where(p => p.OrderId == orderId && !unRefundStatus.Contains(p.RefundStatus))
                .ToListAsync();

            // 退款中的订单
            if (!orderRefunds.Any(a => a.RefundOrderNo == input.OutRefundNo && a.RefundStatus == RefundStatusEnum.Refunding))
            {
                logger.LogInformation("[订单退款状态同步]未查询到有效的退款中的退款订单" + JsonConvert.SerializeObject(input));
                return;
            }
            var refundIds = orderRefunds.Where(a => a.RefundOrderNo == input.OutRefundNo && a.RefundStatus == RefundStatusEnum.Refunding).Select(a => a.Id).ToList();
            #endregion

            #region 发起第三方查询
            var refundRes = await paymentPlatformUtil.OrderRefundQuery(order.Id, input.OutRefundNo);
            // 退款中继续延时处理
            if (refundRes.RefundStatus == RefundStatusEnum.Refunding)
            {
                var refundTime = orderRefunds.Where(a => a.RefundOrderNo == input.OutRefundNo).Max(a => a.InitiationTime);
                // 发起时间超过10分钟则每10分钟查询一次，没超过10分钟就1分钟查询一次
                var second = (DateTime.UtcNow - refundTime).Minutes > 10 ? 600 : 60;
                input.TransId = $"{order.Code}_{YitIdHelper.NextId()}";
                await publish.SendDelayMessage(CapConst.DomesticPaymentOrderRefundSyncStatus, input, second);
                logger.LogInformation($"[订单退款状态同步]{JsonConvert.SerializeObject(input)}退款中延迟{second}s再次查询");
                return;
            }
            #endregion

            // 退款失败或者成功，则更新状态
            foreach (var orderRefund in orderRefunds)
            {
                if (!refundIds.Contains(orderRefund.Id))
                    continue;
                if (orderRefund.RefundStatus != RefundStatusEnum.Refunding)
                    continue;
                orderRefund.RefundStatus = refundRes.RefundStatus;
            }

            // 当前订单已退款的金额
            decimal refundAmount = orderRefunds.Where(p => p.RefundStatus == RefundStatusEnum.Success).Sum(p => p.RefundAmount);
            // 主订单状态（枚举类型：OrderStatusEnum）
            var orderStatus = OrderStatusEnum.FullRefund;
            var orderSaleResult = OrderSaleResult.Refund;
            if (refundAmount == 0)
            {
                orderStatus = OrderStatusEnum.Success;
                orderSaleResult = OrderSaleResult.Success;
            }
            else if (order.Amount > refundAmount)
            {
                orderStatus = OrderStatusEnum.PartialRefund;
                orderSaleResult = OrderSaleResult.PartialRefund;
            }

            if (orderRefunds.Any(p => p.RefundStatus == RefundStatusEnum.Refunding))
            {
                orderStatus = OrderStatusEnum.Refunding;
                orderSaleResult = OrderSaleResult.Refunding;
            }

            logger.LogInformation($"[订单退款状态同步]{input.OrderId}-{input.OutRefundNo},更新订单退款状态，主订单最新状态:{orderStatus.ToString()}");

            DateTime currUtcTime = DateTime.UtcNow;
            try
            {
                // 修改主订单状态
                order.SetOrderStatus(orderStatus);
                order.SetSaleResult(orderSaleResult);
                order.SetReturnAmount(refundAmount);
                order.LastModifyTime = currUtcTime;
                context.OrderInfo.Update(order);

                // 更新退款表状态
                var orderRefund = await context.OrderRefund.IgnoreQueryFilters().Where(w => refundIds.Contains(w.Id)).ToListAsync();
                if (orderRefund != null && orderRefund.Count > 0)
                {
                    orderRefund.ForEach(e =>
                    {
                        e.RefundStatus = refundRes.RefundStatus;
                        e.SuccessTime = currUtcTime;
                    });
                    context.OrderRefund.UpdateRange(orderRefund);
                }

                var count = await context.SaveChangesAsync();

                if (count <= 0)
                    throw ExceptionHelper.AppFriendly("系统繁忙，请稍后重试");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "订单退款状态同步失败消息订阅，执行事务操作发生异常，请求内容：" + JsonConvert.SerializeObject(input));
                throw ExceptionHelper.AppFriendly("订单退款状态同步失败消息订阅");
            }
        }
    }
}