using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries
{
    /// <summary>
    /// 用户查询
    /// </summary>
    public interface IP_ApplicationUserQueries
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeRole"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetApplicationUserInfoAsync(long id, bool IncludeRole);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="IncludeRole"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserListAsync(IP_ApplicationUserInput request);

        /// <summary>
        /// 根据企业Id获取默认用户列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        Task<List<P_ApplicationUserDto>> GetDefaultUserByEnterpriseIdAsync(long enterpriseId);
    }
}
