using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 清洗部件预警设置
    /// </summary>
    public class FlushCfgDto
    {
        /// <summary>
        /// 清洗部件类型
        /// </summary>
        public FlushComponentTypeEnum Type { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int Counts { get; set; }
    }
}
