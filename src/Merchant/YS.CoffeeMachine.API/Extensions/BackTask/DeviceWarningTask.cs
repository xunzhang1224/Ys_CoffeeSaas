using DotNetCore.CAP;
using Magicodes.ExporterAndImporter.Core.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Extensions.BackTask
{
    /// <summary>
    /// 设备预警任务
    /// </summary>
    public class DeviceWarningTask : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DeviceWarningTask> _logger;
        private readonly IConfiguration _cfg;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(30);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scopeFactory"></param>
        /// <param name="logger"></param>
        /// <param name="cfg"></param>
        public DeviceWarningTask(IServiceScopeFactory scopeFactory, ILogger<DeviceWarningTask> logger, IConfiguration cfg)
        {
            _serviceScopeFactory = scopeFactory;
            _logger = logger;
            _cfg = cfg;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("设备预警任务开始执行...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //var scopedServices = scope.ServiceProvider;
                    //var db = scopedServices.GetRequiredService<CoffeeMachinePlatformDbContext>();
                    //var commonHelper = scopedServices.GetRequiredService<CommonHelper>();
                    //var redis = scopedServices.GetRequiredService<IRedisService>();
                    //var cap = scopedServices.GetRequiredService<IPublishService>();

                    await ProcessDeviceWarningsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "设备预警通知异常");
                }

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }

        private async Task ProcessDeviceWarningsAsync(CancellationToken stoppingToken)
        {
            using var mainScope = _serviceScopeFactory.CreateScope();
            var mainServices = mainScope.ServiceProvider;
            var db = mainServices.GetRequiredService<CoffeeMachinePlatformDbContext>();
            var commonHelper = mainServices.GetRequiredService<CommonHelper>();
            var redis = mainServices.GetRequiredService<IRedisService>();
            //var cap = mainServices.GetRequiredService<IPublishService>();
            var warningData = await GetWarningDataAsync(db, commonHelper, redis);
            if (warningData.WarningsDic == null || !warningData.WarningsDic.Any())
            {
                _logger.LogInformation("未找到需要处理的预警配置");
                return;
            }

            var (warningsDic, devices, deviceBases, deviceBindUsers) = warningData;

            _logger.LogInformation($"本次任务需要处理 {warningsDic.Count} 个设备的预警信息");

            var processingTasks = warningsDic.Select(async deviceWarning =>
            {
                await _semaphore.WaitAsync(stoppingToken);
                try
                {
                    using var taskScope = _serviceScopeFactory.CreateScope();
                    var taskServices = taskScope.ServiceProvider;
                    var taskDb = taskServices.GetRequiredService<CoffeeMachinePlatformDbContext>();
                    var taskCommonHelper = taskServices.GetRequiredService<CommonHelper>();
                    var taskRedis = taskServices.GetRequiredService<IRedisService>();
                    var taskCap = taskServices.GetRequiredService<IPublishService>();

                    await ProcessSingleDeviceWarningAsync(
                        deviceWarning, devices, deviceBases, deviceBindUsers,
                        taskDb, taskCommonHelper, taskRedis, taskCap, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"处理设备预警时发生错误: {deviceWarning.Key}");
                }
                finally
                {
                    _semaphore.Release();
                }
            });
            await Task.WhenAll(processingTasks);
        }

        private async Task<(
            Dictionary<long, List<DeviceEarlyWarnings>> WarningsDic,
            List<DeviceInfo> Devices,
            List<DeviceBaseInfo> DeviceBases,
            Dictionary<long, List<long>> DeviceBindUsers
        )> GetWarningDataAsync(CoffeeMachinePlatformDbContext db, CommonHelper commonHelper, IRedisService redis)
        {
            var allWarnings = await db.DeviceEarlyWarnings
                .AsNoTracking()
                .Where(x => x.IsOn &&
                           !string.IsNullOrWhiteSpace(x.WarningValue) &&
                           x.DeviceBaseId != 0)
                .ToListAsync();

            if (!allWarnings.Any())
            {
                _logger.LogInformation("未找到开启的预警配置");
                return (null, null, null, null);
            }

            _logger.LogInformation($"找到 {allWarnings.Count} 个开启的预警配置");

            var warningsByDevice = allWarnings
                .GroupBy(x => x.DeviceBaseId)
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.CreateTime).ToList());

            var deviceBaseIds = warningsByDevice.Keys.ToList();

            var deviceBasesQuery = await db.DeviceBaseInfo
                .AsNoTracking()
                .Where(x => deviceBaseIds.Contains(x.Id))
                .Select(x => new DeviceBaseQueryResult
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    UpdateOfflineTime = x.UpdateOfflineTime,
                    Mid = x.Mid,
                    MachineStickerCode = x.MachineStickerCode
                })
                .ToListAsync();

            var devicesQuery = await db.DeviceInfo
                .AsNoTracking()
                .Where(x => deviceBaseIds.Contains(x.DeviceBaseId))
                .Select(x => new DeviceQueryResult
                {
                    Id = x.Id,
                    DeviceBaseId = x.DeviceBaseId,
                    Name = x.Name,
                    EnterpriseinfoId = x.EnterpriseinfoId,
                    Province = x.Province,
                    City = x.City,
                    District = x.District,
                    Street = x.Street,
                    DetailedAddress = x.DetailedAddress,
                    CountryRegionText = x.CountryRegionText
                })
                .ToListAsync();

            if (!devicesQuery.Any())
            {
                _logger.LogWarning("未找到对应的设备信息");
                return (null, null, null, null);
            }

            var materialInfos = await db.DeviceMaterialInfo
                .AsNoTracking()
                .Where(x => deviceBaseIds.Contains(x.DeviceBaseId))
                .Select(x => new MaterialInfoQueryResult
                {
                    DeviceBaseId = x.DeviceBaseId,
                    Id = x.Id,
                    Name = x.Name,
                    Stock = x.Stock,
                    LastModifyTime = x.LastModifyTime,
                    Type = x.Type

                })
                .ToListAsync();

            var validWarningsDic = new Dictionary<long, List<DeviceEarlyWarnings>>();
            var validDeviceBaseIds = new List<long>();

            foreach (var deviceBaseId in deviceBaseIds)
            {
                var deviceWarnings = warningsByDevice[deviceBaseId];
                var deviceBase = deviceBasesQuery.FirstOrDefault(x => x.Id == deviceBaseId);
                var device = devicesQuery.FirstOrDefault(x => x.DeviceBaseId == deviceBaseId);

                if (device == null || deviceBase == null)
                {
                    _logger.LogWarning($"未找到设备基础ID {deviceBaseId} 对应的设备信息");
                    continue;
                }

                var validWarnings = new List<DeviceEarlyWarnings>();

                foreach (var warning in deviceWarnings)
                {
                    var shouldInclude = await ShouldIncludeWarningAsync(
                        warning, deviceBase, device, materialInfos, redis);

                    if (shouldInclude)
                        validWarnings.Add(warning);
                }

                if (validWarnings.Any())
                {
                    validWarningsDic[deviceBaseId] = validWarnings;
                    validDeviceBaseIds.Add(deviceBaseId);
                }
            }

            if (!validWarningsDic.Any())
            {
                _logger.LogInformation("经过条件筛选，没有需要处理的预警");
                return (null, null, null, null);
            }

            _logger.LogInformation($"条件筛选后，剩余 {validWarningsDic.Count} 个设备需要处理预警");

            var validDeviceIds = devicesQuery.Where(x => validDeviceBaseIds.Contains(x.DeviceBaseId))
                                           .Select(x => x.Id)
                                           .ToList();
            var deviceBindUsers = await commonHelper.GetUserByDeviceId(validDeviceIds);

            var validDeviceBases = await db.DeviceBaseInfo
                .AsNoTracking()
                .Where(x => validDeviceBaseIds.Contains(x.Id))
                .ToListAsync();

            var validDevices = await db.DeviceInfo
                .AsNoTracking()
                .Where(x => validDeviceBaseIds.Contains(x.DeviceBaseId))
                .ToListAsync();

            return (validWarningsDic, validDevices, validDeviceBases, deviceBindUsers);
        }

        /// <summary>
        /// 判断是否应该包含此预警
        /// </summary>
        private async Task<bool> ShouldIncludeWarningAsync(
            DeviceEarlyWarnings warning,
            DeviceBaseQueryResult deviceBase,
            DeviceQueryResult device,
            List<MaterialInfoQueryResult> materialInfos,
            IRedisService redis)
        {
            try
            {
                switch (warning.WarningType)
                {
                    case EarlyWarningTypeEnum.OfflineWarning:
                        return await CheckOfflineWarningAsync(warning, deviceBase, device, redis);

                    case EarlyWarningTypeEnum.ShortageWarning:
                        return CheckShortageWarning(warning, deviceBase.Id, materialInfos);

                    default:
                        _logger.LogWarning($"未知的预警类型: {warning.WarningType}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"验证预警条件时发生错误，预警ID: {warning.Id}, 设备基础ID: {deviceBase.Id}");
                return false;
            }
        }

        /// <summary>
        /// 检查离线预警条件
        /// </summary>
        private async Task<bool> CheckOfflineWarningAsync(
            DeviceEarlyWarnings warning,
            DeviceBaseQueryResult deviceBase,
            DeviceQueryResult device,
            IRedisService redis)
        {
            // 设备在线，不需要离线预警
            if (deviceBase.IsOnline)
            {
                _logger.LogDebug($"设备 {deviceBase.Mid} 在线，跳过离线预警");
                return false;
            }

            // 没有离线时间，无法计算离线时长
            if (!deviceBase.UpdateOfflineTime.HasValue)
            {
                _logger.LogWarning($"设备 {deviceBase.Mid} 离线但无离线时间");
                return false;
            }

            // 检查预警值是否有效
            if (!double.TryParse(warning.WarningValue, out double threshold) || threshold <= 0)
            {
                _logger.LogWarning($"设备 {deviceBase.Mid} 的离线预警阈值配置无效: {warning.WarningValue}");
                return false;
            }

            // 检查是否已经发送过该预警
            var cacheKey = string.Format(CacheConst.OffOnlineTask, deviceBase.Mid);
            var lastOfflineTimeStr = await redis.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(lastOfflineTimeStr))
            {
                if (DateTime.TryParse(lastOfflineTimeStr, out DateTime lastOfflineTime) &&
                    lastOfflineTime == deviceBase.UpdateOfflineTime.Value)
                {
                    _logger.LogDebug($"设备 {deviceBase.Mid} 的离线预警已发送过（时间: {lastOfflineTime}）");
                    return false;
                }
            }

            // 计算离线时长并比较阈值
            var offlineDuration = YS.Util.Core.Util.GetWholeHHDifference(
                DateTime.UtcNow, deviceBase.UpdateOfflineTime.Value);

            if (offlineDuration >= threshold)
            {
                _logger.LogInformation($"设备 {deviceBase.Mid} 满足离线预警条件，离线时长: {offlineDuration}小时，阈值: {threshold}小时");
                return true;
            }

            _logger.LogDebug($"设备 {deviceBase.Mid} 离线时长 {offlineDuration}小时 未达到阈值 {threshold}小时");
            return false;
        }

        /// <summary>
        /// 检查缺料预警条件
        /// </summary>
        private bool CheckShortageWarning(
            DeviceEarlyWarnings warning,
            long deviceBaseId,
            List<MaterialInfoQueryResult> materialInfos)
        {
            if (!warning.DeviceMaterialId.HasValue)
            {
                _logger.LogWarning($"设备基础ID {deviceBaseId} 的缺料预警未配置物料ID");
                return false;
            }

            var material = materialInfos
                .FirstOrDefault(x => x.DeviceBaseId == deviceBaseId && x.Id == warning.DeviceMaterialId.Value);

            if (material == null)
            {
                _logger.LogWarning($"未找到设备基础ID {deviceBaseId} 的物料ID {warning.DeviceMaterialId.Value}");
                return false;
            }

            // 检查预警值是否有效
            if (!int.TryParse(warning.WarningValue, out int threshold))
            {
                _logger.LogWarning($"设备基础ID {deviceBaseId} 的缺料预警阈值配置无效: {warning.WarningValue}");
                return false;
            }

            // 库存小于等于预警阈值
            if (material.Stock <= threshold)
            {
                _logger.LogInformation($"设备基础ID {deviceBaseId} 的物料 {material.Name} 满足缺料预警条件，库存: {material.Stock}，阈值: {threshold}");
                return true;
            }

            _logger.LogDebug($"设备基础ID {deviceBaseId} 的物料 {material.Name} 库存 {material.Stock} 充足，阈值: {threshold}");
            return false;
        }

        private async Task ProcessSingleDeviceWarningAsync(
            KeyValuePair<long, List<DeviceEarlyWarnings>> deviceWarning,
            List<DeviceInfo> devices,
            List<DeviceBaseInfo> deviceBases,
            Dictionary<long, List<long>> deviceBindUsers,
            CoffeeMachinePlatformDbContext db,
            CommonHelper commonHelper,
            IRedisService redis,
            IPublishService cap,
            CancellationToken stoppingToken)
        {
            var deviceBaseId = deviceWarning.Key;
            var warnings = deviceWarning.Value;

            _logger.LogInformation($"处理设备预警 ----》deviceBaseId: {deviceBaseId}, 预警数量: {warnings.Count}");

            var device = devices.FirstOrDefault(x => x.DeviceBaseId == deviceBaseId);
            if (device == null)
            {
                _logger.LogWarning($"未找到设备基础ID {deviceBaseId} 对应的设备信息");
                return;
            }

            var notificationConfig = await GetNotificationConfigAsync(device, deviceBindUsers, db);
            if (!notificationConfig.HasAnyNotification)
            {
                _logger.LogWarning($"设备 {device.Name} 未配置需要通知的短信或邮件地址！");
                return;
            }

            var deviceBase = deviceBases.FirstOrDefault(x => x.Id == deviceBaseId);
            if (deviceBase == null)
            {
                _logger.LogWarning($"未找到设备基础ID {deviceBaseId} 对应的设备基础信息");
                return;
            }

            var templates = await GetNotificationTemplatesAsync(device.EnterpriseinfoId, db);
            if (templates == null)
            {
                _logger.LogWarning($"设备 {device.Name} 未配置获取模板的相关信息！");
                return;
            }

            await ProcessWarningTypesAsync(
                device, deviceBase, warnings, notificationConfig, templates,
                stoppingToken, db, commonHelper, redis, cap);
        }

        /// <summary>
        /// 需要通知的短信和邮件地址
        /// </summary>
        private async Task<NotificationConfig> GetNotificationConfigAsync(
            DeviceInfo device,
            Dictionary<long, List<long>> deviceBindUsers,
            CoffeeMachinePlatformDbContext db)
        {
            deviceBindUsers ??= new Dictionary<long, List<long>>();
            var bindUsers = deviceBindUsers.GetValueOrDefault(device.Id) ?? new List<long>();

            var noticeConfigs = await db.NoticeCfg
                .AsNoTracking()
                .Where(x => x.Type == 0 &&
                           x.EnterpriseinfoId == device.EnterpriseinfoId &&
                           x.Status)
                .Select(x => new { x.UserId, x.Method })
                .ToListAsync();

            var config = new NotificationConfig();

            if (!noticeConfigs.Any() || !bindUsers.Any())
            {
                var superAdmins = await db.ApplicationUser
                    .AsNoTracking()
                    .Where(x => x.AccountType == AccountTypeEnum.SuperAdmin &&
                               x.EnterpriseId == device.EnterpriseinfoId)
                    .Select(x => new { x.Email, x.Phone, x.Account })
                    .ToListAsync();

                config.Emails = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Email))
                                          .Select(x => x.Email)
                                          .Distinct()
                                          .ToList();
                config.PhoneNumbers = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Phone))
                                                .Select(x => x.Phone)
                                                .Distinct()
                                                .ToList();
                config.Accounts = superAdmins.Where(x => !string.IsNullOrWhiteSpace(x.Account))
                                            .Select(x => x.Account)
                                            .Distinct()
                                            .ToList();
            }
            else
            {
                var validUserIds = noticeConfigs.Select(x => x.UserId).Intersect(bindUsers).ToList();
                var validConfigs = noticeConfigs.Where(x => validUserIds.Contains(x.UserId)).ToList();

                var users = await db.ApplicationUser
                    .AsNoTracking()
                    .Where(x => validUserIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Email, x.Phone, x.Account })
                    .ToListAsync();

                var emailUserIds = validConfigs.Where(x => x.Method.Contains("1")).Select(x => x.UserId).ToList();
                var smsUserIds = validConfigs.Where(x => x.Method.Contains("0")).Select(x => x.UserId).ToList();

                config.Emails = users.Where(x => emailUserIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Email))
                                   .Select(x => x.Email)
                                   .Distinct()
                                   .ToList();

                config.PhoneNumbers = users.Where(x => smsUserIds.Contains(x.Id) && !string.IsNullOrWhiteSpace(x.Phone))
                                         .Select(x => x.Phone)
                                         .Distinct()
                                         .ToList();
                config.Accounts = users.Where(x => !string.IsNullOrWhiteSpace(x.Account))
                                      .Select(x => x.Account)
                                      .Distinct()
                                      .ToList();
            }

            _logger.LogDebug($"设备 {device.Name} 通知配置 - 邮件: {config.Emails.Count} 个, 短信: {config.PhoneNumbers.Count} 个, 账号: {config.Accounts.Count} 个");

            return config;
        }

        /// <summary>
        /// 获取模板信息
        /// </summary>
        private async Task<NotificationTemplates> GetNotificationTemplatesAsync(
            long enterpriseId,
            CoffeeMachinePlatformDbContext db)
        {
            var areaInfo = await db.EnterpriseInfo
                .AsNoTracking()
                .Where(x => x.Id == enterpriseId)
                .Select(x => new { x.AreaRelationId })
                .FirstOrDefaultAsync();

            if (areaInfo?.AreaRelationId == null)
            {
                _logger.LogWarning($"企业 {enterpriseId} 未配置区域关系");
                return null;
            }

            var language = await db.AreaRelation
                .AsNoTracking()
                .Where(x => x.Id == areaInfo.AreaRelationId)
                .Select(x => x.Language)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(language))
            {
                _logger.LogWarning($"企业 {enterpriseId} 的区域关系 {areaInfo.AreaRelationId} 未配置语言");
                return null;
            }

            var templates = await db.LanguageText
                .AsNoTracking()
                .Where(x => x.LangCode == language &&
                           (x.Code == nameof(TemplateEnum.OfflineTemplate) ||
                            x.Code == nameof(TemplateEnum.ShortageTemplate) ||
                            x.Code == nameof(TemplateEnum.EmailSubject)))
                .ToListAsync();

            var subject = templates.FirstOrDefault(x => x.Code == nameof(TemplateEnum.EmailSubject))?.Value;
            var offlineTemplate = templates.FirstOrDefault(x => x.Code == nameof(TemplateEnum.OfflineTemplate))?.Value;
            var shortageTemplate = templates.FirstOrDefault(x => x.Code == nameof(TemplateEnum.ShortageTemplate))?.Value;

            if (string.IsNullOrWhiteSpace(offlineTemplate) || string.IsNullOrWhiteSpace(shortageTemplate) || string.IsNullOrWhiteSpace(subject))
            {
                _logger.LogWarning($"企业 {enterpriseId} 未完整配置邮件模板信息");
                return null;
            }

            return new NotificationTemplates
            {
                Subject = subject ?? "",
                OfflineTemplate = offlineTemplate,
                ShortageTemplate = shortageTemplate,
            };
        }

        private async Task ProcessWarningTypesAsync(
            DeviceInfo device,
            DeviceBaseInfo deviceBase,
            List<DeviceEarlyWarnings> warnings,
            NotificationConfig notificationConfig,
            NotificationTemplates templates,
            CancellationToken stoppingToken,
            CoffeeMachinePlatformDbContext db,
            CommonHelper commonHelper,
            IRedisService redis,
            IPublishService cap)
        {
            var shortageMessages = new List<string>();
            var shortageTimes = new List<DateTime?>();
            var hasOfflineWarning = false;
            var offlineWarningMessage = string.Empty;
            var offlineWarningTime = DateTime.MinValue;

            var materialInfos = await db.DeviceMaterialInfo
                .AsNoTracking()
                .Where(x => x.DeviceBaseId == deviceBase.Id)
                .ToListAsync();

            foreach (var warning in warnings)
            {
                switch (warning.WarningType)
                {
                    case EarlyWarningTypeEnum.OfflineWarning:
                        var (hasOffline, message, time) = await ProcessOfflineWarningCheckAsync(
                            device, deviceBase, warning, templates, commonHelper, redis);
                        if (hasOffline)
                        {
                            hasOfflineWarning = true;
                            offlineWarningMessage = message;
                            offlineWarningTime = time;
                        }
                        break;

                    case EarlyWarningTypeEnum.ShortageWarning:
                        ProcessShortageWarning(warning, materialInfos, shortageMessages, shortageTimes);
                        break;
                }

                stoppingToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"设备 {device.Name} 预警结果 - 离线: {hasOfflineWarning}, 缺料: {shortageMessages.Count} 种物料");

            // 统一发送通知
            await SendNotificationsAsync(
                device, deviceBase, hasOfflineWarning, offlineWarningMessage,
                shortageMessages, shortageTimes, notificationConfig, templates,
                offlineWarningTime, commonHelper, redis, cap);
        }

        /// <summary>
        /// 验证并组装离线预警
        /// </summary>
        private async Task<(bool HasWarning, string Message, DateTime Time)> ProcessOfflineWarningCheckAsync(
            DeviceInfo device,
            DeviceBaseInfo deviceBase,
            DeviceEarlyWarnings warning,
            NotificationTemplates templates,
            CommonHelper commonHelper,
            IRedisService redis)
        {
            // 由于在数据源阶段已经验证过条件，这里主要组装消息
            if (!deviceBase.IsOnline &&
                deviceBase.UpdateOfflineTime.HasValue &&
                !string.IsNullOrWhiteSpace(warning.WarningValue))
            {
                var localTime = await commonHelper.GetDateTimeByEnterprise(
                    device.EnterpriseinfoId, deviceBase.UpdateOfflineTime.Value);

                var messageBody = string.Format(templates.OfflineTemplate,
                    GetAddress(device), device.Name, deviceBase.MachineStickerCode,
                    localTime.ToString("yyyy-MM-dd HH:mm:ss"));

                return (true, messageBody, localTime);
            }

            return (false, string.Empty, DateTime.MinValue);
        }

        private async Task SendNotificationsAsync(
            DeviceInfo device,
            DeviceBaseInfo deviceBase,
            bool hasOfflineWarning,
            string offlineMessage,
            List<string> shortageMessages,
            List<DateTime?> shortageTimes,
            NotificationConfig notificationConfig,
            NotificationTemplates templates,
            DateTime updateOfflineTime,
            CommonHelper commonHelper,
            IRedisService redis,
            IPublishService cap)
        {
            var notifications = new List<(string Message, Dictionary<string, string> SmsMessageDic, EarlyWarningTypeEnum Type)>();

            // 离线预警
            if (hasOfflineWarning && !string.IsNullOrEmpty(offlineMessage))
            {
                var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device), device_code = deviceBase.MachineStickerCode, time = updateOfflineTime.ToString("yyyy-MM-dd HH:mm:ss") });
                var SmsMessageDic = new Dictionary<string, string>() { { SmsConst.SmsOffLineTemplate, smsMessage } };

                notifications.Add((offlineMessage, SmsMessageDic, EarlyWarningTypeEnum.OfflineWarning));
            }

            // 缺料预警
            if (shortageMessages.Any())
            {
                var maxTime = shortageTimes.Max() ?? DateTime.MinValue;
                var localTime = await commonHelper.GetDateTimeByEnterprise(device.EnterpriseinfoId, DateTime.UtcNow);
                var materialNames = string.Join("、", shortageMessages);

                var emailMessage = string.Format(templates.ShortageTemplate,
                    device.CountryRegionText, device.Name, deviceBase.MachineStickerCode,
                    localTime.ToString("yyyy-MM-dd"), materialNames);

                var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device), device_code = deviceBase.MachineStickerCode, material_name = materialNames, detection_time = localTime.ToString("yyyy-MM-dd") });
                var SmsMessageDic = new Dictionary<string, string>() { { SmsConst.SmsShortageTemplate, smsMessage } };
                notifications.Add((emailMessage, SmsMessageDic, EarlyWarningTypeEnum.ShortageWarning));
            }

            _logger.LogInformation($"设备 {device.Name} 准备发送 {notifications.Count} 条预警通知");

            // 批量发送通知
            foreach (var (emailMessage, SmsMessageDic, type) in notifications)
            {
                var isCreateNotityMsg = false;

                // 邮件通知
                if (notificationConfig.Emails.Any())
                {
                    var emailCacheKey = GetEmailCacheKey(device.Id.ToString(), emailMessage);
                    if (await CanSendNotificationAsync(emailCacheKey, "Email", redis))
                    {
                        await SendEmailNotificationAsync(device, notificationConfig.Emails, emailMessage, templates.Subject, cap);
                        isCreateNotityMsg = true;
                    }
                }

                // 短信通知
                if (notificationConfig.PhoneNumbers.Any())
                {
                    var smsCacheKey = GetSmsCacheKey(device.Id.ToString(), SmsMessageDic.FirstOrDefault().Value);
                    if (await CanSendNotificationAsync(smsCacheKey, "Sms", redis))
                    {
                        await SendSmsNotificationAsync(device, notificationConfig.PhoneNumbers, SmsMessageDic, templates.Subject, cap);
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
                            Msg = emailMessage,
                            EnterpriseinfoId = device.EnterpriseinfoId,
                        })
                        .ToList();
                    await cap.SendMessage(CapConst.CreateNotityMsg, notifyMessages);
                }
            }
        }

        /// <summary>
        /// 组装缺料预警消息通知
        /// </summary>
        private void ProcessShortageWarning(
            DeviceEarlyWarnings warning,
            List<DeviceMaterialInfo> materialInfos,
            List<string> shortageMessages,
            List<DateTime?> shortageTimes)
        {
            if (warning.DeviceMaterialId.HasValue)
            {
                var material = materialInfos.FirstOrDefault(x => x.Id == warning.DeviceMaterialId.Value);
                if (material != null && material.Stock <= Convert.ToInt32(warning.WarningValue))
                {
                    shortageMessages.Add(material.Type == MaterialTypeEnum.Cassette ? material.Name : L.Text[$"MaterialTypeEnum{(int)material.Type}"]);
                    shortageTimes.Add(material.LastModifyTime);
                }
            }
        }

        /// <summary>
        /// 获取分布式锁key
        /// </summary>
        private string GetEmailCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Email, deviceId, messageHash);
        }

        /// <summary>
        /// 获取分布式锁key
        /// </summary>
        private string GetSmsCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Sms, deviceId, messageHash);
        }

        /// <summary>
        /// 验证重复消息
        /// </summary>
        private async Task<bool> CanSendNotificationAsync(string cacheKey, string notificationType, IRedisService redis)
        {
            var intervalConfig = _cfg[$"{notificationType}:SendingInterval"];
            if (!int.TryParse(intervalConfig, out int intervalDays) || intervalDays <= 0)
            {
                _logger.LogWarning($"{notificationType}发送间隔配置无效: {intervalConfig}");
                return false;
            }

            // 使用分布式锁防止重复发送
            return await redis.SetNxAsync(cacheKey, 0, intervalDays * 24 * 3600);
        }

        private async Task SendEmailNotificationAsync(DeviceInfo device, List<string> emails, string messageBody, string subject, IPublishService cap)
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

            await cap.SendMessage(CapConst.Email, emailDto);
            _logger.LogInformation($"已发送邮件通知给设备 {device.Name}，收件人: {emails.Count} 个");
        }

        private async Task SendSmsNotificationAsync(DeviceInfo device, List<string> phoneNumbers, Dictionary<string, string> messageBodyDic, string subject, IPublishService cap)
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

            await cap.SendMessage(CapConst.Sms, smsDto);
            _logger.LogInformation($"已发送短信通知给设备 {device.Name}，收件人: {phoneNumbers.Count} 个");
        }

        private string GetAddress(DeviceInfo device)
        {
            var adr = $"{device.Province}{device.City}{device.District}{device.Street}{device.DetailedAddress}";
            if (string.IsNullOrWhiteSpace(adr))
                adr = "未知";
            return adr;
        }

        // 查询结果辅助类
        private class DeviceBaseQueryResult
        {
            public long Id { get; set; }
            public bool IsOnline { get; set; }
            public DateTime? UpdateOfflineTime { get; set; }
            public string Mid { get; set; }
            public string MachineStickerCode { get; set; }
        }

        private class DeviceQueryResult
        {
            public long Id { get; set; }
            public long DeviceBaseId { get; set; }
            public string Name { get; set; }
            public long EnterpriseinfoId { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Street { get; set; }
            public string DetailedAddress { get; set; }
            public string CountryRegionText { get; set; }
        }

        private class MaterialInfoQueryResult
        {
            public long DeviceBaseId { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            public int Stock { get; set; }
            public MaterialTypeEnum Type { get; set; }
            public DateTime? LastModifyTime { get; set; }
        }

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
            public string OfflineTemplate { get; set; } = string.Empty;
            public string ShortageTemplate { get; set; } = string.Empty;
        }

    }
}