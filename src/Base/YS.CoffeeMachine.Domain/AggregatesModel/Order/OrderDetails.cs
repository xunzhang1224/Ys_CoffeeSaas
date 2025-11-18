using System.ComponentModel.DataAnnotations;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Order
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetails : BaseEntity
    {
        /// <summary>
        /// 订单
        /// </summary>
        [Required]
        public long OrderId { get; private set; }

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; private set; } = 0;

        /// <summary>
        /// 货道编号
        /// </summary>
        public int SlotNo { get; private set; } = 0;

        /// <summary>
        /// 饮品SKU
        /// </summary>
        [Required]
        public string ItemCode { get; private set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        public string BeverageName { get; private set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; private set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; private set; } = 1;

        /// <summary>
        /// 1 出货成功 0-出货失败
        /// </summary>
        public int Result { get; private set; } = 0;

        /// <summary>
        /// 错误
        /// </summary>
        public int Error { get; private set; } = 0;

        /// <summary>
        /// 错误描述
        /// </summary>
        public string? ErrorDescription { get; private set; }

        /// <summary>
        /// 出货时间
        /// </summary>
        public long ActionTimeSp { get; private set; } = 0;

        /// <summary>
        /// 出货时间Utc
        /// </summary>
        public DateTime ActionTime => ActionTimeSp == 0 ? CreateTime : DateTimeOffset.FromUnixTimeMilliseconds(ActionTimeSp).UtcDateTime;

        /// <summary>
        /// 订单
        /// </summary>
        public OrderInfo OrderInfo { get; private set; }

        /// <summary>
        /// 物料使用
        /// </summary>
        public List<OrderDetaliMaterial> OrderDetaliMaterials { get; private set; }

        /// <summary>
        /// 饮品信息
        /// </summary>
        public string BeverageInfoData { get; private set; }

        /// <summary>
        /// 私有构造
        /// </summary>
        protected OrderDetails() { }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="counterNo"></param>
        /// <param name="slotNo"></param>
        /// <param name="itemCode"></param>
        /// <param name="beverageName"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <param name="errorDescription"></param>
        /// <param name="actionTimeSp"></param>
        /// <param name="beverageInfoData"></param>
        /// <param name="orderDetaliMaterials"></param>
        public OrderDetails(int counterNo, int slotNo, string itemCode, string beverageName, decimal price, int quantity, int result, int error, string errorDescription, string url,
            long actionTimeSp, string beverageInfoData, List<OrderDetaliMaterial> orderDetaliMaterials)
        {
            Url = url;
            CounterNo = counterNo;
            SlotNo = slotNo;
            ItemCode = itemCode;
            BeverageName = beverageName;
            Price = price;
            Quantity = quantity;
            Result = result;
            Error = error;
            ErrorDescription = errorDescription;
            ActionTimeSp = actionTimeSp;
            BeverageInfoData = beverageInfoData;
            OrderDetaliMaterials = orderDetaliMaterials;
        }

        /// <summary>
        /// 设置出货结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="errCode"></param>
        /// <param name="errorDescription"></param>
        public void SetShipmentResults(int result, long actionTimeSp, int errCode = 0, string? errorDescription = null)
        {
            if (result == 0)
            {
                Error = errCode;
                ErrorDescription = errorDescription;
            }
            ActionTimeSp = actionTimeSp;
            Result = result;
        }

        /// <summary>
        /// 设置出货状态
        /// </summary>
        /// <param name="result"></param>
        public void SetShipmentStatus(int result)
        {
            Result = result;
        }

        /// <summary>
        /// 设置物料使用情况
        /// </summary>
        /// <param name="orderDetalis"></param>
        public void SetOrderDetaliMaterials(List<OrderDetaliMaterial> orderDetalis)
        {
            OrderDetaliMaterials = orderDetalis;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="errorDescription"></param>
        public void SetErrorInfo(int error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}