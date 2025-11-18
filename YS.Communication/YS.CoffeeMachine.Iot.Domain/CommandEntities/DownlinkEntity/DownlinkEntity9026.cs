using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    /// <summary>
    /// 9026 饮品下发
    /// </summary>
    [MessagePackObject(true)]
    public class DownlinkEntity9026 : BaseCmd
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 9026;

        /// <summary>
        /// 是否批量应用
        /// true:安卓全部替换饮品
        /// false:安卓对应增改
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 被替换的商品code
        /// 当IsApply为true时启用
        /// code为空清除所有，不为空清除一个
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 饮品信息。
        /// </summary>
        public List<Beverage> CoffeeInfo { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [MessagePackObject(true)]
        public class Beverage
        {
            /// <summary>
            /// 饮品分类
            /// </summary>
            public List<long> Types { get; set; }

            /// <summary>
            /// 是否显示
            /// </summary>
            public bool DisplayStatus { get; set; }

            /// <summary>
            /// 商品唯一标识
            /// </summary>
            public string? Sku { get; set; }

            /// <summary>
            /// 商品
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 商品图片地址，新增时为空
            /// </summary>
            public string? ImageUrl { get; set; }

            /// <summary>
            /// 原价
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 折扣价
            /// </summary>
            public decimal? DiscountedPrice { get; set; }

            /// <summary>
            /// 商品规格，即配方内的总和小于该值
            /// </summary>
            public int Spec { get; set; }

            /// <summary>
            /// 冷饮或热饮
            /// 0冷饮 1热饮
            /// </summary>
            public int Cold { get; set; }

            /// <summary>
            ///  配方
            /// </summary>
            public List<RecipeInfo> Recipe { get; set; }

            /// <summary>
            ///  售卖时间
            /// </summary>
            public SellStradgyInfo SellStradgy { get; set; }
        }

        /// <summary>
        ///  配方
        /// </summary>
        [MessagePackObject(true)]
        public class RecipeInfo
        {
            /// <summary>
            /// 配方信息
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 料盒
            /// </summary>
            public string MaterialBoxName { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public int FormulaType { get; set; }

            /// <summary>
            /// 料盒序号
            /// </summary>
            public int MaterialBoxid { get; set; }

            /// <summary>
            /// 序号
            /// </summary>
            public int Sort { get; set; }
        }

        /// <summary>
        ///  售卖时间
        /// </summary>
        [MessagePackObject(true)]
        public class SellStradgyInfo
        {
            /// <summary>
            /// 售卖时间
            /// 0:1:2:3:4:5:6
            /// </summary>
            public string SellDateIndex { get; set; }

            /// <summary>
            /// 售卖开始时间
            /// 00:00
            /// </summary>
            public string SellStartTime { get; set; }

            /// <summary>
            /// 售卖结束时间
            /// 00:00
            /// </summary>
            public string SellEndTime { get; set; }
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
