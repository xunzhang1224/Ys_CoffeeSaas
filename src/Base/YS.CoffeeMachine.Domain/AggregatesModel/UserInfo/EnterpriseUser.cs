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
    /// </summary>
    public class EnterpriseUser : BaseEntity
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; private set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// 用户
        /// </summary>
        public ApplicationUser User { get; private set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected EnterpriseUser() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseUser(long enterpriseId, long userId)
        {
            EnterpriseId = enterpriseId;
            UserId = userId;
        }
    }
}
