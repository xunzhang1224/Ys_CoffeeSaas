using Autofac.Core;
using MathNet.Numerics.Distributions;
using Microsoft.EntityFrameworkCore;
using Polly.Caching;
using System.Linq.Dynamic.Core;
using YS.CoffeeMachine.Application.Dtos.ReportsDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IReportsQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.ReportsQueries
{
    /// <summary>
    /// 报表查询
    /// </summary>
    public class ReportsQuerie(UserHttpContext _user, CoffeeMachineDbContext _context, ITimezoneContext tz) : IReportsQuerie
    {
        /// <summary>
        /// 设备物料报表(设备信息总览)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Dictionary<int, DeviceMeterialReportDto>> DeviceMaterialReport(ReportsInput input)
        {
            var deviceBaseIds = await GetDeviceBaseIds(input);

            bool isOnlyId = false;
            if (input.DeviceId != null || (input.GroupIds != null && input.GroupIds.Count > 0))
            {
                isOnlyId = true;
            }

            var curData = await CommonMeterialValues(input.DateRange, deviceBaseIds, input.EnterpriseInfoId, isOnlyId);
            var tData = await CommonMeterialValues(input.DateRangeT, deviceBaseIds, input.EnterpriseInfoId, isOnlyId);
            var hData = await CommonMeterialValues(input.DateRangeH, deviceBaseIds, input.EnterpriseInfoId, isOnlyId);

            Dictionary<int, DeviceMeterialReportDto> dto = new Dictionary<int, DeviceMeterialReportDto>();

            foreach (var item in curData)
            {
                var tValue = tData.FirstOrDefault(f => f.Type == item.Type)?.TotalValue;
                var hValue = hData.FirstOrDefault(f => f.Type == item.Type)?.TotalValue;
                DeviceMeterialReportDto tt = new DeviceMeterialReportDto();
                tt.TotalValue = item.TotalValue;
                tt.YoyValue = tValue ?? 0;
                tt.MomValue = hValue ?? 0;
                dto[(int)item.Type] = tt;
            }

            dto[0] = await GetSaleCount(input);

            return dto;
        }

        /// <summary>
        /// 获取设备售卖统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DeviceMeterialReportDto> GetSaleCount(ReportsInput input)
        {
            var tt1 = from a in _context.OrderInfo
                      join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                      from b in ab.DefaultIfEmpty()
                          //where b.CreateTime >= date && b.CreateTime < nextDay
                      where a.IsDelete == false
                       && (
(a.ShipmentResult == OrderShipmentResult.Success && (a.SaleResult == OrderSaleResult.Success || a.SaleResult == OrderSaleResult.Refund || a.SaleResult == OrderSaleResult.PartialRefund))
||
(a.ShipmentResult == OrderShipmentResult.Fail && a.SaleResult == OrderSaleResult.Refund)
)
                      select new
                      {
                          a,
                          b
                      };

            if (!_user.AllDeviceRole || (input.DeviceId != null || (input.GroupIds != null && input.GroupIds.Count > 0)))
            {
                //ReportsInput input = new ReportsInput();
                //input.EnterpriseInfoId = _user.TenantId;
                var deviceBaseInfoIds = await GetDeviceBaseIds(input);
                tt1 = tt1.Where(w => deviceBaseInfoIds.Contains(w.a.DeviceBaseId));
            }
            else
            {
                tt1 = tt1.AsQueryable().IgnoreQueryFilters().Where(w => w.a.EnterpriseinfoId == input.EnterpriseInfoId);
            }
            var curCount = await tt1.Where(w => w.b.CreateTime >= input.DateRange[0] && w.b.CreateTime <= input.DateRange[1]).CountAsync();
            var tCount = await tt1.Where(w => w.b.CreateTime >= input.DateRangeT[0] && w.b.CreateTime <= input.DateRangeT[1]).CountAsync();
            var hCount = await tt1.Where(w => w.b.CreateTime >= input.DateRangeH[0] && w.b.CreateTime <= input.DateRangeH[1]).CountAsync();
            DeviceMeterialReportDto tt = new DeviceMeterialReportDto();
            tt.TotalValue = curCount;
            tt.YoyValue = tCount;
            tt.MomValue = hCount;

            return tt;
        }

        #region 设备信息总览通用

        /// <summary>
        /// 设备材质统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<MeterialValue>> CommonMeterialValues(List<DateTime> dateRange, List<long> deviceBaseIds, long enterpriseId, bool isOnlyId)
        {
            // 1. 原始查询
            var query = from a in _context.DeviceMaterialInfo
                        join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join d in _context.OrderInfo on c.OrderId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where d.CreateTime >= dateRange[0] && d.CreateTime <= dateRange[1] && d.IsDelete == false
                                    && (
(d.ShipmentResult == OrderShipmentResult.Success && (d.SaleResult == OrderSaleResult.Success || d.SaleResult == OrderSaleResult.Refund || d.SaleResult == OrderSaleResult.PartialRefund))
||
(d.ShipmentResult == OrderShipmentResult.Fail && d.SaleResult == OrderSaleResult.Refund)
)
                        select new
                        {
                            a.Type,
                            b.Value,
                            d.DeviceBaseId,
                            d.EnterpriseinfoId
                        };

            // 2. 动态条件
            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.EnterpriseinfoId == enterpriseId);
            }

            // 3. 聚合结果
            var result = await query
                .GroupBy(x => x.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    TotalValue = g.Sum(x => x.Value)
                })
                .ToListAsync();

            // 4. 固定类型补全
            //var allTypes = new[] { "TypeA", "TypeB", "TypeC", "TypeD", "TypeE" };
            var allTypes = Enum.GetValues<MaterialTypeEnum>();

            // 5. 最终补全结果
            var finalResult = allTypes
                .Select(type => new MeterialValue
                {
                    Type = type,
                    TotalValue = result.FirstOrDefault(x => x.Type == type)?.TotalValue ?? 0
                })
                .ToList();
            return finalResult;
        }

        /// <summary>
        /// 获取登录人  所能访问的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<long>> GetDeviceBaseIds(ReportsInput input)
        {
            var deviceBaseIds = new List<long>();
            // 如果选定了指定设备
            if (input.DeviceId != null) // 如果选定了指定设备
            {
                var deviceBaseId = await _context.DeviceInfo.AsQueryable().Where(w => w.Id == input.DeviceId).Select(s => s.DeviceBaseId).FirstOrDefaultAsync();
                deviceBaseIds.Add(deviceBaseId);
            }
            else if (input.GroupIds != null && input.GroupIds.Count > 0) // 如果选定了指定分组
            {
                //var deviceIdsSql =
                //        //await (
                //        from a in _context.GroupUsers
                //        join b in _context.GroupDevices on a.GroupsId equals b.GroupsId into ab
                //        from b in ab.DefaultIfEmpty() // LEFT JOIN
                //        where input.GroupIds.Contains(a.GroupsId) && a.ApplicationUserId == _user.UserId
                //        select new
                //        {
                //            a,
                //            b,
                //        };
                ////).ToListAsync();

                //if (!_user.AllDeviceRole)
                //{
                //    deviceIdsSql = deviceIdsSql.Where(w => input.GroupIds.Contains(w.a.GroupsId) && w.a.ApplicationUserId == _user.UserId);
                //}

                //var deviceIds = await deviceIdsSql.Select(s => s.b.DeviceInfoId).ToListAsync();
                //deviceBaseIds = await _context.DeviceInfo.AsQueryable().Where(w => deviceIds.Contains(w.Id)).Select(s => s.DeviceBaseId).ToListAsync();

                //deviceBaseIds = await (
                //    from a in _context.GroupUsers
                //    join b in _context.GroupDevices on a.GroupsId equals b.GroupsId into ab
                //    from b in ab.DefaultIfEmpty()
                //    join d in _context.DeviceInfo on b.DeviceInfoId equals d.Id
                //    where input.GroupIds.Contains(a.GroupsId)
                //          && a.ApplicationUserId == _user.UserId
                //          && (_user.AllDeviceRole || a.ApplicationUserId == _user.UserId)
                //          && b != null
                //    select d.DeviceBaseId
                //).ToListAsync();

                // deviceBaseIds = await (
                //    from a in _context.GroupDevices
                //    join b in _context.GroupUsers on a.GroupsId equals b.GroupsId into ab
                //    from b in ab.DefaultIfEmpty()
                //    join d in _context.DeviceInfo on a.DeviceInfoId equals d.Id
                //    where input.GroupIds.Contains(a.GroupsId)
                //          && (_user.AllDeviceRole || b.ApplicationUserId == _user.UserId)
                //    select d.DeviceBaseId
                //).ToListAsync();

                //// 第一步：获取用户有权限的设备ID
                //var userDeviceIds = await _context.DeviceUserAssociation
                //    .Where(dua => dua.UserId == _user.UserId)
                //    .Select(dua => dua.DeviceId)
                //    .ToListAsync();

                //// 第二步：获取用户有权限的组ID
                //var userGroupIds = await _context.GroupUsers
                //    .Where(gu => gu.ApplicationUserId == _user.UserId)
                //    .Select(gu => gu.GroupsId)
                //    .ToListAsync();

                //// 第三步：合并查询
                //var deviceInfoIds = await _context.GroupDevices
                //    .AsNoTracking()
                //    .Where(gd => input.GroupIds.Contains(gd.GroupsId) &&
                //        (userGroupIds.Contains(gd.GroupsId) || userDeviceIds.Contains(gd.DeviceInfoId)))
                //    .Select(s => s.DeviceInfoId)
                //    .ToListAsync();
                //deviceBaseIds = await _context.DeviceInfo.AsQueryable().Where(w => deviceInfoIds.Contains(w.Id)).Select(s => s.DeviceBaseId).ToListAsync();

                if (_user.AllDeviceRole)
                {
                    deviceBaseIds = await (
                  from gd in _context.GroupDevices
                  join di in _context.DeviceInfo on gd.DeviceInfoId equals di.Id
                  where gd.IsDelete == false && input.GroupIds.Contains(gd.GroupsId)
                  select di.DeviceBaseId
                  ).Distinct()
                  .ToListAsync();
                }
                else
                {
                    deviceBaseIds = await (
                    from gd in _context.GroupDevices
                    join di in _context.DeviceInfo on gd.DeviceInfoId equals di.Id
                    where gd.IsDelete == false && input.GroupIds.Contains(gd.GroupsId) &&
                            (_context.GroupUsers.Any(gu => gu.GroupsId == gd.GroupsId && gu.ApplicationUserId == _user.UserId) ||
                             _context.DeviceUserAssociation.Any(dua => dua.DeviceId == gd.DeviceInfoId && dua.UserId == _user.UserId))
                    select di.DeviceBaseId
                    ).Distinct()
                    .ToListAsync();
                }
            }
            else
            {
                if (!_user.AllDeviceRole)
                {
                    var userInfo = await _context.ApplicationUser.AsQueryable().Where(w => w.Id == _user.UserId).FirstOrDefaultAsync();
                    if (userInfo.EnterpriseId == _user.TenantId && _user.TenantId == input.EnterpriseInfoId)
                    {
                        deviceBaseIds = await _context.DeviceInfo.AsNoTracking().Where(w => w.EnterpriseinfoId == _user.TenantId
                    && (w.DeviceUserAssociations.Select(s => s.UserId).Contains(_user.UserId)
                    || (_context.GroupDevices.Any(b => b.DeviceInfoId == w.Id && _context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId)))))
                            .Select(s => s.DeviceBaseId).Distinct().ToListAsync();
                    }
                }
            }

            return deviceBaseIds;
        }

        #endregion

        /// <summary>
        /// 制作数趋势
        /// </summary>
        /// <returns></returns>
        public async Task<ProductionTrendReportDto> ProductionTrendReport(ReportsInput input)
        {
            var deviceBaseIds = await GetDeviceBaseIds(input);

            bool isOnlyId = false;
            if (input.DeviceId != null || (input.GroupIds != null && input.GroupIds.Count > 0))
            {
                isOnlyId = true;
            }

            List<CupDto> cupDtos = new List<CupDto>();
            List<MeterialDto> meterialDtos = new List<MeterialDto>();

            if (input.TimeType == 1)
            {
                cupDtos = await CupDayData(input, deviceBaseIds, isOnlyId);
                meterialDtos = await MeterialDayData(input, deviceBaseIds, isOnlyId);
            }
            else if (input.TimeType == 0)
            {
                cupDtos = await CupHourData(input, deviceBaseIds, isOnlyId);
                meterialDtos = await MeterialHourData(input, deviceBaseIds, isOnlyId);
            }
            else
            {
                cupDtos = await CupMotherData(input, deviceBaseIds, isOnlyId);
                meterialDtos = await MeterialMonthData(input, deviceBaseIds, isOnlyId);
            }

            // 输出格式
            ProductionTrendReportDto dto = new ProductionTrendReportDto();
            dto.CupEcharts = cupDtos;
            dto.MaterialEcharts = meterialDtos.GroupBy(g => g.DateStr).ToDictionary(d => d.Key, d => d.ToDictionary(t => (int)t.Type, t => t.Value));
            return dto;
        }

        #region 制作数趋势通用

        /// <summary>
        /// 每天的杯数统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<CupDto>> CupDayData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            // 1. 生成日期范围内所有的日期
            var startDate = ChangeTimeZone(input.DateRange[0]).Date;
            var endDate = ChangeTimeZone(input.DateRange[1]).Date;
            var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            // 2. 查询实际有数据的日期统计
            var query =
            from a in _context.OrderDetails
            join b in _context.OrderInfo on a.OrderId equals b.Id into ab
            from b in ab.DefaultIfEmpty()
            where a.CreateTime.Date >= input.DateRange[0]
                && a.CreateTime.Date <= input.DateRange[1]
                && b != null && b.IsDelete == false
                   //&& ((b.OrderStatus != null && b.OrderStatus == OrderStatusEnum.Success)
                   //|| (b.OrderStatus != null && (b.OrderStatus == OrderStatusEnum.PartialRefund || b.OrderStatus == OrderStatusEnum.FullRefund) && b.ShipmentResult != OrderShipmentResult.Fail)
                   //|| b.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                   && (
(b.ShipmentResult == OrderShipmentResult.Success && (b.SaleResult == OrderSaleResult.Success || b.SaleResult == OrderSaleResult.Refund || b.SaleResult == OrderSaleResult.PartialRefund))
||
(b.ShipmentResult == OrderShipmentResult.Fail && b.SaleResult == OrderSaleResult.Refund)
)
            select new { a, b };

            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.b.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.b.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            //var data = await query
            //    .GroupBy(x => x.a.CreateTime.Date)
            //    .Select(g => new
            //    {
            //        CreateDate = g.Key,
            //        Count = g.Count()
            //    })
            //    .ToListAsync();

            //var data = await query
            //    .GroupBy(x => x.a.CreateTime.Date)
            //.Select(g => new Temp
            // {
            //     Day = g.Key,
            //     Value = g.Count()
            // })
            //.ToListAsync();

            var data = await query.Select(s => new Temp { Day = s.a.CreateTime }).ToListAsync();

            foreach (var item in data)
            {
                item.Day = ChangeTimeZone(item.Day).Date;
            }

            var groupData = data.GroupBy(x => x.Day).Select(g => new Temp
            {
                Day = g.Key,
                Value = g.Count()
            });

            // 3. 补全没有数据的日期
            return allDates
                .Select(date => new CupDto
                {
                    DateStr = date.ToString("yyyy-MM-dd"),
                    Value = groupData.FirstOrDefault(x => x.Day == date)?.Value ?? 0
                })
                .ToList();
        }

        /// <summary>
        /// 每小时的杯数统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<CupDto>> CupHourData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            var start = ChangeTimeZone(input.DateRange[0]).Date;
            var end = ChangeTimeZone(input.DateRange[1]).Date.AddDays(1); // 包含到最后一整天的 23:00

            // 1. 构造每一个小时段
            var allHours = Enumerable.Range(0, (int)(end - start).TotalHours)
                .Select(i => start.AddHours(i))
                .ToList();

            // 2. 查询真实数据（按小时分组）
            var query = from a in _context.OrderDetails
                        join b in _context.OrderInfo on a.OrderId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        where a.CreateTime >= input.DateRange[0] && a.CreateTime <= input.DateRange[1] && b.IsDelete == false
                //&& ((b.OrderStatus != null && b.OrderStatus == OrderStatusEnum.Success) || b.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                && (
(b.ShipmentResult == OrderShipmentResult.Success && (b.SaleResult == OrderSaleResult.Success || b.SaleResult == OrderSaleResult.Refund || b.SaleResult == OrderSaleResult.PartialRefund))
||
(b.ShipmentResult == OrderShipmentResult.Fail && b.SaleResult == OrderSaleResult.Refund)
)
                        select new { a, b };

            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.b.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.b.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            var dataE = await query.Select(s => new Temp { Day = s.a.CreateTime }).ToListAsync();
            foreach (var item in dataE)
            {
                item.Day = ChangeTimeZone(item.Day);
            }
            var data = dataE.GroupBy(x => new DateTime(
                    x.Day.Year,
                    x.Day.Month,
                    x.Day.Day,
                    x.Day.Hour,
                    0, 0))
                .Select(g => new
                {
                    Time = g.Key,
                    Count = g.Count()
                }).ToList();

            //var data = await query
            //    .GroupBy(x => new DateTime(
            //        x.a.CreateTime.Year,
            //        x.a.CreateTime.Month,
            //        x.a.CreateTime.Day,
            //        x.a.CreateTime.Hour,
            //        0, 0))
            //    .Select(g => new
            //    {
            //        Time = g.Key,
            //        Count = g.Count()
            //    })
            //    .ToListAsync();

            // 2. 查询真实数据（按小时分组）
            //var data = await (
            //    from a in _context.OrderDetails
            //    join b in _context.OrderInfo on a.OrderId equals b.Id into ab
            //    from b in ab.DefaultIfEmpty()
            //    where a.CreateTime >= start && a.CreateTime < end
            //    group a by new DateTime(
            //        a.CreateTime.Year,
            //        a.CreateTime.Month,
            //        a.CreateTime.Day,
            //        a.CreateTime.Hour,
            //        0, 0) into g
            //    select new
            //    {
            //        Time = g.Key,
            //        Count = g.Count()
            //    }
            //).ToListAsync();

            // 3. 补全小时段 + 格式化时间输出
            return allHours
                .Select(h => new CupDto
                {
                    DateStr = h.ToString("yyyy-MM-dd HH:mm"),
                    Value = data.FirstOrDefault(x => x.Time == h)?.Count ?? 0
                })
                .ToList();
        }

        /// <summary>
        /// 每月制杯数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<CupDto>> CupMotherData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            //var startDate = new DateTime(input.DateRange[0].Year, input.DateRange[0].Month, 1);
            //var endDate = new DateTime(input.DateRange[1].Year, input.DateRange[1].Month, 1);

            //// 1. 构造全部月份（yyyy-MM）
            //var allMonths = Enumerable.Range(0,
            //        ((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1))
            //    .Select(offset => startDate.AddMonths(offset))
            //    .ToList();
            var startDate = ChangeTimeZone(input.DateRange[0]).Date;
            var endDate = ChangeTimeZone(input.DateRange[1]).Date;

            var allMonths = Enumerable.Range(0, ((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1))
                .Select(i => new DateTime(startDate.Year, startDate.Month, 1).AddMonths(i))
                .ToList();

            // 2. 查询已有数据
            var query = from a in _context.OrderDetails
                        join b in _context.OrderInfo on a.OrderId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        where a.CreateTime >= input.DateRange[0] && a.CreateTime <= input.DateRange[1] && b.IsDelete == false
                //&& ((b.OrderStatus != null && b.OrderStatus == OrderStatusEnum.Success) || b.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                && (
(b.ShipmentResult == OrderShipmentResult.Success && (b.SaleResult == OrderSaleResult.Success || b.SaleResult == OrderSaleResult.Refund || b.SaleResult == OrderSaleResult.PartialRefund))
||
(b.ShipmentResult == OrderShipmentResult.Fail && b.SaleResult == OrderSaleResult.Refund)
)
                        select new { a, b };

            // 3. 如果需要，添加 deviceBaseIds 条件
            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.b.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.b.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            var dataE = await query.Select(s => new Temp { Day = s.a.CreateTime }).ToListAsync();
            foreach (var item in dataE)
            {
                item.Day = ChangeTimeZone(item.Day);
            }

            // 4. 按年月分组
            var data = dataE.GroupBy(x => new
            {
                x.Day.Year,
                x.Day.Month
            })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                }).ToList();
            //var data = await query
            //    .GroupBy(x => new
            //    {
            //        x.a.CreateTime.Year,
            //        x.a.CreateTime.Month
            //    })
            //    .Select(g => new
            //    {
            //        Year = g.Key.Year,
            //        Month = g.Key.Month,
            //        Count = g.Count()
            //    })
            //    .ToListAsync();

            // 3. 补全月份
            return allMonths
                .Select(m => new CupDto
                {
                    DateStr = m.ToString("yyyy-MM"),
                    Value = data.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month)?.Count ?? 0
                })
                .ToList();
        }

        /// <summary>
        /// 物料消耗量（小时）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MeterialDto>> MeterialHourData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            // 1. 构建时间段（小时级）
            var start = ChangeTimeZone(input.DateRange[0]).Date;
            var end = ChangeTimeZone(input.DateRange[1]).Date.AddDays(1); // 包含整天
            var allHours = Enumerable.Range(0, (int)(end - start).TotalHours)
                .Select(h => start.AddHours(h))
                .ToList();

            // 2. 构建原始查询
            var query = from a in _context.DeviceMaterialInfo
                        join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join d in _context.OrderInfo on c.OrderId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where d.CreateTime >= input.DateRange[0] && d.CreateTime <= input.DateRange[1] && d.IsDelete == false
                                //&& ((d.OrderStatus != null && d.OrderStatus == OrderStatusEnum.Success) || d.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                                && (
(d.ShipmentResult == OrderShipmentResult.Success && (d.SaleResult == OrderSaleResult.Success || d.SaleResult == OrderSaleResult.Refund || d.SaleResult == OrderSaleResult.PartialRefund))
||
(d.ShipmentResult == OrderShipmentResult.Fail && d.SaleResult == OrderSaleResult.Refund)
)
                        select new
                        {
                            Hour = new DateTime(d.CreateTime.Year, d.CreateTime.Month, d.CreateTime.Day, d.CreateTime.Hour, 0, 0),
                            a.Type,
                            Value = b != null ? b.Value : 0,
                            DeviceBaseId = d != null ? d.DeviceBaseId : 0,
                            EnterpriseinfoId = d.EnterpriseinfoId
                        };

            // 3. 如果需要，添加 deviceBaseIds 条件
            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            // 4. 执行查询
            var rawData = await query
                .Select(x => new Temp
                {
                    Day = x.Hour,
                    Type = x.Type,
                    Value = x.Value
                })
                .ToListAsync();

            foreach (var item in rawData)
            {
                item.Day = ChangeTimeZone(item.Day);
            }

            // 5. 分组聚合
            var grouped = rawData
                .GroupBy(x => new { x.Day, x.Type })
                .Select(g => new
                {
                    g.Key.Day,
                    g.Key.Type,
                    Total = g.Sum(x => x.Value)
                })
                .ToList();

            // 6. 获取所有类型
            //var allTypes = grouped.Select(x => x.Type).Distinct().ToList();
            var allTypes = Enum.GetValues<MaterialTypeEnum>();

            // 7. 补全所有小时段和类型组合
            return allHours
                .SelectMany(hour => allTypes.Select(type => new MeterialDto
                {
                    DateStr = hour.ToString("yyyy-MM-dd HH:mm"), // 输出时间字符串格式,
                    Type = type,
                    Value = grouped.FirstOrDefault(g => g.Day == hour && g.Type == type)?.Total ?? 0
                }))
                .OrderBy(x => x.DateStr)
                .ToList();
        }

        /// <summary>
        /// 物料消耗量（天）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MeterialDto>> MeterialDayData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            var start = ChangeTimeZone(input.DateRange[0]).Date;
            var end = ChangeTimeZone(input.DateRange[1]).Date;
            //var allDates = Enumerable.Range(0, (int)(end - start).TotalDays + 1)
            //    .Select(i => start.AddDays(i))
            //    .ToList();
            var allDates = Enumerable.Range(0, (int)(end - start).TotalDays + 1)
              .Select(i => start.AddDays(i))
              .ToList();

            // 查询数据

            // 1. 构建查询
            var query = from a in _context.DeviceMaterialInfo
                        join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join d in _context.OrderInfo on c.OrderId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where d.CreateTime >= input.DateRange[0] && d.CreateTime <= input.DateRange[1] && d.IsDelete == false
                                //&& ((d.OrderStatus != null && d.OrderStatus == OrderStatusEnum.Success) || d.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                                && (
(d.ShipmentResult == OrderShipmentResult.Success && (d.SaleResult == OrderSaleResult.Success || d.SaleResult == OrderSaleResult.Refund || d.SaleResult == OrderSaleResult.PartialRefund))
||
(d.ShipmentResult == OrderShipmentResult.Fail && d.SaleResult == OrderSaleResult.Refund)
)
                        select new
                        {
                            Day = d.CreateTime,  // .Date
                            a.Type,
                            Value = b != null ? b.Value : 0,
                            DeviceBaseId = d != null ? d.DeviceBaseId : 0,
                            EnterpriseinfoId = d.EnterpriseinfoId
                        };

            // 2. 如果传入了 deviceBaseIds，则添加筛选条件
            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            // 3. 查询数据
            var rawData = await query
                .Select(x => new Temp
                {
                    Day = x.Day,
                    Type = x.Type,
                    Value = x.Value
                })
                .ToListAsync();

            foreach (var item in rawData)
            {
                item.Day = ChangeTimeZone(item.Day, "").Date;
            }

            //var rawData = await (
            //    from a in _context.DeviceMaterialInfo
            //    join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
            //    from b in ab.DefaultIfEmpty()
            //    join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
            //    from c in bc.DefaultIfEmpty()
            //    join d in _context.OrderInfo on c.OrderId equals d.Id into cd
            //    from d in cd.DefaultIfEmpty()
            //    where a.CreateTime >= start && a.CreateTime <= end
            //    select new
            //    {
            //        Day = a.CreateTime.Date,
            //        a.Type,
            //        Value = b != null ? b.Value : 0
            //    }
            //).ToListAsync();

            // 分组
            var grouped = rawData
                .GroupBy(x => new { x.Day, x.Type })
                .Select(g => new
                {
                    g.Key.Day,
                    g.Key.Type,
                    Total = g.Sum(x => x.Value)
                })
                .ToList();

            // 获取所有类型
            //var allTypes = grouped.Select(x => x.Type).Distinct().ToList();
            var allTypes = Enum.GetValues<MaterialTypeEnum>();

            // 补全无数据的天 + 类型组合
            return allDates
                .SelectMany(date => allTypes.Select(type => new MeterialDto
                {
                    DateStr = date.ToString("yyyy-MM-dd"),
                    Type = type,
                    Value = grouped.FirstOrDefault(g => g.Day == date && g.Type == type)?.Total ?? 0
                }))
                .OrderBy(x => x.DateStr)
                .ThenBy(x => x.Type)
                .ToList();
        }

        /// <summary>
        /// 物料消耗量（月）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MeterialDto>> MeterialMonthData(ReportsInput input, List<long> deviceBaseIds, bool isOnlyId)
        {
            var startDate = ChangeTimeZone(input.DateRange[0]).Date;
            var endDate = ChangeTimeZone(input.DateRange[1]).Date;

            var allMonths = Enumerable.Range(0, ((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1))
                .Select(i => new DateTime(startDate.Year, startDate.Month, 1).AddMonths(i))
                .ToList();

            // 1. 构建查询
            var query = from a in _context.DeviceMaterialInfo
                        join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join d in _context.OrderInfo on c.OrderId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where d.CreateTime >= input.DateRange[0] && d.CreateTime <= input.DateRange[1] && d.IsDelete == false
                                //&& ((d.OrderStatus != null && d.OrderStatus == OrderStatusEnum.Success) || d.OrderStatus == null) // 若是线上支付，则只统计支付成功的
                                && (
(d.ShipmentResult == OrderShipmentResult.Success && (d.SaleResult == OrderSaleResult.Success || d.SaleResult == OrderSaleResult.Refund || d.SaleResult == OrderSaleResult.PartialRefund))
||
(d.ShipmentResult == OrderShipmentResult.Fail && d.SaleResult == OrderSaleResult.Refund)
)
                        select new
                        {
                            //Month = new DateTime(d.CreateTime.Year, d.CreateTime.Month, 1),
                            Month = d.CreateTime,
                            a.Type,
                            Value = b != null ? b.Value : 0,
                            DeviceBaseId = d != null ? d.DeviceBaseId : 0,
                            d.EnterpriseinfoId
                        };

            // 2. 根据 deviceBaseIds 条件进行筛选
            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                query = query.Where(x => deviceBaseIds.Contains(x.DeviceBaseId));
            }
            else
            {
                query = query.IgnoreQueryFilters().AsNoTracking().Where(w => w.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            // 3. 执行查询
            var rawData = await query
                .Select(x => new Temp
                {
                    Day = x.Month,
                    Type = x.Type,
                    Value = x.Value
                })
                .ToListAsync();

            foreach (var item in rawData)
            {
                var createTime = ChangeTimeZone(item.Day);
                item.Day = new DateTime(createTime.Year, createTime.Month, 1);
            }

            var grouped = rawData
                .GroupBy(x => new { x.Day, x.Type })
                .Select(g => new
                {
                    g.Key.Day,
                    g.Key.Type,
                    Total = g.Sum(x => x.Value)
                })
                .ToList();

            //var allTypes = grouped.Select(g => g.Type).Distinct().ToList();
            var allTypes = Enum.GetValues<MaterialTypeEnum>();

            return allMonths
                .SelectMany(month => allTypes.Select(type => new MeterialDto
                {
                    DateStr = month.ToString("yyyy-MM"),
                    Type = type,
                    Value = grouped.FirstOrDefault(g => g.Day == month && g.Type == type)?.Total ?? 0
                }))
                .OrderBy(x => x.DateStr)
                .ThenBy(x => x.Type)
                .ToList();
        }

        #endregion

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceInfo>> GetDeviceInfo(ReportsInput input)
        {
            List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();
            // 如果选定了指定设备
            if (input.DeviceId != null) // 如果选定了指定设备
            {
                var tt = await _context.DeviceInfo.AsQueryable().Where(w => w.Id == input.DeviceId).FirstOrDefaultAsync();
                deviceInfoList.Add(tt);
            }
            else if (input.GroupIds != null && input.GroupIds.Count > 0) // 如果选定了指定分组
            {
                //var deviceIdsSql =
                //        //await (
                //        from a in _context.GroupUsers
                //        join b in _context.GroupDevices on a.GroupsId equals b.GroupsId into ab
                //        from b in ab.DefaultIfEmpty() // LEFT JOIN
                //        where input.GroupIds.Contains(a.GroupsId) && a.ApplicationUserId == _user.UserId
                //        select new
                //        {
                //            a,
                //            b,
                //        };
                ////).ToListAsync();

                //if (!_user.AllDeviceRole)
                //{
                //    deviceIdsSql = deviceIdsSql.Where(w => input.GroupIds.Contains(w.a.GroupsId) && w.a.ApplicationUserId == _user.UserId);
                //}

                //var deviceIds = await deviceIdsSql.Select(s => s.b.DeviceInfoId).ToListAsync();
                //deviceInfoList = await _context.DeviceInfo.AsQueryable().Where(w => deviceIds.Contains(w.Id)).ToListAsync();

                //var query =
                //    from a in _context.GroupUsers
                //    join b in _context.GroupDevices on a.GroupsId equals b.GroupsId into ab
                //    from b in ab.DefaultIfEmpty()
                //    join d in _context.DeviceInfo on b.DeviceInfoId equals d.Id
                //    where input.GroupIds.Contains(a.GroupsId)
                //          && a.ApplicationUserId == _user.UserId
                //          && (_user.AllDeviceRole || a.ApplicationUserId == _user.UserId)
                //          && b != null
                //    select d;

                //var query =
                //   from a in _context.GroupDevices
                //   join b in _context.GroupUsers on a.GroupsId equals b.GroupsId into ab
                //   from b in ab.DefaultIfEmpty()
                //   join d in _context.DeviceInfo on a.DeviceInfoId equals d.Id
                //   where input.GroupIds.Contains(a.GroupsId)
                //         && (_user.AllDeviceRole || b.ApplicationUserId == _user.UserId)
                //   select d;

                //deviceInfoList = await query.ToListAsync();

                //// 第一步：获取用户有权限的设备ID
                //var userDeviceIds = await _context.DeviceUserAssociation
                //    .Where(dua => dua.UserId == _user.UserId)
                //    .Select(dua => dua.DeviceId)
                //    .ToListAsync();

                //// 第二步：获取用户有权限的组ID
                //var userGroupIds = await _context.GroupUsers
                //    .Where(gu => gu.ApplicationUserId == _user.UserId)
                //    .Select(gu => gu.GroupsId)
                //    .ToListAsync();

                //// 第三步：合并查询
                //var deviceInfoIds = await _context.GroupDevices
                //    .AsNoTracking()
                //    .Where(gd => input.GroupIds.Contains(gd.GroupsId) &&
                //        (userGroupIds.Contains(gd.GroupsId) || userDeviceIds.Contains(gd.DeviceInfoId)))
                //    .Select(s => s.DeviceInfoId)
                //    .ToListAsync();

                //deviceInfoList = await _context.DeviceInfo.AsQueryable().Where(w => deviceInfoIds.Contains(w.Id)).ToListAsync();

                if (_user.AllDeviceRole)
                {
                    deviceInfoList = await (
                      from gd in _context.GroupDevices
                      join di in _context.DeviceInfo on gd.DeviceInfoId equals di.Id
                      where gd.IsDelete == false && input.GroupIds.Contains(gd.GroupsId)
                      select di
                  ).Distinct()
                  .ToListAsync();
                }
                else
                {
                    deviceInfoList = await (
                       from gd in _context.GroupDevices
                       join di in _context.DeviceInfo on gd.DeviceInfoId equals di.Id
                       where gd.IsDelete == false && input.GroupIds.Contains(gd.GroupsId) &&
                           (_context.GroupUsers.Any(gu => gu.GroupsId == gd.GroupsId && gu.ApplicationUserId == _user.UserId) ||
                            _context.DeviceUserAssociation.Any(dua => dua.DeviceId == gd.DeviceInfoId && dua.UserId == _user.UserId))
                       select di
                   ).Distinct()
                   .ToListAsync();
                }
            }
            else
            {
                if (!_user.AllDeviceRole)
                {
                    var userInfo = await _context.ApplicationUser.AsQueryable().Where(w => w.Id == _user.UserId).FirstOrDefaultAsync();
                    if (userInfo.EnterpriseId == _user.TenantId && _user.TenantId == input.EnterpriseInfoId)
                    {
                        deviceInfoList = await _context.DeviceInfo.AsNoTracking().Where(w => w.EnterpriseinfoId == _user.TenantId
                    && (w.DeviceUserAssociations.Select(s => s.UserId).Contains(_user.UserId)
                    || (_context.GroupDevices.Any(b => b.DeviceInfoId == w.Id && _context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId)))))
                            .Distinct().ToListAsync();
                    }
                }
            }

            return deviceInfoList;
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RankingReportDto> RankingReport(ReportsInput input)
        {
            var deviceInfoList = new List<DeviceInfo>();
            var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                deviceInfoList = await GetDeviceInfo(input);
                deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }
            else
            {
                deviceInfoList = await _context.DeviceInfo.AsQueryable().ToListAsync();
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<OrderInfo> orderQuery = _context.OrderInfo.Where(w => w.IsDelete == false
                                //&& ((w.OrderStatus != null && w.OrderStatus == OrderStatusEnum.Success) || w.OrderStatus == null)); // 若是线上支付，则只统计支付成功的
                                && (
(w.ShipmentResult == OrderShipmentResult.Success && (w.SaleResult == OrderSaleResult.Success || w.SaleResult == OrderSaleResult.Refund || w.SaleResult == OrderSaleResult.PartialRefund))
||
(w.ShipmentResult == OrderShipmentResult.Fail && w.SaleResult == OrderSaleResult.Refund)
));

            if (_user.AllDeviceRole)
            {
                orderQuery = orderQuery.IgnoreQueryFilters()
                                       .Where(w => w.EnterpriseinfoId == input.EnterpriseInfoId);
            }
            else
            {
                orderQuery = orderQuery.Where(w => deviceBaseIds.Contains(w.DeviceBaseId));
            }

            orderQuery = orderQuery.Where(w => w.CreateTime >= input.DateRange[0] && w.CreateTime <= input.DateRange[1]);

            #region 设备制作数top10

            // 2. 基于过滤后的 OrderInfo 进行 Left Join + 分组统计
            //var deviceTop10Data = await orderQuery
            //    .GroupJoin(
            //        _context.OrderDetails,
            //        a => a.Id,
            //        b => b.OrderId,
            //        (a, bGroup) => new
            //        {
            //            a.DeviceBaseId,
            //            Count = bGroup.Count()
            //        }
            //    )
            //    .OrderByDescending(x => x.Count)
            //    .Take(10)
            //    .ToListAsync();

            //var deviceTop10Data = await (
            //    from order in orderQuery
            //    join detail in _context.OrderDetails on order.Id equals detail.OrderId
            //    group detail by order.DeviceBaseId into g
            //    select new
            //    {
            //        DeviceBaseId = g.Key,
            //        Count = g.Count()
            //    }
            //)
            //.OrderByDescending(x => x.Count)
            //.Take(10)
            //.ToListAsync();

            var deviceTop10Data = await (
                 from order in orderQuery
                 join detail in _context.OrderDetails on order.Id equals detail.OrderId
                 group order by order.DeviceBaseId into g
                 select new
                 {
                     DeviceBaseId = g.Key,
                     Count = g.Count(),
                     // 按更新时间/ID 取最新的名字
                     DeviceName = g.OrderByDescending(o => o.CreateTime)   // 或者 o.Id
                                  .Select(o => o.BaseDeviceName)
                                  .FirstOrDefault()
                 }
             )
             .OrderByDescending(x => x.Count)
             .Take(10)
             .ToListAsync();

            var notNameDeviceIds = deviceTop10Data.Where(w => string.IsNullOrEmpty(w.DeviceName)).ToList();
            var notNameDeviceBases = await _context.DeviceBaseInfo.AsQueryable().Where(w => notNameDeviceIds.Select(s => s.DeviceBaseId).Contains(w.Id)).ToListAsync();
            List<CommonDto> deviceTop10 = new List<CommonDto>();
            foreach (var item in deviceTop10Data)
            {
                CommonDto tt = new CommonDto();
                tt.Name = string.IsNullOrWhiteSpace(item.DeviceName) ? notNameDeviceBases.FirstOrDefault(w => w.Id == item.DeviceBaseId).MachineStickerCode : item.DeviceName; //deviceInfoList.FirstOrDefault(f => f.DeviceBaseId == item.DeviceBaseId)?.Name ?? string.Empty;
                tt.Value = item.Count;
                deviceTop10.Add(tt);
            }

            #endregion

            #region 制作饮品top10

            //var top10Beverages = await (
            //      from a in orderQuery
            //      join b in _context.OrderDetails on a.Id equals b.OrderId into ab
            //      from b in ab.DefaultIfEmpty()
            //      group b by b.BeverageName into g
            //      select new CommonDto
            //      {
            //          Name = g.Key,
            //          Value = g.Count(x => x != null)
            //      }
            //  )
            //  .OrderByDescending(x => x.Value)
            //  .Take(10)
            //  .ToListAsync();

            var top10Beverages = await _context.OrderDetails
                .Where(od => orderQuery.Select(o => o.Id).Contains(od.OrderId)) // 使用子查询替代 join
                .Where(od => !string.IsNullOrEmpty(od.BeverageName)) // 排除空名称
                .GroupBy(od => od.BeverageName)
                .Select(g => new CommonDto
                {
                    Name = g.Key,
                    Value = g.Count()
                })
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToListAsync();

            List<CommonDto> beveragesTop10 = top10Beverages;

            RankingReportDto rankingReportDto = new RankingReportDto();
            rankingReportDto.DeviceTop10 = deviceTop10;
            rankingReportDto.BeveragesTop10 = beveragesTop10;

            return rankingReportDto;
            #endregion
        }

        /// <summary>
        /// 饮品制作量占比
        /// </summary>
        public async Task<List<CommonDto>> BeverageProductionReport(ReportsInput input)
        {
            var deviceBaseIds = await GetDeviceBaseIds(input);

            bool isOnlyId = false;
            if (input.DeviceId != null || (input.GroupIds != null && input.GroupIds.Count > 0))
            {
                isOnlyId = true;
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<OrderInfo> orderQuery = _context.OrderInfo.Where(w => w.IsDelete == false
                                 //&& ((w.OrderStatus != null && w.OrderStatus == OrderStatusEnum.Success) || w.OrderStatus == null)); // 若是线上支付，则只统计支付成功的
                                 && (
(w.ShipmentResult == OrderShipmentResult.Success && (w.SaleResult == OrderSaleResult.Success || w.SaleResult == OrderSaleResult.Refund || w.SaleResult == OrderSaleResult.PartialRefund))
||
(w.ShipmentResult == OrderShipmentResult.Fail && w.SaleResult == OrderSaleResult.Refund)
));

            // 动态拼接 deviceBaseId 条件

            if (isOnlyId || (!isOnlyId && !_user.AllDeviceRole))
            {
                orderQuery = orderQuery.Where(x => deviceBaseIds.Contains(x.DeviceBaseId));
            }
            else
            {
                orderQuery = orderQuery.IgnoreQueryFilters().AsNoTracking().Where(w => w.EnterpriseinfoId == input.EnterpriseInfoId);
            }

            return await (
                  from a in orderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  where b.CreateTime >= input.DateRange[0] && b.CreateTime <= input.DateRange[1]
                  group b by b.BeverageName into g
                  select new CommonDto
                  {
                      Name = g.Key,
                      Value = g.Count(x => x != null)
                  }
              )
              .OrderByDescending(x => x.Value)
              .ToListAsync();
        }

        /// <summary>
        /// 总览
        /// </summary>
        /// <returns></returns>
        public async Task<OverviewReportDto> OverviewReport(ReportsInput input)
        {
            OverviewReportDto dto = new OverviewReportDto();
            var deviceInfoList = new List<DeviceInfo>();
            //var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                deviceInfoList = await GetDeviceInfo(input);
                //deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }
            else
            {
                deviceInfoList = await _context.DeviceInfo.IgnoreQueryFilters().Where(w => !w.IsDelete && w.EnterpriseinfoId == input.EnterpriseInfoId).ToListAsync();
            }

            if (input.DeviceModelId != null)
            {
                deviceInfoList = deviceInfoList.Where(w => w.DeviceModelId == input.DeviceModelId).ToList();
            }

            dto.DeviceCount = deviceInfoList.Count;
            dto.DeviceCountByYear = deviceInfoList
                .GroupBy(d => d.CreateTime.Year)
                .Select(g => new CommonDto
                {
                    Name = g.Key.ToString(),
                    Value = g.Count()
                })
                .OrderBy(x => x.Name)
                .ToList();

            return dto;

        }

        #region 首页

        /// <summary>
        /// 设备概览
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceOverview> DeviceOverview()
        {

            DeviceOverview deviceOverview = new DeviceOverview();
            // 1. 构建基础的 OrderInfo 查询
            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                var deviceInfos = await GetDeviceInfo(input);
                var tt = await _context.DeviceBaseInfo.AsQueryable().Where(w => deviceInfos.Select(s => s.DeviceBaseId).ToList().Contains(w.Id) && w.IsOnline).ToListAsync();
                deviceOverview.OnlineCount = tt.Count();
            }
            else
            {
                // 1. 构建基础的 OrderInfo 查询
                IQueryable<DeviceInfo> orderQuery = _context.DeviceInfo.Where(w => w.IsDelete == false);

                orderQuery = orderQuery.AsQueryable().AsNoTracking().IgnoreQueryFilters().Where(w => w.EnterpriseinfoId == _user.TenantId && !w.IsDelete);

                deviceOverview.OnlineCount = await (
                         from a in orderQuery
                         join b in _context.DeviceBaseInfo on a.DeviceBaseId equals b.Id into ab
                         from b in ab.DefaultIfEmpty()
                         where b != null && b.IsOnline
                         select a.Id // 或 select 1，只要非 null 字段都行
                     ).CountAsync();
            }
            return deviceOverview;
        }

        /// <summary>
        /// 今日制杯及消耗
        /// </summary>
        /// <returns></returns>
        public async Task<List<MeterialValue>> TodayMeterial(List<DateTime> dateRange)
        {

            //var date = DateTime.Parse(dateStr);
            //var nextDay = date.AddDays(1);

            var tt = from a in _context.DeviceMaterialInfo
                     join b in _context.OrderDetaliMaterial on a.Id equals b.DeviceMaterialInfoId into ab
                     from b in ab.DefaultIfEmpty()
                     join c in _context.OrderDetails on b.OrderDetailsId equals c.Id into bc
                     from c in bc.DefaultIfEmpty()
                     join d in _context.OrderInfo on c.OrderId equals d.Id into cd
                     from d in cd.DefaultIfEmpty()
                     where d.CreateTime >= dateRange[0] && d.CreateTime < dateRange[1] && d.IsDelete == false
                     select new
                     {
                         a,
                         b,
                         c,
                         d
                     };

            var tt1 = from a in _context.OrderInfo
                      join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                      from b in ab.DefaultIfEmpty()
                      where b.CreateTime >= dateRange[0] && b.CreateTime < dateRange[1] && a.IsDelete == false
                      //&& a.SaleResult != OrderSaleResult.Cancel && a.SaleResult != OrderSaleResult.Fail && a.SaleResult != OrderSaleResult.Timeout && a.SaleResult != OrderSaleResult.NotPay
                      //&& a.SaleResult != OrderSaleResult.Refund
                       && (
                         (a.ShipmentResult == OrderShipmentResult.Success && (a.SaleResult == OrderSaleResult.Success || a.SaleResult == OrderSaleResult.Refund || a.SaleResult == OrderSaleResult.PartialRefund))
                         ||
                         (a.ShipmentResult == OrderShipmentResult.Fail && a.SaleResult == OrderSaleResult.Refund)
                         )
                      select new
                      {
                          a,
                          b
                      };

            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                var deviceBaseInfoIds = await GetDeviceBaseIds(input);
                tt = tt.Where(w => deviceBaseInfoIds.Contains(w.d.DeviceBaseId));
                tt1 = tt1.Where(w => deviceBaseInfoIds.Contains(w.a.DeviceBaseId));
            }
            else
            {
                tt = tt.AsQueryable().IgnoreQueryFilters().Where(w => w.d.EnterpriseinfoId == _user.TenantId);
                tt1 = tt1.AsQueryable().IgnoreQueryFilters().Where(w => w.a.EnterpriseinfoId == _user.TenantId);
            }

            //var allTypes = Enum.GetValues<MaterialTypeEnum>();

            //// 3. 按 a.Type 分组统计 b.Value 的总和
            //var result = await tt
            //    .GroupBy(x => x.a.Type)
            //    .Select(g => new MeterialValue
            //    {
            //        Type = g.Key,
            //        TotalValue = g.Sum(x => x.b != null ? x.b.Value : 0)
            //    })
            //    .ToListAsync();

            //MeterialValue meterialValue = new MeterialValue();
            //meterialValue.Type = MaterialTypeEnum.Not;
            //meterialValue.TotalValue = await tt1.CountAsync();
            //result.Add(meterialValue);
            //return result;

            var allTypes = Enum.GetValues<MaterialTypeEnum>();

            // 原始统计数据
            var result = await tt
                .GroupBy(x => x.a.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    TotalValue = g.Sum(x => x.b != null ? x.b.Value : 0)
                })
                .ToListAsync();

            // 转换为字典方便查找
            var resultDict = result.ToDictionary(x => x.Type, x => x.TotalValue);

            // 构造最终结果，确保所有类型都有
            var finalResult = allTypes
                .Where(t => !t.Equals(MaterialTypeEnum.Not)) // 不含 Not，单独添加
                .Select(type => new MeterialValue
                {
                    Type = type,
                    TotalValue = resultDict.TryGetValue(type, out var value) ? value : 0
                })
                .ToList();

            // 单独添加枚举 Not
            finalResult.Add(new MeterialValue
            {
                Type = MaterialTypeEnum.Not,
                TotalValue = await tt1.CountAsync()
            });

            return finalResult;
        }

        /// <summary>
        /// 今日饮品排行
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public async Task<List<CommonDto>> TodayBeverage(List<DateTime> dateRange)
        {
            //var date = DateTime.Parse(dateStr);
            //var nextDay = date.AddDays(1);

            var deviceInfoList = new List<DeviceInfo>();
            var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                deviceInfoList = await GetDeviceInfo(input);
                deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<OrderInfo> orderQuery = _context.OrderInfo.Where(w => w.IsDelete == false && w.Provider != "OTHER");

            if (_user.AllDeviceRole)
            {
                orderQuery = orderQuery.IgnoreQueryFilters()
                                       .Where(w => w.EnterpriseinfoId == _user.TenantId);
            }
            else
            {
                orderQuery = orderQuery.Where(w => deviceBaseIds.Contains(w.DeviceBaseId));
            }

            orderQuery = orderQuery.Where(w => w.CreateTime >= dateRange[0] && w.CreateTime < dateRange[1]
            //&& w.SaleResult != OrderSaleResult.Cancel && w.SaleResult != OrderSaleResult.Fail && w.SaleResult != OrderSaleResult.Timeout && w.SaleResult != OrderSaleResult.NotPay
            // && w.SaleResult != OrderSaleResult.Refund
             && (
 (w.ShipmentResult == OrderShipmentResult.Success && (w.SaleResult == OrderSaleResult.Success || w.SaleResult == OrderSaleResult.Refund || w.SaleResult == OrderSaleResult.PartialRefund))
 ||
 (w.ShipmentResult == OrderShipmentResult.Fail && w.SaleResult == OrderSaleResult.Refund)
 )
             );

            return await (
                  from a in orderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  group b by b.BeverageName into g
                  select new CommonDto
                  {
                      Name = g.Key,
                      Value = g.Count(x => x != null)
                  }
              )
              .OrderByDescending(x => x.Value)
              //.Take(10)
              .ToListAsync();
        }

        /// <summary>
        /// 今日设备制作排行top5
        /// </summary>
        public async Task<List<CommonDto>> TodayDeviceMakeRanking(List<DateTime> dateRange)
        {
            var deviceInfoList = new List<DeviceInfo>();
            var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                deviceInfoList = await GetDeviceInfo(input);
                deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<OrderInfo> orderQuery = _context.OrderInfo.Where(w => w.IsDelete == false);

            if (_user.AllDeviceRole)
            {
                orderQuery = orderQuery.IgnoreQueryFilters()
                                       .Where(w => w.EnterpriseinfoId == _user.TenantId);
            }
            else
            {
                orderQuery = orderQuery.Where(w => deviceBaseIds.Contains(w.DeviceBaseId));
            }

            orderQuery = orderQuery.Where(w => w.CreateTime >= dateRange[0] && w.CreateTime < dateRange[1]
            && w.Provider != "OTHER"
            //&& w.SaleResult != OrderSaleResult.Cancel && w.SaleResult != OrderSaleResult.Fail && w.SaleResult != OrderSaleResult.Timeout && w.SaleResult != OrderSaleResult.NotPay
            //&& w.SaleResult != OrderSaleResult.Refund
             && (
 (w.ShipmentResult == OrderShipmentResult.Success && (w.SaleResult == OrderSaleResult.Success || w.SaleResult == OrderSaleResult.Refund || w.SaleResult == OrderSaleResult.PartialRefund))
 ||
 (w.ShipmentResult == OrderShipmentResult.Fail && w.SaleResult == OrderSaleResult.Refund)
 )
            );

            var data = await (
                from order in orderQuery
                join detail in _context.OrderDetails on order.Id equals detail.OrderId
                join device in _context.DeviceInfo
                on order.DeviceBaseId equals device.DeviceBaseId
                //group order by order.DeviceBaseId into g
                group new { order, device } by new { device.DeviceBaseId, device.Name } into g
                select new CommonDto
                {
                    Name = g.Key.Name,
                    Value = g.Count()
                    //DeviceBaseId = g.Key,
                    //Count = g.Count(),
                    //// 按更新时间/ID 取最新的名字
                    //    DeviceName = g.OrderByDescending(o => o.CreateTime)   // 或者 o.Id
                    //                 .Select(o => o.BaseDeviceName)
                    //                 .FirstOrDefault()
                    //
                }
            )
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToListAsync();

            return data;

            //List<CommonDto> chartsData = new List<CommonDto>();
            //foreach (var item in data)
            //{
            //    CommonDto tt = new CommonDto();
            //    tt.Name = item.DeviceName;
            //    tt.Value = item.Count;
            //    chartsData.Add(tt);
            //}

            //return chartsData;
        }

        /// <summary>
        /// 今日设备错误统计top5
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public async Task<List<CommonDto>> TodayDeviceErrorReport(List<DateTime> dateRange)
        {
            var deviceInfoList = new List<DeviceInfo>();
            var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                deviceInfoList = await GetDeviceInfo(input);
                deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<DeviceAbnormal> errorQuery = _context.DeviceAbnormal.Where(w => w.IsDelete == false);

            if (_user.AllDeviceRole)
            {
                errorQuery = errorQuery.IgnoreQueryFilters()
                                       .Where(w => w.EnterpriseinfoId == _user.TenantId);
            }
            else
            {
                errorQuery = errorQuery.Where(w => deviceBaseIds.Contains(w.DeviceBaseId));
            }

            errorQuery = errorQuery.Where(w => w.CreateTime >= dateRange[0] && w.CreateTime < dateRange[1]);

            var result = await (
            from abnormal in errorQuery
            join device in _context.DeviceInfo
                on abnormal.DeviceBaseId equals device.DeviceBaseId
            where device.IsDelete == false
            group new { abnormal, device } by new { device.DeviceBaseId, device.Name } into g
            select new CommonDto
            {
                //DeviceBaseId = g.Key.DeviceBaseId,
                Name = g.Key.Name,
                Value = g.Count(x => x.abnormal.Id != null) // 或者直接 g.Count()
            }
            )
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToListAsync();

            return result;
        }
        #endregion

        #region 设备统计

        /// <summary>
        /// 设备制作数量
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceMakeReportDto> GetDeviceMakeReport(DeviceMakeReportInput input)
        {
            var deviceInfoList = new List<DeviceInfo>();
            var deviceBaseIds = new List<long>();
            if (!_user.AllDeviceRole)
            {
                ReportsInput tt = new ReportsInput();
                tt.EnterpriseInfoId = _user.TenantId;
                deviceInfoList = await GetDeviceInfo(tt);
                deviceBaseIds = deviceInfoList.Select(s => s.DeviceBaseId).ToList();
            }

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<OrderInfo> orderQuery = _context.OrderInfo.Where(w => w.IsDelete == false
            //&& w.SaleResult != OrderSaleResult.Cancel && w.SaleResult != OrderSaleResult.Fail
            //&& w.SaleResult != OrderSaleResult.Timeout && w.SaleResult != OrderSaleResult.NotPay
            //&& (w.SaleResult != OrderSaleResult.Refund || (w.SaleResult == OrderSaleResult.Refund && w.ShipmentResult == OrderShipmentResult.Success))
             && (
 (w.ShipmentResult == OrderShipmentResult.Success && (w.SaleResult == OrderSaleResult.Success || w.SaleResult == OrderSaleResult.Refund || w.SaleResult == OrderSaleResult.PartialRefund))
 ||
 (w.ShipmentResult == OrderShipmentResult.Fail && w.SaleResult == OrderSaleResult.Refund)
 )
            );

            if (_user.AllDeviceRole)
            {
                orderQuery = orderQuery.IgnoreQueryFilters()
                                       .Where(w => w.EnterpriseinfoId == _user.TenantId);
            }
            else
            {
                orderQuery = orderQuery.Where(w => deviceBaseIds.Contains(w.DeviceBaseId));
            }

            var todayOrderQuery = orderQuery.Where(w => w.CreateTime >= input.DayDateRange[0] && w.CreateTime <= input.DayDateRange[1]);
            var todayOrderCount = await (
                  from a in todayOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var yesterdayOrderQuery = orderQuery.Where(w => w.CreateTime >= input.YesterdayDateRange[0] && w.CreateTime <= input.YesterdayDateRange[1]);
            var yesterdayOrderCount = await (
                  from a in yesterdayOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var weekOrderQuery = orderQuery.Where(w => w.CreateTime >= input.WeekDateRange[0] && w.CreateTime <= input.WeekDateRange[1]);
            var weekOrderCount = await (
                  from a in weekOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var lastWeekOrderQuery = orderQuery.Where(w => w.CreateTime >= input.LastWeekDateRange[0] && w.CreateTime <= input.LastWeekDateRange[1]);
            var lastWeekOrderCount = await (
                  from a in lastWeekOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var monthOrderQuery = orderQuery.Where(w => w.CreateTime >= input.MonthDateRange[0] && w.CreateTime <= input.MonthDateRange[1]);
            var monthOrderCount = await (
                  from a in monthOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var lastMonthOrderQuery = orderQuery.Where(w => w.CreateTime >= input.LastMonthDateRange[0] && w.CreateTime <= input.LastMonthDateRange[1]);
            var lastMonthOrderCount = await (
                  from a in lastMonthOrderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            var allOrderCount = await (
                  from a in orderQuery
                  join b in _context.OrderDetails on a.Id equals b.OrderId into ab
                  from b in ab.DefaultIfEmpty()
                  select a.Id
              ).CountAsync();

            DeviceMakeReportDto dto = new DeviceMakeReportDto();
            dto.DayCount = todayOrderCount;
            dto.YesterdayCount = yesterdayOrderCount;
            dto.WeekCount = weekOrderCount;
            dto.LastWeekCount = lastWeekOrderCount;
            dto.MonthCount = monthOrderCount;
            dto.LastMonthCount = lastMonthOrderCount;
            dto.TotalCount = allOrderCount;

            return dto;
        }

        /// <summary>
        /// 根据设备id获取饮品制作数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDto>> GetDrinksByDeviceBaseId(long deviceBaseId)
        {
            return await (from orderDetail in _context.OrderDetails
                          join orderInfo in _context.OrderInfo
                          on orderDetail.OrderId equals orderInfo.Id into orderGroup
                          from order in orderGroup.DefaultIfEmpty() // 左连接
                          where order.DeviceBaseId == deviceBaseId
                          //&& order.SaleResult != OrderSaleResult.Cancel && order.SaleResult != OrderSaleResult.Fail && order.SaleResult != OrderSaleResult.Timeout
                          //&& order.SaleResult != OrderSaleResult.NotPay && order.SaleResult != OrderSaleResult.Refund
                          && (
                          (order.ShipmentResult == OrderShipmentResult.Success && (order.SaleResult == OrderSaleResult.Success || order.SaleResult == OrderSaleResult.Refund || order.SaleResult == OrderSaleResult.PartialRefund))
                          ||
                          (order.ShipmentResult == OrderShipmentResult.Fail && order.SaleResult == OrderSaleResult.Refund)
                          )
                          group orderDetail by orderDetail.BeverageName into beverageGroup
                          select new CommonDto
                          {
                              Name = beverageGroup.Key,
                              Value = beverageGroup.Count()
                          })
                   .OrderByDescending(x => x.Value)
                   .ToListAsync();
        }
        #endregion

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="oldTime"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public DateTime ChangeTimeZone(DateTime oldTime, string timeZone = "")
        {
            return tz.ConvertToLocal(oldTime);
            //timeZone = "UTC+8";
            //if (string.IsNullOrEmpty(timeZone))
            //{
            //    return oldTime;
            //}
            //else
            //{

            //    // 解析偏移量
            //    int offsetHours = int.Parse(timeZone.Replace("UTC", ""));
            //    TimeSpan offset = TimeSpan.FromHours(offsetHours);

            //    // 转换为指定偏移时间
            //    DateTime targetTime = oldTime + offset;
            //    return targetTime;
            //}
        }

        /// <summary>
        /// 获取饮品总览
        /// </summary>
        /// <returns></returns>
        public async Task<BeverageTotalDto> GetBeverageTotal()
        {
            BeverageTotalDto dto = new BeverageTotalDto();
            dto.BeverageInventoryCount = await _context.BeverageInfoTemplate.AsQueryable().Where(w => w.EnterpriseInfoId == _user.TenantId).CountAsync();
            dto.BeverageCollectionCount = await _context.BeverageCollection.AsQueryable().Where(w => w.EnterpriseInfoId == _user.TenantId).CountAsync();
            return dto;
        }

        /// <summary>
        /// 获取订单详情总览
        /// </summary>
        /// <returns></returns>
        public async Task<OrderDetailTotal> GetOrderDetailTotal(List<DateTime> dateRange)
        {
            OrderDetailTotal dto = new OrderDetailTotal();
            //var data = await _context.OrderInfo.AsQueryable().GroupBy(o => o.SaleResult)
            //    .Select(g => new
            //    {
            //        SaleResult = g.Key,
            //        Count = g.Count()
            //    })
            //    .ToListAsync();

            var orderInfo = _context.OrderInfo.AsQueryable();

            var data = new List<OrderTemp>();
            if (!_user.AllDeviceRole)
            {
                ReportsInput input = new ReportsInput();
                input.EnterpriseInfoId = _user.TenantId;
                var deviceBaseInfoIds = await GetDeviceBaseIds(input);
                data = await orderInfo.Where(w => w.CreateTime >= dateRange[0] && w.CreateTime <= dateRange[1] && deviceBaseInfoIds.Contains(w.DeviceBaseId)).GroupBy(o => o.SaleResult)
                     .Select(g => new OrderTemp
                     {
                         SaleResult = g.Key,
                         Count = g.Count()
                     })
                .ToListAsync();
            }
            else
            {
                data = await orderInfo.IgnoreQueryFilters().Where(w => w.CreateTime >= dateRange[0] && w.CreateTime <= dateRange[1] && w.EnterpriseinfoId == _user.TenantId && w.IsDelete == false)
                    .GroupBy(o => o.SaleResult)
                     .Select(g => new OrderTemp
                     {
                         SaleResult = g.Key,
                         Count = g.Count()
                     })
                .ToListAsync();
            }

            foreach (var item in data)
            {
                switch (item.SaleResult)
                {
                    case OrderSaleResult.Success:
                        dto.PaySuccess = item.Count;
                        break;
                    case OrderSaleResult.Fail:
                        dto.PayFail = item.Count;
                        break;
                    case OrderSaleResult.Refund:
                        dto.Refund = item.Count;
                        break;
                    case OrderSaleResult.PartialRefund:
                        dto.PartialRefund = item.Count;
                        break;
                    case OrderSaleResult.Refunding:
                        dto.Refunding = item.Count;
                        break;
                    case OrderSaleResult.NotPay:
                        dto.NotPay = item.Count;
                        break;
                    case OrderSaleResult.Timeout:
                        dto.Timeout = item.Count;
                        break;
                    case OrderSaleResult.Cancel:
                        dto.Cancel = item.Count;
                        break;
                }
            }

            dto.TotalCount = dto.PaySuccess + dto.PayFail + dto.Refund + dto.PartialRefund + dto.Refunding + dto.NotPay + dto.Timeout + dto.Cancel;
            return dto;
        }
    }

    /// <summary>
    /// 临时
    /// </summary>
    public class OrderTemp
    {
        /// <summary>
        /// 订单结果
        /// </summary>
        public OrderSaleResult SaleResult { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int Count { get; set; }
    }
}
