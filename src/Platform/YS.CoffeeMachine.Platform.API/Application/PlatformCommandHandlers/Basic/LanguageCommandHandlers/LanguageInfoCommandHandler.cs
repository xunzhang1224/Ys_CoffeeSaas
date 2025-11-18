using MediatR;
using YS.CoffeeMachine.Platform.API.Utils;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
using YS.CoffeeMachine.Domain.IRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language;
using Polly;
using YS.CoffeeMachine.Infrastructure;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Provider.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.LanguageCommandHandlers
{
    /// <summary>
    /// 添加语言
    /// </summary>
    /// <param name="repository"></param>
    public class LanguageInfoCommandHandler(IPLanguageInfoRepository _repository, CoffeeMachinePlatformDbContext _context,IRedisService _redis) : ICommandHandler<CreateLanguageInfoCommand, bool>
    {
        /// <summary>
        /// 添加语言
        /// </summary>
        public async Task<bool> Handle(CreateLanguageInfoCommand request, CancellationToken cancellationToken)
        {
            BasicUtils.CheckLanguageInfo(request.isDefault, request.isEnabled);

            // 验证企业类型名称是否存在 && x.Astrict == request.astrict
            var isExist = await _context.LanguageInfo.AnyAsync(x => x.Code == request.code || x.Name == request.name);
            if (isExist)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0018)]);
            await _redis.DelKeyAsync(CacheConst.MultilingualAll);
            var info = new LanguageInfo(request.name, request.code, request.isEnabled, request.isDefault);
            var res = await _repository.AddAsync(info);
            return res != null;
        }
    }

    /// <summary>
    /// 删除语言
    /// </summary>
    /// <param name="_repository"></param>
    /// <param name="mediator"></param>
    public class DeleteLanguageInfoCommandHandler(IPLanguageInfoRepository _repository, IMediator mediator, IRedisService _redis) : ICommandHandler<DeleteLanguageInfoCommand, bool>
    {
        /// <summary>
        /// 删除语言
        /// </summary>
        public async Task<bool> Handle(DeleteLanguageInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await _repository.GetLanguageInfoByCodeAsync(request.code);
            if (info == null)
                return false;

            if (info.IsDefault == IsDefaultEnum.Yes)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0003)]);

            await mediator.Publish(new DeleteLanguageDomainEvent(info));
            //info.DelRangeLanguageText(info.LanguageTextEntitys);
            //info.Delete();
            await _redis.DelKeyAsync(CacheConst.MultilingualAll);
            return await _repository.RemoveAsync(info);
        }
    }

    /// <summary>
    /// 更新语言
    /// </summary>
    /// <param name="_repository"></param>
    public class UpdateLanguageInfoCommandHandler(IPLanguageInfoRepository _repository, IRedisService _redis) : ICommandHandler<UpdateLanguageInfoCommand, bool>
    {
        /// <summary>
        /// 更新语言
        /// </summary>
        public async Task<bool> Handle(UpdateLanguageInfoCommand request, CancellationToken cancellationToken)
        {
            BasicUtils.CheckLanguageInfo(request.isDefault, request.isEnabled);
            var info = await _repository.GetLanguageInfoByCodeAsync(request.code);
            if (info == null)
                return false;
            await _redis.DelKeyAsync(CacheConst.MultilingualAll);
            info.Update(request.name, request.code, request.isEnabled, request.isDefault);
            var res = _repository.UpdateAsync(info);
            return res != null;
        }
    }
}
