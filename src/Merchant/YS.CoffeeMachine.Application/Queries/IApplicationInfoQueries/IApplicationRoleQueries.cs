using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 角色查询接口
    /// </summary>
    public interface IApplicationRoleQueries
    {
        /// <summary>
        /// 根据Id查询角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeMenu"></param>
        /// <returns></returns>
        Task<ApplicationRoleDto> GetApplicationRoleAsync(long id, bool IncludeMenu);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleListAsync(QueryRequest request);

        /// <summary>
        /// 根据企业Id获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationRoleDto>> GetApplicationRoleByEnterpriseIdListAsync(QueryRequest request, long enterpriseId, string name, RoleStatusEnum? roleStatus);

        /// <summary>
        /// 获取当前企业角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationRoleDto>> GetEnterpriceRoleList(long enterpriseId);

        /// <summary>
        /// 根据角色Id获取菜单Ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<long>> GetUserMenusByRoleIdAsync(long roleId);

        /// <summary>
        /// 获取角色下拉框
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        Task<List<object>> GetRoleSelectAsync(long enterpriseId);
    }
}
