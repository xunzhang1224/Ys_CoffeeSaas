using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Const
{
    /// <summary>
    /// Iot常量
    /// </summary>
    public class IotConst
    {
        /// <summary>
        /// 事件编码key
        /// </summary>
        public static string LangKeyForEventNo = "IOT";

        /// <summary>
        /// 设备心跳间隔分钟
        /// </summary>
        public static int DeviceHeartbeatIntervalMinutes = 2;
    }
}
