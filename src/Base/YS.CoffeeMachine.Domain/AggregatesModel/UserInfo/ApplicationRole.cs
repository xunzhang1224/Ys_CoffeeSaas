using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class ApplicationRole : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 编码
        /// </summary>
        //public string Code { get; private set; }
        /// <summary>
        /// 状态
        /// </summary>
        public RoleStatusEnum Status { get; private set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleTypeEnum Type { get; private set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum SysMenuType { get; private set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }
        /// <summary>
        /// 部分业务拥有超级管理员权限
        /// </summary>
        public bool? HasSuperAdmin { get; private set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string? Code { get; private set; }

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<ApplicationRoleMenu> ApplicationRoleMenus { get; private set; }

        /// <summary>
        /// 私有构造
        /// </summary>
        protected ApplicationRole()
        {
            ApplicationRoleMenus = new List<ApplicationRoleMenu>();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roleStatus"></param>
        /// <param name="sysMenuType"></param>
        /// <param name="hasSuperAdmin"></param>
        /// <param name="sort"></param>
        /// <param name="remark"></param>
        /// <param name="menuIds"></param>
        /// <param name="isDefault"></param>
        public ApplicationRole(string name, RoleStatusEnum roleStatus, SysMenuTypeEnum sysMenuType, bool? hasSuperAdmin, int sort, string remark, List<long>? menuIds, bool isDefault = false)
        {
            Name = name;
            Status = roleStatus;
            Type = RoleTypeEnum.RrdinaryRole;
            SysMenuType = sysMenuType;
            HasSuperAdmin = hasSuperAdmin;
            IsDefault = isDefault;
            Sort = sort;
            Remark = remark;

            if (menuIds != null)
            {
                ApplicationRoleMenus = new List<ApplicationRoleMenu>();
                menuIds.ForEach(s =>
                {
                    ApplicationRoleMenus.Add(new ApplicationRoleMenu(Id, s));
                });
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="sort"></param>
        /// <param name="remark"></param>
        public void Update(string name, int sort, string remark)
        {
            Name = name;
            Sort = sort;
            Remark = remark;
        }

        /// <summary>
        /// 修改管理员状态
        /// </summary>
        /// <param name="hasSuperAdmin"></param>
        public void UpdateSuperAdmin(bool? hasSuperAdmin)
        {
            HasSuperAdmin = hasSuperAdmin;
        }

        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="roleStatus"></param>
        public void UpdateStatus(RoleStatusEnum roleStatus)
        {
            Status = roleStatus;
        }

        /// <summary>
        /// 修改所属系统类型
        /// </summary>
        /// <param name="sysMenuType"></param>
        public void UpdateSysMenuType(SysMenuTypeEnum sysMenuType)
        {
            SysMenuType = sysMenuType;
        }

        /// <summary>
        /// 绑定角色菜单关系
        /// </summary>
        /// <param name="menuIds"></param>
        public void UpdateRoleMenuIds(List<long> menuIds, List<long>? halfMenuIds = null)
        {
            ApplicationRoleMenus.Clear();
            menuIds.ForEach(s =>
            {
                bool? isHalf = null;
                if (halfMenuIds != null)
                {
                    isHalf = halfMenuIds.Contains(s);
                }
                ApplicationRoleMenus.Add(new ApplicationRoleMenu(Id, s, isHalf));
            });
        }

        /// <summary>
        /// 清除角色菜单关系
        /// </summary>
        public void ClearRoleMenu()
        {
            ApplicationRoleMenus.Clear();
        }
    }
}
