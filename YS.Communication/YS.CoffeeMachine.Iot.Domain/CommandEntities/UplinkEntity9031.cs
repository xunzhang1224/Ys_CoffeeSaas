using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// 9031 冲洗部件上报
    /// </summary>
    public class UplinkEntity9031
    {
        /// <summary>
        /// 命令编号
        /// </summary>
        public static readonly int KEY = 9031;

        /// <summary>
        /// 部件信息列表
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 0:搅拌器 1：冲泡器 2：料盒
            /// </summary>
            public List<Equipment> Equipments { get; set; }
        }

        /// <summary>
        /// 部件信息
        /// </summary>
        [MessagePackObject(true)]
        public class Equipment
        {
            /// <summary>
            /// 0:搅拌器 1：冲泡器 2：料盒
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 序号
            /// </summary>
            public List<EquipmentSub> Subs { get; set; }
        }

        /// <summary>
        /// 部件信息详情
        /// </summary>
        [MessagePackObject(true)]
        public class EquipmentSub
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int Index { get; set; } = 0;

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
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
