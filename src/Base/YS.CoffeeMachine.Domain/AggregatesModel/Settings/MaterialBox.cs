namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示料盒配置的实体类。
    /// 用于管理咖啡机中各个料盒的基本信息与预警规则。
    /// </summary>
    public class MaterialBox : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的设置信息唯一标识符。
        /// </summary>
        public long SettingInfoId { get; private set; }

        /// <summary>
        /// 获取或设置料盒的名称（例如：咖啡豆仓、奶泡盒等）。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置该料盒是否启用。
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// 获取或设置料盒在界面上显示的排序顺序。
        /// </summary>
        public int Sort { get; private set; }

        /// <summary>
        /// 获取或设置最小剩余量预警阈值（单位可为克、毫升等）。
        /// 当前值低于此值时触发预警。
        /// </summary>
        public double MinMeasureWarning { get; private set; }

        /// <summary>
        /// 获取或设置最小制作数量预警阈值。
        /// 当剩余材料不足以完成指定数量制作时触发预警。
        /// </summary>
        public double MinQuantityWarning { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected MaterialBox() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 MaterialBox 实例。
        /// </summary>
        /// <param name="settingInfoId">关联的设置信息ID。</param>
        /// <param name="name">料盒名称。</param>
        /// <param name="sort">显示排序顺序。</param>
        /// <param name="isActive">是否启用该料盒，默认为 true。</param>
        public MaterialBox(long settingInfoId, string name, int sort, bool isActive = true)
        {
            SettingInfoId = settingInfoId;
            Name = name;
            MinMeasureWarning = 0;
            MinQuantityWarning = 0;
            IsActive = isActive;
            Sort = sort;
        }

        /// <summary>
        /// 更新当前料盒的基础信息。
        /// </summary>
        /// <param name="name">新的料盒名称。</param>
        /// <param name="isActive">新的启用状态。</param>
        public void Update(string name, bool isActive = true)
        {
            Name = name;
            IsActive = isActive;
        }

        /// <summary>
        /// 设置料盒的启用/禁用状态。
        /// </summary>
        /// <param name="isActive">true 表示启用；false 表示禁用。</param>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        /// <summary>
        /// 设置料盒的预警阈值。
        /// </summary>
        /// <param name="minMeasureWarning">最小剩余量预警值。</param>
        /// <param name="minQuantityWarning">最小制作数量预警值。</param>
        public void SetWarning(double minMeasureWarning, double minQuantityWarning)
        {
            MinMeasureWarning = minMeasureWarning;
            MinQuantityWarning = minQuantityWarning;
        }
    }
}