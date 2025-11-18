using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.Application.IoT.Manager;

namespace YS.Application.IoT.Manager.Dto
{
    /// <summary>
    /// 黑名单列表
    /// </summary>
    public class GetBlackListOutput
    {
        /// <summary>
        /// 黑名单类型
        /// </summary>
        public BlacklistTypeEnum Type { get; set; }

        /// <summary>
        /// 黑名单项
        /// </summary>
        public string Item { get; set; }
    }
}
