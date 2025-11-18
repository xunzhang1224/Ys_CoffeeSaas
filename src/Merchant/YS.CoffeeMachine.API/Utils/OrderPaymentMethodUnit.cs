using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 订单支付方式帮助类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="_publish"></param>
    public class OrderPaymentMethodUnit(CoffeeMachineDbContext context, ILogger<OrderPaymentMethodUnit> logger, IPublishService _publish)
    {
        /// <summary>
        /// 支付成功回调超时(在线支付）
        /// </summary>
        /// <param name="dto">入参</param>
        /// <returns></returns>
        public async Task PaymentSuccessCallbackTimeout(PaymentSuccessCallbackTimeoutDto dto)
        {
            // 获取订单
            var order = await context.OrderInfo.AsNoTracking().IgnoreQueryFilters()
              .Where(p => p.Code == dto.OrderCode)
              .Select(p => new
              {
                  p.Id,
                  p.Code,
                  p.OrderStatus
              })
              .FirstAsync();

            if (order == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (order.OrderStatus != OrderStatusEnum.Success)
                return;

            var orderDetails = await context.OrderDetails.IgnoreQueryFilters()
                    .Where(p => p.OrderId == order.Id)
                .ToListAsync();

            // 要退款的订单商品信息
            List<RefundOrderProduct> refundOrderProducts = new List<RefundOrderProduct>();
            foreach (var item in orderDetails)
            {
                // 设置子订单出货状态
                item.SetShipmentStatus(0);

                // 设置子订单出货错误信息
                item.SetErrorInfo(1, CommonConst.PaymentSuccessCallbackTimeout);

                // 添加要退款的订单商品信息
                refundOrderProducts.Add(new RefundOrderProduct()
                {
                    OrderProductId = item.Id.ToString(),
                    RefundAmount = -1
                });
            }

            // 订单退款发布订阅（入参）
            OrderRefundSubscriberDto orderRefundSubscribeDto = null;
            if (refundOrderProducts.Count() > 0)
            {
                orderRefundSubscribeDto = new OrderRefundSubscriberDto()
                {
                    TransId = "OrderRefund" + YitIdHelper.NextId(),
                    OrderId = order.Id,
                    RefundOrderProducts = refundOrderProducts,
                    RefundReason = "设备等待超时退款",
                    OperateUserId = 0,
                    OrderRefundType = true
                };
            }

            try
            {
                // 更新子订单
                context.OrderDetails.UpdateRange(orderDetails);

                if (orderRefundSubscribeDto != null)
                {
                    // cap发布执行订单退款操作
                    await _publish.SendMessage(CapConst.DomesticPaymentOrderRefund, orderRefundSubscribeDto);
                }
            }
            catch (Exception ex)
            {
                // 回滚事务
                logger.LogInformation(ex, "支付成功回调超时(在线支付）,执行具体的操作,事务发生异常,请求参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(dto));
                throw;
            }

            return;
        }
    }
}