using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.Basics.Language
{
    /// <summary>
    /// 多语言信息
    /// </summary>
    /// <param name="context"></param>
    public class PLanguageInfoRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<LanguageInfo, CoffeeMachinePlatformDbContext>(context), IPLanguageInfoRepository
    {

        /// <summary>
        /// 获取默认语种
        /// </summary>
        /// <returns></returns>
        public string GetDefaultLanguageCode()
        {
            return context.LanguageInfo.FirstOrDefault(x => x.IsDefault == IsDefaultEnum.Yes)?.Code;
        }

        /// <summary>
        /// 更新多语言信息
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<LanguageInfo> entities)
        {
            context.LanguageInfo.UpdateRange(entities);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据文本码获取多语言信息
        /// </summary>
        /// <param name="textCode"></param>
        /// <param name="languageCode"></param>
        /// <param name="isDefault"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public async Task<List<LanguageInfo>> GetLanguageInfosByTextCodeAsync(string textCode, string? languageCode = null, bool isDefault = false, bool isAll = true)
        {
            var datas = await context.LanguageInfo
                 .Include(x => x.LanguageTextEntitys.Where(s => s.Code == textCode))
                 .ToListAsync();

            return GetLanguageInfos(datas, languageCode, isDefault, isAll);
        }

        /// <summary>
        /// 根据语种码获取多语言信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<LanguageInfo> GetLanguageInfoByCodeAsync(string code)
        {
            return await context.LanguageInfo.Include(i => i.LanguageTextEntitys)
               .Where(x => x.Code == code).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<LanguageInfo>> GetLanguageInfoAsync()
        {
            return await context.LanguageInfo.Include(i => i.LanguageTextEntitys).ToListAsync();
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        public List<LanguageInfo> GetLanguageInfos(List<LanguageInfo> datas, string? languageCode = null, bool isDefault = false, bool isAll = true)
        {
            if (string.IsNullOrWhiteSpace(languageCode) && !isDefault)
                datas = datas;

            // 查默认语种
            if (string.IsNullOrWhiteSpace(languageCode) && isDefault)
                datas = datas.Where(x => x.IsDefault == IsDefaultEnum.Yes).ToList();
            // 查指定语种
            if (!string.IsNullOrWhiteSpace(languageCode))
                datas = datas.Where(x => x.Code == languageCode).ToList();
            // 是否包含未启用的语种信息
            if (!isAll)
                datas = datas.Where(x => x.IsEnabled == EnabledEnum.Enable).ToList();
            return datas;

        }
    }
}
