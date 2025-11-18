using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries
{
    /// <summary>
    /// 平台角色查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class P_ApplicationRoleQueries(CoffeeMachinePlatformDbContext context, IMapper mapper, UserHttpContext _user) : IP_ApplicationRoleQueries
    {
        /// <summary>
        /// 根据Id查询平台角色信息
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

        #region 平台角色查询

        /// <summary>
        /// 获取平台角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roleStatus"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<ApplicationRoleDto>> GetPlatformRoleListAsync(QueryRequest request, RoleStatusEnum? roleStatus)
        {
            if ((request.PageNumber <= 0 || request.PageSize <= 0) && !request.NotPaginate)
                throw new ArgumentException("参数异常，请检查");
            //获取分页数据
            var list = await context.ApplicationRole.Where(w => w.SysMenuType == SysMenuTypeEnum.Platform && (roleStatus == null || w.Status == roleStatus) && !w.IsDelete).ToPagedListAsync(request, "ApplicationRoleMenus.Menu");
            //var dic = list.Items.SelectMany(s => s.ApplicationRoleMenus).GroupBy(g => g.RoleId).ToDictionary(k => k.Key, v => v.ToList());

            var roleMenusDict = list.Items.SelectMany(s => s.ApplicationRoleMenus)
            .GroupBy(s => s.RoleId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.MenuId).ToList());

            // 获取所有唯一的菜单ID
            var allMenuIds = roleMenusDict.SelectMany(s => s.Value).Distinct().ToList();

            // 查询所有菜单
            var allMenus = await context.ApplicationMenu
                .AsNoTracking()
                .Where(w => allMenuIds.Contains(w.Id))
                .ToListAsync();

            ApplicationRoleListDto listDto = new ApplicationRoleListDto(list.Items, roleMenusDict, allMenus);
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
        /// 根据角色Id获取平台菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetPlatformMenusByRoleIdAsync(long roleId)
        {
            return await context.ApplicationRole.Where(w => w.Id == roleId).AsNoTracking()
                .Include(i => i.ApplicationRoleMenus)
                .Where(w => w.SysMenuType == SysMenuTypeEnum.Platform)
                .SelectMany(s => s.ApplicationRoleMenus.Where(w => w.IsHalf != true).Select(s => s.MenuId)).ToListAsync();
        }
        #endregion

        #region 商户角色查询

        /// <summary>
        /// 获取默认商户角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<ApplicationRoleDto>> GetMerchantRoleListAsync(QueryRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw new ArgumentException("参数异常，请检查");
            //获取分页数据
            var list = await context.ApplicationRole.Where(w => (w.SysMenuType == SysMenuTypeEnum.Merchant || w.SysMenuType == SysMenuTypeEnum.H5) && w.IsDefault && !w.IsDelete).Include(i => i.ApplicationRoleMenus.Where(w => w.IsHalf != true)).ThenInclude(ti => ti.Menu)
                .ToPagedListAsync(request);
            var menuIds = list.Items.SelectMany(s => s.ApplicationRoleMenus.Select(s => s.MenuId)).Distinct().ToList();
            var menus = await context.ApplicationMenu.Where(w => menuIds.Contains(w.Id)).ToListAsync();

            //var roleMenus = list.Items.SelectMany(s => s.ApplicationRoleMenus).Where(w => menus.Where(m => m.SysMenuType == SysMenuTypeEnum.Merchant).Select(ms => ms.Id).Contains(w.MenuId)).ToList();
            //var roleH5Menus = list.Items.SelectMany(s => s.ApplicationRoleMenus).Where(w => menus.Where(m => m.SysMenuType == SysMenuTypeEnum.H5).Select(ms => ms.Id).Contains(w.MenuId)).ToList();

            //        var merchantMenuIds = menus
            //.Where(m => m.SysMenuType == SysMenuTypeEnum.Merchant)
            //.Select(m => m.Id)
            //.ToHashSet();

            //        var roleMenus = list.Items
            //.SelectMany(s => s.ApplicationRoleMenus)
            //.Where(w => w != null && merchantMenuIds.Contains(w.MenuId))
            //.GroupBy(g => g.RoleId)
            //.ToDictionary(g => g.Key, g => g.ToList());

            //        var merchantH5MenuIds = menus
            //.Where(m => m.SysMenuType == SysMenuTypeEnum.H5)
            //.Select(m => m.Id)
            //.ToHashSet();

            //        var roleH5Menus = list.Items
            //            .SelectMany(s => s.ApplicationRoleMenus)
            //            .Where(w => merchantH5MenuIds.Contains(w.MenuId))
            //            .GroupBy(g => g.RoleId)
            //            .ToDictionary(g => g.Key, g => g.ToList());

            var roleMenusDict = list.Items.SelectMany(s => s.ApplicationRoleMenus)
            .GroupBy(s => s.RoleId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.MenuId).ToList());

            // 获取所有唯一的菜单ID
            var allMenuIds = roleMenusDict.SelectMany(s => s.Value).Distinct().ToList();

            // 查询所有菜单
            var allMenus = await context.ApplicationMenu
                .AsNoTracking()
                .Where(w => allMenuIds.Contains(w.Id))
                .ToListAsync();

            ApplicationRoleListDto listDto = new ApplicationRoleListDto(list.Items, roleMenusDict, allMenus);
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
        /// 根据角色Id获取平台菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetMerchantMenusByRoleIdAsync(long roleId)
        {
            return await context.ApplicationRole.Where(w => w.Id == roleId).AsNoTracking()
                .Include(i => i.ApplicationRoleMenus)
                .Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant)
                .SelectMany(s => s.ApplicationRoleMenus.Where(w => w.IsHalf != true).Select(s => s.MenuId)).ToListAsync();
        }
        #endregion
    }
}
