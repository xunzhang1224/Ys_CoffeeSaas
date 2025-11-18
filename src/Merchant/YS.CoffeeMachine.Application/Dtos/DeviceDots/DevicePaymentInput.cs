using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备支付方式
    /// </summary>
    public class DevicePaymentInput: QueryRequest
    {
        /// <summary>
        /// 支付方式id
        /// </summary>
        public long PaymentConfigId { get; set; }

        /// <summary>
        /// 设备名称或mid
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 分组list
        /// </summary>
        public List<long> GroupIds { get; set; }
    }
}
