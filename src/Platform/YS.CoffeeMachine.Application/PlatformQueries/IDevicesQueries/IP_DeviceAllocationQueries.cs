using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;

namespace YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries
{
    /// <summary>
    /// 设备应用查询
    /// </summary>
    public interface IP_DeviceAllocationQueries
    {
        /// <summary>
        /// 设备应用分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceAllocationDto>> GetDeviceAllocationPageListAsync(DeviceAllocationInput request);
    }
}
