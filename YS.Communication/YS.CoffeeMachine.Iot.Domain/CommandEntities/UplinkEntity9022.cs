using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// 9022: 物料上报
    /// </summary>
    public class UplinkEntity9022
    {
        /// <summary>
        /// 命令编号
        /// </summary>
        public static readonly int KEY = 9022;

        /// <summary>
        /// 订单详情
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 是否补货
            /// </summary>
            public bool IsRestock { get; set; }

            /// <summary>
            /// 物料集合。
            /// </summary>
            public List<MaterialInfo> Details { get; set; }

            /// <summary>
            /// 物料
            /// </summary>
            [MessagePackObject(true)]
            public class MaterialInfo
            {
                /// <summary>
                /// 类型
                /// </summary>
                public int Type { get; set; }

                /// <summary>
                ///  物料信息
                /// </summary>
                public List<CartridgeInfo> CartridgeInfos { get; set; }
            }

            /// <summary>
            ///  物料详情
            /// </summary>
            [MessagePackObject(true)]
            public class CartridgeInfo
            {
                /// <summary>
                /// 序列号
                /// 从0开始
                /// </summary>
                public int Index { get; set; } = 0;

                /// <summary>
                /// 名字
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 容量
                /// </summary>
                public int Capacity { get; set; } = 0;

                /// <summary>
                /// 存量
                /// </summary>
                public int Stock { get; set; } = 0;

                /// <summary>
                /// 预警数
                /// </summary>
                public int Warning { get; set; } = 0;

                /// <summary>
                /// 是否是糖
                /// </summary>
                public bool IsSugar { get; set; } = false;
            }
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
