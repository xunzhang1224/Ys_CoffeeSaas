using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.CoffeeMachine.Application.Queries.IPaymentInfoQueries;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.PaymentInfoQueries
{
    /// <summary>
    /// 支付相关查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userHttpContext"></param>
    public class PaymentQueries(CoffeeMachineDbContext context, UserHttpContext userHttpContext) : IPaymentQueries
    {
        /// <summary>
        /// 获取符合条件的平台端支付配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<P_PaymentConfigDto>> GetPpaymentConfig()
        {
            // 获取企业的地区关联id
            var areaRelationId = await context.EnterpriseInfo.AsQueryable().Where(a => a.Id == userHttpContext.TenantId).Select(a => a.AreaRelationId).FirstOrDefaultAsync();

            // 获取地区关联的国家
            var country = await context.AreaRelation.AsQueryable().Where(a => a.Id == areaRelationId).Select(a => a.Country).FirstOrDefaultAsync();

            // 返回符合条件的平台端支付配置
            return await context.P_PaymentConfig.AsQueryable()
                .Where(a => a.Countrys.Contains(country ?? "") && a.Enabled == EnabledEnum.Enable)
                .Select(a => new P_PaymentConfigDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    PictureUrl = a.PictureUrl,
                }).ToListAsync();
        }

        /// <summary>
        /// 商户端支付配置列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<PaymentConfigDto>> GetPaymentConfig()
        {
            return await context.PaymentConfig.AsQueryable()
                .Select(a => new PaymentConfigDto
                {
                    Id = a.Id,
                    P_PaymentConfigId = a.P_PaymentConfigId,
                    Email = a.Email,
                    PaymentConfigStatue = a.PaymentConfigStatue,
                    MerchantCode = a.MerchantCode,
                    Remark = a.Remark,
                    PictureUrl = a.PictureUrl,
                    Enabled = a.Enabled
                }).ToListAsync();
        }
    }
}
