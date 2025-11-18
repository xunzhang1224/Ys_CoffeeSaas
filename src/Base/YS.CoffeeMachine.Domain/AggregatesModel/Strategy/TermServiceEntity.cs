using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Strategy
{
    /// <summary>
    /// 服务条款实体
    /// </summary>
    public class TermServiceEntity : BaseEntity
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

        /// <summary>
        /// 保护构造函数
        /// </summary>
        protected TermServiceEntity() { }

        /// <summary>
        /// 添加服务条款
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="description"></param>
        /// <param name="enabled"></param>
        public TermServiceEntity(string title, string content, string description, EnabledEnum enabled)
        {
            Title = title;
            Content = content;
            Description = description;
            Enabled = enabled;
        }

        /// <summary>
        /// 编辑服务条款
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="description"></param>
        /// <param name="enabled"></param>
        public void Update(string title, string content, string description, EnabledEnum enabled)
        {
            Title = title;
            Content = content;
            Description = description;
            Enabled = enabled;
        }

        /// <summary>
        /// 设置启用/禁用
        /// </summary>
        /// <param name="enabled"></param>
        public void SetEnabled(EnabledEnum enabled)
        {
            Enabled = enabled;
        }
    }
}