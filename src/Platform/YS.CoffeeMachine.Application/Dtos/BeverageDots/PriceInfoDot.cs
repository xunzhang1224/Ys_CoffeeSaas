namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品价格dto
    /// </summary>
    public class PriceInfoDot
    {
        /// <summary>
        /// 饮品Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal? DiscountedPrice { get; set; }
    }
}
