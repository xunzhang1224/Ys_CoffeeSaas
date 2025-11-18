using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 设备补货记录
    /// </summary>
    public class DeviceRestockLog : EnterpriseBaseEntity
    {
        /// <summary>
        /// 租户设备id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; private set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        public string DeviceCode { get; private set; }

        /// <summary>
        /// 补货编号
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public string? DeviceDZ { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        public RestockTypeEnum Type { get; private set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<DeviceRestockLogSub> DeviceRestockLogSubs { get; private set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DeviceRestockLog()
        {
            DeviceRestockLogSubs = new List<DeviceRestockLogSub>();
        }

        /// <summary>
        /// 添加补货记录
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="deviceCode">设备编号</param>
        /// <param name="deviceDZ">设备地址</param>
        /// <param name="type">补货类型</param>
        /// <param name="enterpriseinfoId">补货类型</param>
        public DeviceRestockLog(
            long deviceId,
            string deviceName,
            string deviceCode,
            string deviceDZ,
            RestockTypeEnum type,long enterpriseinfoId) : this()
        {
            DeviceId = deviceId;
            Code = "BH" + YitIdHelper.NextId();
            DeviceName = deviceName;
            DeviceCode = deviceCode;
            DeviceDZ = deviceDZ;
            Type = type;
            EnterpriseinfoId = enterpriseinfoId;
        }

        /// <summary>
        /// 手动设置创建人
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public void SetCreateInfo(long userId, string userName)
        {
            CreateUserId = userId;
            CreateUserName = userName;
        }

        /// <summary>
        /// 添加详情
        /// </summary>
        /// <param name="subItem"></param>
        public void AddSubItem(List<DeviceRestockLogSub> subItem)
        {
            DeviceRestockLogSubs.AddRange(subItem);
        }
    }
}
