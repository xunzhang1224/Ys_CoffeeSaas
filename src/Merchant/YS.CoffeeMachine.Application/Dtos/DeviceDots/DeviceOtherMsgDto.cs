using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备其他信息
    /// </summary>
    public class DeviceOtherMsgDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 设备基本信息
    /// </summary>
    public class DeviceBaseInfoForBind
    {
        /// <summary>
        /// baseinfo Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 机器永久编码
        /// </summary>
        public string MachineStickerCode { get; set; }
    }
}
