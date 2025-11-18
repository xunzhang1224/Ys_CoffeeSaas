using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.FaultCodeCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.FaultCodeCommandHandlers
{
    /// <summary>
    /// 故障码命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class FaultCodeCommandHandler(CoffeeMachinePlatformDbContext context, IRedisClient _redisClient) : ICommandHandler<FaultCodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(FaultCodeCommand request, CancellationToken cancellationToken)
        {
            // 验证Code是否存在
            if (await context.FaultCodeEntitie.AnyAsync(w => w.Code == request.code))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            var info = new FaultCodeEntity(request.code, request.lanCode, request.type, request.name, request.description, request.remark);
            await context.FaultCodeEntitie.AddAsync(info);

            // 缓存故障码多语言Code
            var cacheKey = string.Format(CacheConst.FaultCodeKey, request.code);
            await _redisClient.SetAsync(cacheKey, request.lanCode);

            return true;
        }
    }

    /// <summary>
    /// 更改故障码命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class UpdateFaultCodeCommandHandler(CoffeeMachinePlatformDbContext context, IRedisClient _redisClient) : ICommandHandler<UpdateFaultCodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateFaultCodeCommand request, CancellationToken cancellationToken)
        {
            var info = await context.FaultCodeEntitie.FirstOrDefaultAsync(w => w.Code == request.code);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.lanCode, request.name, request.description, request.remark);

            // 更新故障码多语言Code缓存
            var cacheKey = string.Format(CacheConst.FaultCodeKey, request.code);
            await _redisClient.DelAsync(cacheKey);
            await _redisClient.SetAsync(cacheKey, request.lanCode);

            return true;
        }
    }

    /// <summary>
    /// 删除故障码命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    public class DeleteFaultCodeCommandHandler(CoffeeMachinePlatformDbContext context, IRedisClient _redisClient) : ICommandHandler<DeleteFaultCodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteFaultCodeCommand request, CancellationToken cancellationToken)
        {
            var info = await context.FaultCodeEntitie.FirstOrDefaultAsync(w => w.Code == request.code);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            context.FaultCodeEntitie.Remove(info);

            // 删除故障码多语言Code缓存
            var cacheKey = string.Format(CacheConst.FaultCodeKey, request.code);
            await _redisClient.DelAsync(cacheKey);

            return true;
        }
    }
}
