using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 企业类型
    /// </summary>
    public class EnterpriseTypes : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 企业类型
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Status { get; private set; }
        /// <summary>
        /// 是否限制 0:没有设备分配  1:有设备分配
        /// </summary>
        public bool Astrict { get; private set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        protected EnterpriseTypes()
        {
        }

        /// <summary>
        /// 新增企业类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="astrict"></param>
        public EnterpriseTypes(string name, bool astrict)
        {
            Name = name;
            Status = true;
            Astrict = astrict;
            IsDefault = false;
        }

        /// <summary>
        /// 更新企业类型
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name, bool astrict)
        {
            Name = name;
            Astrict = astrict;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(bool status)
        {
            Status = status;
        }
    }
}
