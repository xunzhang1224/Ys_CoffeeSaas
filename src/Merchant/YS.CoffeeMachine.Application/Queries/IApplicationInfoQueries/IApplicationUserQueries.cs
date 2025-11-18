using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 用户查询接口
    /// </summary>
    public interface IApplicationUserQueries
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        //UserInfoDto GetUserInfoDtoAsync();

        /// <summary>
        /// 通过Id查询用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetApplicationUserInfoAsync(long id, bool IncludeRole);

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUserDto> GetCurrentApplicationUserInfoAsync();

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserListAsync(QueryRequest request);

        /// <summary>
        /// 根据企业Id获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserByEnterpriseIdListAsync(ApplicationUserInput input);

        /// <summary>
        /// 获取同邮箱的用户列表
        /// </summary>
        /// <returns></returns>
        Task<List<ChangeUserDto>> GetChangeUserListAsync();

        /// <summary>
        /// 获取H5用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<H5Users>> GetH5UsersListAsync(QueryRequest request);

        /// <summary>
        /// 根据Id获取单个服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SingleTermServiceOutput> GetSingleTermServiceById(long id);
    }
}
