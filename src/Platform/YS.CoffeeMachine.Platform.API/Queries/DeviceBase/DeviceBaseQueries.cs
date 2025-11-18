using Aop.Api.Domain;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System.Dynamic;
using System.Linq.Dynamic.Core;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.Queries.DeviceBase
{
    /// <summary>
    /// 设备基本信息查询
    /// </summary>
    /// <param name="_db"></param>
    public class DeviceBaseQueries(CoffeeMachinePlatformDbContext _db) : IDeviceBaseQueries
    {
        /// <summary>
        /// GetDeviceCapacityCfgs
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>
        public async Task<List<DeviceCapacityCfg>> GetDeviceCapacityCfgs(long deviceBaseId)
        {
            return await _db.DeviceCapacityCfg.Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// 获取管理列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceVersionManageDto>> GetDeviceVersionManageList(DeviceVersionManageInput input)
        {
            var info = await _db.DeviceVersionManage
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), a => a.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceType), a => a.DeviceType == input.DeviceType)
                .WhereIf(input.DeviceModelId != null, a => a.DeviceModelId == input.DeviceModelId)
                .WhereIf(input.ProgramTypeName != null, a => a.ProgramTypeName == input.ProgramTypeName)
                .WhereIf(input.VersionType != null, a => a.VersionType == input.VersionType)
                .WhereIf(input.CreateTimeRange != null && input.CreateTimeRange.Count == 2, a => a.CreateTime >= input.CreateTimeRange![0] && a.CreateTime <= input.CreateTimeRange[1])
                .WhereIf(input.Enabled != null, w => w.Enabled == EnabledEnum.Enable)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Remark), w => w.Remark.Contains(input.Remark))
                 .Select(a => new DeviceVersionManageDto
                 {
                     Id = a.Id,
                     Name = a.Name,
                     DeviceType = a.DeviceType,
                     DeviceModelId = a.DeviceModelId,
                     DeviceModelName = _db.DeviceModel.Where(x => x.Id == a.DeviceModelId).Select(x => x.Name).FirstOrDefault(),
                     ProgramTypeName = a.ProgramTypeName,
                     VersionType = a.VersionType,
                     Url = a.Url,
                     Remark = a.Remark,
                     Enabled = a.Enabled,
                     PushCount = _db.DeviceVsersionUpdateRecord.Where(x => x.DeviceVersionManageId == a.Id).Count(),
                     CreateUserName = a.CreateUserName,
                     CreateTime = a.CreateTime,
                     VersionNumber = a.VersionNumber,
                     ProgramType = a.ProgramType,
                 })
                 .OrderByDescending(x => x.CreateTime)
                 .ToPagedListAsync(input);

            // 翻译
            //info.Items.ForEach(e =>
            //{
            //    e.DeviceType = L.Text[e.DeviceType];
            //    e.ProgramType = L.Text[e.ProgramType];
            //    e.VersionType = L.Text[e.VersionType];
            //});
            return info;
        }

        /// <summary>
        /// 获取更新记录
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<DeviceVersionUpdateRecordDto>> GetDeviceVersionUpdateRecord(DeviceVersionUpdateRecordInput input)
        {
            var info =
                from a in _db.DeviceBaseInfo
                join b in _db.DeviceInfo on a.Id equals b.DeviceBaseId into ab
                from b in ab.DefaultIfEmpty()
                join c in _db.EnterpriseInfo on b.EnterpriseinfoId equals c.Id into bc
                from c in bc.DefaultIfEmpty()
                select new
                {
                    a.Id,
                    a.MachineStickerCode,
                    DeviceInfoName = b != null ? b.Name : null,
                    a.DeviceModelId,
                    DeviceModelName = _db.DeviceModel.Where(x => x.Id == a.DeviceModelId).Select(x => x.Name).FirstOrDefault(),
                    EnterpriseName = c != null ? c.Name : null,
                    Latest =
                    //(
                    //    from d in _db.DeviceVsersionUpdateRecord
                    //    join e in _db.DeviceVersionManage on d.DeviceVersionManageId equals e.Id into de
                    //    from e in de.DefaultIfEmpty()
                    //    where d.DeviceVersionManageId == a.Id
                    //    orderby d.CreateTime descending
                    //    select new
                    //    {
                    //        e.Name,
                    //        d.CreateTime
                    //    }
                    //).FirstOrDefault()
                    _db.DeviceVsersionUpdateRecord
                    .Where(d => d.DeviceVersionManageId == a.Id)
                    .OrderByDescending(d => d.CreateTime)
                    .Select(d => new { d.Name, d.CreateTime })
                    .FirstOrDefault()
                };
            var result = await info
                .WhereIf(!string.IsNullOrEmpty(input.DeviceNameSn), a => a.MachineStickerCode.Contains(input.DeviceNameSn) || a.DeviceInfoName.Contains(input.DeviceNameSn))
                .WhereIf(input.EnterpriseName != null, a => a.EnterpriseName.Contains(input.EnterpriseName))
                //.WhereIf(input.State != null, a => a.State == input.State)
                .WhereIf(input.UpdateTimeRange != null && input.UpdateTimeRange.Count == 2, a => a.Latest != null && a.Latest.CreateTime >= input.UpdateTimeRange[0] && a.Latest.CreateTime <= input.UpdateTimeRange[1])
                .Select(a => new DeviceVersionUpdateRecordDto
                {
                    DeviceBaseInfoId = a.Id,
                    Sn = a.MachineStickerCode,
                    DeviceName = a.DeviceInfoName,
                    DeviceModelId = a.DeviceModelId,
                    DeviceModelName = a.DeviceModelName,
                    EnterpriseName = a.EnterpriseName,
                    Name = a.Latest != null ? a.Latest.Name : string.Empty,
                    UpdateTime = a.Latest != null ? a.Latest.CreateTime : null
                }).ToPagedListAsync(input);

            return result;
        }

        /// <summary>
        /// 获取更新记录（new）
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<ExpandoObject>> GetDeviceVersionUpdateRecordNew(DeviceVersionUpdateRecordInput input)
        {

            var groupedPage = from a in _db.DeviceBaseInfo
                              join b in _db.DeviceInfo on a.Id equals b.DeviceBaseId into ab
                              from b in ab.DefaultIfEmpty()
                              join d in _db.DeviceModel on a.DeviceModelId equals d.Id into ad
                              from d in ad.DefaultIfEmpty()
                              join e in _db.EnterpriseInfo on b.EnterpriseinfoId equals e.Id into ae
                              from e in ae.DefaultIfEmpty()
                              join f in _db.Dictionary on d.Type equals f.Key into df
                              from f in df.DefaultIfEmpty()
                              select new
                              {
                                  a.Id,
                                  a.MachineStickerCode,
                                  a.Mid,
                                  a.DeviceModelId,
                                  DeviceName = b.Name,
                                  ModelName = d.Name,
                                  EnterpriseId = e != null ? e.Id.ToString() : null,
                                  EnterpriseName = e.Name,
                                  IsOnline = a.IsOnline,
                                  DeviceType = f.Key,
                                  DeviceTypeName = f.Value
                              };
            var groupedPageInfo = await groupedPage
                 .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceNameSn), w => w.DeviceName.Contains(input.DeviceNameSn) || w.MachineStickerCode == input.DeviceNameSn)
                 .WhereIf(!string.IsNullOrEmpty(input.EnterpriseName), w => w.EnterpriseName.Contains(input.EnterpriseName))
                 .WhereIf(input.EnterpriseId != null, w => w.EnterpriseId == input.EnterpriseId.ToString())
                 .WhereIf(input.IsOnline != null, w => w.IsOnline == input.IsOnline)
                 .Distinct()
                 .ToPagedListAsync(input);

            var deviceIds = groupedPageInfo.Items.Select(x => x.Id).ToList();

            //var versionRecords = await _db.DeviceVsersionUpdateRecord
            //    .Where(x => deviceIds.Contains(x.DeviceBaseId))
            //    .ToListAsync();

            var versionRecords = await _db.DeviceVsersionUpdateRecord
                .GroupBy(x => new { x.DeviceBaseId, x.ProgramTypeName })
                .Select(g => g
                    .OrderByDescending(x => x.CreateTime)
                    .FirstOrDefault())
                .ToListAsync();

            var programTypeList = await _db.DictionaryEntity
                .Where(w => w.ParentKey == "ProgramType" && w.IsEnabled == EnabledEnum.Enable)
                .Select(x => x.Key) // 或 x.Value，如果你需要 value 来 pivot
                .ToListAsync();

            var result = new List<ExpandoObject>();

            foreach (var item in groupedPageInfo.Items)
            {
                dynamic row = new ExpandoObject();
                var dict = (IDictionary<string, object>)row;

                // 固定字段
                dict["id"] = item.Id;
                dict["mid"] = item.Mid;
                dict["machineStickerCode"] = item.MachineStickerCode;
                dict["deviceModelId"] = item.DeviceModelId;
                dict["deviceName"] = item.DeviceName;
                dict["modelName"] = item.ModelName;
                dict["enterpriseName"] = item.EnterpriseName;
                dict["isOnline"] = item.IsOnline;
                dict["deviceType"] = item.DeviceType;
                dict["deviceTypeName"] = item.DeviceTypeName;
                dict["enterpriseId"] = item.EnterpriseId;

                var software = await _db.DeviceSoftwareInfo.AsQueryable().Where(w => w.DeviceBaseId == item.Id).ToListAsync();

                // 动态 ProgramType 字段
                foreach (var type in programTypeList)
                {
                    var version = versionRecords.FirstOrDefault(v =>
                        v.DeviceBaseId == item.Id && v.ProgramTypeName == type);

                    var curentVersion = software.Where(w => w.Name == type).Select(s => s.VersionName).FirstOrDefault();
                    if (curentVersion != null && curentVersion != version?.Name)
                    {
                        if (version?.Name == null)
                        {
                            dict[type] = curentVersion;
                        }
                        else
                        {
                            dict[type] = "当前版本：" + curentVersion + "\n下推版本：" + version?.Name;
                        }
                    }
                    else
                    {
                        dict[type] = version?.Name ?? "";
                    }

                }

                result.Add(row);
            }
            PagedResultDto<ExpandoObject> tt = new PagedResultDto<ExpandoObject>();
            tt.Items = result;
            tt.TotalCount = groupedPageInfo.TotalCount;
            tt.PageNumber = groupedPageInfo.PageNumber;
            tt.PageSize = groupedPageInfo.PageSize;
            //tt.TotalPages = groupedPage.TotalPages;
            return tt;
        }

        /// <summary>
        /// 更新记录（根据版本）
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<PushDataByVersionDto>> GetPushDataByVersion(PushDataByVersionInput input)
        {
            var query =
                from a in _db.DeviceVsersionUpdateRecord
                join b in _db.DeviceInfo on a.DeviceBaseId equals b.DeviceBaseId into ab
                from b in ab.DefaultIfEmpty()
                join c in _db.DeviceBaseInfo on a.DeviceBaseId equals c.Id into ac
                from c in ac.DefaultIfEmpty()
                join d in _db.EnterpriseInfo on b.EnterpriseinfoId equals d.Id into bd
                from d in bd.DefaultIfEmpty()
                join e in _db.DeviceVersionManage on a.DeviceVersionManageId equals e.Id into ae
                from e in ae.DefaultIfEmpty()
                where a.DeviceVersionManageId == input.DeviceVersionManageId
                select new
                {
                    a,
                    b,
                    c,
                    d,
                    e
                    //DeviceName = b != null ? b.Name : null,
                    //MachineStickerCode = c != null ? c.MachineStickerCode : null,
                    //VersionName = e != null ? e.Name : null,
                    //EnterpriseName = d != null ? d.Name : null,
                    //CreateTime = a.CreateTime,
                    //CreateUserName = a.CreateUserName
                };

            return await query.WhereIf(!string.IsNullOrEmpty(input.DeviceNameSn), a => a.b.Name.Contains(input.DeviceNameSn) || a.c.MachineStickerCode.Contains(input.DeviceNameSn))
                .WhereIf(input.PushTimeRange != null && input.PushTimeRange.Count == 2, a => a.a.CreateTime >= input.PushTimeRange[0] && a.a.CreateTime <= input.PushTimeRange[1])
                .Select(x => new PushDataByVersionDto
                {
                    DeviceName = x.b != null ? x.b.Name : null,
                    MachineStickerCode = x.c != null ? x.c.MachineStickerCode : null,
                    VersionName = x.e != null ? x.e.Name : null,
                    EnterpriseName = x.d != null ? x.d.Name : null,
                    CreateTime = x.a.CreateTime,
                    CreateUserName = x.a.CreateUserName,
                    PushState = x.a.PushState,
                    ProgramTypeName = x.a.ProgramTypeName
                })
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// 更新记录(根据设备)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PushDataByDeviceDto>> GetPushDateByDevice(PushDataByDeviceInput input)
        {
            return await _db.DeviceVsersionUpdateRecord.AsQueryable().Where(a => a.DeviceBaseId == input.DeviceBaseInfoId)
                .Select(a => new PushDataByDeviceDto
                {
                    Type = a.Type,
                    Name = a.Name,
                    ProgramType = a.ProgramType ?? 0,
                    VersionType = a.VersionType ?? 0,
                    CreateTime = a.CreateTime,
                    CreateUserName = a.CreateUserName,
                    PushState = a.PushState,
                    ProgramTypeName = a.ProgramTypeName
                })
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// GetDeviceMetrics
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <returns></returns>

        public async Task<List<DeviceMetrics>> GetDeviceMetrics(long deviceBaseId)
        {
            return await _db.DeviceMetrics.Where(x => x.DeviceBaseId == deviceBaseId).ToListAsync();
        }

        /// <summary>
        /// 获取其他信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<List<DeviceOtherMsgDto>> GetDeviceOtherMsg(long deviceId)
        {
            //var deviceInfo = await _db.DeviceInfo.AsQueryable().IgnoreQueryFilters().Where(a => a.Id == deviceId).SingleOrDefaultAsync();

            // 软件版本信息
            var softInfo = await _db.DeviceSoftwareInfo.AsQueryable().Where(a => a.DeviceBaseId == deviceId).ToListAsync();

            // 网络信息+屏幕
            var attrInfo = await _db.DeviceAttribute
                .Where(d => d.DeviceBaseId == deviceId)
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
        /// 设备统计
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetDeivceCount()
        {
            return new
            {
                AllDeviceCount = await _db.DeviceBaseInfo.CountAsync(),
                NotOnlineCount = await _db.DeviceBaseInfo.Where(x => x.IsOnline == false).CountAsync(),
                ErrDeviceCount = await _db.DeviceAbnormal.Where(x => x.Status == false).Select(x => x.DeviceBaseId).Distinct().CountAsync()
            };
        }
    }
}
