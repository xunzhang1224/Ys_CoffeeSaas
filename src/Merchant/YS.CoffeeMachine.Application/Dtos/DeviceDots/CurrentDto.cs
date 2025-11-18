namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 货币信息
    /// </summary>
    public class CurrentDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 货币名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 货币编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        public string Symbol { get; set; }
    }
}