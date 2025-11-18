using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 用户权限关联表
    /// </summary>
    public class ApplicationUserRole : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; private set; }

        /// <summary>
        /// 角色
        /// </summary>
        public ApplicationRole Role { get; private set; }

        /// <summary>
        /// 私有构造
        /// </summary>
        protected ApplicationUserRole() { }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>

        public ApplicationUserRole(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
