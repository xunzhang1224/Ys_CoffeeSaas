using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IServiceQueries;

namespace YS.CoffeeMachine.API.Queries.ServiceQueries
{
    /// <summary>
    /// 枚举帮助查询
    /// </summary>
    /// <param name="enumHelper"></param>
    public class EnumHelperQueries(IEnumHelper enumHelper) : IEnumHelperQueries
    {
        /// <summary>
        /// 获取项目所有枚举信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<EnumInfo>> GetAllEnumInfos()
        {
            return await enumHelper.GetAllEnumInfos();
        }
    }
}
