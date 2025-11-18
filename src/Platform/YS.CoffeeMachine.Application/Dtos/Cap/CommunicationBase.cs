using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    /// <summary>
    /// CommunicationBase
    /// </summary>
    public class CommunicationBase
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        public string Mid { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 服务/命令 唯一标识
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 消息唯一id
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsRecordLog { get; set; } = false;
    }
}
