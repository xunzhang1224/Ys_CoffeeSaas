namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language
{
    using System.ComponentModel.DataAnnotations;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 语种文本实体
    /// </summary>
    public class LanguageTextEntity : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 值(对应语言翻译)
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 语种表编码(SysLanguage表的Code)
        /// </summary>
        public string LangCode { get; private set; }

        /// <summary>
        /// 语种表
        /// </summary>
        public LanguageInfo Lang { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected LanguageTextEntity() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <param name="langCode"></param>
        public LanguageTextEntity(string code, string value, string langCode)
        {
            Code = code;
            Value = value;
            LangCode = langCode;
        }
    }
}
