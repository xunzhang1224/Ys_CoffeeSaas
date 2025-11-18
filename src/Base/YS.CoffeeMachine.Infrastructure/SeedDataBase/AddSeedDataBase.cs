using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Currencys;

namespace YS.CoffeeMachine.Infrastructure.SeedDataBase
{
    /// <summary>
    /// 添加种子数据
    /// </summary>
    /// <param name="context"></param>
    public class AddSeedDataBase(CoffeeMachineDbContext context)
    {
        /// <summary>
        /// 添加种子数据
        /// </summary>
        public async void AddSeedData()
        {
            //if (!context.CurrencyInfo.Any())
            //{
            //    var cultureInfo = new List<CurrencyInfo>();
            //    foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            //    {
            //        var region = new RegionInfo(culture.Name);
            //        cultureInfo.Add(new CurrencyInfo(culture.Name, region.CurrencySymbol, region.ISOCurrencySymbol, region.EnglishName, region.DisplayName));
            //    }
            //    await context.AddRangeAsync(cultureInfo);
            //    await context.SaveChangesAsync();
            //}
        }
    }
}
