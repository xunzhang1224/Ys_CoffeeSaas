namespace YS.CoffeeMachine.Domain.AggregatesModel.ServiceProviders
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示服务商信息的聚合根实体。
    /// 用于管理与设备相关的服务提供商基本信息。
    /// </summary>
    public class ServiceProviderInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置服务商名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置服务商联系方式（电话号码或邮箱等）。
        /// </summary>
        public string Tel { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected ServiceProviderInfo() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 ServiceProviderInfo 实例。
        /// </summary>
        /// <param name="name">服务商名称。</param>
        /// <param name="tel">服务商联系方式。</param>
        public ServiceProviderInfo(string name, string tel)
        {
            Name = name;
            Tel = tel;
        }

        /// <summary>
        /// 更新当前服务商的基本信息。
        /// </summary>
        /// <param name="name">新的服务商名称。</param>
        /// <param name="tel">新的联系方式。</param>
        public void Update(string name, string tel)
        {
            Name = name;
            Tel = tel;
        }
    }
}