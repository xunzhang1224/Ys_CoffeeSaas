using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Application.IoT.Service.Http.Dto
{
    /// <summary>
    /// 售货机信息
    /// </summary>
    public class VendInfoForMqttDto
    {
        ///// <summary>
        ///// 箱体Id
        ///// </summary>
        //public string XT_Id { get; set; }

        /// <summary>
        /// 机器ID
        /// </summary>
        public string VendId { get; set; }

        /// <summary>
        /// 机器编码
        /// </summary>
        public string VendCode { get; set; }

        ///// <summary>
        ///// 机器类型
        ///// </summary>
        // public string MType { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string IMKey { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PubKey { get; set; }
    }
}
