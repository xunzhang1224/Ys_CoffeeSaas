using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformDto.BasicDtos
{
    /// <summary>
    /// 字典入参
    /// </summary>
    public class DictionaryInput : QueryRequest
    {
        /// <summary>
        /// 字典key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 字典value
        /// </summary>
        public string Name { get; set; }
    }
}