using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 权限与菜单关联表
    /// </summary>
    public class ApplicationRoleMenu : BaseEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; private set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; private set; }

        /// <summary>
        /// 是否半选
        /// </summary>
        public bool? IsHalf { get; private set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public ApplicationMenu Menu { get; private set; }

        /// <summary>
        /// 私有构造
        /// </summary>
        protected ApplicationRoleMenu() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        public ApplicationRoleMenu(long roleId, long menuId, bool? isHalf = null)
        {
            RoleId = roleId;
            MenuId = menuId;
            IsHalf = isHalf;
        }
    }
}