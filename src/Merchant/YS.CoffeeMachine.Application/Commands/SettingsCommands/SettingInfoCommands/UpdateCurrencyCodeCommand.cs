using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands
{
    public record UpdateCurrencyCodeCommand(long settingInfoId, string currencyCode) : ICommand<bool>;
}
