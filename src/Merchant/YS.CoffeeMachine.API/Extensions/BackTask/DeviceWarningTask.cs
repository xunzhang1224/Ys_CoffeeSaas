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
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<CoffeeMachinePlatformDbContext>();
                        var commonHelper = scopedServices.GetRequiredService<CommonHelper>();
                        var redis = scopedServices.GetRequiredService<IRedisService>();
                        var cap = scopedServices.GetRequiredService<IPublishService>();

                        await ProcessDeviceWarningsAsync(db, commonHelper, redis, cap, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "设备预警通知异常");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }

        private async Task ProcessDeviceWarningsAsync(
            CoffeeMachinePlatformDbContext db,
            CommonHelper commonHelper,
            IRedisService redis,
            IPublishService cap,
            CancellationToken stoppingToken)
        {
            var warningData = await GetWarningDataAsync(db, commonHelper);
            if (warningData.WarningsDic == null || !warningData.WarningsDic.Any())
            {
                _logger.LogInformation("未找到需要处理的预警配置");
                return;
            }

            var (warningsDic, devices, deviceBases, deviceBindUsers) = warningData;

            foreach (var deviceWarning in warningsDic)
            {
                await ProcessSingleDeviceWarningAsync(
                    deviceWarning, devices, deviceBases, deviceBindUsers,
                    db, commonHelper, redis, cap, stoppingToken);
            }
        }

        private async Task<(
            Dictionary<long, List<DeviceEarlyWarnings>> WarningsDic,
            List<DeviceInfo> Devices,
            List<DeviceBaseInfo> DeviceBases,
            Dictionary<long, List<long>> DeviceBindUsers
        )> GetWarningDataAsync(CoffeeMachinePlatformDbContext db, CommonHelper commonHelper)
        {
            var warningsDic = await db.DeviceEarlyWarnings
                .AsNoTracking()
                .Where(x => x.IsOn &&
                           !string.IsNullOrWhiteSpace(x.WarningValue) &&
                           x.DeviceBaseId != 0)
                .GroupBy(x => x.DeviceBaseId)
                .ToDictionaryAsync(g => g.Key, g => g.OrderBy(x => x.CreateTime).ToList());

            if (warningsDic == null || !warningsDic.Any())
                return (null, null, null, null);

            var deviceBaseIds = warningsDic.Keys.ToList();

            var devices = await db.DeviceInfo
                .AsNoTracking()
                .Where(x => deviceBaseIds.Contains(x.DeviceBaseId))
                .ToListAsync();

            if (devices == null || !devices.Any())
                return (null, null, null, null);

            var deviceBases = await db.DeviceBaseInfo
                .AsNoTracking()
                .Where(x => deviceBaseIds.Contains(x.Id))
                .ToListAsync();

            var deviceIds = devices.Select(x => x.Id).ToList();
            var deviceBindUsers = await commonHelper.GetUserByDeviceId(deviceIds);

            return (warningsDic, devices, deviceBases, deviceBindUsers);
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

            _logger.LogInformation($"预警任务执行设备----》deviceBaseId:{deviceBaseId};warnings:{JsonConvert.SerializeObject(warnings)}");

            var device = devices.FirstOrDefault(x => x.DeviceBaseId == deviceBaseId);
            if (device == null) return;

            var notificationConfig = await GetNotificationConfigAsync(device, deviceBindUsers, db);
            if (!notificationConfig.HasAnyNotification)
            {
                _logger.LogWarning($"设备 {device.Name} 未配置需要通知的短信或邮件地址！");
                return;
            }

            var deviceBase = deviceBases.FirstOrDefault(x => x.Id == deviceBaseId);
            if (deviceBase == null) return;

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

                var users = await db.ApplicationUser
                    .AsNoTracking()
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
        private async Task<NotificationTemplates> GetNotificationTemplatesAsync(
            long enterpriseId,
            CoffeeMachinePlatformDbContext db)
        {
            var areaInfo = await db.EnterpriseInfo
                .AsNoTracking()
                .Where(x => x.Id == enterpriseId)
                .Select(x => new { x.AreaRelationId })
                .FirstOrDefaultAsync();

            if (areaInfo?.AreaRelationId == null) return null;

            var language = await db.AreaRelation
                .AsNoTracking()
                .Where(x => x.Id == areaInfo.AreaRelationId)
                .Select(x => x.Language)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(language)) return null;

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
                _logger.LogWarning($"未配置邮件模板相关信息！");
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

            _logger.LogInformation($"设备----》{device.Name};离线信息-------》hasOfflineWarning：{hasOfflineWarning}；offlineWarningMessage{offlineWarningMessage}；offlineWarningTime{offlineWarningTime}");
            _logger.LogInformation($"设备----》{device.Name};缺料信息-------》{JsonConvert.SerializeObject(shortageMessages)}");

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
            if (!deviceBase.IsOnline &&
                deviceBase.UpdateOfflineTime.HasValue &&
                !string.IsNullOrWhiteSpace(warning.WarningValue))
            {
                var cacheKey = string.Format(CacheConst.OffOnlineTask, deviceBase.Mid);
                var lastOfflineTimeStr = await redis.GetStringAsync(cacheKey);

                if (string.IsNullOrWhiteSpace(lastOfflineTimeStr) ||
                    Convert.ToDateTime(lastOfflineTimeStr) != deviceBase.UpdateOfflineTime.Value)
                {
                    var offlineDuration = YS.Util.Core.Util.GetWholeHHDifference(
                        DateTime.UtcNow, deviceBase.UpdateOfflineTime.Value);

                    var threshold = Convert.ToDouble(warning.WarningValue);

                    if (offlineDuration >= threshold)
                    {
                        var localTime = await commonHelper.GetDateTimeByEnterprise(
                            device.EnterpriseinfoId, deviceBase.UpdateOfflineTime.Value);

                        var messageBody = string.Format(templates.OfflineTemplate,
                            GetAddress(device) ?? "未知", device.Name, deviceBase.MachineStickerCode,
                            localTime.ToString("yyyy-MM-dd HH:mm:ss"));

                        // 更新缓存，防止重复推送
                        await redis.SetStringAsync(cacheKey,
                            deviceBase.UpdateOfflineTime.Value.ToString(),
                            TimeSpan.FromDays(1));

                        return (true, messageBody, localTime);
                    }
                }
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
                var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device) ?? "未知", device_code = deviceBase.MachineStickerCode, time = updateOfflineTime.ToString("yyyy-MM-dd HH:mm:ss") });
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

                var smsMessage = JsonConvert.SerializeObject(new { device_address = GetAddress(device) ?? "未知", device_code = deviceBase.MachineStickerCode, material_name = materialNames, detection_time = localTime.ToString("yyyy-MM-dd") });
                var SmsMessageDic = new Dictionary<string, string>() { { SmsConst.SmsShortageTemplate, smsMessage } };
                notifications.Add((emailMessage, SmsMessageDic, EarlyWarningTypeEnum.ShortageWarning));
            }

            _logger.LogInformation($"预警通知----------------》{JsonConvert.SerializeObject(notifications)};通知邮件集合：{JsonConvert.SerializeObject(notificationConfig.Emails)};通知短信集合：{JsonConvert.SerializeObject(notificationConfig.PhoneNumbers)}");

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
                    shortageMessages.Add(material.Name);
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
        }

        private string GetAddress(DeviceInfo device)
        {
            return $"{device.Province}{device.City}{device.District}{device.Street}{device.DetailedAddress}";
        }

        // 辅助类
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