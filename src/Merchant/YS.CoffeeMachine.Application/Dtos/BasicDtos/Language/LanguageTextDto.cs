using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;

namespace YS.CoffeeMachine.Application.Dtos.BasicDtos.Language
{
    /// <summary>
    /// 语言文本
    /// </summary>
    public class LanguageTextDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值(对应语言翻译)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 语种表编码(SysLanguage表的Code)
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public LanguageInfo Language { get; set; }
    }

    /// <summary>
    /// 获取语言文本详情
    /// </summary>
    public class GetLanguageTextDetailDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 平台端是否使用当前语言文本（true：使用）
        /// </summary>
        public bool Platform { get; set; }

        /// <summary>
        /// 商户PC端是否使用当前语言文本（true：使用）
        /// </summary>
        public bool MerchantPC { get; set; }

        /// <summary>
        /// 商户移动端是否使用当前语言文本（true：使用）
        /// </summary>
        public bool MerchantMove { get; set; }

        /// <summary>
        /// 后端是否使用当前语言文本（true：使用）
        /// </summary>
        public bool Backend { get; set; }

        /// <summary>
        /// 语种列表
        /// </summary>
        public List<LanguageTextItem> LangList { get; set; }
    }

    /// <summary>
    /// 语种列表项
    /// </summary>
    public class LanguageTextItem
    {
        /// <summary>
        /// 语种编码
        /// </summary>
        public string LangCode { get; set; }
        /// <summary>
        /// 语种名称
        /// </summary>
        public string LangName { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string Value { get; set; }
    }
}

