namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备指标数据的聚合根实体。
    /// 用于记录与设备运行状态相关的各类指标信息，如常规参数、物料状态、关键部件状态等。
    /// </summary>
    public class DeviceMetrics : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的设备基础信息唯一标识符。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 获取或设置货柜编号，默认值为 0。
        /// 通常用于区分设备内部不同模块或容器。
        /// </summary>
        public int CounterNo { get; private set; } = 0;

        /// <summary>
        /// 获取或设置指标类型，表示该条目所代表的具体监控项。
        /// 参考 MetricTypeEnum 定义。
        /// </summary>
        [Required]
        public MetricTypeEnum MetricType { get; private set; }

        /// <summary>
        /// 获取或设置指标分类：
        /// 0：常规，1：物料，2：关键部件。
        /// 默认为 0（常规）。
        /// </summary>
        public int Type { get; private set; } = 0;

        /// <summary>
        /// 获取或设置指标序号，用于排序或匹配逻辑。
        /// 默认值为 0。
        /// </summary>
        public int Index { get; private set; } = 0;

        /// <summary>
        /// 获取或设置指标值，可以是任意字符串形式的数据。
        /// 可为空。
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// 获取或设置当前指标的状态，默认为正常。
        /// 参考 MetricsStatusEnum 定义。
        /// </summary>
        public MetricsStatusEnum Status { get; private set; } = MetricsStatusEnum.Normal;

        /// <summary>
        /// 获取或设置指标的描述信息，可用于展示额外说明。
        /// 可为空。
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceMetrics() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceMetrics 实例。
        /// </summary>
        /// <param name="deviceBaseId">关联的设备基础信息唯一标识。</param>
        /// <param name="counterNo">货柜编号。</param>
        /// <param name="metricType">指标类型。</param>
        /// <param name="index">指标序号。</param>
        /// <param name="value">指标值。</param>
        /// <param name="status">当前指标状态。</param>
        /// <param name="description">指标描述信息。</param>
        /// <param name="type">指标分类（0:常规，1:物料，2:关键部件）。</param>
        public DeviceMetrics(long deviceBaseId, int counterNo, MetricTypeEnum metricType, int index, string? value, MetricsStatusEnum status, string? description, int type)
        {
            DeviceBaseId = deviceBaseId;
            CounterNo = counterNo;
            MetricType = metricType;
            Index = index;
            Value = value;
            Status = status;
            Description = description;
            Type = type;
        }

        /// <summary>
        /// 更新当前指标的各项属性值。
        /// </summary>
        /// <param name="index">新的指标序号。</param>
        /// <param name="value">新的指标值。</param>
        /// <param name="status">新的指标状态。</param>
        /// <param name="description">新的描述信息。</param>
        /// <param name="type">新的指标分类。</param>
        public void Update(int index, string? value, MetricsStatusEnum status, string? description, int type)
        {
            Index = index;
            Value = value;
            Status = status;
            Description = description;
            Type = type;
        }
    }
}