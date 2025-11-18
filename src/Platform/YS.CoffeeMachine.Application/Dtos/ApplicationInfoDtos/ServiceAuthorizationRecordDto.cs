using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// ServiceAuthorizationRecordDto
    /// </summary>
    public class ServiceAuthorizationRecordDto
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string FounderUserAccount { get; private set; }
        /// <summary>
        /// 服务人员账号
        /// </summary>
        public string ServiceUserAccount { get; private set; }
        /// <summary>
        /// 服务开始时间
        /// </summary>
        public string AuthorizationStartTime { get; private set; }
        /// <summary>
        /// 服务结束时间
        /// </summary>
        public string AuthorizationEndTime { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ServiceAuthorizationStateEnum State { get; private set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// ServiceAuthorizationRecordDto
        /// </summary>
        protected ServiceAuthorizationRecordDto() { }

        /// <summary>
        /// ServiceAuthorizationRecordDto
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serviceUserAccount"></param>
        /// <param name="founderUserAccount"></param>
        /// <param name="authorizationStartTime"></param>
        /// <param name="authorizationEndTime"></param>
        public ServiceAuthorizationRecordDto(string name, string serviceUserAccount, string founderUserAccount, DateTime authorizationStartTime, DateTime? authorizationEndTime)
        {
            Name = name;
            ServiceUserAccount = serviceUserAccount;
            FounderUserAccount = founderUserAccount;
            AuthorizationStartTime = authorizationStartTime.ToString("yyyy-MM-dd");
            AuthorizationEndTime = authorizationEndTime == null ? "永久" : authorizationEndTime?.ToString("yyyy-MM-dd");
        }
    }
}
