using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IFaultCodeInfoQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Queries.FaultCodeInfoQueries
{
    /// <summary>
    /// 故障码查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class FaultCodeInfoQueries(CoffeeMachinePlatformDbContext context, IRedisClient _redisClient) : IFaultCodeInfoQueries
    {
        /// <summary>
        /// 故障码分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<FaultCodeDto>> GetFalutCodeList(FaultCodeInput input)
        {
            return context.FaultCodeEntitie.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(input.Code), w => w.Code.Contains(input.Code!))
                .WhereIf(!string.IsNullOrEmpty(input.LanCode), w => w.LanCode.Contains(input.LanCode!))
                .WhereIf(!string.IsNullOrEmpty(input.Name), w => w.Name.Contains(input.Name!))
                .WhereIf(input.Type != null, w => w.Type == input.Type)
                .Select(s => new FaultCodeDto()
                {
                    Code = s.Code,
                    LanCode = s.LanCode,
                    Type = s.Type,
                    Name = s.Name,
                    Description = s.Description,
                    Remark = s.Remark
                })
                .ToPagedListAsync(input);
        }

        /// <summary>
        /// 根据code查询故障码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<FaultCodeDto> GetFaultCode(string code)
        {
            return await context.FaultCodeEntitie
                .Select(s => new FaultCodeDto()
                {
                    Code = s.Code,
                    LanCode = s.LanCode,
                    Name = s.Name,
                    Description = s.Description,
                })
                .FirstOrDefaultAsync(w => w.Code == code) ?? throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
        }

        /// <summary>
        /// 根据故障码获取多语言code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetFaultLanCodeByCode(string code)
        {
            var cacheKey = string.Format(CacheConst.FaultCodeKey, code);
            var cacheLanCode = await _redisClient.GetAsync(cacheKey);
            if (cacheLanCode != null)
                return cacheLanCode;

            var info = await context.FaultCodeEntitie.FirstOrDefaultAsync(w => w.Code == code);
            if (info == null)
                return string.Empty;

            await _redisClient.SetAsync(cacheKey, info.LanCode);
            return info.LanCode;
        }
    }
}
