using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries
{
    /// <summary>
    /// 平台端用户信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class P_ApplicationUserQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IP_ApplicationUserQueries
    {
        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeRole"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApplicationUserDto> GetApplicationUserInfoAsync(long id, bool IncludeRole)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = IncludeRole ? await context.ApplicationUser.Includes("ApplicationUserRoles.Role").FirstOrDefaultAsync(x => x.Id == id && x.SysMenuType == SysMenuTypeEnum.Platform) : await context.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            ApplicationUserDto applicationUserDto = mapper.Map<ApplicationUserDto>(info);
            applicationUserDto.BindUserRole(info.ApplicationUserRoles);
            return applicationUserDto;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserListAsync(IP_ApplicationUserInput request)
        {
            //获取分页数据
            var list = await context.ApplicationUser.Where(w => (request.Status == null || w.Status == request.Status)
            && (request.RoleId == null || w.ApplicationUserRoles.Where(rw => rw.RoleId == request.RoleId).Count() > 0)
            && w.SysMenuType == SysMenuTypeEnum.Platform)
                .ToPagedListAsync(request, "ApplicationUserRoles.Role");

            ApplicationUserListDto listDtos = new (list.Items);
            PagedResultDto<ApplicationUserDto> pagedResultDto = new PagedResultDto<ApplicationUserDto>()
            {
                Items = listDtos.ApplicationUserDots,
                PageNumber = list.PageNumber,
                PageSize = list.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }

        /// <summary>
        /// 根据企业Id获取默认用户列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<List<P_ApplicationUserDto>> GetDefaultUserByEnterpriseIdAsync(long enterpriseId)
        {
            var list = await context.ApplicationUser.Where(w => w.EnterpriseId == enterpriseId && w.IsDefault).ToListAsync();
            return list.Select(s => new P_ApplicationUserDto() { Id = s.Id, Account = s.Account }).ToList();
        }
    }
}
