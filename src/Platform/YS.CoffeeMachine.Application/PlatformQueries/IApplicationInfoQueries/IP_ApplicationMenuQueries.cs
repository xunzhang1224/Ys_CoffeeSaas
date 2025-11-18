using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries
{
    /// <summary>
    /// 菜单查询接口
    /// </summary>
    public interface IP_ApplicationMenuQueries
    {
        /// <summary>
        /// 根据Id获取菜单权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationMenuDto> GetApplicationMenuAsync(long id);
        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuSelectDto>> GetAllMenuTreeAsync();
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApplicationMenuListDto> GetApplicationMenuListAsync(QueryRequest request, SysMenuTypeEnum sysMenuType);
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        Task<AllMenuSelectDto> GetMenuSelectByEnterpriseTypeIdAsync(long id);
        /// <summary>
        /// 获取当前用户菜单树
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
