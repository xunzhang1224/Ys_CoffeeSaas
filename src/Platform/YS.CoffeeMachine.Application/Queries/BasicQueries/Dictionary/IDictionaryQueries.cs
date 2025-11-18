using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;

namespace YS.CoffeeMachine.Application.Queries.BasicQueries.Language
{
    /// <summary>
    /// 字典查询
    /// </summary>
    public interface IDictionaryQueries
    {
        /// <summary>
        /// 通过key获取字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResultDto<DictionaryDto>> GetDictionaryByKey(DictionaryInput req);

        /// <summary>
        /// 获取字典详细
        /// </summary>
        /// <param name="parentKey">父级key</param>
        /// <returns>字典集合</returns>
        Task<List<DicionaryUseDto>> GetDictionarySubAsync(string parentKey);

        /// <summary>
        /// 根据父级Key获取字典列表，包含禁用
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        Task<List<DicionaryUseDto>> GetDictionarySubContainDisableAsync(string parentKey);

        /// <summary>
        /// 获取语言字典列表
        /// </summary>
        /// <returns></returns>
        Task<List<DicionaryUseDto>> GetLanguageDicList();

        /// <summary>
        /// 获取地区关联所需的字典list
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetAreaRelationNeedDictionaryAsync();

        /// <summary>
        /// 根据父级key的数组返回对应
        /// </summary>
        /// <param name="parentKeys">父级keys</param>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetDictionaryByArry(List<string> parentKeys);
    }
}