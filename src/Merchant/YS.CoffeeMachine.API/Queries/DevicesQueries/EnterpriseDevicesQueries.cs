using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 企业设备查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class EnterpriseDevicesQueries(CoffeeMachineDbContext context, UserHttpContext _user) : IEnterpriseDevicesQueries
    {
        /// <summary>
        /// 通过企业Id获取设备分配信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnterpriseDevicesDto> GetEnterpriseDevicesAsync(long id)
        {
            //if (id <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(id));
            //var info = await context.EnterpriseDevices.AsNoTracking().Includes("Device", "Enterprise").AsNoTracking().FirstOrDefaultAsync(x => x.TransId == id);
            //if (info is null)
            //    throw new KeyNotFoundException();
            var dto = new EnterpriseDevicesDto()
            {
                //DeviceBaseId = info.DeviceBaseId,
                //EquipmentNumber = info.Device.EquipmentNumber,
                //DeviceModel = info.Device.DeviceModel?.Name,
                //EnterpriseId = info.EnterpriseId,
                //EnterpriseName = info.Enterprise.Name,
                //DeviceAllocationType = info.DeviceAllocationType,
                //RecyclingTime = info.RecyclingTime,
                //AllocateTime = info.AllocateTime
            };
            return dto;
        }

        /// <summary>
        /// 获取设备分配列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EnterpriseDevicesDto>> GetEnterpriseDevicesListAsync(EnterpriseDevicesInput request)
        {
            //return await context.EnterpriseDevices.AsQueryable()
            //    .LeftJoin(context.EnterpriseInfo, a => a.BelongingEnterpriseId, b => b.Id, (a, b) => new { EnterpriseDevices = a, BelongingEnterpriseName = b.Name })
            //    .LeftJoin(context.EnterpriseInfo, temp1 => temp1.EnterpriseDevices.EnterpriseId, b => b.Id, (temp1, b) => new { temp1, EnterpriseName = b.Name })
            //    .LeftJoin(context.DeviceInfo, temp2 => temp2.temp1.EnterpriseDevices.DeviceId, b => b.Id, (temp2, b) => new { temp2, Sn = b.EquipmentNumber })
            //    .WhereIf(!string.IsNullOrEmpty(request.EquipmentNumber), a => a.Sn.Contains(request.EquipmentNumber))
            //    .WhereIf(request.EnterpriseId != null, a => a.temp2.temp1.EnterpriseDevices.EnterpriseId == request.EnterpriseId)
            //    .WhereIf(request.BelongingEnterpriseId != null, a => a.temp2.temp1.EnterpriseDevices.BelongingEnterpriseId == request.BelongingEnterpriseId)
            //    .WhereIf(request.DeviceAllocationType != null, a => a.temp2.temp1.EnterpriseDevices.DeviceAllocationType == request.DeviceAllocationType)
            //    .WhereIf(request.AllocateTimeRange.Count == 2, a => a.temp2.temp1.EnterpriseDevices.AllocateTime >= request.AllocateTimeRange[0] && a.temp2.temp1.EnterpriseDevices.AllocateTime <= request.AllocateTimeRange[1])
            //    .WhereIf(request.RecyclingTimeRange.Count == 2, a => a.temp2.temp1.EnterpriseDevices.RecyclingTime >= request.RecyclingTimeRange[0] && a.temp2.temp1.EnterpriseDevices.RecyclingTime <= request.RecyclingTimeRange[1])
            //    .Select(a => new EnterpriseDevicesDto
            //    {
            //        Id = a.temp2.temp1.EnterpriseDevices.Id,
            //        DeviceId = a.temp2.temp1.EnterpriseDevices.DeviceId,
            //        EquipmentNumber = a.Sn,
            //        BelongingEnterpriseId = a.temp2.temp1.EnterpriseDevices.BelongingEnterpriseId,
            //        BelongingEnterpriseName = a.temp2.temp1.BelongingEnterpriseName,
            //        EnterpriseId = a.temp2.temp1.EnterpriseDevices.EnterpriseId,
            //        EnterpriseName = a.temp2.EnterpriseName,
            //        DeviceAllocationType = a.temp2.temp1.EnterpriseDevices.DeviceAllocationType,
            //        RecyclingTime = a.temp2.temp1.EnterpriseDevices.RecyclingTime,
            //        AllocateTime = a.temp2.temp1.EnterpriseDevices.AllocateTime
            //    }).ToPagedListAsync(request);

            // 1. 构建基础的 OrderInfo 查询
            IQueryable<DeviceInfo> deviceInfos = context.DeviceInfo;
            deviceInfos = deviceInfos.IgnoreQueryFilters().Where(w => !w.IsDelete);

            var query =
                from device in context.EnterpriseDevices
                join belongingEnterprise in context.EnterpriseInfo
                    on device.BelongingEnterpriseId equals belongingEnterprise.Id into belongingGroup
                from belongingEnterprise in belongingGroup.DefaultIfEmpty()
                join enterprise in context.EnterpriseInfo
                    on device.EnterpriseId equals enterprise.Id into enterpriseGroup
                from enterprise in enterpriseGroup.DefaultIfEmpty()
                join deviceInfo in deviceInfos
                    on device.DeviceId equals deviceInfo.Id into deviceInfoGroup
                from deviceInfo in deviceInfoGroup.DefaultIfEmpty()
                join deviceBaseInfo in context.DeviceBaseInfo
                    on deviceInfo.DeviceBaseId equals deviceBaseInfo.Id into deviceBaseInfoGroup
                from deviceBaseInfo in deviceBaseInfoGroup.DefaultIfEmpty()
                join devicemodel in context.DeviceModel
                    on deviceBaseInfo.DeviceModelId equals devicemodel.Id into devicemodelGroup
                from devicemodel in devicemodelGroup.DefaultIfEmpty()
                where device.IsDelete == false && device.EnterpriseinfoId == _user.TenantId
                select new EnterpriseDevicesDto
                {
                    Id = device.Id,
                    DeviceId = device.DeviceId,
                    EquipmentNumber = deviceBaseInfo != null ? deviceBaseInfo.MachineStickerCode : null,
                    BelongingEnterpriseId = device.BelongingEnterpriseId,
                    BelongingEnterpriseName = belongingEnterprise != null ? belongingEnterprise.Name : null,
                    EnterpriseId = device.EnterpriseId,
                    EnterpriseName = enterprise != null ? enterprise.Name : null,
                    DeviceAllocationType = device.DeviceAllocationType,
                    RecyclingTime = device.RecyclingTime,
                    AllocateTime = device.AllocateTime,
                    DeviceModel = devicemodel != null ? devicemodel.Name : null,
                };

            if (!_user.AllDeviceRole)
            {
                query = (from device in query
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

            return await query.WhereIf(!string.IsNullOrEmpty(request.EquipmentNumber), d => d.EquipmentNumber.Contains(request.EquipmentNumber))
                .WhereIf(request.EnterpriseId != null, d => d.EnterpriseId == request.EnterpriseId)
                .WhereIf(request.BelongingEnterpriseId != null, d => d.BelongingEnterpriseId == request.BelongingEnterpriseId)
                .WhereIf(request.DeviceAllocationType != null, d => d.DeviceAllocationType == request.DeviceAllocationType)
                .WhereIf(request.AllocateTimeRange.Count == 2, d => d.AllocateTime >= request.AllocateTimeRange[0] && d.AllocateTime <= request.AllocateTimeRange[1])
                .WhereIf(request.RecyclingTimeRange.Count == 2, d => d.RecyclingTime >= request.RecyclingTimeRange[0] && d.RecyclingTime <= request.RecyclingTimeRange[1])
                .OrderByDescending(d => d.AllocateTime)
                .ToPagedListAsync(request);

            //var query = context.EnterpriseDevices.IgnoreQueryFilters().AsNoTracking().AsNoTracking().Where(w => (w.BelongingEnterpriseId == _user.TenantId || w.EnterpriseId == _user.TenantId) && !w.IsDelete);
            ////查询条件
            //query = query.WhereIf(!string.IsNullOrWhiteSpace(request.EquipmentNumber), w => w.Device.EquipmentNumber == request.EquipmentNumber)
            //    .WhereIf(request.BelongingEnterpriseId != null, w => w.BelongingEnterpriseId == request.BelongingEnterpriseId)
            //    .WhereIf(request.EnterpriseId != null, w => w.EnterpriseId == request.EnterpriseId)
            //    .WhereIf(request.DeviceAllocationType != null, w => w.DeviceAllocationType == request.DeviceAllocationType)
            //    .WhereIf(request.AllocateTimeRange.Count > 0 && request.AllocateTimeRange.Count == 2, w => w.AllocateTime >= request.AllocateTimeRange[0] && w.AllocateTime < request.AllocateTimeRange[1].AddDays(1))
            //    .WhereIf(request.RecyclingTimeRange.Count > 0 && request.RecyclingTimeRange.Count == 2, w => w.RecyclingTime >= request.RecyclingTimeRange[0] && w.RecyclingTime < request.RecyclingTimeRange[1].AddDays(1));

            //var list = await query.ToPagedListAsync(request, "Device.DeviceModel", "Enterprise");
            //if (list.TotalCount == 0)
            //    return new PagedResultDto<EnterpriseDevicesDto>();
            //var listDto = new EnterpriseDevicesListDto();
            ////获取当前设备企业信息
            //var curAllEnterprise = await context.EnterpriseInfo.AsNoTracking().Where(w => list.Items.Select(s => s.BelongingEnterpriseId).Contains(w.TransId)).ToListAsync();
            //list.Items.ForEach(s =>
            //{
            //    listDto.EnterpriseDevicesList.Add(new EnterpriseDevicesDto
            //    {
            //        TransId = s.TransId,
            //        DeviceBaseId = s.DeviceBaseId,
            //        EquipmentNumber = s.Device.EquipmentNumber,
            //        DeviceModel = s.Device.DeviceModel.Name,
            //        BelongingEnterpriseId = s.BelongingEnterpriseId,
            //        BelongingEnterpriseName = curAllEnterprise.FirstOrDefault(w => w.TransId == s.BelongingEnterpriseId)?.Name,
            //        EnterpriseId = s.EnterpriseId,
            //        EnterpriseName = s.Enterprise.Name,
            //        DeviceAllocationType = s.DeviceAllocationType,
            //        AllocateTime = s.AllocateTime,
            //        RecyclingTime = s.RecyclingTime
            //    });
            //});
            PagedResultDto<EnterpriseDevicesDto> pagedResultDto = new PagedResultDto<EnterpriseDevicesDto>()
            {
                //Items = listDto.EnterpriseDevicesList,
                //PageNumber = request.PageNumber,
                //PageSize = request.PageSize,
                //TotalCount = listDto.EnterpriseDevicesList.Count
            };
            return pagedResultDto;
        }
    }
}