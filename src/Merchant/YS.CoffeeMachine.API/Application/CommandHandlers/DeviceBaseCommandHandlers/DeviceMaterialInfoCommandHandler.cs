using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DeviceBaseCommandHandlers
{
    /// <summary>
    /// UpdateDeviceMaterialWarningCommand
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateDeviceMaterialWarningCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceMaterialWarningCommand, bool>
    {
        /// <summary>
        /// UpdateDeviceUpdateDeviceMaterialWarningCommandCapacityCfg
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceMaterialWarningCommand request, CancellationToken cancellationToken)
        {
            var ids = request.dtos.Select(x => x.Id);
            var olds = await _db.DeviceEarlyWarnings.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var warning in request.dtos)
            {
                var old = olds.FirstOrDefault(x => x.Id == warning.Id);
                if (old != null)
                {
                    old.Update(warning.IsOn, warning.WarningValue);
                    _db.Update(old);
                }
                else
                {
                    await _db.AddAsync(new DeviceEarlyWarnings(request.deviceBaseId, warning.WarningType, warning.IsOn, warning.WarningValue));
                }
            }
            return true;
        }
    }
}
