using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// 9030 刷卡开门
    /// </summary>
    public class UplinkEntity9030
    {
        /// <summary>
        /// 命令编号
        /// </summary>
        public static readonly int KEY = 9030;

        /// <summary>
        /// 饮品详情
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 卡号
            /// </summary>
            public string CardNumber { get; set; }
        }

        /// <summary>
        /// 回复
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
            /// <summary>
            /// 是否开门
            /// </summary>
            public bool IsOpenDoor { get; set; } = false;
        }
    }
}
