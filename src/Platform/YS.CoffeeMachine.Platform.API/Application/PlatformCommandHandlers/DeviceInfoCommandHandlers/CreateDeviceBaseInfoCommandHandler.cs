using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 添加设备
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class CreateDeviceBaseInfoCommandHandler(IPublishService _capPublisher, IRedisService _redisService, IDeviceInfoRepository repository, CoffeeMachinePlatformDbContext _dbContext, UserHttpContext _user, IDeviceInfoQueries _deviceInfoQueries) : ICommandHandler<CreateDeviceBaseInfoCommand, string>
    {
        /// <summary>
        /// 添加设备
        /// </summary>
        public async Task<string> Handle(CreateDeviceBaseInfoCommand input, CancellationToken cancellationToken)
        {
            string machineStickerCode = string.Empty;
            var info = await _deviceInfoQueries.GetDeviceBaseInfoByBoxIdAsync(input.BoxId);
            var init = await _deviceInfoQueries.GetDeviceInitializationAsync(input.Mid);
            // 新设备绑定
            var device = await _dbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == input.Mid);

            if (init == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0013)]);
            if (init.IsBind || device != null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0014)]);

            if (info == null)
            {
                if (string.IsNullOrWhiteSpace(input.MachineStickerCode))
                {
                    var machineStickerCodes = await CreateMachineStickerCodes(1);
                    machineStickerCode = machineStickerCodes[0];
                }
                else
                {
                    machineStickerCode = input.MachineStickerCode;
                }
                var models = await _dbContext.DeviceModel.Where(x => input.ModelName.Contains(x.Key) || x.Key == "TCN-NCF-DEFAULT").ToListAsync();
                var modelId = models.Count > 1 ? models.Where(x => input.ModelName.Contains(x.Key)).Select(x => x.Id).FirstOrDefault()
                    : models.Where(x => x.Key == "TCN-NCF-DEFAULT").Select(x => x.Id).FirstOrDefault();
                var devicebase = new DeviceBaseInfo(input.Mid, machineStickerCode, input.BoxId, modelId);
                devicebase.Online();
                await _dbContext.DeviceBaseInfo.AddAsync(devicebase);
            }
            else
            {
                // 解绑后绑定
                if (!string.IsNullOrWhiteSpace(info.Mid))
                {
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0024)]);
                }
                info.BindMid(input.Mid);
                info.Online();
                machineStickerCode = info.MachineStickerCode;
            }
            init.UpdateBind(true);
            await _dbContext.SaveChangesAsync();
            await _capPublisher.SendMessage(CapConst.DeviceBind, new DownlinkEntity1100()
            {
                PMid = machineStickerCode,
                Mid = input.Mid,
                TransId = YitIdHelper.NextId().ToString(),
                PriKey = init.PriKey
            });
            return machineStickerCode;
        }
        private async Task<List<string>> CreateMachineStickerCodes(int num = 1)
        {
            var key = GenerateKey();
            var codes = new List<string>();
            for (int i = 0; i < num; i++)
                codes.Add(await _redisService.CreateMachineStickerCodesAsync(key));
            return codes;
        }
        private string GenerateKey()
        {
            var dt = DateTime.Now;
            var year = dt.Year.ToString();
            var week = Util.Core.Util.GetWeekOfMonth(dt);
            int m = dt.Month;
            var mon = m == 1 ? "11" : m == 2 ? "22" : dt.Month.ToString("D2");
            year = year.Substring(year.Length - 2);
            return week + mon + ((int)dt.DayOfWeek).ToString() + year;
        }
    }

    /// <summary>
    /// 解绑
    /// </summary>
    /// <param name="_capPublisher"></param>
    /// <param name="_redisService"></param>
    /// <param name="_dbContext"></param>
    /// <param name="_user"></param>
    public class UnBindCommandHandler(IPublishService _capPublisher, IRedisService _redisService, CoffeeMachinePlatformDbContext _dbContext, UserHttpContext _user) : ICommandHandler<UnBindCommand, bool>
    {
        /// <summary>
        /// 解绑
        /// </summary>
        public async Task<bool> Handle(UnBindCommand input, CancellationToken cancellationToken)
        {
            var device = await _dbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == input.Mid);
            if (device == null)
                return false;
            var deviceInit = await _dbContext.DeviceInitialization.FirstOrDefaultAsync(x => x.Mid == device.Mid);
            if (deviceInit == null)
                return false;
            device.UnBindMid();
            device.Offline();
            deviceInit.UpdateBind(false);
            await _redisService.DelKeyAsync(CacheConst.DeviceBaseKey, input.Mid);
            await _capPublisher.SendMessage(CapConst.GeneralSeed, new CommandDownSend()
            {
                Method = "6216",
                Mid = input.Mid,
                TransId = YitIdHelper.NextId().ToString(),
                Params = JsonConvert.SerializeObject(new DownlinkEntity6216()
                {
                    CapabilityId = (int)CapabilityIdEnum.Init,
                    Parameters = new List<string>()
                })
            });
            return true;
        }
    }
}
