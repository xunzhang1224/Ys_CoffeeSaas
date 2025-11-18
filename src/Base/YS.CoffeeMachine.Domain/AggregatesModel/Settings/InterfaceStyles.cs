namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示系统界面风格配置的聚合根实体。
    /// 用于管理后台可选的UI样式方案，支持切换主题风格。
    /// </summary>
    public class InterfaceStyles : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置风格的可读名称（例如：“深色模式”、“浅色模式”等）。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置风格的唯一标识码，用于程序识别和应用。
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 获取或设置风格预览图的路径或URL，用于前端展示。
        /// </summary>
        public string Preview { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected InterfaceStyles() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 InterfaceStyles 实例。
        /// </summary>
        /// <param name="name">风格的可读名称。</param>
        /// <param name="code">风格的唯一标识码。</param>
        /// <param name="preview">风格预览图的路径或URL。</param>
        public InterfaceStyles(string name, string code, string preview)
        {
            Name = name;
            Code = code;
            Preview = preview;
        }

        /// <summary>
        /// 更新当前风格配置的基本信息。
        /// </summary>
        /// <param name="name">新的风格名称。</param>
        /// <param name="code">新的风格标识码。</param>
        /// <param name="preview">新的预览图路径或URL。</param>
        public void Update(string name, string code, string preview)
        {
            Name = name;
            Code = code;
            Preview = preview;
        }
    }
}