using AutoMapper;
using FreeRedis;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.AMP.Shared.Enum;
using YS.AMP.Shared.Request;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Extensions.Grpc.Aop;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Application.GRPC.DTO;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.Iot.Api.Iot.CommandHandler;
/// <summary>
/// 消息接收处理类
/// </summary>
[HeartbeatFilter]
public class CommandService(IRedisService _redis,
    IRedisClient _redisClient, IPublishService _capPublisher, DeviceService _deviceService,
    CoffeeMachinePlatformDbContext _platformDbContext,
   IMapper _map) : ServiceBase<IGrpcCommandService>, IGrpcCommandService
{
    #region 构造函数
    #endregion
    // public async UnaryResult<(bool Success, string ResponseId, YS.BaseShared.CommandEntities.UplinkEntity10030.Response Data)> Uplink2006HandlerAsync(UplinkEntity10030.Request request) => throw new NotImplementedException();

    /// <summary>
    /// TestPayOrder
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> TestPayOrder(MockWechatPaymentDto request)
    {
        //支持终止请求
        throw new NotImplementedException();
    }

    /// <summary>
    /// TestOrder
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<string> TestOrder(UplinkEntity8888.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// TestAsync
    /// </summary>
    /// <param name="vendNo"></param>
    /// <returns></returns>
    public async UnaryResult<string> TestAsync(string vendNo)
    {
        await Task.Delay(100);
        return vendNo + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    /// <summary>
    /// 获取初始化信息
    /// </summary>
    /// <param name="mid"></param>
    /// <returns></returns>
    public async UnaryResult<SecretInfoOutput> GetDeviceInitByMidAsync(string mid)
    {
        var key = string.Format(CacheConst.DeviceInitializationKey, mid);
        var initInfo = await _redisClient.GetAsync<SecretInfoOutput>(key);
        if (initInfo == null)
        {
            var initialization = await _platformDbContext.DeviceInitialization.FirstOrDefaultAsync(x => mid == x.Mid);
            if (initialization != null)
            {
                initInfo = _map.Map<SecretInfoOutput>(initialization);
                await _redisClient.SetAsync(key, initInfo, 7 * 24 * 60 * 60);
            }
        }
        return initInfo ?? new SecretInfoOutput();
    }

    /// <summary>
    /// 1000.VMC向服务器获取设备编号
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    public async UnaryResult<UplinkEntity1000.Response> Uplink1000HandleAsync(UplinkEntity1000.Request request)
    {
        var initinfo = await _platformDbContext.DeviceInitialization.FirstOrDefaultAsync(x => request.SN == x.EquipmentNumber);
        if (initinfo != null)
        {
            var device = await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == initinfo.Mid);
            return new UplinkEntity1000.Response
            {
                Mid = initinfo.Mid,
                PriKey = initinfo.PriKey,
                PMid = device?.MachineStickerCode ?? ""
            };
        }
        var mid = await _redis.GetMidAsync();
        var pkey = Util.Core.Util.CreateKey();
        await _platformDbContext.AddAsync(new DeviceInitialization(mid, request.SN, request.IMEI, pkey, request.PubKey, null));
        await _platformDbContext.SaveChangesAsync();
        return new UplinkEntity1000.Response
        {
            Mid = mid,
            PriKey = pkey,
        };
    }

    /// <summary>
    /// 1012.VMC向服务器上报指标
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    public async UnaryResult<bool> Uplink1012HandleAsync(UplinkEntity1012.Request request)
    {
        await _capPublisher.SendMessage(CapConst.MetricCap, request);
        return true;
    }

    /// <summary>
    /// 9022
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    public async UnaryResult<bool> Uplink9022HandleAsync(UplinkEntity9022.Request request)
    {
        var devicebase = await _deviceService.GetDeviceBaseInfoAsync(request.Mid);
        var device = await _platformDbContext.DeviceInfo.Include(x => x.DeviceUserAssociations).FirstOrDefaultAsync(x => x.DeviceBaseId == devicebase.Id);
        var olds = await _platformDbContext.DeviceMaterialInfo.Where(x => x.DeviceBaseId == devicebase.Id).ToListAsync();
        var warnings = await _platformDbContext.DeviceEarlyWarnings.Where(x => x.DeviceBaseId == devicebase.Id && x.WarningType == EarlyWarningTypeEnum.ShortageWarning).ToListAsync();
        var yjStr = string.Empty;
        var bhsublogs = new List<DeviceRestockLogSub>();
        foreach (var materialInfo in request.Details)
        {
            var type = materialInfo.Type;
            int i = 1;
            foreach (var cartridgeInfo in materialInfo.CartridgeInfos.OrderBy(x => x.Index))
            {
                long wlid = 0;
                var wlname = string.Empty;
                var oldvalue = 0;
                var value = 0;
                var newvalue = 0;
                // 物料信息
                var old = olds.FirstOrDefault(x => x.Type == (MaterialTypeEnum)type && x.Index == cartridgeInfo.Index);
                var warning = warnings.FirstOrDefault(x => x.DeviceMaterialId == old.Id);
                if (old == null)
                {
                    var id = YitIdHelper.NextId();

                    // 物料
                    await _platformDbContext.AddAsync(new DeviceMaterialInfo(id, devicebase.Id, (MaterialTypeEnum)type, i, cartridgeInfo.Name, cartridgeInfo.Capacity, cartridgeInfo.Stock));

                    // 物料预警信息
                    await _platformDbContext.AddAsync(new DeviceEarlyWarnings(devicebase.Id, EarlyWarningTypeEnum.ShortageWarning, false, cartridgeInfo.Warning.ToString(), id));

                    if (cartridgeInfo.Warning >= cartridgeInfo.Stock)
                    {
                        yjStr += $"{cartridgeInfo.Name}、";
                    }
                    wlid = id;
                    wlname = cartridgeInfo.Name;
                    oldvalue = 0;
                    value = cartridgeInfo.Stock-oldvalue;
                    newvalue = cartridgeInfo.Stock;
                }
                else
                {
                    wlid = old.Id;
                    wlname = old.Name;
                    oldvalue = old.Stock;
                    value = cartridgeInfo.Stock - oldvalue;
                    newvalue = cartridgeInfo.Stock;
                    old.Update(cartridgeInfo.Capacity, cartridgeInfo.Stock);
                    _platformDbContext.Update(old);
                    if (warning == null)
                    {
                        await _platformDbContext.AddAsync(new DeviceEarlyWarnings(devicebase.Id, EarlyWarningTypeEnum.ShortageWarning, false, cartridgeInfo.Warning.ToString(), old.Id));
                    }
                    else
                    {
                        warning.Update(warning.IsOn, cartridgeInfo.Warning.ToString());
                        warning.BindMaterial(old.Id);
                        _platformDbContext.Update(warning);
                    }
                    if (cartridgeInfo.Warning >= cartridgeInfo.Stock)
                    {
                        yjStr += $"{cartridgeInfo.Name}、";
                    }

                }
                i++;
                if (value != 0)
                    bhsublogs.Add(new DeviceRestockLogSub(HGTypeEnum.CoffeeMachine, wlid, wlname, oldvalue, value, newvalue));
            }
        }

        // 补货记录
        if (request.IsRestock && bhsublogs.Count > 0)
        {
            var log = new DeviceRestockLog(device.Id, device.Name, devicebase.MachineStickerCode, device
                .DetailedAddress, RestockTypeEnum.BD, device.EnterpriseinfoId);
            log.AddSubItem(bhsublogs);
            await _platformDbContext.AddAsync(log);
        }
        await _platformDbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 属性上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Uplink1013HandleAsync(UplinkEntity1013.Request request)
    {
        if (request.Attributes == null || !request.Attributes.Any())
            return false;

        await _capPublisher.SendMessage(CapConst.AttributeReporting, request);
        return true;
    }

    /// <summary>
    /// 能力上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async UnaryResult<bool> Uplink1008HandleAsync(UplinkEntity1008 request)
    {
        await _capPublisher.SendMessage(CapConst.AbilityReporting, request);
        return true;
    }

    /// <summary>
    /// 上报能力配置
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity1010.Response> Uplink1010HandleAsync(UplinkEntity1010.Request request)
    {
        await _capPublisher.SendMessage(CapConst.CapabilityConfigure, request);
        return new UplinkEntity1010.Response()
        {
            Mid = request.Mid,
            CapabilityType = request.CapabilityType,
            CapabilityConfigure = request.CapabilityConfigure.Select(i => new UplinkEntity1010.Response.ConfigureEntity
            {
                Id = i.Id
            }).ToList()
        };
    }

    /// <summary>
    /// 上报软件版本
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Uplink1205HandleAsync(UplinkEntity1205.Request request)
    {
        var devicebase = await _deviceService.GetDeviceBaseInfoAsync(request.Mid);
        var softs = await _platformDbContext.DeviceSoftwareInfo.Where(x => x.DeviceBaseId == devicebase.Id).ToListAsync();
        request.Releases.ForEach(release =>
        {
            // 程序版本id
            long softVersionId = 0;
            var soft = softs.FirstOrDefault(x => x.ProgramType == release.Type && x.Title == release.Title && x.Name == release.Name);
            if (soft != null)
            {
                soft.Update(release.VersionName, softVersionId, release.Version, release.Extra);
                _platformDbContext.Update(soft);
            }
            else
            {
                soft = new DeviceSoftwareInfo(devicebase.Id, release.Type, release.Title, release.Name, release.VersionName, softVersionId, release.Version, release.Extra);
                _platformDbContext.AddAsync(soft);
            }
        });
        await _platformDbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 下发能力配置回复
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [UpdateOperationLogFilter]
    public async UnaryResult<bool> Downlink1011Handle(DownlinkEntity1011.Response request)
    {
        return true;
    }

    /// <summary>
    /// Downlink6216HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async UnaryResult<bool> Downlink6216HandleAsync(DownlinkEntity6216.Response request)
    {
        if (request.Status != 0)
        {
            bool status = request.Status == 1 ? true : false;
            var msg = request.Status == 1 ? null : request.Description;
            await _deviceService.UpdateLogActionResult(status, msg, request.TransId, request.Mid);

            switch ((CapabilityIdEnum)request.CapabilityId)
            {
                case CapabilityIdEnum.LogUpload:
                    await _deviceService.LogUploadAsync(request.Output, request.Mid, request.TransId, request.Status, request.Description);
                    break;
                default:
                    break;
            }
        }
        return true;
    }

    /// <summary>
    /// Uplink5204HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Uplink5204HandleAsync(UplinkEntity5204.Request request)
    {
        return await _deviceService.ErrAsync(request);
    }

    /// <summary>
    /// Uplink5206HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Uplink5206HandleAsync(UplinkEntity5206.Request request)
    {
        return await _deviceService.UpdateErrStatusAsync(request.Mid);
    }

    /// <summary>
    /// VMC向服务器请求商品列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity5213.Response> Uplink5213HandleAsync(UplinkEntity5213.Request request)
    {
        return await _deviceService.SendGoodsAsync(request);
    }

    /// <summary>
    /// Uplink7212HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity7212.Response> Uplink7212HandleAsync(UplinkEntity7212.Request request)
    {
        return await _deviceService.UplinkOrderAsync(request);
    }

    /// <summary>
    /// Uplink9027HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity9027.Response> Uplink9027HandleAsync(UplinkEntity9027.Request request)
    {
        return await _deviceService.UplinkGoodAsync(request);
    }

    /// <summary>
    /// Downlink1203HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Downlink1203HandleAsync(DownlinkEntity1203.Response request)
    {
        //if (request.Status != 1)
        //{
        //    bool status = request.Status == 2 ? true : false;
        //    var msg = request.Status == 2 ? null : request.Description;

        //    //await _deviceService.UpdateLogActionResult(status, msg, request.TransId, request.Mid);
        //    await _deviceService.UpdateDeviceSJLogActionResult(status, msg, request.TransId, request.Mid);
        //}

        var deviceBase = await _deviceService.GetDeviceBaseInfoAsync(request.Mid);
        if (request.Status != 1)
        {
            bool status = request.Status == 2 ? true : false;
            var msg = request.Status == 2 ? "升级成功" : request.Description ?? "升级失败！";
            await _capPublisher.SendMessage(CapConst.SeedYYPSoftUpdate, new UpdateResultInput()
            {
                PlatformCode = "2",
                ProductionNumber = deviceBase.MachineStickerCode,
                TransId = request.TransId,
                UpdateType = status ? UpdateTypeEnum.UpdateSuccess : UpdateTypeEnum.UpdateError,
                UpdateTime = DateTime.Now,
                Message = msg
            });
        }
        return true;
    }

    /// <summary>
    /// Downlink3201HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<bool> Downlink3201HandleAsync(DownlinkEntity3201.Response request)
    {
        return true;
    }

    /// <summary>
    /// Uplink1204HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity1204.Response> Uplink1204HandleAsync(UplinkEntity1204.Request request)
    {
        return await _deviceService.GetReleases(request);
    }

    /// <summary>
    /// Uplink9030HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity9030.Response> Uplink9030HandleAsync(UplinkEntity9030.Request request)
    {
        return await _deviceService.IsOpenDoor(request);
    }

    /// <summary>
    /// 清洗部件上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity9031.Response> Uplink9031HandleAsync(UplinkEntity9031.Request request)
    {
        return await _deviceService.ReportFlush(request);
    }

    /// <summary>
    /// 清洗上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity9032.Response> Uplink9032HandleAsync(UplinkEntity9032.Request request)
    {
        return await _deviceService.ReportFlushLog(request);
    }

    /// <summary>
    /// Uplink4201HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity4201.Response> Uplink4201HandleAsync(UplinkEntity4201.Request request)
    {
        return await _deviceService.ShipmentResults(request);
    }

    /// <summary>
    /// 9033
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity9033.Response> Uplink9033HandleAsync(UplinkEntity9033.Request request)
    {
        return await _deviceService.FulshYj(request);
    }

    /// <summary>
    /// Uplink5205HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity5205.Response> Uplink5205HandleAsync(UplinkEntity5205.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink5207HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity5207.Response> Uplink5207HandleAsync(UplinkEntity5207.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Downlink6002HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<bool> Downlink6002HandleAsync(DownlinkEntity6002.Response request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink2000HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<bool> Uplink2000HandleAsync(UplinkEntity2000.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Downlink6200HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<bool> Downlink6200HandleAsync(DownlinkEntity6200.Response request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink1006HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<bool> Uplink1006HandleAsync(UplinkEntity1006.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink7200HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity7200.Response> Uplink7200HandleAsync(UplinkEntity7200.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink7201HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity7201.Response> Uplink7201HandleAsync(UplinkEntity7201.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink7202HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity7202.Response> Uplink7202HandleAsync(UplinkEntity7202.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink1014HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity1014.Response> Uplink1014HandleAsync(UplinkEntity1014.Request request)
    {
        return await _deviceService.GetCapabilityConfigureAsync(request);
    }

    /// <summary>
    /// Uplink7211HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async UnaryResult<UplinkEntity7211.Response> Uplink7211HandleAsync(UplinkEntity7211.Request request)
    {
        return await _deviceService.Request7211(request);
    }

    /// <summary>
    /// Uplink7221HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<bool> Uplink7221HandleAsync(UplinkEntity7221.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink5209HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity5209.Response> Uplink5209HandleAsync(UplinkEntity5209.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink1210HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity1210.Response> Uplink1210HandleAsync(UplinkEntity1210.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uplink7214HandleAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<UplinkEntity7214.Response> Uplink7214HandleAsync(UplinkEntity7214.Request request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// CommandHandleAsync
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public UnaryResult<string> CommandHandleAsync(string content)
    {
        throw new NotImplementedException();
    }
}
