using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Domain.IoT.Model
{
    /// <summary>
    /// 售货机通道
    /// </summary>
    /// </summary>
    public class VendToken
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivaKey { get; set; } = string.Empty;

        /// <summary>
        /// 公钥
        /// </summary>
        public string PubKey { get; set; } = string.Empty;
    }
}
