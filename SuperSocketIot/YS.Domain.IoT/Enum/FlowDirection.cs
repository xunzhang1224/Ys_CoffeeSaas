using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Domain.IoT.Enum
{
    /// <summary>
    /// 流向
    /// </summary>
    public enum FlowDirection
    {
        /// <summary>
        /// IoT设备上传至服务器
        /// </summary>
        [Description("IoT设备上传至服务器")]
        UP = 1,

        /// <summary>
        /// 服务器下发至IoT设备
        /// </summary>
        [Description("服务器下发至IoT设备")]
        DOWN = 2,
    }
}
