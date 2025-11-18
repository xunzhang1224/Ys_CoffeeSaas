using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{

    /// <summary>
    /// 设备支付关系
    /// </summary>
    public class DevicePaymentDto
    {
        ///// <summary>
        ///// 设备支付关系表id
        ///// </summary>
        //public long Id { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 分组ids
        /// </summary>
        public List<long> GroupIds { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupNames { get; set; }

        /// <summary>
        /// 支付配置id
        /// </summary>
        public long? PaymentConfigId { get; set; }
    }
}
