using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.BasicQueries.FaultCode;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 设备基本信息查询
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_redis"></param>
    /// <param name="_timeDb"></param>
    /// <param name="_user"></param>
    /// <param name="_context"></param>
    /// <param name="faultCodeInfoQueries"></param>
    public class DeviceBaseQueries(CoffeeMachinePlatformDbContext _db, IRedisService _redis, CoffeeMachineTimescaleDBContext _timeDb, UserHttpContext _user, IDeviceInfoQueries _deviceInfoQueries, CoffeeMachineDbContext _context, IFaultCodeInfoQueries faultCodeInfoQueries) : IDeviceBaseQueries
    {
        /// <summary>
        /// 获取GetDeviceCapacityCfgs
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId)
        {
            return await _db.DeviceCapacityCfg.Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// GetDeviceMetrics
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>

        public async Task<List<DeviceMetricsOutput>> GetDeviceMetrics(long deviceBaseId)
        {
            var tt = await _db.DeviceMetrics.Where(x => x.DeviceBaseId == deviceBaseId)
                .Select(s => new DeviceMetricsOutput
                {
                    DeviceBaseId = s.DeviceBaseId,
                    CounterNo = s.CounterNo,
                    MetricType = s.MetricType,
                    Index = s.Index,
                    Value = s.Value,
                    Status = s.Status,
                    Description = s.Description,
                    Type = s.Type
                })
                .ToListAsync();
            foreach (var item in tt)
            {
                item.Description = L.Text["MetricType_" + (int)item.MetricType] == "MetricType_" + (int)item.MetricType ? item.Description : L.Text["MetricType_" + (int)item.MetricType];
                //item.Description = L.Text["MetricType_" + item.MetricType];
            }
            return tt;
        }

        /// <summary>
        /// GetDeviceMetrics
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceMaterialInfo>> GetDeviceMaterialInfos(long deviceBaseId, MaterialTypeEnum? type = null)
        {
            return await _db.DeviceMaterialInfo.AsQueryable()
                .WhereIf(type != null, x => x.Type == type)
                .Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// 获取预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarnings(long deviceBaseId)
        {
            var into = from m in _db.DeviceMaterialInfo
                       join w in _db.DeviceEarlyWarnings on m.Id equals w.DeviceMaterialId into ds
                       from d in ds.DefaultIfEmpty()
                       where m.DeviceBaseId == deviceBaseId
                       select new DeviceEarlyWarningsOutput
                       {
                           Id = d.Id,
                           DeviceMaterialId = m.Id,
                           WarningType = d.WarningType,
                           Name = m.Name,
                           IsOn = d.IsOn,
                           Value = m.Stock.ToString(),
                           Capacity = m.Capacity.ToString(),
                           WarningValue = d.WarningValue,
                           Type = m.Type,
                           Index = m.Index
                       };

            return await into.ToListAsync();
        }

        /// <summary>
        /// 获取预警
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceEarlyWarningsOutput>> GetDeviceWarningsAll(long deviceBaseId)
        {
            var into = from w in _db.DeviceEarlyWarnings
                       join m in _db.DeviceMaterialInfo on w.DeviceMaterialId equals m.Id into ds
                       from d in ds.DefaultIfEmpty()
                       where w.DeviceBaseId == deviceBaseId
                       select new DeviceEarlyWarningsOutput
                       {
                           Id = w.Id,
                           DeviceMaterialId = d.Id,
                           WarningType = w.WarningType,
                           Name = d.Name,
                           IsOn = w.IsOn,
                           Value = d.Stock.ToString(),
                           Capacity = d.Capacity.ToString(),
                           WarningValue = w.WarningValue,
                           Type = d.Type,
                           Index = d.Index
                       };

            return await into.ToListAsync();
        }

        /// <summary>
        /// GetDeviceAbnormals
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceAbnormal>> GetDeviceAbnormals(long deviceBaseId)
        {
            return await _db.DeviceAbnormal.Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// GetDeviceAbnormals
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceEarlyWarnings>> GetDeviceEarlyWarnings(long deviceBaseId)
        {
            return await _db.DeviceEarlyWarnings.Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// 获取其他信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsg(long deviceId)
        {
            var deviceInfo = await _db.DeviceInfo.AsQueryable().Where(a => a.Id == deviceId).SingleOrDefaultAsync();

            // 软件版本信息
            var softInfo = await _db.DeviceSoftwareInfo.AsQueryable().Where(a => a.DeviceBaseId == deviceInfo.DeviceBaseId).ToListAsync();

            // 网络信息+屏幕
            var attrInfo = await _db.DeviceAttribute
                .Where(d => d.DeviceBaseId == deviceInfo.DeviceBaseId)
                .GroupBy(d => d.Key)
                .Select(g => new
                {
                    Key = g.Key,
                    MaxValue = g.Max(x => x.Value)
                })
                .ToListAsync();

            List<DeviceOtherMsgDto> list = new List<DeviceOtherMsgDto>();

            foreach (var item in softInfo)
            {
                DeviceOtherMsgDto dto = new DeviceOtherMsgDto();
                dto.Code = item.Name;
                dto.Value = item.VersionName;
                list.Add(dto);
            }

            foreach (var item in attrInfo)
            {
                DeviceOtherMsgDto dto = new DeviceOtherMsgDto();
                dto.Code = item.Key;
                dto.Value = item.MaxValue;
                list.Add(dto);
            }

            return list;
        }

        /// <summary>
        /// 获取设备在线日志
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceOnlineLogDto>> GetDeviceOnlineLog(DeviceOnlineLogInput input)
        {
            if (input.DateTimes == null || input.DateTimes.Count != 2)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0084)]);
            }

            var into = from deviceInfo in _context.DeviceInfo
                       join deviceModel in _context.DeviceModel on deviceInfo.DeviceModelId equals deviceModel.Id into deviceModelGroup
                       from deviceModel in deviceModelGroup.DefaultIfEmpty()
                       join deviceBaseInfo in _context.DeviceBaseInfo on deviceInfo.DeviceBaseId equals deviceBaseInfo.Id into deviceBaseInfoGroup
                       from deviceBaseInfo in deviceBaseInfoGroup.DefaultIfEmpty()
                           //where deviceInfo.Id == input.DeviceId
                       select new
                       {
                           deviceInfo.Name,
                           deviceBaseInfo.MachineStickerCode,
                           deviceModelName = deviceModel.Name,
                           deviceModelId = deviceModel.Id,
                           deviceInfo.DeviceBaseId,
                           DeviceId = deviceInfo.Id
                       };

            // 如果设备id没传，需要判断当前人拥有哪些设备权限
            if (input.DeviceId == null)
            {
                //into.WhereIf(!_user.AllDeviceRole, w => _db.DeviceUserAssociation.Any(b => w.DeviceId == b.DeviceId && b.UserId == _user.UserId)
                //|| (_db.GroupDevices.Any(b => b.DeviceInfoId == w.DeviceId && _db.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId))));

                //into.WhereIf(!_user.AllDeviceRole, w => _db.DeviceUserAssociation.Any(b => w.DeviceId == b.DeviceId && b.UserId == _user.UserId));

                if (!_user.AllDeviceRole)
                {
                    if (!_user.AllDeviceRole)
                    {
                        into = (from device in @into
                                join gd in _context.GroupDevices
                               on device.DeviceId equals gd.DeviceInfoId into gdGroup
                                from gd in gdGroup.DefaultIfEmpty()

                                join gu in _context.GroupUsers
                                    on gd.GroupsId equals gu.GroupsId into guGroup
                                from gu in guGroup.DefaultIfEmpty()

                                join du in _context.DeviceUserAssociation
                                    on device.DeviceId equals du.DeviceId into duGroup
                                from du in duGroup.DefaultIfEmpty()

                                where (gu.ApplicationUserId == _user.UserId || du.UserId == _user.UserId)

                                select device).Distinct();
                    }
                }
            }

            var info = await into.AsQueryable().AsNoTracking()
                .WhereIf(input.DeviceId != null && input.DeviceId > 0, w => w.DeviceId == input.DeviceId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceNo), w => w.MachineStickerCode == input.DeviceNo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), w => w.Name.Contains(input.DeviceName!))
                .WhereIf(input.DeviceModelId != null, w => w.deviceModelId == input.DeviceModelId)
                .WhereIf(input.DeviceId != null, w => w.DeviceId == input.DeviceId)
                .ToListAsync();
            var deviceBaseIds = info.Select(s => s.DeviceBaseId).ToList();

            var result = await _timeDb.DeviceOnlineLog.AsQueryable()
               .WhereIf(input.IsOnline != null, a => a.Status == input.IsOnline)
                .Where(a => deviceBaseIds.Contains(a.DeviceId) && a.Timestamp >= input.DateTimes[0] && a.Timestamp <= input.DateTimes[1] && a.EnterpriseinfoId == _user.TenantId)
                .Select(a => new DeviceOnlineLogDto
                {
                    DeviceBaseId = a.DeviceId,
                    IsOnline = a.Status,
                    DateTime = a.Timestamp
                })
                .OrderByDescending(w => w.DateTime)
                .ToPagedListAsync(input);

            foreach (var item in result.Items)
            {
                var deviceName = string.IsNullOrWhiteSpace(info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).Name) ? info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).MachineStickerCode : info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).Name;
                item.DeviceName = deviceName;
                item.DeviceNo = info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).MachineStickerCode;
                item.DeviceModelName = info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).deviceModelName;
                item.MachineStickerCode = info.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId).MachineStickerCode;
            }

            return result;
        }

        /// <summary>
        /// 获取设备事件日志
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceEventLogDto>> GetDeviceEventLog(DeviceEventLogInput input)
        {
            var result =
                from a in _context.OperationSubLog
                join b in _context.OperationLog on a.OperationLogId equals b.Id into ab
                from b in ab.DefaultIfEmpty()

                join c in _context.DeviceBaseInfo on a.Mid equals c.MachineStickerCode into ac
                from c in ac.DefaultIfEmpty()

                join d in _context.DeviceInfo on c.Id equals d.DeviceBaseId into cd
                from d in cd.DefaultIfEmpty()

                    //join e in _context.DeviceModel on c.DeviceModelId equals e.Id into ce
                    //from e in ce.DefaultIfEmpty()

                    //join f in _context.EnterpriseInfo on d.EnterpriseinfoId equals f.Id into df
                    //from f in df.DefaultIfEmpty()
                select new
                {
                    a,
                    b,
                    c,
                    d,
                    //e,
                    //f
                };

            if (!_user.AllDeviceRole)
            {
                result = (from device in result
                          join gd in _context.GroupDevices
                       on device.d.Id equals gd.DeviceInfoId into gdGroup
                          from gd in gdGroup.DefaultIfEmpty()

                          join gu in _context.GroupUsers
                              on gd.GroupsId equals gu.GroupsId into guGroup
                          from gu in guGroup.DefaultIfEmpty()

                          join du in _context.DeviceUserAssociation
                              on device.d.Id equals du.DeviceId into duGroup
                          from du in duGroup.DefaultIfEmpty()

                          where (gu.ApplicationUserId == _user.UserId || du.UserId == _user.UserId)

                          select device).Distinct();
            }

            var res = await result
                .WhereIf(!string.IsNullOrEmpty(input.EventName), w => w.b.OperationName == input.EventName)
                .WhereIf(!string.IsNullOrEmpty(input.EnterpriseName), w => w.b.BaseEnterpriseName.Contains(input.EnterpriseName))
                .WhereIf(!string.IsNullOrEmpty(input.Sn), w => w.c.MachineStickerCode == input.Sn)
                .WhereIf(input.DeviceModelId != null, w => w.b.BaseDeviceModelId == input.DeviceModelId)
                .WhereIf(!string.IsNullOrEmpty(input.DeviceName), w => w.d.Name.Contains(input.DeviceName))
                .WhereIf(input.DateTimeRange != null && input.DateTimeRange.Count == 2, w => w.b.CreateTime >= input.DateTimeRange[0] && w.b.CreateTime <= input.DateTimeRange[1])
                .WhereIf(input.DeviceId != null, w => w.d.Id == input.DeviceId) // 针对设备内的统计使用
                .OrderByDescending(x => x.b.CreateTime)
                .Select(s => new DeviceEventLogDto()
                {
                    Sn = s.c != null ? s.c.MachineStickerCode : null,
                    DeviceName = s.b.BaseDeviceName == null && s.c != null ? s.c.MachineStickerCode : s.b.BaseDeviceName,
                    DeviceModelName = s.b.BaseDeviceModelName,
                    EventName = s.b != null ? s.b.OperationName : null,
                    EnterpriseName = s.b.BaseEnterpriseName,
                    OperationTime = s.b != null ? s.b.CreateTime : null
                }).ToPagedListAsync(input);

            var faultCodeDic = new Dictionary<string, string>();
            var EventNames = res.Items.Select(x => x.EventName).Distinct().ToList();
            if (EventNames.Any())
            {
                // 获取所有事件名称对应的多语言code
                foreach (var eventName in EventNames)
                {
                    var faultCode = await faultCodeInfoQueries.GetFaultLanCodeByCode(eventName);
                    if (!string.IsNullOrEmpty(faultCode))
                    {
                        faultCodeDic[eventName] = faultCode;
                    }
                }

                // 如果有多语言code，则替换
                if (faultCodeDic.Count > 0)
                {
                    foreach (var item in res.Items)
                    {
                        if (faultCodeDic.TryGetValue(item.EventName, out var lanCode))
                        {
                            item.EventName = L.Text[lanCode];
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 获取设备异常日志
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceErrorLogDto>> GetDeviceErrorLog(DeviceErrorLogInput input)
        {
            var result =
                from a in _context.DeviceAbnormal
                join b in _context.DeviceBaseInfo on a.DeviceBaseId equals b.Id into ab
                from b in ab.DefaultIfEmpty()

                join c in _context.DeviceInfo on b.Id equals c.DeviceBaseId into bc
                from c in bc.DefaultIfEmpty()

                    //join d in _context.DeviceModel on b.DeviceModelId equals d.Id into bd
                    //from d in bd.DefaultIfEmpty()

                    //join e in _context.EnterpriseInfo on c.EnterpriseinfoId equals e.Id into ce
                    //from e in ce.DefaultIfEmpty()

                select new //DeviceErrorLogDto
                {
                    a,
                    b,
                    c,
                    //d,
                    //e

                    //DeviceId = c.Id,
                    //Sn = b != null ? b.MachineStickerCode : null,
                    //DeviceName = c != null ? c.Name : null,
                    //DeviceModelName = d != null ? d.Name : null,
                    //AbnormalCode = a.Code,
                    //Status = a.Status,
                    //EnterpriseName = e != null ? e.Name : null,
                    //CreateTime = a.CreateTime
                };

            if (!_user.AllDeviceRole)
            {
                result = (from device in result
                          join gd in _context.GroupDevices
                          on device.c.Id equals gd.DeviceInfoId into gdGroup
                          from gd in gdGroup.DefaultIfEmpty()

                          join gu in _context.GroupUsers
                              on gd.GroupsId equals gu.GroupsId into guGroup
                          from gu in guGroup.DefaultIfEmpty()

                          join du in _context.DeviceUserAssociation
                              on device.c.Id equals du.DeviceId into duGroup
                          from du in duGroup.DefaultIfEmpty()

                          where (gu.ApplicationUserId == _user.UserId || du.UserId == _user.UserId)

                          select device).Distinct();

            }

            var data = await result.WhereIf(!string.IsNullOrWhiteSpace(input.ErrorName), w => w.a.Code == input.ErrorName)
                .WhereIf(input.DeviceId != null, w => w.c.Id == input.DeviceId) //针对设备内的统计使用
                .WhereIf(input.Status != null, w => w.a.Status == input.Status)
                .WhereIf(!string.IsNullOrEmpty(input.Code), w => w.a.Code == input.Code)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EnterpriseName), w => w.a.BaseEnterpriseName.Contains(input.EnterpriseName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Sn), w => w.b.MachineStickerCode == input.Sn)
                .WhereIf(input.DeviceModelId != null, w => w.a.BaseDeviceModelId == input.DeviceModelId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), w => w.c.Name.Contains(input.DeviceName))
                .WhereIf(input.DateTimeRange != null && input.DateTimeRange.Count == 2, w => w.a.CreateTime >= input.DateTimeRange[0] && w.a.CreateTime <= input.DateTimeRange[1])
                .Select(s => new DeviceErrorLogDto
                {
                    DeviceId = s.c.Id,
                    Sn = s.b != null ? s.b.MachineStickerCode : null,
                    DeviceName = s.a.BaseDeviceName ?? (s.b != null ? s.b.MachineStickerCode : string.Empty), //s.c != null ? s.c.Name : null,
                    DeviceModelName = s.a.BaseDeviceModelName ?? string.Empty,
                    AbnormalCode = s.a.Code,
                    Status = s.a.Status,
                    EnterpriseName = s.a.BaseDeviceModelName ?? string.Empty,
                    CreateTime = s.a.CreateTime
                })
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);

            var faultCodes = data.Items.Select(x => x.AbnormalCode).Distinct().ToList();
            var faultCodeList = await _context.FaultCodeEntitie.Where(w => faultCodes.Contains(w.Code) || faultCodes.Contains(w.LanCode)).ToListAsync();

            foreach (var item in data.Items)
            {
                item.AbnormalCode = L.Text[item.AbnormalCode] == item.AbnormalCode ? faultCodeList.FirstOrDefault(x => x.Code == item.AbnormalCode || x.LanCode == item.AbnormalCode)?.Name
                    : L.Text[item.AbnormalCode];
            }

            return data;
        }

        /// <summary>
        /// 获取设备base info信息
        /// </summary>
        /// <param name="code">永久编码</param>
        /// <returns></returns>
        public async Task<DeviceBaseInfoForBind> GetDeviceBaseInfo(string code)
        {
            var info = await _context.DeviceBaseInfo.AsQueryable()
                 .Where(w => w.MachineStickerCode == code)
                 .Select(s => new
                 {
                     s.Id,
                     s.DeviceModelId,
                     s.MachineStickerCode
                 }).FirstOrDefaultAsync();
            if (info != null)
            {
                DeviceBaseInfoForBind result = new DeviceBaseInfoForBind();
                result.Id = info.Id;
                result.DeviceModelName = await _context.DeviceModel.AsQueryable().Where(w => w.Id == info.DeviceModelId).Select(s => s.Name).FirstOrDefaultAsync();
                result.MachineStickerCode = info.MachineStickerCode;
                return result;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取清洗日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceFlushComponentsLog>> GetDeviceFlushLog(GetDeviceFlushLogInput input)
        {
            return await _context.DeviceFlushComponentsLog
                .WhereIf(input.DateTimes != null && input.DateTimes.Count == 2, x => x.CreateTime >= input.DateTimes[0] && x.CreateTime <= input.DateTimes[1])
                .WhereIf(input.FlushType != null, x => x.FlushType == input.FlushType)
                .WhereIf(input.DeviceId != null, x => x.DeviceBaseId == input.DeviceId)
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// 获取设备补货日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceRestockLogDto>> GetDeviceRestockLogs(GetDeviceRestockLogInput input)
        {
            var userDevices = await _deviceInfoQueries.GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            return await _context.DeviceRestockLog.Include(x => x.DeviceRestockLogSubs).AsQueryable().AsNoTracking()
                .WhereIf(input.DateTimes != null && input.DateTimes.Count == 2, x => x.CreateTime >= input.DateTimes[0] && x.CreateTime <= input.DateTimes[1])
                .WhereIf(input.DeviceId != null, x => x.DeviceId == input.DeviceId)
                .WhereIf(input.UserId != null, x => x.CreateUserId == input.UserId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.str), x => x.DeviceCode.Contains(input.str) || x.DeviceName.Contains(input.str))
                .Where(x => accessibleDeviceIds.Contains(x.DeviceId))
                .OrderByDescending(x => x.CreateTime)
                .Select(s => new DeviceRestockLogDto
                {
                    Id = s.Id,
                    DeviceId = s.DeviceId,
                    DeviceName = string.IsNullOrWhiteSpace(s.DeviceName) ? s.DeviceCode : s.DeviceName,
                    DeviceCode = s.DeviceCode,
                    Code = s.Code,
                    DeviceDZ = s.DeviceDZ,
                    Type = s.Type,
                    DeviceRestockLogSubs = s.DeviceRestockLogSubs,
                    CreateTime = s.CreateTime,
                    CreateUserName = s.CreateUserName
                })
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// 获取设备补货日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeviceRestockLog> GetDeviceRestockSubLogs(long id)
        {
            return await _context.DeviceRestockLog.Include(x => x.DeviceRestockLogSubs).Where(x => x.Id == id).FirstOrDefaultAsync() ?? new DeviceRestockLog();
        }

        /// <summary>
        /// 获取设备在线状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> GetDeviceOnLineStatus(long id)
        {
            var info = await _context.DeviceBaseInfo.FirstOrDefaultAsync(w => w.Id == id);
            if (info == null)
                return false;
            return info.IsOnline;
        }
    }
}