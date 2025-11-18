using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// 9032 清洗上报
    /// </summary>
    public class UplinkEntity9032
    {
        /// <summary>
        /// 命令编号
        /// </summary>
        public static readonly int KEY = 9032;

        /// <summary>
        /// 部件信息列表
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 清洗方式 1:自动  2：手动 3：整机清洗（特指点击了整机清洗的操作,还有远程下发）
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            ///  0:整机 1:搅拌器1 2:搅拌器2 3:搅拌器3 4:冲泡器 5:料盒
            /// </summary>
            public string Parts { get; set; }

            /// <summary>
            /// 清洗类型
            /// 区分  搅拌器 冲泡器 料盒清洗
            /// </summary>
            public int FlushType { get; set; }

            /// <summary>
            ///  清洗状态  1： 完成  2失败
            /// </summary>
            public int Status { get; set; }
        }

        /// <summary>
        /// 部件信息
        /// </summary>
        [MessagePackObject(true)]
        public class Equipment
        {
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
