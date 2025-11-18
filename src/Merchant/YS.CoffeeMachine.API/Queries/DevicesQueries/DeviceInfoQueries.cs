using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Util.Core;
using YSCore.Base.Exceptions;
using YSCore.Base.LinqBuilder.Extensions;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;
using static YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys.PromotionOutput;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 设备信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class DeviceInfoQueries(CoffeeMachineDbContext context, IMapper mapper, UserHttpContext _user) : IDeviceInfoQueries
    {
        /// <summary>
        /// 获取设备下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceSelectDto>> GetDeviceSelectListAsync()
        {
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            //var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any())
                return new List<DeviceSelectDto>();

            return await (from device in context.DeviceInfo.AsNoTracking()
                          join baseInfo in context.DeviceBaseInfo.AsNoTracking()
                              on device.DeviceBaseId equals baseInfo.Id into grouping
                          from baseInfo in grouping.DefaultIfEmpty()
                          where accessibleDeviceIds.Contains(device.Id)
                          select new DeviceSelectDto
                          {
                              Id = baseInfo != null ? baseInfo.Id : null,
                              DeviceId = device.Id,
                              Name = device.Name,
                              Code = baseInfo != null ? baseInfo.MachineStickerCode : null
                          }).ToListAsync();
        }

        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeviceInfoDto> GetDeviceInfoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var deviceModel = await context.DeviceModel.AsNoTracking().FirstOrDefaultAsync(w => w.Id == info.DeviceModelId);
            var groupList = await context.Groups.AsNoTracking().Include(i => i.Devices).Where(w => w.Devices.Select(s => s.DeviceInfoId).Contains(info.Id)).ToListAsync();
            var dto = mapper.Map<DeviceInfoDto>(info);
            dto.DeviceModelText = deviceModel?.Name;
            dto.GroupIds = groupList.Select(s => s.Id).ToList();
            dto.GroupsText = string.Join(",", groupList.Select(s => s.Name).ToList());
            dto.SettingInfo = mapper.Map<SettingInfoDto>(await context.SettingInfo.AsNoTracking().Include(i => i.MaterialBoxs).FirstOrDefaultAsync(x => x.DeviceId == info.Id));
            return dto;
        }

        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeviceInfoDto> GetDeviceAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var deviceModel = await context.DeviceModel.AsNoTracking().FirstOrDefaultAsync(w => w.Id == info.DeviceModelId);
            var deviceBase = await context.DeviceBaseInfo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == info.DeviceBaseId);
            var dto = mapper.Map<DeviceInfoDto>(info);
            dto.DeviceModelText = deviceModel?.Name;
            dto.IsOnline = deviceBase.IsOnline;
            dto.Mid = deviceBase.Mid;
            dto.MachineStickerCode = deviceBase.MachineStickerCode;
            dto.UpdateOnlineTime = deviceBase.UpdateOnlineTime;
            dto.UpdateOfflineTime = deviceBase.UpdateOfflineTime;
            return dto;
        }

        /// <summary>
        /// 根据设备Id获取设备信息
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        public async Task<List<Domain.AggregatesModel.Devices.DeviceInfo>> GetDeviceInfoByMids(List<string> mids)
        {
            return await context.DeviceInfo.AsNoTracking().Where(x => mids.Contains(x.Mid)).ToListAsync();
        }

        /// <summary>
        /// H5首页统计
        /// </summary>
        /// <returns></returns>
        public async Task<SyCountOutput> GetSyCount(List<DateTime> times)
        {
            var rsp = new SyCountOutput();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any())
            {
                return rsp;
            }

            rsp.DeviceCount = accessibleDeviceIds.Count;

            var query = context.OrderInfo.AsNoTracking().Where(x => accessibleDeviceBaseIds.Contains(x.DeviceBaseId)
            && x.CreateTime >= times[0] && x.CreateTime <= times[1] && x.OrderType != OrderTypeEnum.Not);

            rsp.OrderCount = await query.CountAsync();

            rsp.TransactionAmount = await query
                .Where(x => x.OrderStatus == OrderStatusEnum.Success || x.OrderStatus == OrderStatusEnum.Refunding || x.OrderStatus == OrderStatusEnum.PartialRefund || x.OrderStatus == OrderStatusEnum.FullRefund)
                .SumAsync(x => x.Amount);

            rsp.RefundOrderCount = await query.SumAsync(x => x.ReturnAmount);

            return rsp;
        }

        /// <summary>
        /// 统计指定月份的每天营业收入（收入与退款金额）
        /// </summary>
        /// <param name="date">指定月份中的任意一天</param>
        /// <returns>指定月份每天的营业收入统计</returns>
        public async Task<List<OperatingRevenueOutput>> GetOperatingRevenueByMonth(List<DateTime> times, int offset)
        {
            var result = new List<OperatingRevenueOutput>();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any())
            {
                return result;
            }

            DateTime startOfMonth = times[0];
            DateTime endOfMonth = times[1];

            var monthOrders = await context.OrderInfo
                .Where(o => o.PayDateTime >= startOfMonth && o.PayDateTime <= endOfMonth
                && accessibleDeviceBaseIds.Contains(o.DeviceBaseId))
                .ToListAsync();

            int daysInMonth = (endOfMonth - startOfMonth).Days;

            // 按天分组统计
            for (int day = 0; day <= daysInMonth; day++)
            {
                DateTime currentDay = startOfMonth.AddDays(day);
                var currentDayEnd = currentDay.AddHours(24);

                var dayOrders = monthOrders
                    .Where(o => o.PayDateTime >= currentDay && o.PayDateTime <= currentDayEnd)
                    .ToList();

                decimal income = dayOrders
                    .Where(o => (o.SaleResult == OrderSaleResult.Success || o.SaleResult == OrderSaleResult.PartialRefund) && o.ShipmentResult != OrderShipmentResult.Fail)
                    .Sum(o => o.Amount - o.ReturnAmount);

                decimal refund = dayOrders.Sum(o => o.ReturnAmount);

                result.Add(new OperatingRevenueOutput
                {
                    time = currentDay.AddHours(offset),
                    Income = income,
                    Refund = refund,
                });
            }

            return result;
        }

        /// <summary>
        /// 今日盈利分析
        /// </summary>
        /// <returns></returns>
        public async Task<List<HourlyRevenueStats>> GetHourlyRevenueStatsFromDbAsync(List<DateTime> times, int offset, int hoursPerSlot = 1)
        {
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any() || times == null || times.Count < 2)
            {
                return new List<HourlyRevenueStats>();
            }

            var startDate = times[0];
            var endDate = times[1];

            var orders = await context.OrderInfo
                .Where(w => w.CreateTime >= startDate && w.CreateTime < endDate &&
                           accessibleDeviceBaseIds.Contains(w.DeviceBaseId)
                           && ((w.OrderType == OrderTypeEnum.OnlineOrder
                    && w.OrderStatus != OrderStatusEnum.PaymentInProgress
                    && w.OrderStatus != OrderStatusEnum.Fail
                    && w.OrderStatus != OrderStatusEnum.CancelPayment)
                    || (w.OrderType != OrderTypeEnum.Not && w.OrderType != OrderTypeEnum.OnlineOrder)))
                .Select(o => new
                {
                    o.CreateTime,
                    o.Amount,
                    o.ReturnAmount
                })
                .ToListAsync();

            var hourlyStats = orders
                .Select(o => new
                {
                    Order = o,
                    LocalHour = o.CreateTime.AddHours(offset).Hour
                })
                .GroupBy(x => x.LocalHour / hoursPerSlot)
                .Select(g => new
                {
                    SlotIndex = g.Key,
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(x => x.Order.Amount - x.Order.ReturnAmount)
                })
                .ToList();

            // 生成动态时间段
            int totalSlots = 24 / hoursPerSlot;
            var allTimeSlots = Enumerable.Range(0, totalSlots).Select(slotIndex =>
            {
                var startHour = slotIndex * hoursPerSlot;
                var endHour = startHour + hoursPerSlot;
                return new
                {
                    SlotIndex = slotIndex,
                    StartHour = startHour,
                    EndHour = endHour,
                    Label = $"{startHour:00}:00-{endHour:00}:00"
                };
            });

            // 构建结果
            var result = allTimeSlots.Select(slot =>
            {
                var existingStat = hourlyStats.FirstOrDefault(s => s.SlotIndex == slot.SlotIndex);
                return new HourlyRevenueStats
                {
                    TimeSlot = slot.Label,
                    OrderCount = existingStat?.OrderCount ?? 0,
                    TotalRevenue = existingStat?.TotalRevenue ?? 0,
                    StartHour = slot.StartHour,
                    EndHour = slot.EndHour,
                };
            }).OrderBy(r => r.StartHour).ToList();

            return result;
        }

        /// <summary>
        /// H5设备统计
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceCountOutput> GetDeviceCountOutput()
        {
            var rsp = new DeviceCountOutput();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any())
            {
                return rsp;
            }
            rsp.DeviceCount = accessibleDeviceIds.Count;

            var validWarnings = context.DeviceEarlyWarnings.Where(w => w.WarningType == EarlyWarningTypeEnum.ShortageWarning
                                && w.DeviceMaterialId.HasValue
                                && !string.IsNullOrEmpty(w.WarningValue)
                                && w.IsOn == true
                                && accessibleDeviceBaseIds.Contains(w.DeviceBaseId));

            var shortageMaterials = from warning in validWarnings
                                    join material in context.DeviceMaterialInfo
                                        on warning.DeviceMaterialId.Value equals material.Id
                                    let warningValue = Convert.ToInt32(warning.WarningValue)
                                    where material.Stock < warningValue
                                    select new
                                    {
                                        DeviceBaseId = warning.DeviceBaseId,
                                        DeviceMaterialId = material.Id,
                                        MaterialStock = material.Stock,
                                        WarningValue = warningValue
                                    };

            rsp.DeviceNotStockCount = await shortageMaterials
                         .Select(x => x.DeviceBaseId)
                         .Distinct()
                         .CountAsync();

            rsp.DeviceActionCount = userDevices.Count(x => x.DeviceActiveState == null || x.DeviceActiveState == DeviceActiveEnum.NoActive);

            rsp.DeviceOnlineCount = userDevices.Count(x => x.IsOnline == true);

            return rsp;
        }

        /// <summary>
        /// 获取用户所有设备
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allDeviceRole"></param>
        /// <returns></returns>
        public async Task<List<DeviceUserDto>> GetDeviceByUser(long? userId = null, bool? allDeviceRole = null)
        {
            if (userId == null || userId == 0)
                userId = _user.UserId;
            if (allDeviceRole == null)
                allDeviceRole = _user.AllDeviceRole;

            List<long> accessibleDeviceIds;

            if (allDeviceRole == true)
            {
                accessibleDeviceIds = await context.DeviceInfo
                    .Select(d => d.Id)
                    .ToListAsync();
            }
            else
            {
                var groupDeviceIds = await (from gu in context.GroupUsers
                                            where gu.ApplicationUserId == userId
                                            join gd in context.GroupDevices on gu.GroupsId equals gd.GroupsId
                                            select gd.DeviceInfoId)
                                          .Distinct()
                                          .ToListAsync();

                var directDeviceIds = await (from du in context.DeviceUserAssociation
                                             where du.UserId == userId
                                             select du.DeviceId)
                                           .Distinct()
                                           .ToListAsync();

                accessibleDeviceIds = groupDeviceIds.Union(directDeviceIds).Distinct().ToList();
            }

            var result = await (from device in context.DeviceInfo
                                where accessibleDeviceIds.Contains(device.Id)
                                join baseInfo in context.DeviceBaseInfo
                                    on device.DeviceBaseId equals baseInfo.Id into grouping
                                from baseInfo in grouping.DefaultIfEmpty()
                                select new DeviceUserDto()
                                {
                                    DeviceId = device.Id,
                                    BaseInfoId = baseInfo.Id,
                                    DeviceName = device.Name ?? string.Empty,
                                    DeviceCode = baseInfo.MachineStickerCode ?? string.Empty,
                                    IsOnline = baseInfo.IsOnline,
                                    DeviceActiveState = device.DeviceActiveState
                                })
                       .OrderBy(d => d.DeviceName)
                               .ToListAsync();

            return result;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListAsync(DevicesListInput request)
        {

            var query01 = from device in context.DeviceInfo
                          join baseInfo in context.DeviceBaseInfo
                              on device.DeviceBaseId equals baseInfo.Id into grouping
                          from baseInfo in grouping.DefaultIfEmpty()
                          join deviceModel in context.DeviceModel
                              on device.DeviceModelId equals deviceModel.Id into modelJoin
                          from deviceModel in modelJoin.DefaultIfEmpty()
                          select new
                          {
                              device.Id,
                              device.Name,
                              device.POSMachineNumber,
                              device.CountryId,
                              device.CountryRegionIds,
                              device.CountryRegionText,
                              device.DetailedAddress,
                              device.DeviceModelId,
                              device.DeviceBaseId,
                              baseInfo.Mid,
                              baseInfo.MachineStickerCode,
                              baseInfo.VersionNumber,
                              baseInfo.UpdateOfflineTime,
                              baseInfo.UpdateOnlineTime,
                              baseInfo.IsOnline,
                              device.CreateTime,
                              device.UsageScenario,
                              deviceModelName = deviceModel.Name,
                              device.ActiveTime,
                              device.Province,
                              device.City,
                              device.District,
                              device.Street,
                              device.Lat,
                              device.Lng
                          };

            if (!_user.AllDeviceRole)
            {
                query01 = (from device in query01
                           join gd in context.GroupDevices
                       on device.Id equals gd.DeviceInfoId into gdGroup
                           from gd in gdGroup.DefaultIfEmpty()

                           join gu in context.GroupUsers
                               on gd.GroupsId equals gu.GroupsId into guGroup
                           from gu in guGroup.DefaultIfEmpty()

                           join du in context.DeviceUserAssociation
                               on device.Id equals du.DeviceId into duGroup
                           from du in duGroup.DefaultIfEmpty()

                           where (gu.ApplicationUserId == _user.UserId || du.UserId == _user.UserId)

                           select device).Distinct();
            }

            //.WhereIf(!_user.AllDeviceRole, a => context.DeviceUserAssociation.Any(b => a.Id == b.DeviceId && b.UserId == _user.UserId))
            var info = await query01
            .WhereIf(!string.IsNullOrWhiteSpace(request.deviceName_Number), a => a.Name.Contains(request.deviceName_Number) || a.MachineStickerCode.Contains(request.deviceName_Number))
            .WhereIf(request.deviceModelId != null, a => a.DeviceModelId == request.deviceModelId)
            .WhereIf(request.status != null, a => a.IsOnline == (request.status == DeviceStatusEnum.Online ? true : false))
            .WhereIf(request.timeRange.Count > 0 && request.timeRange.Count == 2 && DateTime.TryParse(request.timeRange[0], out _) && DateTime.TryParse(request.timeRange[1], out _),
            a => a.CreateTime >= Convert.ToDateTime(request.timeRange[0]) && a.CreateTime <= Convert.ToDateTime(request.timeRange[1]))
            .WhereIf(request.groupIds.Count > 0, a => context.GroupDevices.Any(b => a.Id == b.DeviceInfoId && request.groupIds.Contains(b.GroupsId)))
            .Select(a => new DeviceInfoDto
            {
                Id = a.Id,
                DeviceBaseInfoId = a.DeviceBaseId,
                Name = a.Name,
                POSMachineNumber = a.POSMachineNumber,
                CountryId = a.CountryId,
                CountryRegionIds = a.CountryRegionIds,
                CountryRegionText = a.Lat == null ? a.CountryRegionText : a.Province + "/" + a.City + "/" + a.District + "/" + a.Street + "/" + a.DetailedAddress,
                DetailedAddress = a.DetailedAddress,
                DeviceModelId = a.DeviceModelId,
                Mid = a.Mid,
                MachineStickerCode = a.MachineStickerCode,
                VersionNumber = a.VersionNumber,
                UpdateOfflineTime = a.UpdateOfflineTime,
                UpdateOnlineTime = a.UpdateOnlineTime,
                IsOnline = a.IsOnline,
                CreateTime = a.CreateTime,
                UsageScenario = a.UsageScenario,
                DeviceModelName = a.deviceModelName,
                Status = a.IsOnline ? "在线" : "离线",
                ActiveTime = a.ActiveTime,
                Province = a.Province,
                City = a.City,
                District = a.District,
                Street = a.Street,
                Lat = a.Lat,
                Lng = a.Lng
            }).ToPagedListAsync(request);

            var deviceIds = info.Items.Select(s => s.Id).ToList();

            var result = await context.GroupDevices
                    .Where(gd => deviceIds.Contains(gd.DeviceInfoId))
                    .Join(context.Groups,
                        groupDevice => groupDevice.GroupsId,
                        group => group.Id,
                        (groupDevice, group) => new
                        {
                            group.Id,
                            group.Name,
                            groupDevice.DeviceInfoId
                        })
                    .ToListAsync();

            var countryIds = info.Items.Select(s => s.CountryId).Distinct().ToList();
            var contryDic = context.CountryInfo.AsQueryable().Where(w => countryIds.Contains(w.Id)).ToDictionary(a => a.Id, a => a.CountryName);

            var deviceBaseInfoIds = info.Items.Select(s => s.DeviceBaseInfoId).Distinct().ToList();
            #region 获取制杯数

            Dictionary<long, int> deviceBaseInfoMakeCounts = new Dictionary<long, int>();
            if (request.DateRange != null)
            {
                deviceBaseInfoMakeCounts = await (
                    from a in context.OrderInfo
                    join b in context.OrderDetails on a.Id equals b.OrderId into ab
                    from b in ab.DefaultIfEmpty()
                    where deviceBaseInfoIds.Contains(a.DeviceBaseId) && a.CreateTime >= request.DateRange[0] && a.CreateTime <= request.DateRange[1]
 //        && a.SaleResult != OrderSaleResult.Cancel && a.SaleResult != OrderSaleResult.Fail && a.SaleResult != OrderSaleResult.Timeout && a.SaleResult != OrderSaleResult.NotPay
 //&& a.SaleResult != OrderSaleResult.Refund
 && (
 (a.ShipmentResult == OrderShipmentResult.Success && (a.SaleResult == OrderSaleResult.Success || a.SaleResult == OrderSaleResult.Refund || a.SaleResult == OrderSaleResult.PartialRefund))
 ||
 (a.ShipmentResult == OrderShipmentResult.Fail && a.SaleResult == OrderSaleResult.Refund)
 )
                    group b by a.DeviceBaseId into g
                    select new
                    {
                        DeviceBaseId = g.Key,
                        Count = g.Count(x => x != null)
                    }
                ).ToDictionaryAsync(d => d.DeviceBaseId, d => d.Count);
            }

            #endregion

            #region 获取主程序版本号
            var softwareDic = await context.DeviceSoftwareInfo.AsQueryable().Where(w => deviceBaseInfoIds.Contains(w.DeviceBaseId) && w.Name == "com.tcn.vending").ToDictionaryAsync(d => d.DeviceBaseId, d => d.VersionName);
            #endregion

            foreach (var item in info.Items)
            {
                item.GroupIds = result.Where(w => w.DeviceInfoId == item.Id).Select(s => s.Id).ToList();
                item.GroupsText = string.Join(",", result.Where(w => w.DeviceInfoId == item.Id).Select(s => s.Name).ToList());
                item.CountryText = item.CountryId == null ? null : contryDic[item.CountryId ?? 0];
                item.UsageScenarioText = item.UsageScenario == null ? null : item.UsageScenario.GetDescription();

                if (deviceBaseInfoMakeCounts.TryGetValue(item.DeviceBaseInfoId, out int makeCount))
                    item.MakeCount = makeCount;

                if (softwareDic.TryGetValue(item.DeviceBaseInfoId, out string softwareName))
                    item.VersionNumber = softwareName;
            }

            return info;

            if (request.enterpriseinfoId <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            //条件过滤 || w.EquipmentNumber.Contains(request.deviceName_Number)
            var query = context.DeviceInfo.IgnoreQueryFilters().AsQueryable().Include(i => i.GroupDevices).Include(i => i.DeviceUserAssociations).Where(w => w.EnterpriseinfoId == request.enterpriseinfoId && !w.IsDelete);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(request.deviceName_Number), w => w.Name.Contains(request.deviceName_Number))
                .WhereIf(!_user.AllDeviceRole, w => w.DeviceUserAssociations.Where(s => s.UserId == _user.UserId).Count() > 0)
                .WhereIf(request.deviceModelId != null, w => w.DeviceModelId == request.deviceModelId)
                .WhereIf(request.status != null && request.status != 0, w => (w.DeviceStatus & request.status) != 0)//查询包含当前搜索状态的数据
                .WhereIf(request.status != null && request.status == 0, w => w.DeviceStatus == DeviceStatusEnum.Offline)//设备离线状态筛选
                .WhereIf(request.groupIds.Count > 0, w => w.GroupDevices.Where(w => request.groupIds.Contains(w.GroupsId)).Select(s => s.DeviceInfoId).ToList().Contains(w.Id))//设备分组筛选
                .WhereIf(request.timeRange.Count > 0 && request.timeRange.Count == 2 && DateTime.TryParse(request.timeRange[0], out _) && DateTime.TryParse(request.timeRange[1], out _), w => w.CreateTime >= Convert.ToDateTime(request.timeRange[0]) && w.CreateTime <= Convert.ToDateTime(request.timeRange[1]));

            //获取分页数据
            var deviceModels = await context.DeviceModel.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
            request.IsIncludeQueries = true;
            var list = await query.ToPagedListAsync(request);

            //获取平台设备基础信息Ids
            var deviceBaseIds = list.Items.Select(s => s.DeviceBaseId).ToList();

            //获取设备基础信息
            var deviceBases = await context.DeviceBaseInfo.AsNoTracking().Where(w => deviceBaseIds.Contains(w.Id)).ToListAsync();

            //获取设备分组Ids
            var groupIds = list.Items.SelectMany(s => s.GroupDevices ?? new List<GroupDevices>()).Select(s => s.GroupsId).ToList();

            //获取设备分组信息
            var groups = await context.Groups.Include(i => i.Devices).AsNoTracking().Where(w => groupIds.Contains(w.Id)).ToListAsync();

            //获取设备分组下的设备信息
            var groupsDevices = groups.SelectMany(s => s.Devices).ToList();
            var listDto = new DeviceInfoListDto();

            //组装Dto
            list.Items.ForEach(s =>
            {
                var curGD = groupsDevices.Where(w => w.DeviceInfoId == s.Id).ToList();
                var curDto = mapper.Map<DeviceInfoDto>(s);
                var curdmodel = deviceModels.FirstOrDefault(w => w.Id == s.DeviceModelId);
                if (curdmodel != null && !string.IsNullOrWhiteSpace(curdmodel.Name))
                    curDto.DeviceModelText = curdmodel.Name;
                curDto.GroupIds = curGD.Select(s => s.GroupsId).ToList();
                curDto.GroupsText = string.Join(",", groups.Where(w => curDto.GroupIds.Contains(w.Id)).Select(s => s.Name).ToList());
                if (s.DeviceUserAssociations != null && s.DeviceUserAssociations.Count > 0)
                    curDto.UserIds = s.DeviceUserAssociations.Select(s => s.UserId).ToList();

                var devicebaseInfo = deviceBases.FirstOrDefault(w => w.Id == s.DeviceBaseId);
                if (devicebaseInfo != null)
                    AddDeviceBaseInfoToDto(curDto, devicebaseInfo!);
                listDto.DeviceInfoList.Add(curDto);
            });

            //封装方法将设备基础信息添加到Dto中
            void AddDeviceBaseInfoToDto(DeviceInfoDto dto, DeviceBaseInfo deviceInfo)
            {
                var deviceBase = deviceBases.FirstOrDefault(w => w.Id == deviceInfo.Id);
                if (deviceBase != null)
                {
                    dto.Mid = deviceBase.Mid;
                    dto.MachineStickerCode = deviceBase.MachineStickerCode;
                    dto.BoxId = deviceBase.BoxId;
                    dto.DeviceModelId = deviceBase.DeviceModelId;
                    dto.IsLeaveFactory = deviceBase.IsLeaveFactory;
                    dto.SSID = deviceBase.SSID!;
                    dto.HardwareCapability = deviceBase.HardwareCapability;
                    dto.SoftwareCapability = deviceBase.SoftwareCapability;
                    dto.MAC = deviceBase.MAC!;
                    dto.VersionNumber = deviceBase.VersionNumber!;
                    dto.SkinPluginVersion = deviceBase.SkinPluginVersion!;
                    dto.LanguagePack = deviceBase.LanguagePack!;
                    dto.SoftwareUpdateLastTime = deviceBase.SoftwareUpdateLastTime;
                    dto.IsOnline = deviceBase.IsOnline;
                    dto.UpdateOnlineTime = deviceBase.UpdateOnlineTime;
                    dto.UpdateOfflineTime = deviceBase.UpdateOfflineTime;
                }
            }

            PagedResultDto<DeviceInfoDto> pagedResultDto = new PagedResultDto<DeviceInfoDto>()
            {
                Items = listDto.DeviceInfoList,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 获取设备H5列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceH5Dto>> GetDeviceInfoH5ListAsync(DevicesH5ListInput request)
        {
            var userDevices = await GetDeviceByUser();

            if (!userDevices.Any())
            {
                return new PagedResultDto<DeviceH5Dto>();
            }

            var userDeviceIds = userDevices.Select(d => d.DeviceId).ToList();

            var query = from device in context.DeviceInfo
                        where userDeviceIds.Contains(device.Id)
                        join devicebase in context.DeviceBaseInfo on device.DeviceBaseId equals devicebase.Id into deviceGroup
                        from devicebase in deviceGroup.DefaultIfEmpty()
                            // 在 select 之前过滤，避免不必要的 Materials 查询
                        where (string.IsNullOrWhiteSpace(request.deviceName_Number) ||
                              devicebase.MachineStickerCode.Contains(request.deviceName_Number) ||
                              device.Name.Contains(request.deviceName_Number)) && device.DeviceActiveState == DeviceActiveEnum.Active
                        select new
                        {
                            Device = device,
                            DeviceBase = devicebase,
                            Materials = (from m in context.DeviceMaterialInfo
                                         where m.DeviceBaseId == devicebase.Id
                                         join w in context.DeviceEarlyWarnings on m.Id equals w.DeviceMaterialId into warnings
                                         from w in warnings.DefaultIfEmpty()
                                         select new { m, w }).ToList()
                        };

            return await query.Select(x => new DeviceH5Dto()
            {
                DeviceBaseId = x.DeviceBase != null ? x.DeviceBase.Id : null,
                DeviceCode = x.DeviceBase != null ? x.DeviceBase.MachineStickerCode ?? null : null,
                DeviceId = x.Device.Id,
                Name = x.Device.Name,
                Mid = x.DeviceBase != null ? x.DeviceBase.Mid : null,
                IsQh = x.Materials.Any(m => m.m.Stock < Convert.ToInt32(m.w.WarningValue)),
                IsOnline = x.DeviceBase != null ? x.DeviceBase.IsOnline : false,
            })
            .Distinct()
            .ToPagedListAsync(request);
        }

        /// <summary>
        /// 根据设备Id获取当前设备下，同型号的设备列表（不包含当前设备）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceIdAsync(DeviceInfoListByDeviceIdInput request)
        {
            //获取当前设备
            var deviceInfo = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            //当前企业
            var curTenantId = _user.TenantId;
            if (deviceInfo.EnterpriseinfoId != curTenantId)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0065)]);
            //获取设备型号
            var deviceModel = await context.DeviceModel.AsNoTracking().FirstOrDefaultAsync(w => w.Id == deviceInfo.DeviceModelId);
            if (deviceModel == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0059)]);
            //获取当前用户
            //获取当前企业下的所有设备
            var result = new PagedResultDto<DeviceInfo>();
            //如果是超级管理员，获取当前企业下的所有设备
            //if (_user.SuperAdmin)
            if (_user.AllDeviceRole)
                result = await context.DeviceInfo.AsNoTracking().Include(i => i.BeverageInfos).Include(i => i.GroupDevices)
                    .Where(w => w.EnterpriseinfoId == curTenantId && w.DeviceModelId == deviceModel.Id && w.Id != request.deviceId)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.deviceName_SN), w => w.Name.Contains(request.deviceName_SN))//设备名称或sn模糊查询 || w.EquipmentNumber.Contains(request.deviceName_SN)
                    .WhereIf(request.groupIds.Count > 0, w => w.GroupDevices.Where(w => request.groupIds.Contains(w.GroupsId)).Select(s => s.DeviceInfoId).ToList().Contains(w.Id))//设备分组筛选
                    .ToPagedListAsync(request);
            //如果是普通用户，获取当前用户所属的设备
            else
                result = await context.DeviceInfo.AsNoTracking().Include(i => i.BeverageInfos)
                    .Where(w => w.EnterpriseinfoId == curTenantId && w.DeviceModelId == deviceModel.Id && w.Id != request.deviceId
                    && (w.DeviceUserAssociations.Select(s => s.UserId).Contains(_user.UserId)
                    || (context.GroupDevices.Any(b => b.DeviceInfoId == w.Id && context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId)))))
                    .WhereIf(!string.IsNullOrWhiteSpace(request.deviceName_SN), w => w.Name.Contains(request.deviceName_SN))//设备名称或sn模糊查询 || w.EquipmentNumber.Contains(request.deviceName_SN)
                    .WhereIf(request.groupIds.Count > 0, w => w.GroupDevices.Where(w => request.groupIds.Contains(w.GroupsId)).Select(s => s.DeviceInfoId).ToList().Contains(w.Id))//设备分组筛选
                    .ToPagedListAsync(request);

            var deviceIds = result.Items.Where(a => a.DeviceBaseId != 0 && a.DeviceBaseId != null).Select(a => a.DeviceBaseId).ToList();
            var deviceBaseInfoDic = await context.DeviceBaseInfo.AsQueryable().Where(w => deviceIds.Contains(w.Id)).ToDictionaryAsync(a => a.Id, a => a);

            //返回分页信息
            var DeviceInfoDtos = mapper.Map<List<DeviceInfoDto>>(result.Items);
            DeviceInfoDtos.ForEach(x =>
            {
                x.BeveragesList = x.BeverageInfos == null
                ? new List<object>() :
                x.BeverageInfos.Select(s => new { s.Id, s.Name, s.Code, s.CodeIsShow }).Cast<object>().ToList();
                x.BeverageInfos = null;
                x.DeviceModelText = deviceModel.Name;
                x.Mid = deviceBaseInfoDic.Count == 0 ? string.Empty : (!deviceBaseInfoDic.ContainsKey(x.DeviceBaseId) ? string.Empty : deviceBaseInfoDic[x.DeviceBaseId].Mid);
                x.MachineStickerCode = deviceBaseInfoDic.Count == 0 ? string.Empty : (!deviceBaseInfoDic.ContainsKey(x.DeviceBaseId) ? string.Empty : deviceBaseInfoDic[x.DeviceBaseId].MachineStickerCode);
                x.IsOnline = deviceBaseInfoDic.Count == 0 ? false : (!deviceBaseInfoDic.ContainsKey(x.DeviceBaseId) ? false : deviceBaseInfoDic[x.DeviceBaseId].IsOnline);
                x.Name = !string.IsNullOrWhiteSpace(x.Name) ? x.Name : deviceBaseInfoDic.Count == 0 ? string.Empty : (!deviceBaseInfoDic.ContainsKey(x.DeviceBaseId) ? string.Empty : deviceBaseInfoDic[x.DeviceBaseId].MachineStickerCode);
            });
            var pagedResultDto = new PagedResultDto<DeviceInfoDto>()
            {
                Items = DeviceInfoDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 获取当前企业可分配的设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<DeviceInfoDto>> GetUnDeviceInfoListAsync(UnDeviceInput request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0 || request.EnterpriseinfoId <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);

            // 获取设备型号数据
            var deviceModels = await context.DeviceModel.Where(w => !w.IsDelete).ToListAsync();

            // 获取当前企业租赁中的设备Ids
            //var enterpriseDeviceIds = await context.EnterpriseDevices.AsNoTracking().Where(w => (w.EnterpriseId == request.EnterpriseinfoId && w.DeviceAllocationType == DeviceAllocationEnum.Lease && w.RecyclingTime <= DateTime.UtcNow)
            //                                                                    || (w.BelongingEnterpriseId == request.EnterpriseinfoId && w.DeviceAllocationType == DeviceAllocationEnum.Sale)).
            //                                                                    Select(s => s.DeviceId).ToListAsync();

            var enterpriseDeviceIds = await context.EnterpriseDevices.IgnoreQueryFilters()
                .AsNoTracking().Where(w => ((w.EnterpriseId == request.EnterpriseinfoId && w.DeviceAllocationType == DeviceAllocationEnum.Lease && w.RecyclingTime <= DateTime.UtcNow)
                                                                                || (w.BelongingEnterpriseId == request.EnterpriseinfoId && w.DeviceAllocationType == DeviceAllocationEnum.Sale))
                                                                                && w.EnterpriseinfoId == request.EnterpriseinfoId && !w.IsDelete).
                                                                                Select(s => s.DeviceId).ToListAsync();

            // 条件过滤
            var query = context.DeviceInfo.IgnoreQueryFilters().AsQueryable().Include(i => i.DeviceUserAssociations)
                .Where(w => !w.IsDelete && w.EnterpriseinfoId == request.EnterpriseinfoId && !enterpriseDeviceIds.Contains(w.Id))
                .WhereIf(!_user.AllDeviceRole, w => w.DeviceUserAssociations.Where(s => s.UserId == _user.UserId).Count() > 0
                || (context.GroupDevices.Any(b => b.DeviceInfoId == w.Id && context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId))));

            // .WhereIf(!string.IsNullOrWhiteSpace(request.EquipmentNumber), w => w.EquipmentNumber == request.EquipmentNumber)

            //query = query
            //    .WhereIf(!string.IsNullOrEmpty(request.EquipmentNumber), w => context.DeviceBaseInfo.Where(s => s.Id == w.DeviceBaseId).Any(s => s.MachineStickerCode == request.EquipmentNumber))
            //    .WhereIf(!string.IsNullOrWhiteSpace(request.DeviceName), w => w.Name.Contains(request.DeviceName))
            //    .WhereIf(request.DeviceModelId != null, w => w.DeviceModelId == request.DeviceModelId)
            //    //.WhereIf(request.DeviceStatus != null && request.DeviceStatus != 0, w => (w.DeviceStatus & request.DeviceStatus) != 0)//查询包含当前搜索状态的数据
            //    //.WhereIf(request.DeviceStatus != null && request.DeviceStatus == 0, w => w.DeviceStatus == DeviceStatusEnum.Offline)
            //    .WhereIf(request.DeviceStatus != null, w => w.DeviceStatus == request.DeviceStatus)
            //    .WhereIf(request.OnLineTimeRange != null && request.OnLineTimeRange.Count > 0 && request.OnLineTimeRange.Count == 2,
            //    w => w.LatestOnlineTime >= request.OnLineTimeRange[0] && w.LatestOnlineTime < request.OnLineTimeRange[1].AddDays(1));

            query = query
               .WhereIf(!string.IsNullOrEmpty(request.EquipmentNumber), w => context.DeviceBaseInfo.Where(s => s.Id == w.DeviceBaseId).Any(s => s.MachineStickerCode == request.EquipmentNumber))
               .WhereIf(!string.IsNullOrWhiteSpace(request.DeviceName), w => w.Name.Contains(request.DeviceName))
               .WhereIf(request.DeviceModelId != null, w => w.DeviceModelId == request.DeviceModelId)
               .WhereIf(request.DeviceStatus != null, w => context.DeviceBaseInfo.Where(s => s.Id == w.DeviceBaseId).Any(s => s.IsOnline == (request.DeviceStatus == DeviceStatusEnum.Online)))
               .WhereIf(request.OnLineTimeRange != null && request.OnLineTimeRange.Count > 0 && request.OnLineTimeRange.Count == 2,
               w => context.DeviceBaseInfo.Where(s => s.Id == w.DeviceBaseId).Any(s => s.UpdateOnlineTime >= request.OnLineTimeRange[0] && s.UpdateOnlineTime < request.OnLineTimeRange[1].AddDays(1)));

            // 获取当前企业下的所有设备分页数据,排除租赁中的设备
            var list = await query.ToPagedListAsync(request);

            var deviceBaseIds = list.Items.Select(s => s.DeviceBaseId).ToList();
            var deviceBaseInfos = context.DeviceBaseInfo.AsQueryable().Where(w => deviceBaseIds.Contains(w.Id));
            //foreach (var item in list.Items)
            //{
            //    item.EquipmentNumber = deviceBaseInfos.FirstOrDefault(w => w.Id == item.DeviceBaseId)?.MachineStickerCode;
            //}

            // 组装Dto
            var listDto = new DeviceInfoListDto();
            list.Items.ForEach(s =>
            {
                var curDto = mapper.Map<DeviceInfoDto>(s);
                curDto.EquipmentNumber = deviceBaseInfos.FirstOrDefault(w => w.Id == s.DeviceBaseId)?.MachineStickerCode;
                var curdmodel = deviceModels.FirstOrDefault(w => w.Id == s.DeviceModelId);
                if (curdmodel != null)
                    curDto.DeviceModelText = curdmodel?.Name;
                listDto.DeviceInfoList.Add(curDto);
            });
            PagedResultDto<DeviceInfoDto> pagedResultDto = new PagedResultDto<DeviceInfoDto>()
            {
                Items = listDto.DeviceInfoList,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 根据设备Id获取饮品信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<object>> GetBeverageInfoByDeviceIdAsync(long deviceId)
        {
            var info = await context.DeviceInfo.AsNoTracking().Include(i => i.BeverageInfos).Where(w => w.Id == deviceId).SelectMany(s => s.BeverageInfos).Select(s => new { s.Id, s.Name }).ToListAsync();
            return info.Cast<object>().ToList();
        }

        /// <summary>
        /// 根据设备型号获取当前企业设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoListByDeviceModelId(DeviceInfoListByDeviceModelIdInput request)
        {
            //获取分页数据
            var deviceModels = await context.DeviceModel.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();

            var query = from a in context.DeviceInfo.IgnoreQueryFilters().AsNoTracking().Include(i => i.BeverageInfos).Include(i => i.GroupDevices)
                        join b in context.DeviceBaseInfo on a.DeviceBaseId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        select new
                        {
                            DeviceInfo = a,
                            DeviceBaseInfo = b
                        };

            //context.DeviceInfo.IgnoreQueryFilters().AsNoTracking().Include(i => i.BeverageInfos).Include(i => i.GroupDevices)
            var list = await query
                .Where(w => !w.DeviceInfo.IsDelete && w.DeviceInfo.EnterpriseinfoId == request.enterpriseinfoId && w.DeviceInfo.DeviceModelId == request.deviceModelId && w.DeviceInfo.ActiveTime != null)
                .WhereIf(!string.IsNullOrWhiteSpace(request.deviceName_SN), w => w.DeviceBaseInfo.MachineStickerCode.Contains(request.deviceName_SN) || w.DeviceInfo.Name.Contains(request.deviceName_SN))//设备名称或sn模糊查询 || w.EquipmentNumber.Contains(request.deviceName_SN)
                .WhereIf(request.groupIds.Count > 0, w => w.DeviceInfo.GroupDevices.Where(w => request.groupIds.Contains(w.GroupsId)).Select(s => s.DeviceInfoId).ToList().Contains(w.DeviceInfo.Id))//设备分组筛选
                .WhereIf(!_user.AllDeviceRole, w => w.DeviceInfo.DeviceUserAssociations.Where(s => s.UserId == _user.UserId).Count() > 0
                || (context.GroupDevices.Any(b => b.DeviceInfoId == w.DeviceInfo.Id && context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId))))
                .ToPagedListAsync(request);
            var groupIds = list.Items.SelectMany(s => s.DeviceInfo.GroupDevices ?? new List<GroupDevices>()).Select(s => s.GroupsId).ToList();
            var groups = await context.Groups.AsNoTracking().Where(w => groupIds.Contains(w.Id)).ToListAsync();
            var groupsDevices = groups.SelectMany(s => s.Devices).ToList();
            //var deviceBaseInfos = await context.DeviceBaseInfo.AsNoTracking().Where(w => list.Items.Select(s => s.DeviceInfo.DeviceBaseId).Contains(w.Id)).ToListAsync();
            var listDto = new DeviceInfoListDto();
            list.Items.ForEach(s =>
            {
                var curGD = groupsDevices.Where(w => w.DeviceInfoId == s.DeviceInfo.Id).ToList();
                var curDto = mapper.Map<DeviceInfoDto>(s.DeviceInfo);
                var curdmodel = deviceModels.FirstOrDefault(w => w.Id == s.DeviceInfo.DeviceModelId);
                //var code = deviceBaseInfos.FirstOrDefault(w => w.Id == s.DeviceInfo.DeviceBaseId)?.MachineStickerCode;
                if (curdmodel != null && !string.IsNullOrWhiteSpace(curdmodel.Name))
                    curDto.DeviceModelText = curdmodel.Name;
                curDto.GroupIds = curGD.Select(s => s.GroupsId).ToList();
                curDto.GroupsText = string.Join(",", groups.Where(w => !w.IsDelete && curDto.GroupIds.Contains(w.Id)).Select(s => s.Name).ToList());
                curDto.BeveragesList = s.DeviceInfo.BeverageInfos == null ? new List<object>() : s.DeviceInfo.BeverageInfos.Where(w => !w.IsDelete).Select(s => new { s.Id, s.Name, s.Code, s.CodeIsShow }).Cast<object>().ToList();
                curDto.BeverageInfos = null;
                curDto.Mid = s.DeviceBaseInfo != null ? s.DeviceBaseInfo.MachineStickerCode : null;
                curDto.MachineStickerCode = s.DeviceBaseInfo != null ? s.DeviceBaseInfo.MachineStickerCode : null;
                curDto.Name = string.IsNullOrEmpty(s.DeviceInfo.Name) ? s.DeviceBaseInfo.MachineStickerCode : s.DeviceInfo.Name;
                curDto.IsOnline = s.DeviceBaseInfo != null ? s.DeviceBaseInfo.IsOnline : false;
                //curDto.IsOnline = deviceBaseInfos.FirstOrDefault(w => w.Id == s.DeviceBaseInfo.Id)?.IsOnline;

                //curDto.DeviceModelText = s.Name;
                listDto.DeviceInfoList.Add(curDto);
            });
            PagedResultDto<DeviceInfoDto> pagedResultDto = new PagedResultDto<DeviceInfoDto>()
            {
                Items = listDto.DeviceInfoList,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 根据设备Id获取用户Id集合
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<long>> GetUserIdsByDeviceId(long deviceId)
        {
            return context.DeviceInfo.AsNoTracking().Where(a => a.Id == deviceId).SelectMany(s => s.DeviceUserAssociations).Select(s => s.UserId).ToListAsync();
        }

        /// <summary>
        /// 根据设备Id获取分组Id集合
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Task<List<long>> GetGroupIdsByDeviceId(long deviceId)
        {
            return context.DeviceInfo.AsNoTracking().Where(a => a.Id == deviceId).SelectMany(s => s.GroupDevices).Select(s => s.GroupsId).ToListAsync();
        }

        /// <summary>
        /// 获取设备广告设置
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<DeviceAdInput> GetDeviceAd(long deviceId)
        {
            var result = new DeviceAdInput();
            result.DeviceId = deviceId;

            var deviceAd = await context.DeviceAdvertisement.AsQueryable().Where(w => w.DeviceId == deviceId).ToListAsync();

            if (deviceAd.Count == 0)
            {
                result.DeviceId = null;
            }

            var halfScreen = deviceAd.FirstOrDefault(f => f.Type == AdvertisementEnum.HalfScreen);
            var fullScreen = deviceAd.FirstOrDefault(s => s.Type == AdvertisementEnum.FullScreen);

            if (halfScreen != null)
            {
                HalfScreen hs = new HalfScreen();
                hs.powerOnAdsPlayTime = halfScreen.CarouselIntervalSecond;
                List<AdFile> adFileList = new List<AdFile>();

                var halfFile = await context.DeviceAdvertisementFile.AsQueryable().Where(w => w.DeviceAdvertisementId == halfScreen.Id).ToListAsync();
                foreach (var item in halfFile)
                {
                    AdFile adFile = new AdFile();
                    adFile.Url = item.Url;
                    adFile.Name = item.Name;
                    adFile.Duration = item.Duration;
                    adFile.Sort = item.Sort;
                    adFile.Suffix = item.Suffix;
                    adFile.IsFullScreenAd = false;
                    adFile.FileLength = item.FileLength;
                    adFile.Enable = item.Enable;
                    adFileList.Add(adFile);
                }
                hs.adList = adFileList;
                result.HalfScreen = hs;
            }

            if (fullScreen != null)
            {
                FullScreen fs = new FullScreen();
                fs.StandbyAdStatus = fullScreen.Enabled;
                fs.StandbyAdsPlayTime = fullScreen.CarouselIntervalSecond;
                fs.StandbyAdsAwaitTime = fullScreen.StandbyWaiteSecond;

                List<AdFile> adFileList = new List<AdFile>();
                var fullFile = await context.DeviceAdvertisementFile.AsQueryable().Where(w => w.DeviceAdvertisementId == fullScreen.Id).ToListAsync();
                foreach (var item in fullFile)
                {
                    AdFile adFile = new AdFile();
                    adFile.Url = item.Url;
                    adFile.Name = item.Name;
                    adFile.Duration = item.Duration;
                    adFile.Sort = item.Sort;
                    adFile.Suffix = item.Suffix;
                    adFile.IsFullScreenAd = false;
                    adFile.FileLength = item.FileLength;
                    adFile.Enable = item.Enable;
                    adFileList.Add(adFile);
                }
                fs.adList = adFileList;
                result.FullScreen = fs;
            }

            return result;
        }

        /// <summary>
        /// 设备报表统计
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeviceReportCountOutput> GetDeviceReportCount()
        {
            var result = new DeviceReportCountOutput();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any())
            {
                return result;
            }
            result.DeviceCount = accessibleDeviceIds.Count();
            result.DeviceOnlineCount = userDevices.Count(x => x.IsOnline == true);
            return result;
        }

        /// <summary>
        /// 设备销售排行
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<DeviceSaleRank>> GetDeviceSaleRank(List<DateTime> times)
        {
            var result = new List<DeviceSaleRank>();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any() || times == null || times.Count < 2)
            {
                return result;
            }

            // 获取时间范围
            DateTime startTime = times[0];
            DateTime endTime = times[1];

            // 转换为时间戳（毫秒）
            long startTimeSp = new DateTimeOffset(startTime).ToUnixTimeMilliseconds();
            long endTimeSp = new DateTimeOffset(endTime.AddDays(1).AddTicks(-1)).ToUnixTimeMilliseconds();

            // 查询指定时间范围内的订单数据
            var orders = await context.OrderInfo.AsNoTracking()
                .Where(o => accessibleDeviceBaseIds.Contains(o.DeviceBaseId) &&
                           o.PayTimeSp >= startTimeSp &&
                           o.PayTimeSp <= endTimeSp &&
                           (o.SaleResult == OrderSaleResult.Success || o.SaleResult == OrderSaleResult.PartialRefund))
                .ToListAsync();

            // 按设备分组统计
            var deviceSales = orders
                .GroupBy(o => o.DeviceBaseId)
                .Select(g => new DeviceSaleRank
                {
                    DeviceBaseId = g.Key,
                    SaleCount = g.Sum(o => o.Amount - o.ReturnAmount)
                })
                .OrderByDescending(d => d.SaleCount)
                .Take(10)
                .ToList();

            foreach (var sale in deviceSales)
            {
                var deviceInfo = userDevices.FirstOrDefault(d => d.BaseInfoId == sale.DeviceBaseId);
                if (deviceInfo != null)
                {
                    sale.DeviceName = deviceInfo.DeviceName;
                    sale.DeviceCode = deviceInfo.DeviceCode;
                }
            }

            return deviceSales;
        }

        /// <summary>
        /// 商品销售排行
        /// </summary>
        /// <param name="times"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public async Task<List<GoodsSaleRank>> GetGoodsSaleRank(List<DateTime> times, int top)
        {
            var result = new List<GoodsSaleRank>();
            var userDevices = await GetDeviceByUser();
            var accessibleDeviceIds = userDevices.Select(d => d.DeviceId).ToList();
            var accessibleDeviceBaseIds = userDevices.Select(d => d.BaseInfoId).ToList();
            if (!accessibleDeviceIds.Any() || times == null || times.Count < 2)
            {
                return result;
            }

            // 获取时间范围
            DateTime startTime = times[0];
            DateTime endTime = times[1];

            // 转换为时间戳（毫秒）
            long startTimeSp = new DateTimeOffset(startTime).ToUnixTimeMilliseconds();
            long endTimeSp = new DateTimeOffset(endTime.AddDays(1).AddTicks(-1)).ToUnixTimeMilliseconds();

            // 查询指定时间范围内的订单详情数据
            var orderDetails = await context.OrderDetails
                .Include(od => od.OrderInfo) // 包含订单信息
                .Where(od => accessibleDeviceBaseIds.Contains(od.OrderInfo.DeviceBaseId) &&
                            od.OrderInfo.PayTimeSp >= startTimeSp &&
                            od.OrderInfo.PayTimeSp <= endTimeSp &&
                            od.OrderInfo.OrderType != OrderTypeEnum.Not &&
                            od.Result == 1) // 只统计出货成功的商品
                .ToListAsync();

            // 按商品分组统计销量
            var goodsSales = orderDetails
                .GroupBy(od => od.BeverageName)
                .Select(g => new GoodsSaleRank
                {
                    GoodName = g.Key,
                    Count = g.Sum(od => od.Quantity),
                    Url = g.FirstOrDefault()?.Url ?? ""
                })
                .OrderByDescending(g => g.Count)
                .Take(top)
                .ToList();

            return goodsSales;
        }

        /// <summary>
        /// 获取货币列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<CurrentDto>> GetCurrentList()
        {
            return await context.Currency.AsNoTracking()
                .Select(s => new CurrentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Symbol = s.CurrencySymbol
                }).ToListAsync();
        }

        /// <summary>
        /// 点单获取设备
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<GetDecentlyDevicePageListOutput>> GetDecentlyDevicePageList([FromBody] GetDecentlyDevicePageListInput request)
        {
            var query = context.DeviceInfo
                .Join(context.DeviceBaseInfo,
                      device => device.DeviceBaseId,
                      baseInfo => baseInfo.Id,
                      (device, baseInfo) => new { Device = device, BaseInfo = baseInfo })
                .Where(x => x.Device.DeviceActiveState == DeviceActiveEnum.Active)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.DeviceName))
            {
                query = query.Where(d => d.Device.Name.Contains(request.DeviceName));
            }

            var devicesWithDistance = await query
                .Select(d => new GetDecentlyDevicePageListOutput
                {
                    IsOnline = d.BaseInfo.IsOnline,
                    DeviceName = d.Device.Name ?? d.BaseInfo.MachineStickerCode,
                    DeviceAddress = d.Device.DetailedAddress ??
                                   $"{d.Device.Province}{d.Device.City}{d.Device.District}{d.Device.Street}",
                    DeviceImageUrl = "",
                    OperatingHours = "00:00-24:00",
                    Distance = Math.Round(Util.Core.Util.CalculateDistance(request.Lat, request.Lng,
                               (double)(d.Device.Lat ?? 0),
                               (double)(d.Device.Lng ?? 0)), 2)
                })
                .OrderBy(x => x.Distance)
                .ToPagedListAsync(request);

            return devicesWithDistance;
        }
    }
}