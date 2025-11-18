using AutoMapper;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries
{
    /// <summary>
    /// 菜单权限查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_user"></param>
    public class P_ApplicationMenuQueries(CoffeeMachinePlatformDbContext context, IMapper mapper, IRedisClient _redisClient, UserHttpContext _user) : IP_ApplicationMenuQueries
    {
        private static string GetBasketUserKey(string sysMenuType, long userId) => $"/UserMenus/{sysMenuType}/Menu{userId}";
        /// <summary>
        /// 根据Id获取菜单权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApplicationMenuDto> GetApplicationMenuAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.ApplicationMenu.FirstOrDefaultAsync(x => x.Id == id && x.SysMenuType == SysMenuTypeEnum.Platform);
            if (info is null)
                throw new KeyNotFoundException();
            ApplicationMenuDto applicationMenuDto = mapper.Map<ApplicationMenuDto>(info);
            return applicationMenuDto;
        }

        /// <summary>
        /// 根据平台类型获取所有菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuSelectDto>> GetAllMenuTreeAsync()
        {
            var allMenu = await context.ApplicationMenu.Where(w => !w.IsDelete && w.SysMenuType == SysMenuTypeEnum.Platform).ToListAsync();
            return BuildMenuTree(allMenu); // 从根节点开始构建
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sysMenuType"></param>
        /// <returns></returns>
        public async Task<ApplicationMenuListDto> GetApplicationMenuListAsync(QueryRequest request, SysMenuTypeEnum sysMenuType)
        {
            request.NotPaginate = true;
            var list = await context.ApplicationMenu.Where(w => w.SysMenuType == sysMenuType).ToPagedListAsync(request);
            ApplicationMenuListDto ListDto = new ApplicationMenuListDto(list.Items);
            return ListDto;
        }

        /// <summary>
        /// 获取当前用户的菜单权限列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<object>> GetUserMenuTreeAsync()
        {
            var user = await context.ApplicationUser.FirstOrDefaultAsync(w => w.Id == _user.UserId);
            if (user == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            var menus = new List<ApplicationMenuDto>();
            //超级管理员获取所有菜单
            if (user.AccountType == AccountTypeEnum.SuperAdmin)
                menus = await context.ApplicationMenu.Include(i => i.ApplicationMenus).Where(w => w.SysMenuType == SysMenuTypeEnum.Platform)
                               .Select(menu => new ApplicationMenuDto(menu))
                               .ToListAsync();
            else//普通用户获取当前用户所在企业拥有的菜单
                menus = await context.ApplicationUser
                               .Where(u => u.Id == _user.UserId)
                               .SelectMany(u => u.ApplicationUserRoles.Select(ur => ur.Role).Where(w => w.Status == RoleStatusEnum.Enable)
                                   .SelectMany(r => r.ApplicationRoleMenus.Select(rm => rm.Menu))).Include(i => i.ApplicationMenus)
                               .Distinct() // 去重
                               .Select(menu => new ApplicationMenuDto(menu))
                               .ToListAsync();
            // 构建树形结构
            var menuTree = BuildUserMenuTree(menus);
            return ConvertToJsonFormat(menuTree);
        }

        /// <summary>
        /// 根据企业类型Id获取菜单树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AllMenuSelectDto> GetMenuSelectByEnterpriseTypeIdAsync(long id)
        {
            var enterpriseTypeInfo = await context.EnterpriseTypes.FirstAsync(x => x.Id == id);
            if (enterpriseTypeInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var astrict = enterpriseTypeInfo.Astrict;
            var query = astrict ? context.ApplicationMenu : context.ApplicationMenu.Where(w => w.Title != "设备分配");
            var allMenu = await query.Where(w => !w.IsDelete && (w.SysMenuType == SysMenuTypeEnum.Merchant || w.SysMenuType == SysMenuTypeEnum.H5)).ToListAsync();
            var pcMenu = allMenu.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).ToList();
            var h5Menu = allMenu.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).ToList();

            var allMenuSelectDto = new AllMenuSelectDto()
            {
                PCMenuSelectDtos = BuildMenuTree(pcMenu),
                H5MenuSelectDtos = BuildMenuTree(h5Menu)
            };

            return allMenuSelectDto; // 从根节点开始构建
        }

        /// <summary>
        /// 生成菜单树结构
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public List<MenuSelectDto> BuildMenuTree(List<ApplicationMenu> menus)
        {
            // 获取根节点（ParentId 为 null 的菜单）
            var rootNodes = menus
                .Where(menu => menu.ParentId == null || menu.ParentId == 0)
                .Select(menu => new MenuSelectDto
                {
                    Id = menu.Id,
                    ParentId = menu.ParentId,
                    Title = menu.Title,
                    Name = menu.Name,
                    SubCount = CountButtonChildren(menu.Id, menus),  // 统计按钮数量
                    Children = GetChildren(menu.Id, menus)
                })
                .ToList();

            return rootNodes;
        }

        /// <summary>
        /// 组装菜单树结构
        /// </summary>
        /// <param name="menuTree"></param>
        /// <returns></returns>
        private List<object> ConvertToJsonFormat(List<ApplicationMenuDto> menuTree)
        {
            return menuTree.Select(menu =>
            {
                dynamic result = new ExpandoObject();
                result.path = menu.Path;
                result.name = menu.Name;
                result.redirect = menu.Redirect;
                result.component = menu.Component;
                result.meta = new
                {
                    menu.Id,
                    menu.ParentId,
                    menu.MenuType,
                    menu.SysMenuType,
                    menu.Title,
                    menu.Component,
                    menu.Rank,
                    menu.Icon,
                    menu.ExtraIcon,
                    menu.EnterTransition,
                    menu.LeaveTransition,
                    menu.FrameSrc,
                    menu.FrameLoading,
                    menu.KeepAlive,
                    menu.HiddenTag,
                    menu.FixedTag,
                    menu.ShowLink,
                    menu.ShowParent,
                    menu.Remark,
                    menu.Auths
                };

                if (menu.Children != null && menu.Children.Any())
                {
                    result.children = ConvertToJsonFormat(menu.Children);
                }

                return result;
            }).Cast<object>().ToList();
        }

        /// <summary>
        /// 构建用户菜单树
        /// </summary>
        /// <param name="menuList"></param>
        /// <returns></returns>
        private List<ApplicationMenuDto> BuildUserMenuTree(List<ApplicationMenuDto> menuList)
        {
            var lookup = menuList.ToLookup(m => m.ParentId);
            List<ApplicationMenuDto> BuildTree(long? parentId)
            {
                return lookup[parentId].Where(w => w.MenuType != MenuTypeEnum.Btn)
                    .Select(menu =>
                    {

                        return new ApplicationMenuDto
                        {
                            Id = menu.Id,
                            ParentId = menu.ParentId,
                            MenuType = menu.MenuType,
                            SysMenuType = menu.SysMenuType,
                            Title = menu.Title,
                            Name = menu.Name,
                            Path = menu.Path,
                            Component = menu.Component,
                            Rank = menu.Rank,
                            Redirect = menu.Redirect,
                            Icon = menu.Icon,
                            ExtraIcon = menu.ExtraIcon,
                            EnterTransition = menu.EnterTransition,
                            LeaveTransition = menu.LeaveTransition,
                            FrameSrc = menu.FrameSrc,
                            FrameLoading = menu.FrameLoading,
                            KeepAlive = menu.KeepAlive,
                            HiddenTag = menu.HiddenTag,
                            FixedTag = menu.FixedTag,
                            ShowLink = menu.ShowLink,
                            ShowParent = menu.ShowParent,
                            Remark = menu.Remark,
                            Auths = menu.Auths, // 当前菜单项的按钮权限
                            Auth = menu.Auth,
                            Children = BuildTree(menu.Id)
                        };
                    })
                    .ToList();
            }

            return BuildTree(null);
        }

        // 递归获取子菜单
        private List<MenuSelectDto> GetChildren(long parentId, List<ApplicationMenu> menus)
        {
            return menus
                .Where(menu => menu.ParentId == parentId)
                .Select(menu => new MenuSelectDto
                {
                    Id = menu.Id,
                    ParentId = menu.ParentId,
                    Title = menu.Title,
                    Name = menu.Name,
                    SubCount = CountButtonChildren(menu.Id, menus),
                    Children = GetChildren(menu.Id, menus)
                })
                .ToList();
        }

        // 统计子菜单的数量
        private int CountButtonChildren(long parentId, List<ApplicationMenu> menus)
        {
            return menus
                .Where(menu => menu.ParentId == parentId).Count();
        }

        /// <summary>
        /// 获取当前用户拥有的标识集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetOwnBtnPermList(long userId)
        {
            //获取当前用户账号类型
            var sysMenuType = Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType)).ToString();
            //获取当前用户缓存中的权限标识
            var cacheMenu = await _redisClient.GetAsync<List<string>>(GetBasketUserKey(sysMenuType, userId));
            //如果缓存中有数据，直接返回
            if (cacheMenu != null && cacheMenu.Count > 0)
                return cacheMenu;

            var menus = new List<string>();
            //获取当前用户信息
            var user = await context.ApplicationUser.FirstOrDefaultAsync(w => w.Id == userId);
            if (user == null)
                return menus;
            if (user.AccountType == AccountTypeEnum.SuperAdmin)
            {
                menus = await GetAllBtnPermList();
                await _redisClient.SetAsync(GetBasketUserKey(sysMenuType, userId), menus);
            }
            else
            {
                menus = await context.ApplicationUser.Where(w => w.Id == userId)
                    .SelectMany(s => s.ApplicationUserRoles).Select(s => s.Role)
                    .SelectMany(s => s.ApplicationRoleMenus).Select(s => s.Menu).Where(w => w.SysMenuType == (SysMenuTypeEnum)Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType)))
                    .Select(s => s.Auths)
                    .Distinct()
                    .ToListAsync();
                await _redisClient.SetAsync(GetBasketUserKey(sysMenuType, userId), menus);
            }
            return menus;
        }
        /// <summary>
        /// 获取所有权限标识集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllBtnPermList()
        {
            var sysMenuTypeStr = Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType)).ToString();
            var cacheMenu = await _redisClient.GetAsync<List<string>>(GetBasketUserKey(sysMenuTypeStr, 0).ToString());
            if (cacheMenu != null && cacheMenu.Count > 0)
                return cacheMenu;
            else
            {
                var sysMenuType = (SysMenuTypeEnum)Enum.ToObject(typeof(SysMenuTypeEnum), Convert.ToInt32(_user.SysMenuType));
                var allMenus = await context.ApplicationMenu.Where(w => w.SysMenuType == sysMenuType && !string.IsNullOrWhiteSpace(w.Auths)).AsNoTracking().Select(s => s.Auths).Distinct().ToListAsync();
                await _redisClient.SetAsync(GetBasketUserKey(sysMenuTypeStr, 0), allMenus, TimeSpan.FromDays(3));
                return allMenus;
            }
        }
    }
}
