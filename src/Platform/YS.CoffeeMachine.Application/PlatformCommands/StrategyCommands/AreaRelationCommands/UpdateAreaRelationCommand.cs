using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands
{
    /// <summary>
    /// 更新地区关系
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="area">地区（字典）</param>
    /// <param name="country">国家（字典）</param>
    /// <param name="areaCode">区号</param>
    /// <param name="language">语言（字典）</param>
    /// <param name="currencyId">币种（字典）</param>
    /// <param name="timeZone">时区（字典）</param>
    /// <param name="termServiceUrl">服务条款url</param>
    /// <param name="enabled">是否启用</param>
    public record UpdateAreaRelationCommand(long id, string area, string country, string areaCode, string language, long currencyId, string timeZone, string termServiceUrl, EnabledEnum enabled) : ICommand<bool>;
}