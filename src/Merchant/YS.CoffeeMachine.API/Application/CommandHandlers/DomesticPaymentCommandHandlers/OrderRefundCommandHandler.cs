using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers
{
    /// <summary>
    /// 订单退款
    /// </summary>
    /// <param name="publish"></param>
    /// <param name="httpContext"></param>
    public class OrderRefundCommandHandler(IPublishService publish, UserHttpContext httpContext) : ICommandHandler<OrderRefundCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(OrderRefundCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.input == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            // 组装订阅消息
            var message = new OrderRefundSubscriberDto()
            {
                TransId = YitIdHelper.NextId().ToString(),
                OrderId = request.input.OrderId,
                RefundOrderProducts = request.input.RefundOrderProducts,
                RefundReason = request.input.RefundReason,
                OperateUserId = httpContext.UserId,
                OrderRefundType = false
            };

            // cap发布执行订单退款操作
            await publish.SendMessage(CapConst.DomesticPaymentOrderRefund, message);

            return true;
        }
    }
}