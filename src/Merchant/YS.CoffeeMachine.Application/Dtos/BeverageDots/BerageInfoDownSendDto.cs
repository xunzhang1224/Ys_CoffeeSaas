namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品应用到设备，下发信息
    /// </summary>
    public class BerageInfoDownSendDto
    {
        /// <summary>
        /// 基础饮品信息
        /// </summary>
        public BeverageInfoDto BeverageInfoDto { get; set; } = new BeverageInfoDto();

        /// <summary>
        /// 需要更新的Sku
        /// </summary>
        public Dictionary<string, string> MidsAndSkus { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 编辑部分饮品信息
        /// </summary>
        public Dictionary<long, BeverageInfoDto> Dic { get; set; } = new Dictionary<long, BeverageInfoDto>();
    }
}