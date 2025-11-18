using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 推送版本到设备
    /// </summary>
    /// <param name="_context"></param>
    public class PushVersionToDeviceCommandHandler(CoffeeMachinePlatformDbContext _context) : ICommandHandler<PushVersionToDeviceCommand, List<PushVersionDto>>
    {
        /// <summary>
        /// 推送版本到设备
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PushVersionDto>> Handle(PushVersionToDeviceCommand request, CancellationToken cancellationToken)
        {
            var deviceVersinoManagetInfo = await _context.DeviceVersionManage.AsQueryable().Where(a => a.Id == request.deviceVersionManageId).FirstOrDefaultAsync();
            var infoList = new List<DeviceVsersionUpdateRecord>();

            var deviceMidDic = await _context.DeviceBaseInfo.AsQueryable().Where(w => request.deviceBaseInfoIds.Contains(w.Id)).ToDictionaryAsync(t => t.Id, t => t.Mid);

            List<PushVersionDto> result = new List<PushVersionDto>();
            foreach (var item in request.deviceBaseInfoIds)
            {
                var id = YitIdHelper.NextId();
                var info = new DeviceVsersionUpdateRecord(item, request.deviceVersionManageId, deviceVersinoManagetInfo.Name, deviceVersinoManagetInfo.ProgramType
                    , deviceVersinoManagetInfo.VersionType, request.type, request.programTypeName, id);
                infoList.Add(info);

                PushVersionDto pushVersionDto = new PushVersionDto();
                pushVersionDto.Mid = deviceMidDic[item];
                pushVersionDto.RecordId = id;
                result.Add(pushVersionDto);
            }
            await _context.AddRangeAsync(infoList);

            return result;
        }
    }
}
