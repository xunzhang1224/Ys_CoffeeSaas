using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.EnterpriseDeviceBaseEntity;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 设备异常
    /// </summary>
    public class DeviceAbnormal : EDBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 机器
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string? DeviceName { get; protected set; }

        /// <summary>
        /// 型号名
        /// </summary>
        public string? DeviceModelName { get; protected set; }

        /// <summary>
        /// 业务流水号
        /// </summary>
        public string? TransId { get; private set; }

        /// <summary>
        /// 柜号
        /// </summary>
        public int CounterNo { get; private set; } = 0;

        /// <summary>
        /// 故障货道号
        /// 非货道故障传0
        /// </summary>
        public int Slot { get; private set; } = 0;

        /// <summary>
        /// 异常编码
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string? Desc { get; private set; }

        /// <summary>
        /// 1=有TransId 2自动运行
        /// </summary
        [Required]
        public int CodeType { get; private set; }

        /// <summary>
        /// 正常状态
        /// </summary>
        public bool Status { get; private set; } = false;

        /// <summary>
        /// a
        /// </summary>
        protected DeviceAbnormal() { }

        /// <summary>
        /// a
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="transId"></param>
        /// <param name="counterNo"></param>
        /// <param name="slot"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <param name="codeType"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceModelName"></param>
        /// <param name="enterpriseinfoId"></param>
        public DeviceAbnormal(long deviceBaseId, string transId, int counterNo, int slot, string code, string? desc, int codeType, string deviceName, string deviceModelName, long enterpriseinfoId)
        {
            DeviceBaseId = deviceBaseId;
            TransId = transId;
            CounterNo = counterNo;
            Slot = slot;
            Code = code;
            Desc = desc;
            CodeType = codeType;
            DeviceName = deviceName;
            DeviceModelName = deviceModelName;
            EnterpriseinfoId = enterpriseinfoId;
        }

        /// <summary>
        /// 异常恢复
        /// </summary>
        public void Restore()
        {
            Status = true;
            LastModifyTime = DateTime.UtcNow;
        }
    }
}
