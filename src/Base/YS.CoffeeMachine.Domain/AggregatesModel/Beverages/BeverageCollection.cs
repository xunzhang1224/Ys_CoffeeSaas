namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 饮品合集
    /// </summary>
    public class BeverageCollection : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; private set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; private set; }

        /// <summary>
        /// 合集名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 饮品Id集合
        /// </summary>
        public string BeverageIds { get; private set; }

        /// <summary>
        /// 包含饮品
        /// </summary>
        public string BeverageNames { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BeverageCollection() { }

        /// <summary>
        /// 创建饮品合集
        /// </summary>
        /// <param name="enterpriseInfoId"></param>
        /// <param name="deviceModelId"></param>
        /// <param name="name"></param>
        /// <param name="beverageIds"></param>
        /// <param name="beverageNames"></param>
        public BeverageCollection(long enterpriseInfoId, long? deviceModelId, string name, string beverageIds, string beverageNames)
        {
            EnterpriseInfoId = enterpriseInfoId;
            DeviceModelId = deviceModelId;
            Name = name;
            BeverageIds = beverageIds;
            BeverageNames = beverageNames;
        }

        /// <summary>
        /// 更新饮品合集
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name)
        {
            Name = name;
        }
    }
}
