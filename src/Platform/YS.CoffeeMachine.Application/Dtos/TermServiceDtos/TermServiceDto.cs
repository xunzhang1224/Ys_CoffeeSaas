using YS.CoffeeMachine.Application.Dtos.PagingDto;
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

    /// <summary>
    /// 服务条款输出
    /// </summary>
    public class TermServiceOutput : TermServiceDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 查询服务条款输入
    /// </summary>
    public class TermServiceInput : QueryRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Title { get; set; } = null;
    }

    /// <summary>
    /// 服务条款选择输出
    /// </summary>
    public class TermServiceSelectOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
    }
}