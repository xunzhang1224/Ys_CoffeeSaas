using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Services
{
    /// <summary>
    /// 通讯服务调用接口
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="_configuration"></param>
    public class IotBaseService(IHttpClientFactory httpClientFactory, IConfiguration _configuration, CoffeeMachinePlatformDbContext _coffeeMachinePlatformDb)
    {
        /// <summary>
        /// 判断是否在线
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> IsOnline(string mid)
        {
            if (string.IsNullOrWhiteSpace(mid))
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0016)]);
            var url = _configuration["NotityUrl:Url"] + "/api/Iot/IsOnline?mid={0}";
            var client = httpClientFactory.CreateClient();
            //client.Timeout = TimeSpan.FromSeconds(2);
            var content = await client.GetStringAsync(string.Format(url, mid));
            return JsonConvert.DeserializeObject<bool>(content);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceBaseInfo> GetBaseInfoAsync(string mid)
        {
            return await _coffeeMachinePlatformDb.DeviceBaseInfo.Where(x => x.Mid == mid).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取mid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetMidAsync(string code)
        {
            return await _coffeeMachinePlatformDb.DeviceBaseInfo.Where(x => x.MachineStickerCode == code).Select(x=>x.Mid).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceInfo> GetDeviceInfoAsync(string mid)
        {
            var baseinfo = await GetBaseInfoAsync(mid);
            return await _coffeeMachinePlatformDb.DeviceInfo.Where(x => x.DeviceBaseId == baseinfo.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取设备型号信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeviceModel> GetDeviceModelInfoAsync(long id)
        {
            return await _coffeeMachinePlatformDb.DeviceModel.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
