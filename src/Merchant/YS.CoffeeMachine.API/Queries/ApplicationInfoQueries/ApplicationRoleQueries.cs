using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.ApplicationInfoQueries
{
    /// <summary>
    /// 角色信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class ApplicationRoleQueries(CoffeeMachineDbContext context, IMapper mapper, UserHttpContext _user) : IApplicationRoleQueries
    {
        /// <summary>
        /// 根据Id查询角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeMenu"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApplicationRoleDto> GetApplicationRoleAsync(long id, bool IncludeMenu)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = IncludeMenu ? await context.ApplicationRole.Includes("ApplicationRoleMenus.Menu").FirstOrDefaultAsync(x => x.Id == id) : await context.ApplicationRole.FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            ApplicationRoleDto applicationRoleDto = mapper.Map<ApplicationRoleDto>(info);
            if (IncludeMenu)
                applicationRoleDto.BindRoleMenu(info.ApplicationRoleMenus);
            return applicationRoleDto;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleListAsync(QueryRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            //获取分页数据
            var list = await context.ApplicationRole.Where(w => w.SysMenuType == (SysMenuTypeEnum)Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType))).ToPagedListAsync(request, "ApplicationRoleMenus.Menu");
            ApplicationRoleListDto listDto = new ApplicationRoleListDto(list.Items);
            PagedResultDto<ApplicationRoleDto> pagedResult = new PagedResultDto<ApplicationRoleDto>()
            {
                Items = listDto.ApplicationRoleDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResult;
        }

        /// <summary>
        /// 根据企业Id获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseId"></param>
        /// <param name="name"></param>
        /// <param name="roleStatus"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleByEnterpriseIdListAsync(QueryRequest request, long enterpriseId, string name, RoleStatusEnum? roleStatus)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            // 获取企业下所有角色Id
            var curRoleIds = await context.EnterpriseInfo
                .AsNoTracking()
                .Where(e => e.Id == enterpriseId)
                .SelectMany(e => e.Roles)
                .Select(r => r.RoleId)
                .ToListAsync();

            // 筛选角色  && w.SysMenuType == SysMenuTypeEnum.Merchant && w.Status == RoleStatusEnum.Enable   不需要这个条件
            var rolesQuery = context.ApplicationRole.AsNoTracking().AsSplitQuery().Include(i => i.ApplicationRoleMenus.Where(w => w.IsHalf != true)).ThenInclude(ti => ti.Menu)
                .Where(w =>
                 (w.IsDefault || curRoleIds.Contains(w.Id)) && (w.SysMenuType == SysMenuTypeEnum.Merchant || w.SysMenuType == SysMenuTypeEnum.H5));

            // 使用 WhereIf 进行动态筛选
            rolesQuery = rolesQuery
                        .WhereIf(!string.IsNullOrWhiteSpace(name), w => w.Name.Contains(name))
                        .WhereIf(roleStatus.HasValue, w => w.Status == roleStatus);

            // 分页和排序
            var rolesPagedQuery = rolesQuery
            .OrderByDescending(r => r.IsDefault)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

            var roles = await rolesPagedQuery.ToListAsync();

            // 获取总数
            var totalCount = await rolesQuery.CountAsync();

            //var menuIds = roles.SelectMany(s => s.ApplicationRoleMenus).ToDictionary(s => s.RoleId, s => s.MenuId).ToList();
            //var allMenus = await context.ApplicationMenu.AsNoTracking().Where(w => menuIds.Select(s => s.Value).Contains(w.Id)).ToListAsync();

            var roleMenusDict = roles.SelectMany(s => s.ApplicationRoleMenus)
                        .GroupBy(s => s.RoleId)
                        .ToDictionary(g => g.Key, g => g.Select(x => x.MenuId).ToList());

            // 获取所有唯一的菜单ID
            var allMenuIds = roleMenusDict.SelectMany(s => s.Value).Distinct().ToList();

            // 查询所有菜单
            var allMenus = await context.ApplicationMenu
                .AsNoTracking()
                .Where(w => allMenuIds.Contains(w.Id))
                .ToListAsync();

            // 组装返回结果
            var result = new PagedResultDto<ApplicationRoleDto>
            {
                TotalCount = totalCount,
                Items = new ApplicationRoleListDto(roles.ToList(), roleMenusDict, allMenus).ApplicationRoleDtos
            };

            foreach (var item in result.Items)
            {
                // L.Text[item.Code] == item.Code ? item.Name :
                item.Name = item.Code == null ? item.Name : (L.Text[item.Code]);
            }

            //foreach (var item in data.Items)
            //{
            //    item.AbnormalCode = L.Text["Error_" + item.AbnormalCode] == "Error_" + item.AbnormalCode ? faultCodeList.FirstOrDefault(x => x.Code == item.AbnormalCode)?.Name
            //        : L.Text["Error_" + item.AbnormalCode];
            //}

            return result;
        }

        /// <summary>
        /// 获取当前企业角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRoleDto>> GetEnterpriceRoleList(long enterpriseId)
        {
            // 获取企业下所有角色Id
            var curRoleIds = await context.EnterpriseInfo
                .AsNoTracking()
                .Where(e => e.Id == enterpriseId)
                .SelectMany(e => e.Roles)
                .Select(r => r.RoleId)
                .ToListAsync();

            // 筛选角色  && w.SysMenuType == SysMenuTypeEnum.Merchant && w.Status == RoleStatusEnum.Enable   不需要这个条件
            var roles = await context.ApplicationRole.AsNoTracking().AsSplitQuery().Include(i => i.ApplicationRoleMenus).ThenInclude(ti => ti.Menu)
                .Where(w =>
                 (w.IsDefault || curRoleIds.Contains(w.Id)) && w.SysMenuType == SysMenuTypeEnum.Merchant && w.Status == RoleStatusEnum.Enable).ToListAsync();

            var roleMenusDict = roles.SelectMany(s => s.ApplicationRoleMenus)
                        .GroupBy(s => s.RoleId)
                        .ToDictionary(g => g.Key, g => g.Select(x => x.MenuId).ToList());

            // 获取所有唯一的菜单ID
            var allMenuIds = roleMenusDict.SelectMany(s => s.Value).Distinct().ToList();

            // 查询所有菜单
            var allMenus = await context.ApplicationMenu
                .AsNoTracking()
                .Where(w => allMenuIds.Contains(w.Id))
                .ToListAsync();

            var tt = new ApplicationRoleListDto(roles.ToList(), roleMenusDict, allMenus).ApplicationRoleDtos;
            foreach (var item in tt)
            {
                item.Name = item.Code == null ? item.Name : (L.Text[item.Code]);
            }
            return tt;
        }

        /// <summary>
        /// 根据角色Id获取菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetUserMenusByRoleIdAsync(long roleId)
        {
            return await context.ApplicationRole.Where(w => w.Id == roleId).AsNoTracking()
                .Include(i => i.ApplicationRoleMenus).ThenInclude(t => t.Menu)
                .Where(w => w.SysMenuType == (SysMenuTypeEnum)Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType)))
                .Select(s => s.Id).ToListAsync();
        }

        /// <summary>
        /// 根据企业Id获取角色下拉框
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<List<object>> GetRoleSelectAsync(long enterpriseId)
        {
            var enterprises = await context.EnterpriseInfo.Include(i => i.Roles).Where(w => w.Id == enterpriseId).ToListAsync();
            var roleIds = enterprises.SelectMany(s => s.Roles).Select(s => s.RoleId).ToList();
            // 获取企业信息，并从中选择所需的角色信息
            var roleSelect = await context.ApplicationRole
                .Where(w => (roleIds.Contains(w.Id) || w.IsDefault) && w.Status == RoleStatusEnum.Enable)
                .Select(r => new { r.Id, r.Name })  // 使用匿名类型来简化操作
                .ToListAsync();

            // 转换为对象列表
            return roleSelect.Select(r => (object)new { r.Id, r.Name }).ToList();
        }
    }
}
