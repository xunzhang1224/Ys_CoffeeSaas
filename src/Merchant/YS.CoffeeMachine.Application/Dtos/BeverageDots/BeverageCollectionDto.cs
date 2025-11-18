namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 批量应用饮品合集
    /// </summary>
    public class BeverageCollectionDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long DeviceModelId { get; set; }
        /// <summary>
        /// 合集名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 饮品Id集合
        /// </summary>
        public string BeverageIds { get; set; }
        /// <summary>
        /// 包含饮品
        /// </summary>
        public string BeverageNames { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }

    /// <summary>
    /// 平台端饮品集合下拉数据
    /// </summary>
    public class P_BeverageCollectionSelectedDto
    {
        /// <summary>
        /// 平台端饮品集合Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
    }
}