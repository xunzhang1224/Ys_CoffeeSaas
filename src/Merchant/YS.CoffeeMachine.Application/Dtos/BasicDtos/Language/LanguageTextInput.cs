using System.ComponentModel.DataAnnotations;

namespace YS.CoffeeMachine.Application.Dtos.BasicDtos.Language
{
    /// <summary>
    /// 语言文本
    /// </summary>
    public class LanguageTextInput
    {
        /// <summary>
        /// 语言代码
        /// </summary>
        [Required]
        public string LangCode { get; set; }
    }
}
