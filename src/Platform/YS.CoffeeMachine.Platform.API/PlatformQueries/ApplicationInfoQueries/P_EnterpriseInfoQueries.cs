using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Extensions;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries
{
    /// <summary>
    /// 平台端企业信息查询
    /// </summary>
    /// <param name="context"></param>
    public class P_EnterpriseInfoQueries(CoffeeMachinePlatformDbContext context, IConfiguration configuration) : IP_EnterpriseInfoQueries
    {
        /// <summary>
        /// 获取企业注册审核列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<P_EnterpriseRegisterDto>> GetEnterpriseRegisterListAsync(P_EnterpriseRegisterInput input)
        {
            var result = await context.EnterpriseInfo
                .Include(i => i.Users)
                .ThenInclude(i => i.User)
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), w => w.Name.Contains(input.Name!))
                .WhereIf(input.OrganizationType != null, w => w.OrganizationType == input.OrganizationType)
                .WhereIf(input.RegistrationProgress != null, w => w.RegistrationProgress == input.RegistrationProgress)
                .WhereIf(input.Account != null, w => w.Users.FirstOrDefault(u => u.User.IsDefault).User.Account.Contains(input.Account))
                .Where(w => w.RegistrationProgress != null)
                .OrderByDescending(o => o.CreateTime)
                .Select(s => new P_EnterpriseRegisterDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    OrganizationType = s.OrganizationType, // 企业资质类型
                    Account = s.Users.FirstOrDefault(u => u.User.IsDefault).User.Account, // 获取默认管理员账号
                    NickName = s.Users.FirstOrDefault(u => u.User.IsDefault).User.NickName, // 获取默认管理员昵称
                    AreaCode = s.Users.FirstOrDefault(u => u.User.IsDefault).User.AreaCode, // 获取默认管理员区号
                    Phone = s.Users.FirstOrDefault(u => u.User.IsDefault).User.Phone, // 获取默认管理员手机号
                    Email = s.Users.FirstOrDefault(u => u.User.IsDefault).User.Email, // 获取默认管理员邮箱
                    RegistrationTime = s.CreateTime.ToString("G"),// 注册时间
                    registrationProgress = s.RegistrationProgress
                })
                .ToPagedListAsync(input);

            return result;
        }

        /// <summary>
        /// 获取企业注册审核资质信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public async Task<EnterpriseQualificationInfoDto> GetEnterpriseQualificationInfoAsync(long enterpriseId)
        {
            var result = await context.EnterpriseQualificationInfo
                .AsNoTracking()
                .Include(i => i.EnterpriseInfo)
                .ThenInclude(i => i.Users)
                .ThenInclude(i => i.User)
                .Where(w => w.EnterpriseId == enterpriseId)
                .Select(s => new EnterpriseQualificationInfoDto
                {
                    organizationType = s.EnterpriseInfo.OrganizationType,
                    EnterpriseName = s.EnterpriseInfo.Name,
                    LegalPersonName = s.LegalPersonName,
                    LegalPersonIdCardNumber = s.LegalPersonIdCardNumber,
                    AreaRelationId = s.EnterpriseInfo.AreaRelationId,
                    FrontImageUrl = s.FrontImageUrl,
                    BackImageUrl = s.BackImageUrl,
                    CustomerServiceEmail = s.CustomerServiceEmail,
                    StoreAddress = s.StoreAddress,
                    BusinessLicenseUrl = s.BusinessLicenseUrl,
                    Othercertificate = s.Othercertificate,
                    Email = s.EnterpriseInfo.Users.FirstOrDefault(u => u.User.IsDefault).User.Email, // 获取默认管理员邮箱
                    Remark = s.EnterpriseInfo.Remark,
                }).FirstOrDefaultAsync() ?? throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var areaRelationInfo = await context.AreaRelation.FirstOrDefaultAsync(w => w.Id == result.AreaRelationId);

            if (areaRelationInfo != null)
            {
                var dicInfo = await context.Dictionary.FirstOrDefaultAsync(w => w.Key == areaRelationInfo.Country);
                if (dicInfo != null)
                {
                    result.CountryName = dicInfo.Value;
                }
                result.Area = areaRelationInfo.Area;
                result.AreaCode = areaRelationInfo.AreaCode;
                result.Language = areaRelationInfo.Language;
                result.TimeZone = areaRelationInfo.TimeZone;
                var currencyInfo = await context.Currency.FirstOrDefaultAsync(w => w.Id == areaRelationInfo.CurrencyId);
                if (currencyInfo != null)
                    result.Currency = currencyInfo.CurrencySymbol;
            }

            return result;
        }

        /// <summary>
        /// 获取所有主企业列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<P_EnterpriseSelect>> GetEnterpriseSelectListAsync()
        {
            var list = await context.EnterpriseInfo
            .AsNoTracking()
            .Where(w => w.Pid == null || w.Pid == 0)
            .ToListAsync();
            return list.Select(s => new P_EnterpriseSelect() { Id = s.Id, Name = s.Name }).ToList();
        }

        /// <summary>
        /// 获取企业信息分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<P_EnterpriseInfoDto>> GetEnterpriseInfoListAsync(P_EnterpriseInfoInput request)
        {
            // 先进行基本的查询，加载必要的数据
            var query = context.EnterpriseInfo
                .AsNoTracking()
                .AsQueryable();

            // 企业名称模糊查询
            query = query.WhereIf(!string.IsNullOrEmpty(request.enterpriseName), e => e.Name.Contains(request.enterpriseName));

            // 管理员姓名或账号模糊查询，且只选取IsDefault为true的管理员
            query = query.WhereIf(!string.IsNullOrEmpty(request.userName_account), e => e.Users
                .Any(u => (u.User.NickName.Contains(request.userName_account) ||
                           u.User.Account.Contains(request.userName_account)) &&
                          u.User.IsDefault)); // 只选取IsDefault为true的管理员

            query = query.WhereIf(!string.IsNullOrWhiteSpace(request.id), e => e.Id == long.Parse(request.id));

            // 筛选一级企业（Pid = null）
            query = query.Where(e => e.Pid == null && e.Users.Where(w => w.User.IsDefault).Count() > 0);

            // 分页查询
            var enterpriseQuery = await query.ToPagedListAsync(request);

            //获取当前企业Ids
            var enterpriseIds = enterpriseQuery.Items.Select(s => s.Id).ToList();

            //获取当前企业下默认管理员
            var adminUsers = await context.ApplicationUser.AsNoTracking().Where(w => enterpriseIds.Contains(w.EnterpriseId) && w.IsDefault).ToListAsync();

            //获取当前企业绑定的设备数量字典
            //var deviceCountDic = await context.EnterpriseDevices.AsNoTracking().Where(w => enterpriseIds.Contains(w.EnterpriseId)).GroupBy(g => g.EnterpriseId).ToDictionaryAsync(k => k.Key, v => v.Count());

            // 将查询结果映射到DTO，并计算子企业数量
            var enterpriseDtoQuery = new List<P_EnterpriseInfoDto>();

            // 根据地区关联id获取对应的国家value
            var areaRelationIds = enterpriseQuery.Items.Select(a => a.AreaRelationId).Distinct().ToList();
            var tt = from areaRelation in context.AreaRelation
                     join dictionary in context.Dictionary on areaRelation.Country equals dictionary.Key
                     where areaRelationIds.Contains(areaRelation.Id)
                     select new
                     {
                         AreaRelationId = areaRelation.Id,
                         CountryKey = areaRelation.Country,
                         CountryName = dictionary.Value
                     };

            // 获取设备类型
            var enterpriseTypeDic = await context.EnterpriseTypes.AsQueryable().ToDictionaryAsync(a => a.Id, a => a.Name);

            foreach (var e in enterpriseQuery.Items)
            {
                // 获取所有下级企业的数量
                var allChildrenEnterpriseInfos = await GetTotalChildrenCountAsync(e.Id);

                var allEnterPriseInfoIds = allChildrenEnterpriseInfos.Select(a => a.Id).ToList();
                allEnterPriseInfoIds.Add(e.Id);

                // 获取设备个数
                var deviceCountDic = await context.DeviceInfo.AsQueryable().Where(a => allEnterPriseInfoIds.Any(b => b == a.EnterpriseinfoId))
                     .GroupBy(a => a.EnterpriseinfoId)
                     .Select(g => new
                     {
                         EnterpriseinfoId = g.Key,
                         DeviceCount = g.Count()
                     })
                     .ToDictionaryAsync(a => a.EnterpriseinfoId, a => a.DeviceCount);

                // 获取当前企业的所有下级企业信息
                var childrenInfo = await GetAllSubEnterprisesWithChildrenAsync(e.Id, allChildrenEnterpriseInfos, deviceCountDic, enterpriseTypeDic);
                // 获取当前企业的管理员信息
                var adminUser = adminUsers.FirstOrDefault(w => w.EnterpriseId == e.Id);
                deviceCountDic.TryGetValue(e.Id, out int deviceCount);
                enterpriseDtoQuery.Add(new P_EnterpriseInfoDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    EnterpriseTypeId = e.EnterpriseTypeId,
                    //EnterpriseTypeText = e.EnterpriseType.Name, // 假设 EnterpriseType 有 Name 属性
                    Account = adminUser?.Account,
                    NickName = adminUser?.NickName,
                    AreaCode = adminUser?.AreaCode,
                    Phone = adminUser?.Phone,
                    Email = adminUser?.Email,
                    DeviceCount = deviceCount,
                    ChildrenCount = allChildrenEnterpriseInfos.Count() - 1,
                    Children = childrenInfo.Item2,
                    CreateTime = e.CreateTime.ToString("yyyy -MM-dd HH:mm:ss"), // 格式化创建时间
                    AreaRelationId = e.AreaRelationId,
                    CountryKey = tt.FirstOrDefault(a => a.AreaRelationId == e.AreaRelationId)?.CountryKey ?? string.Empty,
                    CountryValue = tt.FirstOrDefault(a => a.AreaRelationId == e.AreaRelationId)?.CountryName ?? string.Empty,
                });
            }

            return new PagedResultDto<P_EnterpriseInfoDto>()
            {
                Items = enterpriseDtoQuery,
                TotalCount = enterpriseQuery.TotalCount,
                PageNumber = enterpriseQuery.PageNumber,
                PageSize = enterpriseQuery.PageSize
            };
        }

        // 使用CTE查询计算所有子企业的数量
        private async Task<List<EnterpriseInfo>> GetTotalChildrenCountAsync(long parentId)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            await using var connection = new SqlConnection(connectionString);

            // 确保连接打开
            await connection.OpenAsync();

            // 定义查询，使用递归 CTE
            const string sqlQuery = @"
                                    WITH RecursiveCTE AS (
                                        SELECT Id, Name, Pid, EnterpriseTypeId, IsDelete,CreateTime 
                                        FROM EnterpriseInfo 
                                        WHERE Id = @parentId AND IsDelete = 0
                                        UNION ALL
                                        SELECT ei.Id, ei.Name, ei.Pid, ei.EnterpriseTypeId, ei.IsDelete,ei.CreateTime 
                                        FROM EnterpriseInfo ei
                                        INNER JOIN RecursiveCTE rcte ON ei.Pid = rcte.Id
                                        WHERE ei.IsDelete = 0
                                    )
                                    SELECT Id, Name, Pid, EnterpriseTypeId, IsDelete,CreateTime FROM RecursiveCTE";

            // 执行查询并读取结果
            var result = (await connection.QueryAsync<EnterpriseInfo>(sqlQuery, new { parentId })).ToList();
            return result;
        }

        // 获取所有下级企业的数量（递归方式），并且组装 Children 集合
        private async Task<(int, List<P_EnterpriseInfoDto>)> GetAllSubEnterprisesWithChildrenAsync(long parentId, List<EnterpriseInfo> allEnterpriseInfos, Dictionary<long, int> deviceCountDic, Dictionary<long, string> enterpriseTypeDic)
        {
            // 获取当前父企业的所有下级企业
            var subEnterprises = new List<EnterpriseInfo>();
            if (allEnterpriseInfos.Count > 0)
                subEnterprises = allEnterpriseInfos
                    .Where(e => e.Pid == parentId).ToList();
            else
                subEnterprises = await context.EnterpriseInfo
                .AsNoTracking()
                .Where(e => e.Pid == parentId)
                .ToListAsync();

            var childrenDtos = new List<P_EnterpriseInfoDto>();
            int totalCount = subEnterprises.Count;
            foreach (var child in subEnterprises)
            {
                // 获取当前子企业的管理员信息
                var adminUser = subEnterprises.SelectMany(s => s.Users).Select(s => s.User).Where(w => w.IsDefault).FirstOrDefault();

                // 递归获取当前子企业的所有下级企业
                deviceCountDic.TryGetValue(child.Id, out int deviceCount);
                enterpriseTypeDic.TryGetValue(child.EnterpriseTypeId ?? 0, out string enterpriseTypeText);

                var childrenCountAndList = await GetAllSubEnterprisesWithChildrenAsync(child.Id, allEnterpriseInfos, deviceCountDic, enterpriseTypeDic);
                totalCount += childrenCountAndList.Item1; // 递增下级企业的数量
                childrenDtos.Add(new P_EnterpriseInfoDto
                {
                    Id = child.Id,
                    Name = child.Name,
                    EnterpriseTypeId = child.EnterpriseTypeId,
                    EnterpriseTypeText = enterpriseTypeText, // child.EnterpriseType?.Name,
                    Account = adminUser?.Account,
                    NickName = adminUser?.NickName,
                    AreaCode = adminUser?.AreaCode,
                    Phone = adminUser?.Phone,
                    Email = adminUser?.Email,
                    CreateTime = child.CreateTime.ToString("G"),
                    ChildrenCount = childrenCountAndList.Item1,
                    Children = childrenCountAndList.Item2,       // 当前企业的下级企业集合
                    DeviceCount = deviceCount  //deviceCountDic[child.Id]
                });
            }

            // 返回当前父企业下级企业的数量和下级企业详细信息
            return (subEnterprises.Count, childrenDtos);
        }

        /// <summary>
        /// 根据Id获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<P_EnterpriseInfoDto> GetNormalEnterpriseInfoById(long id)
        {
            return await context.EnterpriseInfo.AsQueryable()
                .Where(w => w.Id == id && w.IsDelete == false && (w.RegistrationProgress == null || w.RegistrationProgress == RegistrationProgress.Passed))
                .Select(s => new P_EnterpriseInfoDto
                {
                    Id = s.Id,
                    Name = s.Name
                }
                ).FirstOrDefaultAsync() ?? new P_EnterpriseInfoDto();
        }
    }
}
