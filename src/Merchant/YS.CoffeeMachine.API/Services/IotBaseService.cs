using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Subscribers.PaymentSubscribers;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Services
{
    /// <summary>
    /// 通讯服务调用接
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="_configuration"></param>
    public class IotBaseService(IHttpClientFactory httpClientFactory,
        IConfiguration _configuration, CoffeeMachinePlatformDbContext _coffeeMachinePlatformDb,
        IPublishService _capPublish, ILogger<IotBaseService> logger,
        CommonHelper _commonHelper)
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
            Console.WriteLine($"获取设备在线状态url:{_configuration["NotityUrl:Url"]}");
            var url = _configuration["NotityUrl:Url"] + "/api/Iot/IsOnline?mid={0}";
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);
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
        /// 获取设备信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceBaseInfo> GetBaseInfoByCodeAsync(string code)
        {
            return await _coffeeMachinePlatformDb.DeviceBaseInfo.Where(x => x.MachineStickerCode == code).FirstOrDefaultAsync();
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

        /// <summary>
        /// 检查设备是否在用户下
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<bool> CheckMid(string mid, long userId)
        {
            if (userId != 0 && !string.IsNullOrWhiteSpace(mid))
            {

                var query = from devicebase in _coffeeMachinePlatformDb.DeviceBaseInfo
                            join device in _coffeeMachinePlatformDb.DeviceInfo
                                on devicebase.Id equals device.DeviceBaseId into deviceGroup
                            from deviceInfo in deviceGroup.DefaultIfEmpty()
                            where devicebase.Mid == mid
                            select new
                            {
                                DeviceBaseId = devicebase.Id,
                                Mid = devicebase.Mid,
                                EnterpriseinfoId = deviceInfo != null ? deviceInfo.EnterpriseinfoId : 0,
                                DeviceId = deviceInfo.Id
                            };
                var result = await query.FirstOrDefaultAsync();
                //var enterpriseinfoId = result?.EnterpriseinfoId;
                //return tenantId == enterpriseinfoId;
                var deviceBindUsers = await _commonHelper.GetUserByDeviceId(new List<long>() { result.DeviceId });
                var users = deviceBindUsers[result.DeviceId].ToList();
                if (deviceBindUsers != null && users.Any(x => x == userId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 支付成功下发出货指令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> PaymentSuccessSend(CommandDownSend input)
        {
            // 检查设备是否在线
            if (await IsOnline(input.Mid))
            {
                // 下发出货指令
                await _capPublish.SendMessage(CapConst.GeneralSeed, input);
                return true;
            }
            else
            {
                // 设备不在线，记录日志或处理逻辑
                logger.LogError($"支付成功回调：设备 {input.Mid} 不在线，无法进行出货操作");
                return false;
            }
        }
    }
}