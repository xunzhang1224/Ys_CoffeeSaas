using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.PaymentCommands.SEVMCPayment;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.BasicDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK.Pay.Request;
using YS.Pay.SDK.Response;
using YS.Pay.SDK.Top;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers.SEVMCPayment
{
    /// <summary>
    /// SEVMC创建订单命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userHttp"></param>
    /// <param name="configuration"></param>
    /// <param name="mapper"></param>
    /// <param name="createOrderService"></param>
    public class CreateOrderCommandHandler(CoffeeMachineDbContext context, UserHttpContext userHttp, IConfiguration configuration, IMapper mapper, ICreateOrderService createOrderService) : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
    {
        /// <summary>
        /// 执行创建订单命令
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            // 生成订单号
            string orderNo = BasicUtils.GenerateOrderNo();

            // 创建订单请求
            CreateOrderRequest createOrderRequest = new CreateOrderRequest
            {
                MerchantId = input.MerchantId.ToString(),
                Currency = input.CurrencyCode,
                PayAmount = input.PayAmount,
                CustomContent = JsonConvert.SerializeObject(new OrderCustomContentDto()
                {
                    OrderId = orderNo,
                    ConsumerUserId = userHttp.UserId.ToString(),
                }),
                Extra = JsonConvert.SerializeObject(input.Extra)
            };

            // 设置请求客户端
            DefaultTopApiClient topClient = new DefaultTopApiClient(long.Parse(configuration["YsPaymentPlatform:AppId"] ?? "0"), configuration["YsPaymentPlatform:AppKey"] ?? string.Empty, configuration["YsPaymentPlatform:TestUrl"] ?? string.Empty);

            // 执行创建订单请求
            var result = await topClient.Execute(createOrderRequest);

            // 更新外部订单信息
            if (result.IsError)
                throw ExceptionHelper.AppFriendly($"创建订单失败: {result.ErrCode} - {result.ErrMsg}");

            // 创建订单信息
            await createOrderService.CreateOrderInfo(mapper.Map<CreateOrderBaseInput>(input), orderNo, result.PayOrderId);

            // 返回结果
            return result;
        }
    }
}