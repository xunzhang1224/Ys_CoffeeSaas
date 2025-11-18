namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 企业过滤实体基类
    /// </summary>
    public class EnterpriseBaseEntity : BaseEntity, IEnterpriseFilter
    {
        /// <summary>
        /// 企业信息Id
        /// </summary>
        public long EnterpriseinfoId { get; set; }
    }
}
