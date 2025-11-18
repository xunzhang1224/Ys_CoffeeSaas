using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries
{
    /// <summary>
    /// 设备类型查询
    /// </summary>
    public interface IP_DeviceModelQueries
    {
        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <returns></returns>
        Task<List<object>> GetDeviceModelListAsync();

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceModelDto> GetDeviceModelAsync(long id);

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceModelDto>> GetDeviceModelListAsync(QueryRequest request);
    }
}
