using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    /// <summary>
    /// 5212.服务器向VMC下发商品价格信息
    /// </summary>
    [MessagePackObject(true)]
    public class DownlinkEntity5212 : BaseCmd
    {

        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 5212;

        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 最大货道号
        /// </summary>
        public int MaxSlot { get; set; }

        /// <summary>
        /// 。
        /// </summary>
        public List<Slot> SlotInfo { get; set; }

        /// <summary>
        /// x
        /// </summary>

        [MessagePackObject(true)]
        public class Slot
        {
            /// <summary>
            /// 货道号
            /// </summary>
            public int SlotNo { get; set; } = 0;

            /// <summary>
            /// 商品编号
            /// </summary>
            public string Sku { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public decimal Price { get; set; } = 0;

            /// <summary>
            /// 折扣价
            /// </summary>
            public decimal DiscountedPrice { get; set; } = 0;

            /// <summary>
            /// 状态:正常=1；故障=2；停用=3；缺货=4 不存在 = 255
            /// </summary>
            public int Status { get; set; } = 1;
        }

        #region 响应实体

        /// <summary>
        ///  响应
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
        }
        #endregion

    }
}
