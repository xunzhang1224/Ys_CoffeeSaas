using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// 9033冲洗部件使用
    /// </summary>
    public class UplinkEntity9033
    {
        /// <summary>
        /// 命令编号
        /// </summary>
        public static readonly int KEY = 9033;

        /// <summary>
        /// 部件列表
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 冲洗部件名称
            /// ，隔开
            /// </summary>
            public string FlushNames { get; set; }
        }

        /// <summary>
        /// 回复
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
        }
    }
}
