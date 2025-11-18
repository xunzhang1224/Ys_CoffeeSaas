using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries
{
    /// <summary>
    /// 角色查询
    /// </summary>
    public interface IP_ApplicationRoleQueries
    {
        /// <summary>
        /// 根据Id查询平台角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeMenu"></param>
        /// <returns></returns>
        Task<ApplicationRoleDto> GetApplicationRoleAsync(long id, bool IncludeMenu);

        #region 平台角色

        /// <summary>
        /// 获取平台角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationRoleDto>> GetPlatformRoleListAsync(QueryRequest request, RoleStatusEnum? roleStatus);

        /// <summary>
        /// 根据角色Id获取平台菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<long>> GetPlatformMenusByRoleIdAsync(long roleId);
        #endregion

        #region 商户角色

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationRoleDto>> GetMerchantRoleListAsync(QueryRequest request);

        /// <summary>
        /// 根据角色Id获取平台菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<long>> GetMerchantMenusByRoleIdAsync(long roleId);
        #endregion
    }
}
