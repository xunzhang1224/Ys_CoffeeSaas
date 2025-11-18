namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 字典实体
    /// </summary>
    public class DictionaryEntity : Entity, IAggregateRoot
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 父级key
        /// </summary>
        public string ParentKey { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; private set; }

        /// <summary>
        /// 父级
        /// </summary>
        public DictionaryEntity Parent { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 子级
        /// </summary>
        public List<DictionaryEntity> DictionarySubs { get; private set; } = new List<DictionaryEntity>();

        /// <summary>
        /// GetKeys
        /// </summary>
        /// <returns></returns>
        public override object[] GetKeys() => new object[] { Key };

        /// <summary>
        /// 私有构造
        /// </summary>
        protected DictionaryEntity()
        {
            DictionarySubs = new List<DictionaryEntity>();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="enabled"></param>
        /// <param name="subs">新增的子集</param>
        public DictionaryEntity(string key, string value, EnabledEnum enabled, string parentKey = null, string? remark = null)
        {
            Key = key;
            Value = value;
            IsEnabled = enabled;
            ParentKey = parentKey;
            Remark = remark;
            // 发送领域事件添加对应中文文本多语言
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enabled"></param>
        public void Update(string value, EnabledEnum enabled, string? remark = null)
        {
            Value = value;
            IsEnabled = enabled;
            Remark = remark;
        }
    }
}
