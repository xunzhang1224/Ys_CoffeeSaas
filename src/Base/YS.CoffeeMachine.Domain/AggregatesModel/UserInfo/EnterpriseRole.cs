using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 企业角色
    /// </summary>
    public class EnterpriseRole : BaseEntity
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; private set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; private set; }

        /// <summary>
        /// 角色
        /// </summary>
        public ApplicationRole Role { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EnterpriseRole() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseRole(long enterpriseId, long roleId)
        {
            EnterpriseId = enterpriseId;
            RoleId = roleId;
        }
    }
}
