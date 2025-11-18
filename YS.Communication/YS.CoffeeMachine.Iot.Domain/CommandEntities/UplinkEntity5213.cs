namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 5213.VMC向服务器请求商品列表
/// </summary>
public class UplinkEntity5213
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 5213;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// a
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// a
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public IEnumerable<Goods> GoodsList { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Goods
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// 商品名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 商品条码
            /// </summary>
            public string Barcode { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 主图URL
            /// </summary>
            public string ImagePath { get; set; }

            /// <summary>
            /// 商品描述
            /// </summary>
            public string Desc { get; set; }

            /// <summary>
            /// 规格
            /// </summary>
            public string Spec { get; set; }

            /// <summary>
            /// 进价
            /// </summary>
            public decimal InPrice { get; set; }

            /// <summary>
            /// 商品id，设备新增时为空
            /// </summary>
            public long? Id { get; set; }

            /// <summary>
            /// 折扣价
            /// </summary>
            public decimal? DiscountedPrice { get; set; }

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
            public SellStradgyInfo? SellStradgy { get; set; }

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
            /// 类型
            /// </summary>
            public int BoxNum { get; set; }
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
        #endregion
    }
}
