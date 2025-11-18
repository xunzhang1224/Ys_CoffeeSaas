using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.ApplicationInfoQueries
{
    /// <summary>
    /// 企业信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_user"></param>
    public class EnterpriseInfoQueries(CoffeeMachineDbContext context, IMapper mapper, UserHttpContext _user) : IEnterpriseInfoQueries
    {
        /// <summary>
        /// 通过Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EnterpriseInfoDto> GetEnterpriseInfoAsync(long id, bool isInclude)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = isInclude ? await context.EnterpriseInfo.Includes("Users.User", "Roles.Role").FirstOrDefaultAsync(x => x.Id == id) : await context.EnterpriseInfo.FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            var entetpriseInfoDto = mapper.Map<EnterpriseInfoDto>(info);
            entetpriseInfoDto.BindUsers(info.Users);
            entetpriseInfoDto.BindRoles(info.Roles);
            return entetpriseInfoDto;
        }

        /// <summary>
        /// 根据当前用户获取企业树
        /// </summary>
        /// <returns></returns>
        public async Task<EnterpriseInfoDto> GetEnterpriseTreeAsync()
        {
            // 判断当前用户是否切换了机构
            var enterpriseId = await context.ApplicationUser.IgnoreQueryFilters().Where(w => w.Id == _user.UserId).Select(x => x.EnterpriseId).FirstOrDefaultAsync();

            if (enterpriseId == _user.TenantId)
            {
                // 获取根节点
                var root = await context.EnterpriseInfo.AsNoTracking()
                    .Where(w => _user.TenantId == w.Id)
                    .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                    .FirstOrDefaultAsync();

                if (root == null) return null;

                // 递归加载子节点
                await LoadChildrenAsync(root, false);
                return root;
            }
            else
            {
                // 获取根节点
                var root = await context.EnterpriseInfo.AsNoTracking().IgnoreQueryFilters()
                    .Where(w => enterpriseId == w.Id && !w.IsDelete)
                    .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                    .FirstOrDefaultAsync();

                if (root == null) return null;

                // 递归加载子节点
                await LoadChildrenAsyncNotTenant(root, false);
                return root;
            }

        }

        // 递归加载子节点
        private async Task LoadChildrenAsyncNotTenant(EnterpriseInfoDto parent, bool isInclude)
        {
            var query = isInclude ? context.EnterpriseInfo.AsQueryable().AsNoTracking().IgnoreQueryFilters()
                .Include(i => i.Users).ThenInclude(ti => ti.User).ThenInclude(i => i.ApplicationUserRoles).ThenInclude(i => i.Role)
                : context.EnterpriseInfo.AsQueryable().AsNoTracking().IgnoreQueryFilters();
            // 查询当前节点的直接子节点
            var children = await query.Where(e => e.Pid == parent.Id && !e.IsDelete)
                .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                .ToListAsync();

            parent.children.AddRange(children);

            // 递归加载每个子节点的子节点
            foreach (var child in children)
            {
                await LoadChildrenAsyncNotTenant(child, isInclude);
            }
        }

        /// <summary>
        /// 获取企业列表
        /// 获取当前企业Id节点及其所有下级节点的树形结构
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<EnterpriseInfoDto> GetEnterpriseTreeAsync(long enterpriseId)
        {
            // 获取根节点
            var root = await context.EnterpriseInfo.AsNoTracking().Include(i => i.Users).ThenInclude(ti => ti.User).ThenInclude(i => i.ApplicationUserRoles).ThenInclude(i => i.Role)
                .Where(w => enterpriseId == w.Id)
                .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                .FirstOrDefaultAsync();

            if (root == null) return null;

            // 递归加载子节点
            await LoadChildrenAsync(root, true);
            return root;
        }

        // 递归加载子节点
        private async Task LoadChildrenAsync(EnterpriseInfoDto parent, bool isInclude)
        {
            var query = isInclude ? context.EnterpriseInfo.AsQueryable().AsNoTracking().Include(i => i.Users).ThenInclude(ti => ti.User).ThenInclude(i => i.ApplicationUserRoles).ThenInclude(i => i.Role) : context.EnterpriseInfo.AsQueryable().AsNoTracking();
            // 查询当前节点的直接子节点
            var children = await query.Where(e => e.Pid == parent.Id)
                .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                .ToListAsync();

            parent.children.AddRange(children);

            // 递归加载每个子节点的子节点
            foreach (var child in children)
            {
                await LoadChildrenAsync(child, isInclude);
            }
        }

        /// <summary>
        /// 根据企业Id获取下级企业列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<List<EnterpriseInfoDto>> GetEnterpriseInfoDtosByPId(long enterpriseId)
        {
            if (enterpriseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(enterpriseId));
            var list = await context.EnterpriseInfo.AsNoTracking()
                .Where(w => w.Pid == enterpriseId)
                .Include(i => i.Users).ThenInclude(ti => ti.User).ThenInclude(i => i.ApplicationUserRoles).ThenInclude(i => i.Role)
                .Select(x => new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds, x.AreaRelationId))
                .ToListAsync();
            return list;
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public async Task<EnterpriseInfoListDto> GetEnterpriseListAsync(QueryRequest request)
        //{
        //    var list = await context.EnterpriseInfo.Includes("Users.User", "Roles.Role", "EnterpriseType").Where(w => !(w.TransId != _user.TenantId && (w.Pid == null || w.Pid == 0))).ToListAsync();
        //    var curAllUsers = list.SelectMany(x => x.Users).Select(s => s.User).ToList();
        //    var resList = FlatToHierarchy(list);
        //    EnterpriseInfoListDto listDtos = new(resList, curAllUsers);
        //    return listDtos;
        //}

        //private List<EnterpriseInfo> FlatToHierarchy(IReadOnlyCollection<EnterpriseInfo> list, long? parentId = null)
        //{

        //    var count = list.Count(w => w.Pid == parentId);
        //    if (count == 0) return new List<EnterpriseInfo>();
        //    return list.Where(w => w.Pid == parentId).OrderBy(o => o.TransId).Select(x => new EnterpriseInfo(x.Name, x.EnterpriseTypeId, x.Pid, x.Remark, x.Users.Select(s => s.UserId).ToList(), x.Roles.Select(s => s.RoleId).ToList(), enterpriseInfos: FlatToHierarchy(list, x.TransId), id: x.TransId, x.EnterpriseType)).ToList();
        //}

        /// <summary>
        /// 根据Id获取企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnterpriseTypesDto> GetEnterpriseTypeByIdAsync(long id)
        {
            var info = await context.EnterpriseTypes.FindAsync(id);
            return mapper.Map<EnterpriseTypesDto>(info);
        }

        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<EnterpriseTypesDto>> GetEnterpriseTypesAsync()
        {
            var list = await context.EnterpriseTypes.ToListAsync();
            return mapper.Map<List<EnterpriseTypesDto>>(list);
        }

        /// <summary>
        /// 国家和时区关系列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AreaRelationDto>> GetAreaRelationAllList()
        {
            var list = await (from area in context.AreaRelation.AsNoTracking()
                              join currency in context.Currency.AsNoTracking()
                              on area.CurrencyId equals currency.Id into gj
                              from subCurrency in gj.DefaultIfEmpty()
                              where area.Enabled == Domain.Shared.Enum.EnabledEnum.Enable
                              select new AreaRelationDto
                              {
                                  Id = area.Id,
                                  Country = area.Country,
                                  CurrencySymbol = subCurrency.CurrencySymbol,
                                  CurrencyCode = subCurrency.Code,
                                  TimeZone = area.TimeZone,
                                  TermServiceUrl = area.TermServiceUrl
                              }).ToListAsync();
            // 获取字典的key
            var dicKeys = list.SelectMany(a => new[] { a.Country, a.TimeZone })
                .Distinct().ToList();
            var dics = context.Dictionary.AsQueryable().Where(a => dicKeys.Contains(a.Key)).ToDictionary(a => a.Key, a => a.Value);
            foreach (var item in list)
            {
                item.CountryName = dics[item.Country];
                item.TimeZoneName = dics[item.TimeZone];
            }

            return list;
        }
    }
}