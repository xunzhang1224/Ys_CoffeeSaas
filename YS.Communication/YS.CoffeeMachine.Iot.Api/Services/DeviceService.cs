using Aop.Api.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Net.Mail;
using Yitter.IdGenerator;
using YS.AMP.Shared.Enum;
using YS.AMP.Shared.Request;
using YS.AMP.Shared.Response;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.BasicDtos;
using YS.CoffeeMachine.Domain.Shared;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Dto;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
using YS.CoffeeMachine.Iot.Api.Extensions.Http;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using YS.CoffeeMachine.Provider.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static YS.CoffeeMachine.Iot.Domain.CommandEntities.UplinkEntity1014.Response;
using static YS.CoffeeMachine.Iot.Domain.CommandEntities.UplinkEntity9027.Request;
using DeviceInfo = YS.CoffeeMachine.Domain.AggregatesModel.Devices.DeviceInfo;

namespace YS.CoffeeMachine.Iot.Api.Services
{
    /// <summary>
    /// 设备服务
    /// </summary>
    /// <param name="_logger"></param>
    /// <param name="_redisService"></param>
    /// <param name="_platformDbContext"></param>
    /// <param name="_mapper"></param>
    /// <param name="_timeDb"></param>
    /// <param name="_publish"></param>
    public class DeviceService(ILogger<DeviceService> _logger,
        IRedisService _redisService,
        IConfiguration _cfg,
        CoffeeMachinePlatformDbContext _platformDbContext,
        IMapper _mapper, CommonHelper _commonHelper,
        CoffeeMachineTimescaleDBContext _timeDb,
        IPublishService _publish,
        HttpService _http
        )
    {

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DeviceBaseRedisDto> GetDeviceBaseInfoAsync(string mid)
        {
            var key = CacheConst.DeviceBaseKey;
            var baseCache = await _redisService.HGetAsync<DeviceBaseRedisDto>(key, mid);
            if (baseCache == null)
            {
                var deviceBase = await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == mid);
                if (deviceBase == null)
                    throw new Exception($"未找到mid为[{mid}]的设备");

                baseCache = _mapper.Map<DeviceBaseRedisDto>(deviceBase);
                await _redisService.HSetAsync(key, mid, baseCache, TimeSpan.FromDays(30));
            }
            return baseCache;
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<AmpPagedList<UploadMachineOutput>> GetUploadMachineAsync(FetchMachineInput input)
        {
            var deviceBases = _platformDbContext.DeviceBaseInfo;
            if (input.ProductionNumbers != null && input.ProductionNumbers.Any())
            {
                deviceBases.Where(deviceBase => input.ProductionNumbers.Contains(deviceBase.MachineStickerCode));
            }
            var devices = _platformDbContext.DeviceInfo;
            var softwares = _platformDbContext.DeviceSoftwareInfo;
            var deviceModels = _platformDbContext.DeviceModel;

            // 基础查询
            var baseQuery = deviceBases
                .Join(devices,
                    deviceBase => deviceBase.Id,
                    device => device.DeviceBaseId,
                    (deviceBase, device) => new { DeviceBase = deviceBase, Device = device })
                  .Join(deviceModels,
            combined => combined.Device.DeviceModelId,
            deviceModel => deviceModel.Id,
            (combined, deviceModel) => new { combined.DeviceBase, combined.Device, DeviceModel = deviceModel })
                .GroupJoin(softwares,
        combined => EF.Functions.Collate(combined.DeviceBase.Id.ToString(), "SQL_Latin1_General_CP1_CI_AS"),
        software => EF.Functions.Collate(software.DeviceBaseId.ToString(), "SQL_Latin1_General_CP1_CI_AS"),
        (combined, softwareList) => new
        {
            combined.DeviceBase,
            combined.Device,
            combined.DeviceModel,
            SoftwareList = softwareList
        });

            // 获取总数
            var total = await baseQuery.CountAsync();

            // 分页查询数据
            var items = await baseQuery
                .Select(combined => new UploadMachineOutput
                {
                    ProductionNumber = combined.DeviceBase.MachineStickerCode,
                    BoxId = combined.DeviceBase.BoxId,
                    MachineName = combined.Device.Name,
                    IsActive = combined.Device.DeviceActiveState == DeviceActiveEnum.Active,
                    ActiveTime = combined.Device.ActiveTime,
                    IsLine = combined.DeviceBase.IsOnline,
                    ModelCode = combined.Device.DeviceModel.Key,
                    MerchantId = combined.Device.EnterpriseDevices.FirstOrDefault().EnterpriseId.ToString(),
                    MerchantName = combined.Device.EnterpriseDevices.FirstOrDefault().Enterprise.Name,
                    ApplicationEnd = ApplicationEndEnum.Android,
                    BoxType = combined.DeviceModel.Name,
                    PlatformCode = "coffee",
                    Versions = combined.SoftwareList.Select(s => new VersionsItem
                    {
                        Title = s.Title,
                        Name = s.Name,
                        VersionName = s.VersionName ?? s.Version
                    }).ToList()
                })
                .Skip((input.Page - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToListAsync();

            // 计算分页信息
            var totalPages = (int)Math.Ceiling(total / (double)input.PageSize);
            var hasNextPage = input.Page < totalPages;
            var hasPrevPage = input.Page > 1;

            return new AmpPagedList<UploadMachineOutput>
            {
                Items = items,
                Page = input.Page,
                PageSize = input.PageSize,
                Total = total,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPrevPage = hasPrevPage
            };
        }

        /// <summary>
        /// 设置设备Metric
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> SetDeviceMetric(UplinkEntity1012.Request request)
        {
            var deviceBase = await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == request.Mid);

            if (request == null || deviceBase == null)
            {
                _logger.LogWarning($"SetDeviceMetric-{request.Mid}：机器信息查询失败；");
                return false;
            }
            var deviceMetris = await _platformDbContext.DeviceMetrics.Where(x => x.DeviceBaseId == deviceBase.Id).ToListAsync();
            foreach (var item in request.Metrics)
            {
                var key = (MetricTypeEnum)item.Metric;
                var metric = deviceMetris.FirstOrDefault(x => x.MetricType == key && x.CounterNo == item.CounterNo);
                if (metric != null)
                {
                    metric.Update(item.Index, item.Value, (MetricsStatusEnum)item.Status, item.Description, GetMetricsType(key));
                    _platformDbContext.DeviceMetrics.Update(metric);
                }
                else
                {
                    metric = new DeviceMetrics(deviceBase.Id, item.CounterNo, key, item.Index, item.Value, (MetricsStatusEnum)item.Status, item.Description, GetMetricsType(key));
                    await _platformDbContext.AddAsync(metric);
                }
                var model = await _platformDbContext.DeviceModel.FirstOrDefaultAsync(x => x.Id == deviceBase.DeviceModelId);
                var log = new DeviceMetricsLog(deviceBase.Id, request.Mid, "", model?.Name ?? "", item.CounterNo, key, item.Index, item.Value, (MetricsStatusEnum)item.Status, item.Description);
                await _timeDb.AddAsync(log);
            }
            await _platformDbContext.SaveChangesAsync();
            await _timeDb.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// GetMetricsType
        /// </summary>
        /// <param name="metricType"></param>
        /// <returns></returns>
        public int GetMetricsType(MetricTypeEnum metricType)
        {
            //int[] wls = { }; // 物料
            int[] lbjs = { 1, 4, 2, 20, 21, 22, 23, 24, 25, 26, 27, 28 }; // 关键零部件
            //if (wls.Any(x => x == (int)metricType))
            //    return 1;
            if (lbjs.Any(x => x == (int)metricType))
            {
                return 2;
            }
            else
                return 0;
        }

        /// <summary>
        /// SetDeviceOnline
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<bool> SetDeviceOnline(string mid)
        {
            var key = CacheConst.DeviceOnlineKey;
            try
            {
                var deviceBase = await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == mid);

                if (deviceBase == null || deviceBase == null)
                {
                    throw new Exception($"SetDeviceMetric-{mid}：机器信息查询失败；");
                }
                var dto = await _redisService.HGetAsync<OnlineDto?>(key, mid);
                if (dto == null || dto.Status == false)
                {
                    deviceBase.Online();
                    var model = await _platformDbContext.DeviceModel.FirstOrDefaultAsync(x => x.Id == deviceBase.DeviceModelId);
                    var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
                    _platformDbContext.Update(deviceBase);
                    await _platformDbContext.SaveChangesAsync();
                    var onlineLog = new DeviceOnlineLog(mid, device?.Name ?? "", deviceBase.Id, model?.Name ?? "", true, device?.EnterpriseinfoId ?? 0);
                    await _timeDb.AddAsync(onlineLog);
                    await _timeDb.SaveChangesAsync();
                    // 上线删除上一次存的离线预警时间
                    await _redisService.DelKeyAsync(CacheConst.OffOnlineTask);
                }
                var setdto = new OnlineDto()
                {
                    Mid = mid,
                    Status = true,
                    OnDate = DateTime.UtcNow,
                };
                await _redisService.HSetAsync(key, mid, setdto, TimeSpan.FromMinutes(2));
            }
            catch (Exception ex)
            {
                _logger.LogError($"SetDeviceMetric-{mid}失败！，原因{ex.Message}；");
            }

            return true;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetDeviceAttribute(UplinkEntity1013.Request input)
        {
            var devicebase = await GetDeviceBaseInfoAsync(input.Mid);
            var existingAttrs = await _platformDbContext.DeviceAttribute.Where(x => x.DeviceBaseId == devicebase.Id).ToDictionaryAsync(x => x.Key);

            foreach (var item in input.Attributes)
            {
                if (existingAttrs.TryGetValue(item.Key, out var existingAttr))
                {
                    existingAttr.Update(item.Value);
                    _platformDbContext.Update(existingAttr);
                }
                else
                {
                    var attribute = new DeviceAttribute(devicebase.Id, item.Key, item.Name, item.Value);
                    await _platformDbContext.DeviceAttribute.AddAsync(attribute);
                }
            }
            await _platformDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 设置能力
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetDeviceAbility(UplinkEntity1008 input)
        {
            var device = await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == input.Mid);
            if (device != null)
            {
                device.SetAbility(input.HardwareCapability, input.SoftwareCapability);
                _platformDbContext.Update(device);
                await _platformDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 设置能力
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetCapabilityConfigure(UplinkEntity1010.Request input)
        {
            var devicebase = await GetDeviceBaseInfoAsync(input.Mid);
            if (devicebase != null)
            {
                var cfgs = await _platformDbContext.DeviceCapacityCfg
                    .Where(x => x.DeviceBaseId == devicebase.Id && x.CapacityType == (CapacityTypeEnum)input.CapabilityType)
                    .ToDictionaryAsync(x => x.CapacityId);

                foreach (var item in input.CapabilityConfigure)
                {
                    if (cfgs.TryGetValue((CapabilityIdEnum)item.Id, out var cfg))
                    {
                        cfg.Update(item.Content?.ToString(), (PremissionTypeEnum)item.Premission, (StructureTypeEnum)item.Structure);
                        _platformDbContext.Update(cfg);
                    }
                    else
                    {
                        var cfginfo = new DeviceCapacityCfg(devicebase.Id, (CapabilityIdEnum)item.Id, item.Name,
                            (CapacityTypeEnum)input.CapabilityType, item.Content?.ToString(), (PremissionTypeEnum)item.Premission,
                            (StructureTypeEnum)item.Structure);
                        await _platformDbContext.AddAsync(cfginfo);
                    }

                    // 修改料盒名或新建物料料盒
                    if (item.Id == (int)CapabilityIdEnum.BoxName && !string.IsNullOrWhiteSpace(item.Content))
                    {
                        // 最新的料盒名
                        var lhs = JsonConvert.DeserializeObject<List<LhDto>>(item.Content);

                        // 已有料盒
                        var olds = await _platformDbContext.DeviceMaterialInfo.Where(x => x.DeviceBaseId == devicebase.Id && x.Type == MaterialTypeEnum.Cassette).ToListAsync();

                        if (olds != null && olds.Any())
                        {
                            for (int i = 0; i < lhs.Count; i++)
                            {
                                var index = i + 1;
                                bool issugar = lhs[i].IsSugar;
                                var old = olds.FirstOrDefault(x => x.Index == index);
                                if (old == null)
                                {
                                    await _platformDbContext.AddAsync(new DeviceMaterialInfo(devicebase.Id, MaterialTypeEnum.Cassette, index, lhs[i].Name, issugar));
                                }
                                else
                                {
                                    old.UpdateName(lhs[i].Name);

                                    old.UpdateIsSugar(issugar);
                                    _platformDbContext.Update(old);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < lhs.Count; i++)
                            {
                                await _platformDbContext.AddAsync(new DeviceMaterialInfo(devicebase.Id, MaterialTypeEnum.Cassette, i + 1, lhs[i].Name, lhs[i].IsSugar));
                            }
                        }
                    }
                }
                await _platformDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 修改操作日志
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="isSuccess"></param>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task UpdateLogActionResult(bool isSuccess, string exception, string code, string mid)
        {
            var input = new UpdateOperationLogInput()
            {
                Code = code,
                Mid = mid,
                OperationResult = isSuccess ? OperationResultEnum.CommandExecuted : OperationResultEnum.CommandUnexecuted,
                ErrorMsg = isSuccess ? "" : exception
            };
            await _publish.SendMessage(CapConst.UpdateOperationLog, input);
        }

        /// <summary>
        /// 修改升级日志
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="isSuccess"></param>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task UpdateDeviceSJLogActionResult(bool isSuccess, string exception, string code, string mid)
        {
            var deviceVsersionUpdateRecord = await _platformDbContext.DeviceVsersionUpdateRecord.FirstOrDefaultAsync(x => x.Id.ToString() == code);
            if (deviceVsersionUpdateRecord != null)
            {
                deviceVsersionUpdateRecord.Update(isSuccess ? VersionPushStateEnum.PushSuccess : VersionPushStateEnum.PushFail, exception);
                _platformDbContext.Update(deviceVsersionUpdateRecord);
                await _platformDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 1014.VMC向服务器询问能力配置
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="isSuccess"></param>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<UplinkEntity1014.Response> GetCapabilityConfigureAsync(UplinkEntity1014.Request request)
        {
            var devicebase = await GetDeviceBaseInfoAsync(request.Mid);
            var CapabilityConfigureIds = request.CapabilityConfigure.Select(x => x.Id).ToList();
            var CapabilityConfigures = await _platformDbContext.DeviceCapacityCfg
                .Where(x => CapabilityConfigureIds.Contains((int)x.CapacityId) && x.DeviceBaseId == devicebase.Id && x.CapacityType == (CapacityTypeEnum)request.CapabilityType)
                .ToListAsync();
            var rsp = new UplinkEntity1014.Response()
            {
                Mid = request.Mid,
                TransId = request.TransId,
                CapabilityType = request.CapabilityType,
                CapabilityConfigure = new List<ConfigureEntity>()
            };
            foreach (var capabilityId in CapabilityConfigureIds)
            {
                var info = CapabilityConfigures?.FirstOrDefault(x => x.CapacityId == (CapabilityIdEnum)capabilityId);
                rsp.CapabilityConfigure.Add(new ConfigureEntity()
                {
                    Id = capabilityId,
                    Name = info?.Name ?? "",
                    Content = info?.CfgInfo ?? "",
                    Premission = info == null ? 0 : (int)info.Premission,
                    Structure = info == null ? 0 : (int)info.Structure
                });
            }
            return rsp;
        }

        /// <summary>
        /// 上报日志回调
        /// </summary>
        /// <param name="logUrl"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task LogUploadAsync(string logUrl, string mid, string transId, int status, string? description = null)
        {
            var file = await _platformDbContext.FileCenter.FirstOrDefaultAsync(x => x.Code == transId);
            if (file != null)
            {
                file.Update(status == 1 ? FileStateEnum.Success : FileStateEnum.Fail, description, logUrl);
                _platformDbContext.FileCenter.Update(file);
                await _platformDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 记录文件中心
        /// </summary>
        /// <param name="logUrl"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task LogUploadFileCenterAsync(DownlinkEntity6216 request)
        {
            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var param = request.Parameters[0];
            var logdto = JsonConvert.DeserializeObject<DeviceLogDto>(param);
            var stime = YS.Util.Core.Util.UnixMillisecondsToDateTime(logdto.StartTimestamp).ToString("yyyyMMddHHmmss");
            var dtime = YS.Util.Core.Util.UnixMillisecondsToDateTime(logdto.EndTimestamp).ToString("yyyyMMddHHmmss");
            var logname = $"{deviceBase.MachineStickerCode}日志_{stime}-{dtime}";
            var tenantId = device?.EnterpriseinfoId ?? 0;
            var log = new FileCenter(logname, request.TransId, DocmentTypeEnum.LogUpload, tenantId, SysMenuTypeEnum.Platform);
            await _platformDbContext.AddAsync(log);
            await _platformDbContext.SaveChangesAsync();
        }

        private string GetAddress(DeviceInfo device)
        {
            return $"{device.Province}{device.City}{device.District}{device.Street}{device.DetailedAddress}";
        }

        /// <summary>
        /// 上报异常
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> ErrAsync(UplinkEntity5204.Request request)
        {
            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.AsNoTracking().Include(i => i.DeviceModel).FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var enterprise = await _platformDbContext.EnterpriseInfo.FirstOrDefaultAsync(x => x.Id == device.EnterpriseinfoId);
            var deviceBindUsers = device == null ? null : await _commonHelper.GetUserByDeviceId(new List<long>() { device.Id });
            var errs = await _platformDbContext.DeviceAbnormal.Where(x => x.DeviceBaseId == deviceBase.Id).ToListAsync();
            string errStr = string.Empty;
            var codes = request.Info.Select(x => "Error_" + x.Code);
            var gaultCodeEntities = await _platformDbContext.FaultCodeEntitie.Where(x => codes.Contains(x.LanCode)).ToListAsync();
            var istz = false;
            foreach (var item in request.Info)
            {
                if (!errs.Any(x => x.Code == item.Code && x.Status == false))
                {
                    var code = "Error_" + item.Code;
                    var model = device == null ? null : await _platformDbContext.DeviceModel.FirstOrDefaultAsync(x => x.Id == device.DeviceModelId);
                    var err = new DeviceAbnormal(deviceBase.Id, request.TransId, request.CounterNo, item.Slot, code, item.Desc, request.CodeType, device?.Name, model?.Name ?? "", device?.EnterpriseinfoId ?? 0);
                    // 设置当时的设备及企业信息
                    err.SetEDBaseInfo(device.EnterpriseinfoId, enterprise != null ? enterprise.Name : string.Empty, device.Id, device.Name, model?.Id ?? 0, model?.Name ?? "");
                    await _platformDbContext.AddAsync(err);
                    errStr += gaultCodeEntities?.FirstOrDefault(x => x.LanCode == code)?.Name ?? code;
                    istz = true;
                }
            }
            await _platformDbContext.SaveChangesAsync();
            // 异常通知
            if (deviceBindUsers != null && deviceBindUsers.Any() && istz)
            {
                var notificationConfig = await GetNotificationConfigAsync(device, deviceBindUsers);
                if (!notificationConfig.HasAnyNotification)
                {
                    _logger.LogWarning("未配置需要通知的短信或邮件地址！");
                    return false;
                }
                var templates = await GetNotificationTemplatesAsync(device.EnterpriseinfoId, notificationConfig, TemplateEnum.ErrTemplate, TemplateEnum.EmailErrSubject);
                if (templates == null)
                {
                    _logger.LogWarning("未配置获取模板的相关信息！");
                    return false;
                }
                var time = await _commonHelper.GetDateTimeByEnterprise(device.EnterpriseinfoId, DateTime.UtcNow);
                var isCreateNotityMsg = false;
                var emailMsg = string.Format(templates.Template, GetAddress(device) ?? "未知", device.Name, deviceBase.MachineStickerCode, time, errStr);
                if (notificationConfig.Emails?.Any() == true)
                {
                    //var validEmails = new List<string>();

                    //foreach (var email in notificationConfig.Emails)
                    //{
                    //    var emailCacheKey = GetEmailCacheKey(device.Id.ToString(), email + errStr);
                    //    if (await CanSendNotificationAsync(emailCacheKey, "Email"))
                    //    {
                    //        validEmails.Add(email);
                    //    }
                    //}

                    //if (validEmails.Count > 0)
                    //{
                        await SendEmailNotificationAsync(device, notificationConfig.Emails, emailMsg, templates.Subject);
                        isCreateNotityMsg = true;
                    //}
                }

                if (notificationConfig.PhoneNumbers.Count > 0)
                {
                    //var validSmss = new List<string>();
                    var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device) ?? "未知", device_code = deviceBase.MachineStickerCode, error_code = errStr, detection_time = time, advice = "无" });
                    var SmsMessageDic = new Dictionary<string, string>() { { SmsConst.SmsErrTemplate, smsMessage } };
                    //foreach (var sms in notificationConfig.PhoneNumbers)
                    //{
                    //    var emailCacheKey = GetEmailCacheKey(device.Id.ToString(), sms + errStr);
                    //    if (await CanSendNotificationAsync(emailCacheKey, "Email"))
                    //    {
                            //validSmss.Add(sms);
                    //    }
                    //}

                    //if (validSmss.Count > 0)
                    //{
                        await SendSmsNotificationAsync(device, notificationConfig.PhoneNumbers, SmsMessageDic, templates.Subject);
                        isCreateNotityMsg = true;
                    //}
                }

                if (isCreateNotityMsg)
                {
                    var notifyMessages = notificationConfig.Accounts
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => new CreateNotityMsgDto()
                        {
                            DeviceId = device.Id,
                            MsgName = templates.Subject,
                            Type = 1,
                            Account = x,
                            Msg = emailMsg,
                            EnterpriseinfoId = device.EnterpriseinfoId
                        })
                        .ToList();
                    await _publish.SendMessage(CapConst.CreateNotityMsg, notifyMessages);
                }
            }
            return true;
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<DeviceBaseInfo> GetDeviceBaseAsync(string deviceCode)
        {
            return await _platformDbContext.DeviceBaseInfo.FirstOrDefaultAsync(x => x.MachineStickerCode == deviceCode);
        }

        /// <summary>
        /// 上报异常
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateErrStatusAsync(string mid)
        {
            var deviceBase = await GetDeviceBaseInfoAsync(mid);
            var errs = await _platformDbContext.DeviceAbnormal.Where(x => x.DeviceBaseId == deviceBase.Id && x.Status == false).ToListAsync();
            if (errs != null && errs.Any())
            {
                foreach (var item in errs)
                {
                    item.Restore();
                    _platformDbContext.Update(item);
                }
                await _platformDbContext.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// 上报商品
        /// </summary>
        /// <returns></returns>
        public async Task<UplinkEntity9027.Response> UplinkGoodAsync(UplinkEntity9027.Request request)
        {
            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var deviceinfo = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var rsp = new UplinkEntity9027.Response()
            {
                Mid = request.Mid,
                Skus = new List<string>()
            };
            if (deviceinfo == null)
                return rsp;
            if (request.CoffeeInfo != null && request.CoffeeInfo.Any())
            {
                foreach (var good in request.CoffeeInfo)
                {
                    if (!request.IsDel)
                    {
                        var forecastQuantity = await CalculateCupQuantity(good.Recipe);
                        if (string.IsNullOrWhiteSpace(good.Sku))
                        {
                            var id = YitIdHelper.NextId();
                            rsp.Skus.Add(id.ToString());
                            var beverageInfo = new BeverageInfo(deviceinfo.Id, good.Name, good.ImageUrl, good.Cold == 0 ? TemperatureEnum.Low : TemperatureEnum.High, null, good.Spec.ToString()
                                , forecastQuantity, false, false, id, null, good.Price, good.DiscountedPrice, productCategoryIds: good.ProductCategoryIds);
                            beverageInfo.UpdateCodeIsShow(true);
                            beverageInfo.SetSellStradgy(JsonConvert.SerializeObject(good.SellStradgy));
                            foreach (var item in good.Recipe)
                            {
                                item.MaterialBoxid = item.FormulaType == (int)FormulaTypeEnum.Lh ? item.MaterialBoxid : 1;
                                var pf = new FormulaInfo(id, item.MaterialBoxid, item.MaterialBoxName, item.Sort, (FormulaTypeEnum)item.FormulaType, item.Msg);
                                beverageInfo.AddFormulaInfo(pf);
                            }
                            await _platformDbContext.AddAsync(beverageInfo);
                        }
                        else
                        {
                            var id = YitIdHelper.NextId();
                            rsp.Skus.Add(id.ToString());
                            var beverageInfo = await _platformDbContext.BeverageInfo.Include(x => x.FormulaInfos).FirstOrDefaultAsync(x => x.Code == good.Sku && x.DeviceId == deviceinfo.Id);
                            if (beverageInfo == null)
                            {
                                var insert = new BeverageInfo(deviceinfo.Id, good.Name, good.ImageUrl, (TemperatureEnum)good.Cold, null, good.Spec.ToString()
                                    , forecastQuantity, false, false, id, null, good.Price, good.DiscountedPrice, good.Sku);
                                insert.UpdateCodeIsShow(true);
                                insert.SetSellStradgy(JsonConvert.SerializeObject(good.SellStradgy));
                                foreach (var item in good.Recipe)
                                {
                                    item.MaterialBoxid = item.FormulaType == (int)FormulaTypeEnum.Lh ? item.MaterialBoxid : 1;
                                    var pf = new FormulaInfo(id, item.MaterialBoxid, item.MaterialBoxName, item.Sort, (FormulaTypeEnum)item.FormulaType, item.Msg);
                                    insert.AddFormulaInfo(pf);
                                }
                                await _platformDbContext.AddAsync(insert);
                            }
                            else
                            {
                                beverageInfo.Update(good.Name, good.ImageUrl, (TemperatureEnum)good.Cold, good.Spec, good.Price, good.DiscountedPrice, forecastQuantity, good.ProductCategoryIds);
                                beverageInfo.SetSellStradgy(JsonConvert.SerializeObject(good.SellStradgy));
                                beverageInfo.ClearFormulaInfos();
                                beverageInfo.UpdateCodeIsShow(true);
                                foreach (var item in good.Recipe)
                                {
                                    item.MaterialBoxid = item.FormulaType == (int)FormulaTypeEnum.Lh ? item.MaterialBoxid : 1;
                                    var pf = new FormulaInfo(beverageInfo.Id, item.MaterialBoxid, item.MaterialBoxName, item.Sort, (FormulaTypeEnum)item.FormulaType, item.Msg);
                                    beverageInfo.AddFormulaInfo(pf);
                                }
                                _platformDbContext.Update(beverageInfo);
                            }
                        }
                    }
                    else
                    {
                        if (good.Sku != null && deviceinfo != null)
                        {
                            var beverageInfo = await _platformDbContext.BeverageInfo
                            .Include(b => b.BeverageVersions) // 加载关联的 BeverageVersion
                                    .FirstOrDefaultAsync(b => b.Code == good.Sku && b.DeviceId == deviceinfo.Id);
                            if (beverageInfo != null)
                            {
                                beverageInfo.AddCodeNotId(beverageInfo.Id.ToString());
                                beverageInfo.IsDelete = true;
                                _platformDbContext.BeverageInfo.Update(beverageInfo);
                            }
                        }
                    }
                }
                await _platformDbContext.SaveChangesAsync();
            }
            return rsp;
        }

        /// <summary>
        /// 计算预计出杯量
        /// </summary>
        /// <returns></returns>
        public async Task<double> CalculateCupQuantity(List<RecipeInfo> recipe)
        {
            double totalWaterAmount = 0;
            foreach (var item in recipe)
            {
                // 解析JSON字符串为JObject
                JObject jsonObj = JObject.Parse(item.Msg);
                if (/*(FormulaTypeEnum)item.FormulaType != FormulaTypeEnum.Ice && */jsonObj["WaterAmount"] != null)
                {
                    double amount = (double)jsonObj["WaterAmount"];
                    totalWaterAmount += amount;
                }
            }
            return totalWaterAmount;
        }

        /// <summary>
        /// VMC向服务器请求商品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity5213.Response> SendGoodsAsync(UplinkEntity5213.Request request)
        {
            var response = new UplinkEntity5213.Response
            {
                TransId = request.TransId,
                Mid = request.Mid,
                GoodsList = Array.Empty<UplinkEntity5213.Response.Goods>(),
                PageNo = request.PageNo,
                PageCount = request.PageCount,
            };

            try
            {
                var vendInfo = await GetDeviceBaseInfoAsync(request.Mid);
                var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == vendInfo.Id);
                if (device == null)
                {
                    _logger.LogInformation($"设备id：{vendInfo.Id},mid{request.Mid};未绑定租户！");
                    return response;
                }
                var result = await _platformDbContext.BeverageInfo.Include(x => x.FormulaInfos).Where(x => x.DeviceId == device.Id).ToListAsync();

                if (result == null)
                {
                    _logger.LogInformation($"设备id：{vendInfo.Id},mid{request.Mid};没有配置饮品！");
                    return response;
                }

                response.GoodsList = result.Select(i => new UplinkEntity5213.Response.Goods
                {
                    Id = i.Id,
                    Type = "",
                    Barcode = i.Code,
                    Name = i.Name,
                    Price = i.Price ?? 0,
                    ImagePath = i.BeverageIcon,
                    Desc = i.Remarks,
                    Spec = i.ForecastQuantity.ToString(),
                    InPrice = 0,
                    DiscountedPrice = i.DiscountedPrice,
                    Cold = (int)i.Temperature,
                    Recipe = i.FormulaInfos.Select(j => new UplinkEntity5213.Response.RecipeInfo
                    {
                        Msg = JsonConvert.SerializeObject(j.Specs),
                        BoxNum = (int)j.FormulaType
                    }).ToList(),
                    SellStradgy = string.IsNullOrWhiteSpace(i.SellStradgy) ? null : JsonConvert.DeserializeObject<UplinkEntity5213.Response.SellStradgyInfo>(i.SellStradgy),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Uplink5213Handler-{request.Mid}");
            }

            return response;
        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity7212.Response> UplinkOrderAsync(UplinkEntity7212.Request request)
        {
            var response = new UplinkEntity7212.Response
            {
                BizNo = request.BizNo,
                Status = 1,
                Mid = request.Mid,
                OrderNo = string.Empty,
            };
            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var deviceinfo = await _platformDbContext.DeviceInfo.AsNoTracking().Include(i => i.DeviceModel).FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var deviceMaterials = await _platformDbContext.DeviceMaterialInfo.Where(x => x.DeviceBaseId == deviceBase.Id).ToListAsync();
            if (deviceinfo == null)
            {
                _logger.LogError($"设备id：{deviceinfo.Id},mid{request.Mid};未绑定租户！");
                return response;
            }
            var enterprise = await _platformDbContext.EnterpriseInfo.FirstOrDefaultAsync(x => x.Id == deviceinfo.EnterpriseinfoId);
            var codes = request.Details.Select(d => d.ItemCode).ToList();
            // 获取对应的饮品信息
            var beverageInfos = await _platformDbContext.BeverageInfo.AsQueryable().AsNoTracking()
                .Include(i => i.FormulaInfos)
                .Include(i => i.DeviceInfo)
                .Where(b => codes.Contains(b.Code) && b.DeviceId == deviceinfo.Id)
                .ToListAsync();

            // 当前饮品的物料盒信息
            var materialBoxs = await _platformDbContext.DeviceMaterialInfo.Where(w => w.DeviceBaseId == deviceBase.Id && w.Type == MaterialTypeEnum.Cassette).ToListAsync();

            var orderDetails = new List<OrderDetails>();
            var shipmentResult = 0;
            foreach (var item in request.Details)
            {
                //获取当前的饮品信息
                var beverageInfo = beverageInfos.FirstOrDefault(b => b.Code == item.ItemCode);
                var orderDetaliMaterials = new List<OrderDetaliMaterial>();
                if (item.Materials != null && item.Materials.Any())
                {
                    foreach (var material in item.Materials)
                    {
                        var deviceMaterial = deviceMaterials.FirstOrDefault(x => x.DeviceBaseId == deviceBase.Id && x.Type == (MaterialTypeEnum)material.Type && x.Index == material.Index);
                        if (deviceMaterial != null)
                        {
                            orderDetaliMaterials.Add(new OrderDetaliMaterial(deviceMaterial.Id, material.Value));
                        }
                    }
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                var beverageInfoDto = _mapper.Map<OrderBeverageInfoDto>(beverageInfo);

                var categorys = await _platformDbContext.ProductCategory.AsNoTracking()
                    .Where(w => beverageInfoDto.CategoryIds != null && beverageInfoDto.CategoryIds.Contains(w.Id) && w.ProductCategoryType == ProductCategoryTypeEnum.Beverage)
                    .ToListAsync();

                if (categorys != null && categorys.Any())
                    beverageInfoDto.CategoryName = categorys.Select(s => s.Name).ToList();

                // 设置物料盒名称
                if (beverageInfoDto.FormulaInfos != null && beverageInfoDto.FormulaInfos.Any())
                {
                    foreach (var formula in beverageInfoDto.FormulaInfos.Where(w => w.FormulaType == FormulaTypeEnum.Lh))
                    {
                        // 当前饮品的物料盒信息
                        formula.MaterialBoxName = materialBoxs.FirstOrDefault(w => w.Index == formula.MaterialBoxId)?.Name ?? string.Empty;
                    }
                }

                var beverageInfoJson = beverageInfoDto == null ? string.Empty : JsonConvert.SerializeObject(beverageInfoDto, settings);
                orderDetails.Add(new OrderDetails(item.CounterNo, item.SlotNo, item.ItemCode, beverageInfo?.Name!, item.Price, item.Quantity, item.Delivery.Result, item.Delivery.Error, item.Delivery.ErrorDescription, beverageInfo.BeverageIcon, item.Delivery.ActionTimeSp, beverageInfoJson, orderDetaliMaterials));
                if (item.Delivery.Result == 1)
                    shipmentResult++;
            }
            var orderno = await CreateOrderNoAsync();

            // 获取货币信息
            var currencyInfo = await _platformDbContext.Currency.FirstOrDefaultAsync(w => w.Code == request.Payment.CurrencyCode);
            var currencySymbol = currencyInfo == null ? string.Empty : currencyInfo.CurrencySymbol;

            var order = new OrderInfo(request.IsOrder ? OrderTypeEnum.AndroidOrder : OrderTypeEnum.Not, deviceBase.Id, request.BizNo, request.Payment.Amount, request.Payment.Provider, request.Payment.PayTimeSp, request.Payment.CurrencyCode, currencySymbol, deviceinfo.EnterpriseinfoId, orderno, orderDetails);
            order.SetEDBaseInfo(deviceinfo.EnterpriseinfoId, enterprise != null ? enterprise.Name : string.Empty, deviceinfo.Id, deviceinfo.Name, deviceinfo.DeviceModelId ?? 0, deviceinfo.DeviceModel.Name);
            if (shipmentResult == 0)
                order.SetShipmentResult(OrderShipmentResult.Fail);
            else if (shipmentResult == orderDetails.Count)
                order.SetShipmentResult(OrderShipmentResult.Success);
            else
                order.SetShipmentResult(OrderShipmentResult.Part);
            order.SetSaleResult(OrderSaleResult.Success);
            await _platformDbContext.AddAsync(order);
            await _platformDbContext.SaveChangesAsync();
            response.OrderNo = orderno.ToString();
            return response;
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateOrderNoAsync()
        {
            var key = string.Format(CacheConst.DeviceOrderCode, DateTime.Now.ToString("yyyyMMddHHmm"));
            //var no = await _redisService.GetIncrCodeAsync(key);
            //var i = Util.Core.Util.GetRandomNext();
            //return key + i + no;
            return key + YitIdHelper.NextId();
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity1204.Response> GetReleases(UplinkEntity1204.Request request)
        {
            var releasesName = request.Releases.Select(x => x.Name);
            var deviceVersions = await _platformDbContext.DeviceVersionManage.Where(x => releasesName.Contains(x.ProgramTypeName) && x.Enabled == EnabledEnum.Enable).ToListAsync();
            var rsp = new UplinkEntity1204.Response()
            {
                Mid = request.Mid,
            };
            var releases = new List<UplinkEntity1204.Response.Release>();
            foreach (var item in request.Releases)
            {
                var package = deviceVersions.Where(m => m.ProgramTypeName == item.Name && m.ProgramType == item.Type).OrderByDescending(m => m.CreateTime).FirstOrDefault();
                if (package != null)
                {
                    var release = new UplinkEntity1204.Response.Release()
                    {
                        Name = item.Name,
                        Type = package.ProgramType ?? 0,
                        VersionName = package.Name,
                        Description = package.Remark,
                        DownLoadUrl = package.Url,
                        VersionType = (int)package.VersionType,
                    };
                    releases.Add(release);
                }
            }
            if (releases.Any())
            {
                rsp.Releases = releases;
            }
            return rsp;
        }

        /// <summary>
        /// 饮品制作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity9030.Response> IsOpenDoor(UplinkEntity9030.Request request)
        {
            var response = new UplinkEntity9030.Response
            {
                Mid = request.Mid,
            };

            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var info = await _platformDbContext.CardInfo.Include(x => x.Assignments.Where(x => x.DeviceId == device.Id)).FirstOrDefaultAsync(x => x.CardNumber == request.CardNumber && x.IsEnabled == true);
            if (info != null && info.Assignments != null && info.Assignments.Any())
                response.IsOpenDoor = true;
            else
                response.IsOpenDoor = false;
            return response;
        }

        /// <summary>
        /// 清洗部件上报
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity9031.Response> ReportFlush(UplinkEntity9031.Request request)
        {
            var response = new UplinkEntity9031.Response
            {
                Mid = request.Mid,
            };

            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            var olds = await _platformDbContext.DeviceFlushComponents.Where(x => x.DeviceBaseId == deviceBase.Id).ToListAsync();
            foreach (var item in request.Equipments)
            {
                foreach (var sub in item.Subs)
                {
                    var old = olds.FirstOrDefault(x => x.Type == (FlushComponentTypeEnum)item.Type && x.Index == sub.Index);
                    if (old == null)
                    {
                        var deviceFlushComponents = new DeviceFlushComponents(deviceBase.Id, (FlushComponentTypeEnum)item.Type, sub.Index, sub.Name);
                        await _platformDbContext.AddAsync(deviceFlushComponents);
                    }
                    else
                    {
                        old.Update(sub.Name);
                        _platformDbContext.Update(old);
                    }
                }

            }
            await _platformDbContext.SaveChangesAsync();
            return response;
        }

        /// <summary>
        /// 清洗记录上报
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity9032.Response> ReportFlushLog(UplinkEntity9032.Request request)
        {
            var response = new UplinkEntity9032.Response
            {
                Mid = request.Mid,
            };

            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);

            var log = new DeviceFlushComponentsLog(deviceBase.Id, deviceBase.MachineStickerCode, device.Name, (FlushComponentTypeEnum)request.FlushType, request.Type, request.Parts, request.Status, device.EnterpriseinfoId);
            await _platformDbContext.AddAsync(log);
            await _platformDbContext.SaveChangesAsync();
            return response;
        }

        /// <summary>
        /// 需要通知的短信和邮件地址
        /// </summary>
        /// <param name="device"></param>
        /// <param name="deviceBindUsers"></param>
        /// <returns></returns>
        private async Task<NotificationConfig> GetNotificationConfigAsync(DeviceInfo device, Dictionary<long, List<long>> deviceBindUsers)
        {
            deviceBindUsers ??= new Dictionary<long, List<long>>();
            var bindUsers = deviceBindUsers.GetValueOrDefault(device.Id) ?? new List<long>();
            var noticeConfigs = await _platformDbContext.NoticeCfg
                .Where(x => x.Type == 0 &&
                           x.EnterpriseinfoId == device.EnterpriseinfoId &&
                           x.Status)
                .Select(x => new { x.UserId, x.Method })
                .ToListAsync();

            var config = new NotificationConfig();

            if (!noticeConfigs.Any() || !bindUsers.Any())
            {
                var superAdmins = await _platformDbContext.ApplicationUser
                    .Where(x => x.AccountType == AccountTypeEnum.SuperAdmin &&
                               x.EnterpriseId == device.EnterpriseinfoId)
                    .Select(x => new { x.Email, x.Phone, x.Account })
                    .ToListAsync();

                config.Emails = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Email))
                                          .Select(x => x.Email)
                                          .ToList();
                config.PhoneNumbers = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Phone))
                                                .Select(x => x.Phone)
                                                .ToList();
                config.Accounts = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Account))
                                                .Select(x => x.Account)
                                                .ToList();
            }
            else
            {
                var validUserIds = noticeConfigs.Select(x => x.UserId).Intersect(bindUsers).ToList();
                var validConfigs = noticeConfigs.Where(x => validUserIds.Contains(x.UserId)).ToList();

                var users = await _platformDbContext.ApplicationUser
                    .Where(x => validUserIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Email, x.Phone, x.Account })
                    .ToListAsync();

                var emailUserIds = validConfigs.Where(x => x.Method.Contains("1")).Select(x => x.UserId).ToList();
                var smsUserIds = validConfigs.Where(x => x.Method.Contains("0")).Select(x => x.UserId).ToList();

                config.Emails = users.Where(x => emailUserIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Email))
                                   .Select(x => x.Email)
                                   .ToList();

                config.PhoneNumbers = users.Where(x => smsUserIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Phone))
                                         .Select(x => x.Phone)
                                         .ToList();
                config.Accounts = users.Where(x => !string.IsNullOrWhiteSpace(x.Account))
                                                .Select(x => x.Account)
                                                .ToList();
            }

            return config;
        }

        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <param name="config"></param>
        /// <param name="templateEnum"></param>
        /// <param name="subjectEnum"></param>
        /// <returns></returns>
        private async Task<NotificationTemplates> GetNotificationTemplatesAsync(long enterpriseId, NotificationConfig config, TemplateEnum templateEnum, TemplateEnum subjectEnum = TemplateEnum.EmailSubject)
        {
            var areaInfo = await _platformDbContext.EnterpriseInfo
                .Where(x => x.Id == enterpriseId)
                .Select(x => new { x.AreaRelationId })
                .FirstOrDefaultAsync();

            if (areaInfo?.AreaRelationId == null) return null;

            var language = await _platformDbContext.AreaRelation
                .Where(x => x.Id == areaInfo.AreaRelationId)
                .Select(x => x.Language)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(language)) return null;

            var templates = await _platformDbContext.LanguageText
        .Where(x => x.LangCode == language &&
                   (x.Code == templateEnum.ToString() ||
                    x.Code == subjectEnum.ToString()))
        .ToListAsync();

            var subject = templates.FirstOrDefault(x => x.Code == subjectEnum.ToString())?.Value;
            var template = templates.FirstOrDefault(x => x.Code == templateEnum.ToString())?.Value;

            if (string.IsNullOrWhiteSpace(template) || string.IsNullOrWhiteSpace(subject))
            {
                _logger.LogWarning($"未配置邮件模板相关信息！");
                return null;
            }

            return new NotificationTemplates
            {
                Subject = subject ?? "",
                Template = template,
            };
        }

        /// <summary>
        /// 获取分布式锁key
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        private string GetEmailCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Email, deviceId, messageHash);
        }

        private async Task SendSmsNotificationAsync(DeviceInfo device, List<string> phoneNumbers, Dictionary<string, string> messageBodyDic, string subject)
        {
            var smsDto = new SmsDto()
            {
                Subject = subject,
                Type = 0,
                EnterpriseinfoId = device.EnterpriseinfoId,
                DeviceId = device.Id.ToString(),
                PhoneNumbers = phoneNumbers,
                MessageBodyDic = messageBodyDic
            };

            await _publish.SendMessage(CapConst.Sms, smsDto);
        }

        /// <summary>
        /// 验证重复消息
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        private async Task<bool> CanSendNotificationAsync(string cacheKey, string notificationType)
        {
            var intervalConfig = _cfg[$"{notificationType}:SendingInterval"];
            if (!int.TryParse(intervalConfig, out int intervalDays) || intervalDays <= 0)
            {
                _logger.LogWarning($"{notificationType}发送间隔配置无效: {intervalConfig}");
                return false;
            }

            // 使用分布式锁防止重复发送
            return await _redisService.SetNxAsync(cacheKey, intervalDays * 24 * 3600);
        }

        private async Task SendEmailNotificationAsync(DeviceInfo device, List<string> emails, string messageBody, string subject)
        {
            var emailDto = new EmailDto()
            {
                Type = 0,
                EnterpriseinfoId = device.EnterpriseinfoId,
                DeviceId = device.Id.ToString(),
                ToEmail = string.Join(',', emails),
                MessageBody = messageBody,
                Subject = subject
            };

            await _publish.SendMessage(CapConst.Email, emailDto);
        }

        /// <summary>
        /// 获取分布式锁key
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        private string GetSmsCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Sms, deviceId, messageHash);
        }

        /// <summary>
        /// 清洗预警上报
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity9033.Response> FulshYj(UplinkEntity9033.Request request)
        {
            var response = new UplinkEntity9033.Response
            {
                Mid = request.Mid,
            };
            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
            if (device == null)
                return response;
            var names = request.FlushNames;
            var key = $"FulshYj:{device.Id}:{names}";
            if (await _redisService.SetNxAsync(key, "1", 1 * 24 * 60 * 60))
            {
                try
                {
                    var deviceIds = new List<long> { device.Id };
                    var deviceBindUsers = await _commonHelper.GetUserByDeviceId(deviceIds);
                    var notificationConfig = await GetNotificationConfigAsync(device, deviceBindUsers);
                    if (!notificationConfig.HasAnyNotification)
                    {
                        _logger.LogWarning("未配置需要通知的短信或邮件地址！");
                        return response;
                    }
                    var templates = await GetNotificationTemplatesAsync(device.EnterpriseinfoId, notificationConfig, TemplateEnum.FlushTemplate);
                    if (templates == null)
                    {
                        _logger.LogWarning("未配置获取模板的相关信息！");
                        return response;
                    }
                    var time = await _commonHelper.GetDateTimeByEnterprise(device.EnterpriseinfoId, DateTime.UtcNow);
                    var isCreateNotityMsg = false;
                    var emailMsg = string.Format(templates.Template, GetAddress(device) ?? "未知", device.Name, deviceBase.MachineStickerCode, request.FlushNames, time);
                    if (notificationConfig.Emails != null && notificationConfig.Emails.Any())
                    {
                        var validEmails = new List<string>();

                        foreach (var email in notificationConfig.Emails)
                        {
                            var emailCacheKey = GetEmailCacheKey(device.Id.ToString(), email + request.FlushNames);
                            if (await CanSendNotificationAsync(emailCacheKey, "Email"))
                            {
                                validEmails.Add(email);
                            }
                        }
                        if (validEmails.Count > 0)
                        {
                            await SendEmailNotificationAsync(device, validEmails, emailMsg, templates.Subject);
                            isCreateNotityMsg = true;
                        }
                    }

                    if (notificationConfig.PhoneNumbers.Count > 0)
                    {
                        var validSmss = new List<string>();
                        var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device) ?? "未知", device_code = deviceBase.MachineStickerCode, flushNames = request.FlushNames, time = time });
                        var SmsMessageDic = new Dictionary<string, string>() { { SmsConst.SmsFulshWarningTemplate, smsMessage } };
                        foreach (var sms in notificationConfig.PhoneNumbers)
                        {
                            var emailCacheKey = GetEmailCacheKey(device.Id.ToString(), sms + request.FlushNames);
                            if (await CanSendNotificationAsync(emailCacheKey, "Sms"))
                            {
                                validSmss.Add(sms);
                            }
                        }

                        if (validSmss.Count > 0)
                        {
                            await SendSmsNotificationAsync(device, validSmss, SmsMessageDic, templates.Subject);
                            isCreateNotityMsg = true;
                        }
                    }

                    if (isCreateNotityMsg)
                    {
                        var notifyMessages = notificationConfig.Accounts
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => new CreateNotityMsgDto()
                            {
                                DeviceId = device.Id,
                                MsgName = templates.Subject,
                                Type = 0,
                                Account = x,
                                Msg = emailMsg,
                                EnterpriseinfoId = device.EnterpriseinfoId
                            })
                            .ToList();
                        await _publish.SendMessage(CapConst.CreateNotityMsg, notifyMessages);
                    }
                }
                catch (Exception e)
                {
                    await _redisService.DelKeyAsync(key);
                    _logger.LogError($"上报预警错误！原因：{e.Message}");
                }
            }

            return response;
        }

        /// <summary>
        /// 出货结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity4201.Response> ShipmentResults(UplinkEntity4201.Request request)
        {
            var response = new UplinkEntity4201.Response
            {
                Mid = request.Mid,
                OrderNo = request.OrderNo
            };

            var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
            var order = await _platformDbContext.OrderInfo.Include(x => x.OrderDetails).Where(x => x.Code == request.OrderNo).FirstOrDefaultAsync();
            var deviceMaterials = await _platformDbContext.DeviceMaterialInfo.Where(x => x.DeviceBaseId == deviceBase.Id).ToListAsync();
            var failOrders = new List<RefundOrderProduct>();
            if (order != null)
            {
                foreach (var item in request.Orders)
                {
                    var orderDetaliMaterials = new List<OrderDetaliMaterial>();
                    // 设置子订单出货结果
                    var orderSub = order.OrderDetails.FirstOrDefault(x => x.Id.ToString() == item.SubOrderNo);
                    if (orderSub != null)
                    {
                        orderSub.SetShipmentResults(item.Result, item.ActionTimeSp, item.Error, item.ErrorDescription);
                        if (item.Materials != null && item.Materials.Any())
                        {
                            foreach (var material in item.Materials)
                            {
                                var deviceMaterial = deviceMaterials.FirstOrDefault(x => x.DeviceBaseId == deviceBase.Id && x.Type == (MaterialTypeEnum)material.Type && x.Index == material.Index);
                                if (deviceMaterial != null)
                                {
                                    orderDetaliMaterials.Add(new OrderDetaliMaterial(deviceMaterial.Id, material.Value));
                                }
                            }
                            orderSub.SetOrderDetaliMaterials(orderDetaliMaterials);
                        }

                        // 组装出货失败的子订单信息用于发起退款
                        if (item.Result == 0)
                        {
                            failOrders.Add(new RefundOrderProduct
                            {
                                OrderProductId = orderSub.Id.ToString(),
                                RefundAmount = orderSub.Price
                            });
                        }
                    }
                }

                // 设置订单出货结果
                order.SetShipmentResults();
                _platformDbContext.Update(order);
                await _platformDbContext.SaveChangesAsync();

                // cap发布执行订单退款操作
                if (failOrders.Count > 0)
                {
                    // 组装订阅消息
                    var message = new OrderRefundSubscriberDto()
                    {
                        TransId = YitIdHelper.NextId().ToString(),
                        OrderId = order.Id,
                        RefundOrderProducts = failOrders,
                        RefundReason = "出货失败退款",
                        OperateUserId = 0,
                        OrderRefundType = true
                    };
                    await _publish.SendMessage(CapConst.DomesticPaymentOrderRefund, message);
                }
            }

            return response;
        }

        /// <summary>
        /// 7211
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UplinkEntity7211.Response> Request7211(UplinkEntity7211.Request request)
        {
            var response = new UplinkEntity7211.Response
            {
                Mid = request.Mid,
                TransId = request.TransId,
                Status = 0,
                Amount = request.Payment.Amount
            };
            try
            {
                var fullUrl = new Uri(new Uri(_cfg["SHAddress"]), "/api/DomesticPayment/CreateNativeOrder").ToString();
                var deviceBase = await GetDeviceBaseInfoAsync(request.Mid);
                var device = await _platformDbContext.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
                var skus = request.Details.Select(x => x.ItemCode).ToList();
                var beverageInfos = await _platformDbContext.BeverageInfo.Where(x => skus.Contains(x.Code)).ToListAsync();
                var jjid = await _platformDbContext.M_PaymentMethodBindDevice.Where(x => x.DeviceId == device.Id).Select(x => x.PaymentMethodId).FirstOrDefaultAsync();
                if (jjid == 0) throw new Exception("未找到该设备的进件id!");

                var input = new CreateNativeOrderResponse()
                {
                    Mid = deviceBase.MachineStickerCode,
                    BizNo = "",
                    Provider = request.Payment.Provider,
                    PayTimeSp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    MerchantId = jjid,
                    PayAmount = request.Payment.Amount,
                    CustomContent = "",
                    OrderPaymentType = (OrderPaymentTypeEnum)Convert.ToInt32(request.Payment.Provider)
                };
                foreach (var item in request.Details)
                {
                    //var beverageInfo = beverageInfos.FirstOrDefault(x => x.Code == item.ItemCode && x.DeviceId == device.Id);
                    //if (beverageInfo == null) throw new Exception($"未找到饮品{beverageInfo.Name}");
                    input.OrderDetails.Add(new OrderDetailDto()
                    {
                        CounterNo = item.CounterNo,
                        SlotNo = item.SlotNo,
                        ItemCode = item.ItemCode,
                        BeverageName = item.Name,
                        Price = item.Price,
                        Quantity = (int)item.Quantity
                    });
                }
                var i = new CreateOrderCommand() { Input = input };

                _logger.LogInformation($"7211调用第三方支付获取支付链接！入参：{JsonConvert.SerializeObject(i)}");
                var rsp = await _http.PostAsync<CreateOrderCommand, CreateNativeOrderOutput>(fullUrl, i);
                response.Status = 0;
                response.OrderNo = rsp.Code;
                response.Result = rsp.QrCodeData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"7211上报调用订单生成接口报错！原因：{ex}");
                response.Status = 1;
                response.Description = ex.Message;
            }
            return response;
        }
        #region 辅助类
        private class NotificationConfig
        {
            public List<string> Emails { get; set; } = new List<string>();
            public List<string> PhoneNumbers { get; set; } = new List<string>();
            public List<string> Accounts { get; set; } = new List<string>();

            public bool HasAnyNotification => Emails.Any() || PhoneNumbers.Any();
        }

        private class NotificationTemplates
        {
            public string Subject { get; set; } = string.Empty;
            public string Template { get; set; } = string.Empty;
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        private class CreateNotityMsgDto
        {
            /// <summary>
            /// 设备id
            /// </summary>
            public long DeviceId { get; set; }
            /// <summary>
            /// 消息名称
            /// </summary>
            public string MsgName { get; set; }

            /// <summary>
            /// 通知类型
            /// 0：预警 1：异常
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 联系人账号
            /// </summary>
            public string Account { get; set; }

            /// <summary>
            /// 消息
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 租户
            /// </summary>
            public long EnterpriseinfoId { get; set; }
        }
    }
    #endregion
}