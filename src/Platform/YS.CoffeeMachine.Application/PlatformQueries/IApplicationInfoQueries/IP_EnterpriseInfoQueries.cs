using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries
{
    /// <summary>
    /// 企业查询
    /// </summary>
    public interface IP_EnterpriseInfoQueries
    {
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<P_EnterpriseInfoDto>> GetEnterpriseInfoListAsync(P_EnterpriseInfoInput request);

        /// <summary>
        /// 企业选择列表
        /// </summary>
        /// <returns></returns>
        Task<List<P_EnterpriseSelect>> GetEnterpriseSelectListAsync();

        /// <summary>
        /// 企业注册审核列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<P_EnterpriseRegisterDto>> GetEnterpriseRegisterListAsync(P_EnterpriseRegisterInput request);

        /// <summary>
        /// 企业注册审核资质信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        Task<EnterpriseQualificationInfoDto> GetEnterpriseQualificationInfoAsync(long enterpriseId);

        /// <summary>
        /// 根据Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<P_EnterpriseInfoDto> GetNormalEnterpriseInfoById(long id);
    }
}