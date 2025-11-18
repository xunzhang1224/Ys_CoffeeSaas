namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    /// <summary>
    /// 基础数据-省市区
    /// </summary>
    public class SysProvinceCity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 类型（1-省 2-市 3-区/县）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOnUtc { get; set; }
    }
}