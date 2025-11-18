using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 角色
    /// </summary>
    public class ApplicationRoleDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public RoleStatusEnum Status { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 部分业务拥有超级管理员权限
        /// </summary>
        public bool? HasSuperAdmin { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// PC商户菜单Ids
        /// </summary>
        public List<long> MenuIds
        {
            get { return applicationMenuDtos == null ? [] : applicationMenuDtos.Select(s => s.Id).ToList(); }
        }

        /// <summary>
        /// H5菜单Ids
        /// </summary>
        public List<long> H5MenuIds
        {
            get { return applicationH5MenuDtos == null ? [] : applicationH5MenuDtos.Select(s => s.Id).ToList(); }
        }

        /// <summary>
        /// PC商户菜单
        /// </summary>
        public List<ApplicationMenuDto> applicationMenuDtos { get; set; }

        /// <summary>
        /// H5菜单
        /// </summary>
        public List<ApplicationMenuDto> applicationH5MenuDtos { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        protected ApplicationRoleDto() { applicationMenuDtos = new List<ApplicationMenuDto>(); applicationH5MenuDtos = new List<ApplicationMenuDto>(); }

        /// <summary>
        /// 绑定角色菜单
        /// </summary>
        /// <param name="roleMenus"></param>
        public void BindRoleMenu(List<ApplicationRoleMenu> roleMenus)
        {
            var menus = new List<ApplicationMenu>();
            if (roleMenus.Count > 0)
            {
                menus = roleMenus.Select(s => s.Menu).ToList();

                if (menus.Count > 0)
                {
                    applicationMenuDtos = new List<ApplicationMenuDto>();
                    menus.ForEach(x =>
                    {
                        applicationMenuDtos.Add(new ApplicationMenuDto(x.Id, x.ParentId, x.MenuType, x.SysMenuType, x.Title, x.Name, x.Path, x.Component, x.Rank, x.Redirect, x.Icon, x.ExtraIcon, x.EnterTransition, x.LeaveTransition, x.Auths, x.FrameSrc, x.FrameLoading, x.KeepAlive, x.HiddenTag, x.FixedTag, x.ShowLink, x.ShowParent, x.Remark));
                    });
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="isDefault"></param>
        /// <param name="sort"></param>
        /// <param name="hasSuperAdmin"></param>
        /// <param name="remark"></param>
        /// <param name="applicationRoleMenus"></param>
        public ApplicationRoleDto(long id, string name, RoleStatusEnum status, bool isDefault, int sort, bool? hasSuperAdmin, string remark, string code, List<ApplicationMenu>? applicationMenus = null, List<ApplicationMenu>? applicationH5Menus = null)
        {
            Id = id;
            Name = name;
            Status = status;
            IsDefault = isDefault;
            Sort = sort;
            HasSuperAdmin = hasSuperAdmin;
            Remark = remark;
            if (applicationMenus != null && applicationMenus.Count > 0)
            {
                //var menus = new List<ApplicationMenu>();
                //menus = applicationMenus.Select(s => s.Menu).ToList();
                if (applicationMenus.Count > 0)
                {
                    applicationMenuDtos = new List<ApplicationMenuDto>();
                    applicationMenus.ForEach(x =>
                    {
                        applicationMenuDtos.Add(new ApplicationMenuDto(x.Id, x.ParentId, x.MenuType, x.SysMenuType, x.Title, x.Name, x.Path, x.Component, x.Rank, x.Redirect, x.Icon, x.ExtraIcon, x.EnterTransition, x.LeaveTransition, x.Auths, x.FrameSrc, x.FrameLoading, x.KeepAlive, x.HiddenTag, x.FixedTag, x.ShowLink, x.ShowParent, x.Remark));
                    });
                }
            }
            if (applicationH5Menus != null && applicationH5Menus.Count > 0)
            {
                //var h5Menus = new List<ApplicationMenu>();
                //h5Menus = applicationH5Menus.Select(s => s.Menu).ToList();
                if (applicationH5Menus.Count > 0)
                {
                    applicationH5MenuDtos = new List<ApplicationMenuDto>();
                    applicationH5Menus.ForEach(x =>
                    {
                        applicationH5MenuDtos.Add(new ApplicationMenuDto(x.Id, x.ParentId, x.MenuType, x.SysMenuType, x.Title, x.Name, x.Path, x.Component, x.Rank, x.Redirect, x.Icon, x.ExtraIcon, x.EnterTransition, x.LeaveTransition, x.Auths, x.FrameSrc, x.FrameLoading, x.KeepAlive, x.HiddenTag, x.FixedTag, x.ShowLink, x.ShowParent, x.Remark));
                    });
                }
            }
            Code = code;
        }
    }

    /// <summary>
    /// 角色列表
    /// </summary>
    public class ApplicationRoleListDto
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<ApplicationRoleDto> ApplicationRoleDtos { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ApplicationRoleListDto() { ApplicationRoleDtos = new List<ApplicationRoleDto>(); }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationRoleListDto(List<ApplicationRole> applicationRoles, Dictionary<long, List<long>>? roleMenuDic = null, List<ApplicationMenu>? menus = null)
        {
            if (applicationRoles.Count > 0)
            {
                ApplicationRoleDtos = new List<ApplicationRoleDto>();
                //var pcMenuIds = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).Select(s => s.Id).ToList();
                //var h5MenuIds = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToList();
                //var pcMenus = applicationRoles.SelectMany(s => s.ApplicationRoleMenus).Where(w => pcMenuIds.Contains(w.MenuId)).ToList();
                //var h5Menus = applicationRoles.SelectMany(s => s.ApplicationRoleMenus).Where(w => h5MenuIds.Contains(w.MenuId)).ToList();

                // 获取所有PC菜单ID和H5菜单ID
                //var pcMenuIds = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).Select(s => s.Id).ToList();
                //var h5MenuIds = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToList();

                var pcMenusAll = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).ToList();
                var h5MenusAll = menus == null ? [] : menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).ToList();

                applicationRoles.ForEach(x =>
                {
                    // 从字典中获取当前角色的菜单ID列表
                    var roleMenuIds = roleMenuDic.ContainsKey(x.Id) ? roleMenuDic[x.Id] : new List<long>();

                    // 根据角色菜单ID筛选出PC菜单和H5菜单对象
                    var pcMenusForRole = pcMenusAll.Where(menu => roleMenuIds.Contains(menu.Id)).ToList();
                    var h5MenusForRole = h5MenusAll.Where(menu => roleMenuIds.Contains(menu.Id)).ToList();
                    ApplicationRoleDtos.Add(new ApplicationRoleDto(x.Id, x.Name, x.Status, x.IsDefault, x.Sort, x.HasSuperAdmin, x.Remark, x.Code, pcMenusForRole, h5MenusForRole));
                });
            }
        }
    }
}
