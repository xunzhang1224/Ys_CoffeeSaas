using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Model;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.ApplicationInfoQueries
{
    /// <summary>
    /// 用户信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class ApplicationUserQueries(CoffeeMachineDbContext context, IMapper mapper, UserHttpContext _user) : IApplicationUserQueries
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        //public UserInfoDto GetUserInfoDtoAsync()
        //{
        //    var user = _httpContextAccessor.HttpContext?.User;
        //    if (user == null || !user.Identity.IsAuthenticated)
        //    {
        //        throw ExceptionHelper.AppFriendly("用户未登录或 Token 无效"); // 用户未登录或 Token 无效
        //    }
        //    return new UserInfoDto()
        //    {
        //        TransId = Convert.ToInt64(user.Claims.FirstOrDefault(c => c.SubType == "UId")?.Value),
        //        EnterpriseId = Convert.ToInt64(user.Claims.FirstOrDefault(c => c.SubType == "EnterpriseId")?.Value),
        //        Username = user.Claims.FirstOrDefault(c => c.SubType == "Account")?.Value,
        //        NickName = user.Claims.FirstOrDefault(c => c.SubType == "NickName")?.Value,
        //        SysMenuType = user.Claims.FirstOrDefault(c => c.SubType == "SysMenuType")?.Value
        //    };
        //}

        /// <summary>
        /// 通过Id查询用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IncludeRole"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApplicationUserDto> GetApplicationUserInfoAsync(long id, bool IncludeRole = false)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = IncludeRole ? await context.ApplicationUser.Includes("ApplicationUserRoles.Role").FirstOrDefaultAsync(x => x.Id == id && x.SysMenuType == SysMenuTypeEnum.Merchant) : await context.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            ApplicationUserDto applicationUserDto = mapper.Map<ApplicationUserDto>(info);
            applicationUserDto.BindUserRole(info.ApplicationUserRoles);
            return applicationUserDto;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ApplicationUserDto> GetCurrentApplicationUserInfoAsync()
        {
            var info = await context.ApplicationUser.Includes("ApplicationUserRoles.Role").FirstOrDefaultAsync(x => x.Id == _user.UserId && x.SysMenuType == SysMenuTypeEnum.Merchant);
            if (info is null)
                throw new KeyNotFoundException();
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(x => x.Id == _user.TenantId);
            if (enterpriseInfo is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

            var allNodes = await context.EnterpriseInfo.IgnoreQueryFilters()
           .AsNoTracking().Where(w => !w.IsDelete)
           .ToListAsync();

            var allIds = GetAllChildrenIds(allNodes, _user.TenantId);
            allIds.Add(_user.TenantId);

            var deviceCount = await context.DeviceInfo.AsQueryable().IgnoreQueryFilters().Where(w => allIds.Contains(w.EnterpriseinfoId) && !w.IsDelete).CountAsync();
            //if (deviceCount > 0)
            //{
            //    throw ExceptionHelper.AppFriendly("当前企业树下，存在未解绑设备" + deviceCount + "台");
            //}

            var resDto = new ApplicationUserDto
            {
                Id = info.Id,
                NickName = info.NickName,
                Account = info.Account,
                AreaCode = info.AreaCode,
                Phone = info.Phone,
                Email = info.Email,
                Status = info.Status,
                Remark = info.Remark,
                IsDefault = info.IsDefault,
                EnterpriseName = enterpriseInfo.Name,
                RegisterTime = info.CreateTime,
                Role = new RoleDto()
                {
                    Ids = info.ApplicationUserRoles.Select(r => r.Id).ToList(),
                    Names = info.ApplicationUserRoles.Select(s => s.Role).Select(s => s.Name).ToList(),
                    IsAdmin = info.ApplicationUserRoles.Select(s => s.Role).ToList().FirstOrDefault(s => s.Code == "administrator") != null,
                    CodeNames = info.ApplicationUserRoles.Select(s => s.Role).ToDictionary(s => s.Name, s => s.Code)
                },
                IsCanLogOff = !(deviceCount > 0)
            };

            if (resDto.Role != null && resDto.Role.CodeNames != null)
            {
                List<string> newNames = new List<string>();
                foreach (var code in resDto.Role.CodeNames)
                {
                    newNames.Add(code.Value == null ? code.Key : L.Text[code.Value]);
                }
                resDto.Role.Names = newNames;
            }

            return resDto;
        }

        /// <summary>
        /// 根据指定id获取 所有子节点
        /// </summary>
        /// <param name="allNodes"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<long> GetAllChildrenIds(List<EnterpriseInfo> allNodes, long parentId)
        {
            var result = new List<long>();
            void Recurse(long id)
            {
                var children = allNodes.Where(x => x.Pid == id).ToList();
                foreach (var child in children)
                {
                    result.Add(child.Id);
                    Recurse(child.Id); // 递归继续找子节点
                }
            }

            Recurse(parentId);
            return result;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserListAsync(QueryRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);
            //获取分页数据
            var list = await context.ApplicationUser.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).ToPagedListAsync(request, "ApplicationUserRoles.Role");
            ApplicationUserListDto listDtos = new ApplicationUserListDto(list.Items);
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
        /// 根据企业Id获取用户列表及角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PagedResultDto<ApplicationUserDto>> GetApplicationUserByEnterpriseIdListAsync(ApplicationUserInput input)
        {
            if (input.PageNumber <= 0 || input.PageSize <= 0)
                throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.C0011)]);
            // 获取企业信息
            var enterpriseInfoQuery = context.EnterpriseInfo.AsNoTracking()
                .Where(e => e.Id == input.enterpriseId);

            // 筛选用户
            var usersQuery = enterpriseInfoQuery
                .SelectMany(e => e.Users.Select(u => u.User)).Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant);

            // 动态过滤
            usersQuery = usersQuery.WhereIf(!string.IsNullOrWhiteSpace(input.nickName), w => w.NickName.Contains(input.nickName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.account), w => w.Account.Contains(input.account))
                .WhereIf(!string.IsNullOrWhiteSpace(input.phone), w => w.Phone.Contains(input.phone))
                .WhereIf(!string.IsNullOrWhiteSpace(input.email), w => w.Email.Contains(input.email))
                .WhereIf(input.status.HasValue, w => w.Status == input.status)
                .WhereIf(input.roleId != null, w => w.ApplicationUserRoles.Select(s => s.RoleId).ToList().Contains(input.roleId.Value));

            // 获取总数
            var totalCount = await usersQuery.CountAsync();

            // 分页和排序
            var users = await usersQuery
                .OrderBy(u => u.Id)
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .Select(u => new
                {
                    u,
                    Roles = u.ApplicationUserRoles.Select(r => r.Role).Where(w => w.Status == RoleStatusEnum.Enable).Select(rs => new { rs.Id, rs.Name, rs.Code })
                })
                .AsSplitQuery()
                .ToListAsync();
            // 当前页UserId集合
            var userIds = users.Select(s => s.u.Id).ToList();
            // 获取用户关联设备Ids
            var userDeviceDic = await context.DeviceUserAssociation.AsNoTracking()
                .Where(w => userIds.Contains(w.UserId))
                .GroupBy(g => g.UserId)
                .ToDictionaryAsync(d => d.Key, d => d.Select(s => s.DeviceId).ToList());
            // 组装并返回结果
            var tt = new PagedResultDto<ApplicationUserDto>
            {
                Items = users.Select(u => new ApplicationUserDto
                {
                    Id = u.u.Id,
                    NickName = u.u.NickName,
                    Account = u.u.Account,
                    AreaCode = u.u.AreaCode,
                    Phone = u.u.Phone,
                    Email = u.u.Email,
                    Status = u.u.Status,
                    Remark = u.u.Remark,
                    IsDefault = u.u.IsDefault,
                    Role = new RoleDto()
                    {
                        Ids = u.Roles.Select(r => r.Id).ToList(),
                        Names = u.Roles.Select(s => s.Name).ToList(),
                        IsAdmin = u.Roles.FirstOrDefault(s => s.Code == "administrator") != null,
                        //Codes = u.Roles.Select(s => s.Code).ToList(),
                        CodeNames = u.Roles.ToDictionary(s => s.Name, s => s.Code)
                    },
                    DevicesIds = (userDeviceDic.Count == 0 || !userDeviceDic.ContainsKey(u.u.Id)) ? new List<long>() : userDeviceDic[u.u.Id]
                }).OrderByDescending(o => o.IsDefault).ThenBy(o => o.Id).ToList(),
                TotalCount = totalCount,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
            };

            foreach (var item in tt.Items)
            {
                if (item.Role != null && item.Role.CodeNames != null)
                {
                    List<string> newNames = new List<string>();
                    foreach (var code in item.Role.CodeNames)
                    {
                        newNames.Add(code.Value == null ? code.Key : L.Text[code.Value]);
                    }
                    item.Role.Names = newNames;
                }
            }

            return tt;
        }

        /// <summary>
        /// 获取同邮箱的用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ChangeUserDto>> GetChangeUserListAsync()
        {
            return await (from a in context.ApplicationUser
                          join b in context.EnterpriseInfo on a.EnterpriseId equals b.Id into ab
                          from b in ab.DefaultIfEmpty()
                          where (a.Email == _user.Email || (!string.IsNullOrEmpty(_user.Phone) && a.Phone == _user.Phone))
                          && a.SysMenuType == SysMenuTypeEnum.Merchant && !a.IsDelete && a.Status == UserStatusEnum.Enable && !b.IsDelete
                          select new ChangeUserDto
                          {
                              Id = a.Id,
                              NickName = a.NickName,
                              Account = a.Account,
                              EnterpriseName = b != null ? b.Name : string.Empty,
                              RegistrationProgress = b != null ? b.RegistrationProgress : null,

                          }).ToListAsync();
        }

        /// <summary>
        /// 根据Id获取单个服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SingleTermServiceOutput> GetSingleTermServiceById(long id)
        {
            return await context.TermServiceEntity.AsQueryable()
                .Where(a => a.Id == id)
                .Select(a => new SingleTermServiceOutput
                {
                    Title = a.Title,
                    Content = a.Content,
                }).FirstOrDefaultAsync() ?? new SingleTermServiceOutput();
        }

        #region H5

        /// <summary>
        /// 获取H5用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<H5Users>> GetH5UsersListAsync(QueryRequest request)
        {
            // 获取分页数据
            return await context.ApplicationUser
                .Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant && w.EnterpriseId == _user.TenantId && !w.IsDelete)
                .Select(s => new H5Users
                {
                    Id = s.Id,
                    NickName = s.NickName,
                    Account = s.Account,
                    AreaCode = s.AreaCode,
                    Phone = s.Phone,
                    Email = s.Email,
                    RoleIds = s.ApplicationUserRoles.Select(r => r.RoleId).ToList(),
                    RoleNames = string.Join(",", s.ApplicationUserRoles.Select(r => r.Role).Where(w => w.Status == RoleStatusEnum.Enable).Select(rs => rs.Name)),
                    Status = s.Status,
                    isDefault = s.IsDefault
                })
                .ToPagedListAsync(request, "ApplicationUserRoles.Role");
        }
        #endregion
    }
}