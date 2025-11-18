using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.IOperationLog;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.LogDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.BasicQueries.FaultCode;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Localization;
//using YS.CoffeeMachine.Application.Tools;

namespace YS.CoffeeMachine.API.Queries.OperationLog
{
    /// <summary>
    /// 操作日志查询服务
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    /// <param name="faultCodeInfoQueries"></param>
    public class OperationLogQueries(CoffeeMachineDbContext dbContext, IMapper mapper, IFaultCodeInfoQueries faultCodeInfoQueries, IHttpContextAccessor _httpContextAccessor) : IOperationLogQueries
    {
        /// <summary>
        /// 操作日志查询服务
        /// </summary>
        public async Task<OperationLogDto> GetOperationLogAsync(string code)
        {
            var data = await dbContext.OperationLog.Include(x => x.OperationSubLogs).FirstOrDefaultAsync(x => x.Code == code);
            return mapper.Map<OperationLogDto>(data);
        }

        /// <summary>
        /// 操作日志分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OperationLogQueriesDto>> GetOperationLogQueriesAsync(OperationLogInput dto)
        {
            var acceptLanguage = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();
            var data = await dbContext.OperationLog.AsQueryable().AsNoTracking().Include(x => x.OperationSubLogs)
                .WhereIf(!string.IsNullOrEmpty(dto.Code), x => x.Code == dto.Code)
                .WhereIf(!string.IsNullOrEmpty(dto.Mid), x => x.Mid == dto.Mid)
                .WhereIf(dto.OperationResult != null, x => x.OperationResult == dto.OperationResult)
                .WhereIf(!string.IsNullOrEmpty(dto.OperationName), x => x.OperationName == dto.OperationName)
                .OrderByDescending(o => o.CreateTime)
                .ToPagedListAsync(dto.PageNumber, dto.PageSize);
            var operationNames = data.Items.Select(x => x.OperationName).Distinct().ToList();

            var faultCodeDic = new Dictionary<string, string>();
            if (operationNames.Any())
            {
                var names = await dbContext.LanguageText.Where(x => operationNames.Contains(x.Code) && x.LangCode == acceptLanguage).ToListAsync();
                if (names == null || names.Count == 0)
                    names = await dbContext.LanguageText.Where(x => operationNames.Contains(x.Code) && x.LangCode == "zh-CN").ToListAsync();
                foreach (var operationName in operationNames)
                {
                    var name = names?.FirstOrDefault(x => x.Code == operationName)?.Value;
                    //var faultCode = await faultCodeInfoQueries.GetFaultLanCodeByCode(operationName);
                    //if (!string.IsNullOrEmpty(faultCode))
                    //{
                    faultCodeDic[operationName] = name ?? operationName;
                    //}
                }
            }

            // 映射到 DTO
            var result = mapper.Map<PagedResultDto<OperationLogQueriesDto>>(data);

            if (faultCodeDic.Count > 0)
            {
                foreach (var item in result.Items)
                {
                    if (faultCodeDic.TryGetValue(item.OperationName, out var lanCode))
                    {
                        item.OperationName = L.Text[lanCode]; // 展示故障码多语言名称
                    }
                }
            }

            return result;
        }
    }
}