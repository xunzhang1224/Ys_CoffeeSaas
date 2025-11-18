using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using static YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys.PromotionOutput;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备信息查询
    /// </summary>
    public interface IDeviceInfoQueries
    {
        /// <summary>
        /// 点单获取设备
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<GetDecentlyDevicePageListOutput>> GetDecentlyDevicePageList([FromBody] GetDecentlyDevicePageListInput request);

        /// <summary>
        /// 获取设备下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceSelectDto>> GetDeviceSelectListAsync();

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        Task<List<DeviceInfo>> GetDeviceInfoByMids(List<string> mids);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceInfoDto> GetDeviceInfoAsync(long id);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceInfoDto> GetDeviceAsync(long id);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListAsync(DevicesListInput request);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceH5Dto>> GetDeviceInfoH5ListAsync(DevicesH5ListInput request);

        /// <summary>
        /// H5设备列表统计
        /// </summary>
        /// <returns></returns>
        Task<DeviceCountOutput> GetDeviceCountOutput();

        /// <summary>
        /// H5首页统计
        /// </summary>
        /// <returns></returns>
        Task<SyCountOutput> GetSyCount(List<DateTime> times);

        /// <summary>
        /// 本周运营收入
        /// </summary>
        /// <returns></returns>
        Task<List<HourlyRevenueStats>> GetHourlyRevenueStatsFromDbAsync(List<DateTime> times, int offset, int hoursPerSlot = 1);

        /// <summary>
        /// 月运营收入
        /// </summary>
        /// <returns></returns>
        Task<List<OperatingRevenueOutput>> GetOperatingRevenueByMonth(List<DateTime> times, int offset);

        /// <summary>
        /// 本周运营收入
        /// </summary>
        /// <returns></returns>
        Task<DeviceReportCountOutput> GetDeviceReportCount();

        /// <summary>
        /// 设备销售排行
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceSaleRank>> GetDeviceSaleRank(List<DateTime> times);

        /// <summary>
        /// 饮品销售排行
        /// </summary>
        /// <returns></returns>
        Task<List<GoodsSaleRank>> GetGoodsSaleRank(List<DateTime> times, int top);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceInfoDto>> GetUnDeviceInfoListAsync(UnDeviceInput request);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceIdAsync(DeviceInfoListByDeviceIdInput request);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceModelId(DeviceInfoListByDeviceModelIdInput request);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>

        Task<List<object>> GetBeverageInfoByDeviceIdAsync(long deviceId);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>

        Task<List<long>> GetUserIdsByDeviceId(long deviceId);

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>

        Task<List<long>> GetGroupIdsByDeviceId(long deviceId);

        /// <summary>
        /// 获取设备广告设置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<DeviceAdInput> GetDeviceAd(long deviceId);

        /// <summary>
        /// 获取货币列表
        /// </summary>
        /// <returns></returns>
        Task<List<CurrentDto>> GetCurrentList();

        /// <summary>
        /// 获取用户设备列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allDeviceRole"></param>
        /// <returns></returns>
        Task<List<DeviceUserDto>> GetDeviceByUser(long? userId = null, bool? allDeviceRole = null);
    }
}
