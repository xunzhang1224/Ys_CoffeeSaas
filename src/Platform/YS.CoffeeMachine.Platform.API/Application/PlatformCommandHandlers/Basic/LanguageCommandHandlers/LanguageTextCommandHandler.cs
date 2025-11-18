using MediatR;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.IRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.Base.DatabaseAccessor;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic.LanguageCommandHandlers
{
    /// <summary>
    /// 添加语言
    /// </summary>
    /// <param name="_repository"></param>
    public class CreateSysLanguageTextCommandHandler(IPLanguageInfoRepository _repository, IRedisService _redis) : ICommandHandler<CreateLanguageTextCommand>
    {
        /// <summary>
        /// 添加语言
        /// </summary>
        public async Task Handle(CreateLanguageTextCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.languageValue)
            {
                var lang = await _repository.GetLanguageInfoByCodeAsync(item.Key);
                if (lang == null)
                {
                    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0003)], item.Key));
                }
                await _redis.DelKeyAsync(CacheConst.MultilingualAll);
                var text = new LanguageTextEntity(request.code, item.Value, item.Key);
                lang.AddRangeLanguageText(new List<LanguageTextEntity>() { text });
            }
        }
    }

    /// <summary>
    /// 删除语言
    /// </summary>
    /// <param name="_repository"></param>
    public class DeleteSysLanguageTextCommandHandler(IPLanguageInfoRepository _repository, IRedisService _redis) : ICommandHandler<DeleteLanguageTextCommand>
    {
        /// <summary>
        /// 删除语言
        /// </summary>
        public async Task Handle(DeleteLanguageTextCommand request, CancellationToken cancellationToken)
        {
            var res = await _repository.GetLanguageInfosByTextCodeAsync(request.code);
            if (res != null && res.Count > 0)
            {
                await _redis.DelKeyAsync(CacheConst.MultilingualAll);
                foreach (var item in res)
                {
                    item.DelRangeLanguageText(item.LanguageTextEntitys.ToList());
                }
            }
        }
    }

    /// <summary>
    /// 更新语言
    /// </summary>
    /// <param name="_repository"></param>
    public class ImportSysLanguageTextCommandHandler(IPLanguageInfoRepository _repository, IRedisService _redis) : ICommandHandler<ImportSysLanguageTextCommand>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(ImportSysLanguageTextCommand request, CancellationToken cancellationToken)
        {
            var lang = await _repository.GetLanguageInfoByCodeAsync(request.LangCode);
            if (lang == null)
            {
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0003)], request.LangCode));
            }
            var datas = new List<LanguageTextEntity>();
            foreach (var item in request.dic)
            {
                datas.Add(new LanguageTextEntity(item.Key, item.Value, request.LangCode));
            }
            await _redis.DelKeyAsync(CacheConst.MultilingualAll);
            lang.AddRangeLanguageText(datas);
        }
    }
}
