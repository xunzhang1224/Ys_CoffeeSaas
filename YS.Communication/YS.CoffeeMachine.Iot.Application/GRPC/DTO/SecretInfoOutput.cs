using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Application.GRPC.DTO
{
    /// <summary>
    /// 密钥信息输出
    /// </summary>
    [MessagePackObject(true)]
    public class SecretInfoOutput
    {
        /// <summary>
        /// 设备临时通讯编码
        /// 外键
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentNumber { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string? IMEI { get; set; }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool IsBind { get; set; } = false;

        /// <summary>
        /// 私钥
        /// </summary>
        public string PriKey { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PubKey { get; set; }

        /// <summary>
        /// 通道ID
        /// </summary>
        public string ChanneId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
