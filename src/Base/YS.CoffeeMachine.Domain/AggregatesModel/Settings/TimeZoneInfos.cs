namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示时区信息的聚合根实体。
    /// 用于管理系统中支持的不同时区配置，包括名称与标识符。
    /// </summary>
    public class TimeZoneInfos : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置时区的可读名称（例如：“中国标准时间”）。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置时区的唯一标识码（例如：“China Standard Time”）。
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected TimeZoneInfos() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 TimeZoneInfos 实例。
        /// </summary>
        /// <param name="name">时区的可读名称。</param>
        /// <param name="code">时区的唯一标识码。</param>
        public TimeZoneInfos(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}