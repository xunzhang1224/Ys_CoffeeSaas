using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformDto.PaymentDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IPaymentQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.PaymenQueries
{
    /// <summary>
    /// 支付配置相关查询
    /// </summary>
    /// <param name="context">上下文</param>
    public class PaymentConfigQueries(CoffeeMachinePlatformDbContext context) : IPaymentConfigQueries
    {
        /// <summary>
        /// 支付配置相关查询
        /// </summary>
        public async Task<List<P_PaymetConfigDto>> GetPaymentConfigList()
        {
            var configs = await context.P_PaymentConfig.AsQueryable()
                .Select(a => new P_PaymetConfigDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Countrys = a.Countrys,
                    PaymentModel = a.PaymentModel,
                    PictureUrl = a.PictureUrl,
                    Enabled = a.Enabled,
                })
                 .ToListAsync();

            var dictionarys = await context.Dictionary.AsQueryable().Where(a => a.ParentKey == "Country").ToListAsync();
            foreach (var item in configs)
            {
                var countryArry = item.Countrys.Split(',').ToList();
                item.CountryArry = countryArry;
                item.CountryNames = string.Join(",", dictionarys
                                .Where(a => countryArry.Contains(a.Key))
                                .Select(a => a.Value)
                                .ToList());
            }

            return configs;
        }
    }
}
