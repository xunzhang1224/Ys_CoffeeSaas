using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.DictionaryCommand
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="key">code</param>
    /// <param name="value">值</param>
    /// <param name="enabled">是否开启</param>
    /// <param name="parentKey">父级code</param>
    public record AddDictionaryCommand(string key, string value, EnabledEnum enabled = EnabledEnum.Enable, string parentKey = null) : ICommand;

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="key">code</param>
    /// <param name="value">值</param>
    /// <param name="enabled">是否开启</param>
    public record UpdateDictionaryCommand(string key, string value, EnabledEnum enabled) : ICommand;

    /// <summary>
    /// 删除字典（根据key）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public record DeleteDictionaryCommand(string key) : ICommand;
}
