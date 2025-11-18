using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// AppliedBeverageInput
    /// </summary>
    public class AppliedBeverageInput
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long deviceId { get; set; }
        /// <summary>
        /// 饮品Id集合
        /// </summary>
        public List<long> beverageIds { get; set; }
        /// <summary>
        /// 饮品应用方式
        /// </summary>
        public BeverageAppliedType appliedType { get; set; }
        /// <summary>
        /// 替换内容方式
        /// </summary>
        public ReplaceContentType contentType { get; set; }
    }
}
