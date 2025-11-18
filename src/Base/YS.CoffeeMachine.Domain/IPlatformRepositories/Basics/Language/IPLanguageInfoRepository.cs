using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language
{
    /// <summary>
    /// 语种信息仓储
    /// </summary>
    public interface IPLanguageInfoRepository : IYsRepository<LanguageInfo>
    {
        /// <summary>
        /// 语种集合过滤
        /// </summary>
        /// <param name="datas">语种集合</param>
        /// <param name="languageCode">指定语种</param>
        /// <param name="isDefault">指定默认语种</param>
        /// <param name="isAll">是否包含未启用的语种信息</param>
        /// <returns></returns>
        List<LanguageInfo> GetLanguageInfos(List<LanguageInfo> datas, string? languageCode = null, bool isDefault = false, bool isAll = true);

        /// <summary>
        /// 获取默认语种的code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetDefaultLanguageCode();

        /// <summary>
        /// 获取指定语种信息（包含语言文本集合）
        /// </summary>
        /// <param name="code">语种</param>
        /// <returns></returns>
        Task<LanguageInfo> GetLanguageInfoByCodeAsync(string code);

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <returns></returns>
        Task<List<LanguageInfo>> GetLanguageInfoAsync();

        /// <summary>
        /// 指定语言文本获取语种信息列表
        /// </summary>
        /// <param name="textCode">语言文本</param>
        /// <param name="languageCode">指定语种</param>
        /// <param name="isDefault">指定默认语种</param>
        /// <param name="isAll">是否包含未启用的语种信息</param>
        /// <returns></returns>
        Task<List<LanguageInfo>> GetLanguageInfosByTextCodeAsync(string textCode, string? languageCode = null, bool isDefault = false, bool isAll = true);

        /// <summary>
        /// 批量修改语种信息
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<LanguageInfo> entities);
    }
}
