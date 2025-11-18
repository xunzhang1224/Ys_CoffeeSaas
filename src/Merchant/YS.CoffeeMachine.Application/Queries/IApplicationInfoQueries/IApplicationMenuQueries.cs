using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 菜单查询接口
    /// </summary>
    public interface IApplicationMenuQueries
    {
        /// <summary>
        /// 根据Id获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationMenuDto> GetApplicationMenuAsync(long id);

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuSelectDto>> GetMenuSelectAsync();

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<MenusDto> GetApplicationMenuListAsync(QueryRequest queryRequest);

        /// <summary>
        /// 根据上级企业Id获取菜单列表
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <param name="parentTenantId"></param>
        /// <param name="enterpriseTypeId"></param>
        /// <returns></returns>
        Task<MenusDto> GetApplicationMenuListByParentTenantIdAsync(QueryRequest queryRequest, long parentTenantId, long? enterpriseTypeId = null);

        /// <summary>
        /// 获取当前用户的菜单权限列表
        /// </summary>
        /// <returns></returns>
        Task<List<object>> GetUserMenuTreeAsync(SysMenuTypeEnum sysMenuType);

        /// <summary>
        /// 获取当前用户拥有的标识集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetOwnBtnPermList(long userId);

        /// <summary>
        /// 获取所有权限标识集合
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllBtnPermList();
    }
}
