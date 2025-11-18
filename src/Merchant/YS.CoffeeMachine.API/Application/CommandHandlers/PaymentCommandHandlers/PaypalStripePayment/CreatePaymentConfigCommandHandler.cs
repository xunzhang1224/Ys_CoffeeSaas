using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.PaymentCommands.PaypalStripePayment;
using YS.CoffeeMachine.Domain.AggregatesModel.Payment;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK.Pay.Request;
using YS.Pay.SDK.Response;
using YS.Pay.SDK.Top;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers.PaypalStripePayment
{
    /// <summary>
    /// 创建支付配置
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManage"></param>
    /// <param name="configuration"></param>
    public class CreatePaymentConfigCommandHandler(CoffeeMachineDbContext context, UserHttpContext userManage, IConfiguration configuration) : ICommandHandler<CreatePaymentConfigCommand, MerchantIncomingResponse>
    {
        /// <summary>
        /// 创建支付配置
        /// </summary>
        public async Task<MerchantIncomingResponse> Handle(CreatePaymentConfigCommand request, CancellationToken cancellationToken)
        {
            // 获取企业的地区关联id
            var areaRelationId = await context.EnterpriseInfo.AsQueryable().Where(a => a.Id == userManage.TenantId).Select(a => a.AreaRelationId).FirstOrDefaultAsync();
            // 获取地区关联的国家
            var country = await context.AreaRelation.AsQueryable().Where(a => a.Id == areaRelationId).Select(a => a.Country).FirstOrDefaultAsync();

            MerchantIncomingRequest mRequest = new MerchantIncomingRequest()
            {
                PaymentMethodId = request.p_PaymentConfigId,
                ThirdMerchantId = YitIdHelper.NextId().ToString(),
                Email = request.email,
                Country = country ?? "",//后续读取机构
                Extra = request.extra ?? "",
            };
            DefaultTopApiClient topClient = new DefaultTopApiClient(long.Parse(configuration["YsPaymentPlatform:AppId"] ?? "0"), configuration["YsPaymentPlatform:AppKey"] ?? "", configuration["YsPaymentPlatform:TestUrl"] ?? "");
            MerchantIncomingResponse response = await topClient.Execute(mRequest);
            if (!string.IsNullOrWhiteSpace(response.MerchantIncomingUrl))
            {
                var info = new PaymentConfig(request.p_PaymentConfigId, request.email, request.remark, mRequest.ThirdMerchantId, request.pictureUrl);
                await context.AddAsync(info);
            }
            return response;
        }
    }
}