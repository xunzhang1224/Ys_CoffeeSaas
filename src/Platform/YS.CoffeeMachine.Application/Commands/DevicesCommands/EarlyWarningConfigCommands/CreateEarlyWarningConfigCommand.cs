using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.EarlyWarningConfigCommands
{
    public record CreateEarlyWarningConfigCommand(long deviceInfoId, bool wholeMachineCleaningSwitch, DateTime nextWholeMachineCleaningTime, bool brewingMachineCleaningSwitch, DateTime nextBrewingMachineCleaningTime, bool milkFrotherCleaningSwitch, DateTime nextMilkFrotherCleaningTime, bool coffeeWaterwayCleaningSwitch, DateTime nextCoffeeWaterwayCleaningTime, bool steamWaterwayCleaningSwitch, DateTime nextSteamWaterwayCleaningTime, bool offlineWarningSwitch, int offlineDays, bool shortageWarningSwitch, double coffeeBeanRemaining, double waterRemaining) : ICommand<bool>;
}
