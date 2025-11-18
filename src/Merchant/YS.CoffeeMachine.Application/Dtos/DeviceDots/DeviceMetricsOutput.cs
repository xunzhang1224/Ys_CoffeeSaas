using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备运行数据
    /// </summary>
    public class DeviceMetricsOutput
    {
        /// <summary>
        /// 获取或设置关联的设备基础信息唯一标识符。
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 获取或设置货柜编号，默认值为 0。
        /// 通常用于区分设备内部不同模块或容器。
        /// </summary>
        public int CounterNo { get; set; } = 0;

        /// <summary>
        /// 获取或设置指标类型，表示该条目所代表的具体监控项。
        /// 参考 MetricTypeEnum 定义。
        /// </summary>
        public MetricTypeEnum MetricType { get; set; }

        /// <summary>
        /// 获取或设置指标分类：
        /// 0：常规，1：物料，2：关键部件。
        /// 默认为 0（常规）。
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 获取或设置指标序号，用于排序或匹配逻辑。
        /// 默认值为 0。
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// 获取或设置指标值，可以是任意字符串形式的数据。
        /// 可为空。
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 获取或设置当前指标的状态，默认为正常。
        /// 参考 MetricsStatusEnum 定义。
        /// </summary>
        public MetricsStatusEnum Status { get; set; } = MetricsStatusEnum.Normal;

        /// <summary>
        /// 获取或设置指标的描述信息，可用于展示额外说明。
        /// 可为空。
        /// </summary>
        public string? Description { get; set; }
    }
}
