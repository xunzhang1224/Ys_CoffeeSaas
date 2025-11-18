namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 时间数据库基础实体
    /// </summary>
    public class TimedbBase
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        [Required]
        public long DeviceId { get; set; } = 0;

        /// <summary>
        /// 设备生成编号
        /// </summary>
        public string? Mid { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 企业信息Id
        /// </summary>
        public long? EnterpriseinfoId { get; set; }
    }
}
