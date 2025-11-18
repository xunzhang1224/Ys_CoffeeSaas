using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Commands.PaymentCommands.SEVMCPayment;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK.Pay.Request;
using YS.Pay.SDK.Top;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers.SEVMCPayment
{
    /// <summary>
    /// 订单退款命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="httpContext"></param>
    public class OrderRefundCommandHandler(CoffeeMachineDbContext context, IConfiguration configuration, UserHttpContext httpContext) : ICommandHandler<OrderRefundCommand, bool>
    {
        /// <summary>
        /// 执行订单退款
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(OrderRefundCommand input, CancellationToken cancellationToken)
        {
            // 获取订单信息
            var order = context.OrderInfo.AsQueryable().AsNoTracking()
                .Include(i => i.OrderDetails)
                .FirstOrDefault(x => x.ThirdOrderId == input.payOrderId);

            #region 数据校验
            if (order == null)
                throw ExceptionHelper.AppFriendly($"未找到对应的订单信息，PayOrderId: {input.payOrderId}");

            if (order.SaleResult != OrderSaleResult.Success)
                throw ExceptionHelper.AppFriendly($"订单状态异常，当前状态: {order.SaleResult}不允许操作, PayOrderId: {input.payOrderId}");

            var orderDetails = order.OrderDetails.Where(w => input.orderItems.Contains(w.Id)).ToList();

            if (orderDetails.Count == 0)
                throw ExceptionHelper.AppFriendly("未找到对应的订单详情信息");

            var returnAmount = orderDetails.Sum(s => s.Price);

            if (returnAmount <= 0)
                throw ExceptionHelper.AppFriendly("退款金额必须大于0");

            if (returnAmount > order.Amount)
                throw ExceptionHelper.AppFriendly("退款金额不能大于订单总金额");

            if (string.IsNullOrEmpty(order.ThirdOrderId))
                throw ExceptionHelper.AppFriendly("订单第三方ID不能为空");
            #endregion

            // 退款
            OrderRefundRequest request = new OrderRefundRequest()
            {
                PayOrderId = order.ThirdOrderId,
                Amount = returnAmount,
                Reason = input.reason,
                CustomContent = JsonConvert.SerializeObject(new OrderRefundCustomContentInput()
                {
                    I = order.ThirdOrderId,
                    C = httpContext.UserId,
                    Ps = orderDetails.Select(p => new OrderRefundEntityDto()
                    {
                        I = p.Id,
                        PS = p.ItemCode,
                        A = p.Price
                    }).ToList()
                }),
                Extra = input.machineCode
            };
            DefaultTopApiClient topClient = new DefaultTopApiClient(long.Parse(configuration["YsPaymentPlatform:AppId"] ?? "0"), configuration["YsPaymentPlatform:AppKey"] ?? string.Empty, configuration["YsPaymentPlatform:TestUrl"] ?? string.Empty);

            // 执行退款请求
            var resual = await topClient.Execute(request);
            if (resual == null)
                throw ExceptionHelper.AppFriendly("退款失败，未返回结果");

            // 设置订单状态
            order.SetSaleResult(OrderSaleResult.Refund);
            order.SetReturnAmount(returnAmount);
            context.OrderInfo.Update(order);

            // TODO: 订单表缺少退款状态字段
            return true;
        }
    }
}