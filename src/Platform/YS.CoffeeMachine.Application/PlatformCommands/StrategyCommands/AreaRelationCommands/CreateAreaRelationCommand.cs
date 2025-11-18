using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands
{
    /// <summary>
    /// 创建地区关联
    /// </summary>
    /// <param name="area">地区（字典）</param>
    /// <param name="country">国家（字典）</param>
    /// <param name="areaCode">区号</param>
    /// <param name="language">语言（字典）</param>
    /// <param name="currencyId">币种（字典）</param>
    /// <param name="timeZone">时区（字典）</param>
    /// <param name="termServiceUrl">服务条款url</param>
    /// <param name="enabled">是否启用</param>
    public record CreateAreaRelationCommand(string area,string country,string areaCode,string language,long currencyId,string timeZone,string termServiceUrl,EnabledEnum enabled) : ICommand<bool>;
}
