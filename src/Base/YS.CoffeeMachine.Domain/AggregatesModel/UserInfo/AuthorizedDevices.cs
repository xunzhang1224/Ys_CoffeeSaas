using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 授权设备
    /// </summary>
    public class AuthorizedDevices : BaseEntity
    {
        /// <summary>
        /// 服务授权Id
        /// </summary>
        public long ServiceAuthorizationRecordId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; private set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceAuthorizationRecordId"></param>
        /// <param name="deviceId"></param>
        public AuthorizedDevices(long serviceAuthorizationRecordId, long deviceId)
        {
            ServiceAuthorizationRecordId = serviceAuthorizationRecordId;
            DeviceId = deviceId;
        }
    }
}
