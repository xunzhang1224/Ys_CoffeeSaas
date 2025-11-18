using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 菜单查询
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
        Task<ApplicationMenuListDto> GetApplicationMenuListAsync(QueryRequest queryRequest);
        /// <summary>
        /// 根据上级企业Id获取菜单列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<ApplicationMenuListDto> GetApplicationMenuListByParentTenantIdAsync(QueryRequest queryRequest, long parentTenantId);

        /// <summary>
        /// 获取当前用户的菜单权限列表
        /// </summary>
        /// <returns></returns>
        Task<List<object>> GetUserMenuTreeAsync();

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
