using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 企业资质信息查询接口
    /// </summary>
    public interface IEnterpriseQualificationInfoQueries
    {
        /// <summary>
        /// 根据Id获取企业资质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EnterpriseQualificationInfoOutput> GetEnterpriseQualificationInfoAsync();

        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool?> IsAccountExists(string account);
    }
}