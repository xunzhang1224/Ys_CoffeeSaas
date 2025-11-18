using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Domain.IoT.Model
{
    /// <summary>
    /// 售货机通道信息
    /// </summary>
    public class VendChannel
    {
        /// <summary>
        /// 售货机编号
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// Grpc服务地址
        /// </summary>
        public string GrpcServerAddr { get; set; }

        /// <summary>
        ///Tcp服务器地址
        /// </summary>
        public string TcpServerAddr { get; set; }

        /// <summary>
        ///最后更新时间
        /// </summary>
        public DateTime lastSeen { get; set; }

        /// <summary>
        /// 其他信息
        /// </summary>
        public string OtherInfo { get; set; }
    }
}
