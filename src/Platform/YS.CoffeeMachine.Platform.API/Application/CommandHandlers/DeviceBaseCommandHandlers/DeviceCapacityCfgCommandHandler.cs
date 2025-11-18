using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DeviceBaseCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DeviceBaseCommandHandlers
{
    /// <summary>
    /// UpdateDeviceCapacityCfgCommandHandler
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateDeviceCapacityCfgCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceCapacityCfgCommand,bool>
    {
        /// <summary>
        /// UpdateDeviceCapacityCfgCommandHandler
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceCapacityCfgCommand request, CancellationToken cancellationToken)
        {
            var cfgs = await _db.DeviceCapacityCfg.Where(x => x.DeviceBaseId == request.deviceBaseId && request.dic.Keys.Contains((int)x.CapacityId)).ToListAsync();
            cfgs.ForEach(x => x.Update(request.dic[(int)x.CapacityId]));
            return true;
        }
    }
}
