using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands
{
    public record UpdateSettingInfoCommand(long id, long deviceInfoId, bool isShowEquipmentNumber, long interfaceStylesId, WashEnum washType, string regularWashTime, int? washWarning, string afterSalesPhone, string expectedUpdateTime, int screenBrightness, int deviceSound, string administratorPwd, string replenishmentOfficerPwd,  string startTime, int startWeek, string endTime, int endWeek, string languageName, string currencyCode, List<MaterialBoxInput> materialBoxs) : ICommand<bool>;
}
