using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.TermServiceDtos
{
    /// <summary>
    /// 服务条款Dto
    /// </summary>
    public class TermServiceDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public EnabledEnum Enabled { get; set; }
    }

    /// <summary>
    /// 单个服务条款输出
    /// </summary>
    public class SingleTermServiceOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}