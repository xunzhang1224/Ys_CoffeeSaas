using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{

    /// <summary>
    /// 服务授权记录
    /// </summary>
    public class ServiceAuthorizationRecord : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long FounderId { get; private set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public ApplicationUser FounderUser { get; private set; }

        /// <summary>
        /// 授权设备
        /// </summary>
        public List<AuthorizedDevices> AuthorizedDevices { get; private set; }
        /// <summary>
        /// 服务人员账号
        /// </summary>
        public string ServiceUserAccount { get; private set; }
        /// <summary>
        /// 服务人员
        /// </summary>
        public long ServiceUserId { get; private set; }

        /// <summary>
        /// 服务人员
        /// </summary>
        public ApplicationUser ServiceUser { get; private set; }
        /// <summary>
        /// 服务开始时间
        /// </summary>
        public DateTime AuthorizationStartTime { get; private set; }
        /// <summary>
        /// 服务结束时间
        /// </summary>
        public DateTime? AuthorizationEndTime { get; private set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        public ServiceAuthorizationStateEnum State { get; private set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 服务授权记录
        /// </summary>
        protected ServiceAuthorizationRecord() { }

        /// <summary>
        /// 服务授权记录
        /// </summary>
        public ServiceAuthorizationRecord(string name, List<long> deviceIds, string serviceUserAccount, long serviceUserId, DateTime? authorizationEndTime)
        {
            AuthorizedDevices = new List<AuthorizedDevices>();
            Name = name;
            deviceIds.ForEach(x =>
            {
                AuthorizedDevices.Add(new AuthorizedDevices(Id, x));
            });
            ServiceUserAccount = serviceUserAccount;
            ServiceUserId = serviceUserId;
            AuthorizationStartTime = DateTime.UtcNow;
            AuthorizationEndTime = authorizationEndTime;
            State = ServiceAuthorizationStateEnum.Create;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update(ServiceAuthorizationStateEnum state)
        {
            State = state;
            if (state == ServiceAuthorizationStateEnum.Rejected || state == ServiceAuthorizationStateEnum.Completed)
                AuthorizationEndTime = DateTime.UtcNow;
        }
    }
}
