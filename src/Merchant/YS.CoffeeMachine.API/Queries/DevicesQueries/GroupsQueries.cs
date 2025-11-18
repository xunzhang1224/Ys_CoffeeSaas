using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.DevicesQueries
{
    /// <summary>
    /// 分组查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class GroupsQueries(CoffeeMachineDbContext context, IMapper mapper, UserHttpContext _user) : IGroupsQueries
    {
        /// <summary>
        /// 根据Id获取分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GroupsTreeDto> GetGroupsAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            // 查询所有顶级分组
            var group = await context.Groups.Includes("Users.ApplicationUser", "Devices.DeviceInfo").Where(w => w.Id == id).FirstOrDefaultAsync();
            if (group is null)
                throw new KeyNotFoundException();
            var groupDto = new GroupsTreeDto()
            {
                Id = group.Id,
                Name = group.Name,
                PId = group.PId,
                UserIds = group.Users.Select(gu => gu.ApplicationUserId).ToList(),
                UsersText = string.Join(", ", group.Users.Select(gu => gu.ApplicationUser.NickName.ToString())), // 替换为用户名如果需要
                DeviceIds = group.Devices.Select(s => s.DeviceInfoId).ToList(),
                CreateTimeText = group.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                Remark = group.Remark,
            };

            return groupDto;
        }

        /// <summary>
        /// 根据分组Id获取设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoByGroupId(QueryRequest request, long gid)
        {
            var deviceIds = await context.Groups.IgnoreQueryFilters()
                .Where(w => !w.IsDelete && w.Id == gid)
                .SelectMany(s => s.Devices).Select(s => s.DeviceInfo)
                .WhereIf(!_user.AllDeviceRole, w => w.DeviceUserAssociations.Where(s => s.UserId == _user.UserId).Count() > 0
                || context.GroupUsers.Any(gu => gu.ApplicationUserId == _user.UserId && gu.GroupsId == gid))
                .Select(s => s.Id).ToListAsync();

            var query = from d in context.DeviceInfo.IgnoreAutoIncludes()
                        where !d.IsDelete && deviceIds.Contains(d.Id)
                        join db in context.DeviceBaseInfo.IgnoreAutoIncludes() on d.DeviceBaseId equals db.Id into dbInfo
                        from db in dbInfo.DefaultIfEmpty()

                        join dm in context.DeviceModel.IgnoreAutoIncludes() on d.DeviceModelId equals dm.Id into dmInfo
                        from dm in dmInfo.DefaultIfEmpty()

                        select new DeviceInfoDto
                        {
                            Id = d.Id,
                            Name = d.Name,
                            DeviceBaseInfoId = d.DeviceBaseId,
                            MachineStickerCode = db.MachineStickerCode,
                            DeviceModelName = dm.Name,
                        };
            return await query.ToPagedListAsync(request);
            //var deviceList = await context.DeviceInfo.IgnoreQueryFilters()
            //    .Where(w => !w.IsDelete && deviceIds.Contains(w.Id))
            //    .Select(s => new DeviceInfoDto
            //    {
            //        Id = s.Id,
            //        DeviceBaseInfoId = s.DeviceBaseId,
            //        Name = s.Name,
            //    })
            //    .ToPagedListAsync(request);

            //return new PagedResultDto<DeviceInfoDto>
            //{
            //    Items = mapper.Map<List<DeviceInfoDto>>(deviceList.Items),
            //    PageNumber = request.PageNumber,
            //    PageSize = request.PageSize,
            //    TotalCount = deviceList.TotalCount
            //};
        }

        /// <summary>
        /// 获取分组树列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<GroupsTreeDto>> GetGroupTreeListAsync(long enterpriseinfoId)
        {
            // 查询所有顶级分组
            var info = await context.Groups.IgnoreQueryFilters().Where(w => !w.IsDelete && w.EnterpriseInfoId == enterpriseinfoId).Includes("Users.ApplicationUser", "Devices.DeviceInfo").ToListAsync();
            if (info is null)
                throw new KeyNotFoundException();
            // 转换为分组 DTO
            var groupDtos = info
                .Where(g => g.PId == null || g.PId == 0) // 获取当前分组
                .Select(g => MapToGroupsTreeDto(g, info)) // 递归映射
                .ToList();
            return groupDtos;
        }

        /// <summary>
        /// 递归绑定子分组
        /// </summary>
        /// <param name="group"></param>
        /// <param name="allGroups"></param>
        /// <returns></returns>
        private GroupsTreeDto MapToGroupsTreeDto(Groups group, List<Groups> allGroups)
        {
            return new GroupsTreeDto
            {
                Id = group.Id,
                Name = group.Name,
                PId = group.PId,
                UserIds = group.Users.Select(gu => gu.ApplicationUserId).ToList(),
                UsersText = string.Join(", ", group.Users.Select(gu => gu.ApplicationUser.NickName.ToString())), // 替换为用户名如果需要
                CreateTimeText = group.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                //Devices = group.Devices.Select(gd => new DeviceInfoDto
                //{
                //    TransId = gd.DeviceInfo.TransId,
                //    Name = gd.DeviceInfo.Name // 假设设备表有 Name 字段
                //}).ToList(),
                Remark = group.Remark,
                Children = allGroups
                    .Where(g => g.PId == group.Id) // 获取当前分组的子分组
                    .Select(g => MapToGroupsTreeDto(g, allGroups)) // 递归加载子分组
                    .ToList()
            };
        }

        /// <summary>
        /// 获取分组分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GroupListDto>> GetGroupPageList(GroupListInput input)
        {
            var query = context.Groups.IgnoreQueryFilters().AsQueryable().Where(w => !w.IsDelete && w.EnterpriseInfoId == input.EnterpriseInfoId);

            //获取当前企业所有分组
            var allGroups = await query.ToListAsync();

            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Name), w => w.Name.Contains(input.Name))
            .WhereIf(input.TimeRange != null && input.TimeRange.Count == 2 && DateTime.TryParse(input.TimeRange[0], out _) && DateTime.TryParse(input.TimeRange[1], out _), w => w.CreateTime >= Convert.ToDateTime(input.TimeRange[0]) && w.CreateTime <= Convert.ToDateTime(input.TimeRange[1]));

            var groups = await query.OrderByDescending(o => o.Name).ToPagedListAsync(input);
            List<GroupListDto> groupsDto = new List<GroupListDto>();
            foreach (var group in groups.Items)
            {
                groupsDto.Add(new GroupListDto()
                {
                    Id = group.Id,
                    Name = group.Name,
                    Pid = group.PId,
                    LayersText = GetGroupLayerText(group.Id, allGroups),
                    CreateTime = group.CreateTime
                });
            }

            string GetGroupLayerText(long groupId, List<Groups> allGroups)
            {
                var group = allGroups.FirstOrDefault(w => w.Id == groupId);
                if (group == null)
                {
                    return string.Empty;
                }

                var layers = new List<string>();
                while (group != null)
                {
                    layers.Insert(0, group.Name);  // 将当前分组名称插入到最前面
                    group = allGroups.FirstOrDefault(w => w.Id == group.PId);
                }
                if (group != null)
                    layers.Insert(0, group.Name);  // 根分组名称
                return string.Join("/", layers);  // 生成层级路径
            }

            return new PagedResultDto<GroupListDto>()
            {
                Items = groupsDto,
                PageNumber = groups.PageNumber,
                PageSize = groups.PageSize,
                TotalCount = groups.TotalCount
            };
        }
    }
}
