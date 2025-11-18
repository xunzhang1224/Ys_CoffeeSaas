namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language
{
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 语言信息
    /// </summary>
    public class LanguageInfo : Entity, IAggregateRoot
    {
        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 是否默认（枚举类型：IsDefaultEnum)
        /// </summary>
        public IsDefaultEnum IsDefault { get; private set; }

        /// <summary>
        /// LanguageTextEntitys
        /// </summary>
        public List<LanguageTextEntity> LanguageTextEntitys { get; private set; }

        /// <summary>
        /// GetKeys
        /// </summary>
        /// <returns></returns>
        public override object[] GetKeys() => new object[] { Code };

        /// <summary>
        /// LanguageInfo
        /// </summary>
        protected LanguageInfo()
        {
        }

        #region 多语言表操作

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">标识</param>
        /// <param name="isEnabled">是否启用</param>
        /// <param name="isDefault">是否默认</param>
        public LanguageInfo(string name, string code, EnabledEnum isEnabled = EnabledEnum.Enable, IsDefaultEnum isDefault = IsDefaultEnum.No)
        {
            Name = name;
            Code = code;
            IsEnabled = isEnabled;
            IsDefault = isDefault;
            if (isDefault == IsDefaultEnum.Yes)
                SetDefaultLanguage();

            //AddDomainEvent(new CreateLanguageDomainEvent(this));
        }

        /// <summary>
        /// 更新语言信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">标识</param>
        /// <param name="isEnabled">是否启用</param>
        /// <param name="isDefault">是否默认</param>
        public void Update(string name, string code, EnabledEnum isEnabled, IsDefaultEnum isDefault)
        {
            Name = name;
            Code = code;
            IsEnabled = isEnabled;

            if (IsDefault == IsDefaultEnum.No && isDefault == IsDefaultEnum.Yes)
                SetDefaultLanguage();
            //if (IsDefault == IsDefaultEnum.Yes && isDefault == IsDefaultEnum.No)
            //    SetNotDefaultLanguage();
            IsDefault = isDefault;

            //AddDomainEvent(new UpdateLanguageDomainEvent(this));
        }

        /// <summary>
        /// 设置默认语种
        /// </summary>
        /// <param name="enabled"></param>
        public void SetDefaultLanguage()
        {
            IsDefault = IsDefaultEnum.Yes;
            AddDomainEvent(new SetDefaultLanguageDomainEvent(this));
        }

        /// <summary>
        /// 取消默认语种
        /// </summary>
        /// <param name="enabled"></param>
        public void SetNotDefaultLanguage()
        {
            IsDefault = IsDefaultEnum.No;
            //AddDomainEvent(new SetDefaultLanguageDomainEvent(this));
        }

        //public void Delete()
        //{
        //    AddDomainEvent(new DeleteLanguageDomainEvent(this));
        //}

        #endregion

        #region 文本表

        /// <summary>
        /// 添加语言文本
        /// </summary>
        /// <param name="entitie">语言文本实体</param>
        public void AddLanguageText(LanguageTextEntity entitie)
        {
            LanguageTextEntitys.Remove(entitie);

            LanguageTextEntitys.Add(entitie);

            // 发布领域事件
            //AddDomainEvent(new CreateLanguageTextDomainEvent(entitie));
        }

        /// <summary>
        /// 添加语言文本集合
        /// </summary>
        /// <param name="entities">语言文本实体集合</param>
        public void AddRangeLanguageText(List<LanguageTextEntity> entities)
        {
            var ids = entities.Select(s => s.Code).ToList();
            LanguageTextEntitys.RemoveAll(s => ids.Contains(s.Code));
            LanguageTextEntitys.AddRange(entities);

            // 发布领域事件
            //AddDomainEvent(new AddRangeLanguageTextDomainEvent(entities));
        }

        //public void UpdateRangeLanguageText(List<LanguageTextEntity> entities)
        //{
        //    var ids = entities.Select(s => s.Code).ToList();
        //    LanguageTextEntitys.RemoveAll(s => ids.Contains(s.Code));
        //    LanguageTextEntitys.AddRange(entities);

        //    // 发布领域事件
        //    AddDomainEvent(new AddRangeLanguageTextDomainEvent(entities));
        //}

        /// <summary>
        /// 删除语言文本
        /// </summary>
        /// <param name="entitie">语言文本实体</param>
        public void DelLanguageText(LanguageTextEntity entitie)
        {
            LanguageTextEntitys.Remove(entitie);

            // 发布领域事件
            //AddDomainEvent(new DeleteLanguageTextDomainEvent(entitie));
        }

        /// <summary>
        /// 删除语言文本集合
        /// </summary>
        /// <param name="entities">语言文本实体集合</param>
        public void DelRangeLanguageText(List<LanguageTextEntity> entities)
        {
            var ids = entities.Select(s => s.Code).ToList();
            LanguageTextEntitys.RemoveAll(s => ids.Contains(s.Code));

            // 发布领域事件
            //AddDomainEvent(new DelRangeLanguageTextDomainEvent(entities));
        }

        /// <summary>
        /// 删除语言信息
        /// </summary>
        public void Delete()
        {
            AddDomainEvent(new DeleteLanguageDomainEvent(this));
            DelRangeLanguageText(LanguageTextEntitys);
        }
        #endregion
    }
}

