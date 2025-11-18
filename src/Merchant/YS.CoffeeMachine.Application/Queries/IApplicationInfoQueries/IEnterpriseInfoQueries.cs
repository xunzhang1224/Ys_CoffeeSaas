using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 企业信息查询接口
    /// </summary>
    public interface IEnterpriseInfoQueries
    {
        /// <summary>
        /// 通过Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        Task<EnterpriseInfoDto> GetEnterpriseInfoAsync(long id, bool isInclude);
        /// <summary>
        /// 根据当前用户所在企业Id获取企业列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //Task<EnterpriseInfoListDto> GetEnterpriseListByIdAsync(QueryRequest request);
        /// <summary>
        /// 根据企业Id获取企业列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        //Task<EnterpriseInfoListDto> GetEnterpriseListByEnterpriseIdAsync(QueryRequest request, long enterpriseId);

        /// <summary>
        /// 根据企业Id获取企业列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<EnterpriseInfoDto> GetEnterpriseTreeAsync(long enterpriseId);

        /// <summary>
        /// 根据企业Id获取下级企业列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        Task<List<EnterpriseInfoDto>> GetEnterpriseInfoDtosByPId(long enterpriseId);

        /// <summary>
        /// 根据当前用户获取企业树
        /// </summary>
        /// <returns></returns>
        Task<EnterpriseInfoDto> GetEnterpriseTreeAsync();

        /// <summary>
        /// 根据Id获取企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EnterpriseTypesDto> GetEnterpriseTypeByIdAsync(long id);

        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        Task<List<EnterpriseTypesDto>> GetEnterpriseTypesAsync();

        /// <summary>
        /// 获取国家地区关系列表
        /// </summary>
        /// <returns></returns>
        Task<List<AreaRelationDto>> GetAreaRelationAllList();

        /// <summary>
        /// 获取企业资质信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        //Task<EnterpriseQualificationInfoDto> GetEnterpriseQualificationInfoAsync(long enterpriseId);
    }
}
