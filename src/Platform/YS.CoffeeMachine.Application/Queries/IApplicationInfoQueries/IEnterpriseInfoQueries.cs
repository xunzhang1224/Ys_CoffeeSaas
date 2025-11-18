using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 企业信息查询
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
        /// 根据当前用户获取企业树
        /// </summary>
        /// <returns></returns>
        Task<EnterpriseInfoDto> GetEnterpriseTreeAsync();

        /// <summary>
        /// 根据Id获取企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<P_EnterpriseTypesDto> GetEnterpriseTypeByIdAsync(long id);

        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        Task<List<P_EnterpriseTypesDto>> GetEnterpriseTypesAsync();
    }
}
