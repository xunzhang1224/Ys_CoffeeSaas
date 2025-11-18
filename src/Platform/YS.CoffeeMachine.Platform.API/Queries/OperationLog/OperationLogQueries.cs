using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Platform.API.Queries.IOperationLog;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.Platform.API.Queries.OperationLog
{
    /// <summary>
    /// 操作日志查询服务
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    public class OperationLogQueries(CoffeeMachineTimescaleDBContext dbContext,IMapper mapper) : IOperationLogQueries
    {
        /// <summary>
        /// 查询平台操作日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PlatformOperationLog>> GetOperationLogQueriesAsync(POperationLogInput input)
        {
            return await dbContext.PlatformOperationLog.AsQueryable().WhereIf(input.Times != null && input.Times.Count == 2, x => x.Timestamp >= input.Times[0] && x.Timestamp <= input.Times[1])
                .WhereIf(input.TrailType != null, x => x.TrailType == input.TrailType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.OperationUserName.Contains(input.Name))
                .WhereIf(input.Result != null, x => x.Result == input.Result)
                .ToPagedListAsync(input);
        }
    }
}
