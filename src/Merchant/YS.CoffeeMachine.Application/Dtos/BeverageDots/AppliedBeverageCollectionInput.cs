namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 批量应用饮品合集
    /// </summary>
    public class AppliedBeverageCollectionInput
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public List<long> deviceIds { get; set; }
        /// <summary>
        /// 饮品合集Id
        /// </summary>
        public long beverageCollectionId { get; set; }
    }
}
