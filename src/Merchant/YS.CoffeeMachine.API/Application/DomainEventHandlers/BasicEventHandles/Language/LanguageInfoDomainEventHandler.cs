using AutoMapper;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.Events.BasicDomainEvents;
using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
using YS.CoffeeMachine.Domain.IRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.API.Application.DomainEventHandlers.BasicEventHandles.Language
{
    /// <summary>
    /// 设置默认语种
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_languageInfoRepository"></param>
    public class SetDefaultLanguageDomainEventHandler(CoffeeMachineDbContext context,
        IMapper mapper,
        IRedisClient _redisClient,
        ILanguageInfoRepository _languageInfoRepository) : IDomainEventHandler<SetDefaultLanguageDomainEvent>
    {
        /// <summary>
        /// 设置默认语种
        /// </summary>
        public async Task Handle(SetDefaultLanguageDomainEvent notification, CancellationToken cancellationToken)
        {
            var datas = await context.LanguageInfo
                .Where(s => s.IsDefault == IsDefaultEnum.Yes && s.Code != notification.lang.Code)
                .ToListAsync();
            if (datas != null && datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    item.SetNotDefaultLanguage();
                }
                await _languageInfoRepository.UpdateRangeAsync(datas);
            }
        }
    }

    ///// <summary>
    ///// 创建语言
    ///// </summary>
    ///// <param name="mapper"></param>
    ///// <param name="_redisClient"></param>
    //public class CreateLanguageDomainEventHandler(IMapper mapper,
    //    IRedisClient _redisClient) : IDomainEventHandler<CreateLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 创建语言
    //    /// </summary>
    //    public async Task Handle(CreateLanguageDomainEvent notification, CancellationToken cancellationToken)
    //    {
    //        var info = mapper.Map<LanguageInfoDto>(notification.lang);
    //        await _redisClient.HSetAsync(CacheConst.MultilingualAll, notification.lang.Code, info);
    //        await _redisClient.ExpireAsync(CacheConst.MultilingualAll, TimeSpan.MaxValue);
    //    }
    //}

    ///// <summary>
    ///// 更新语言
    ///// </summary>
    ///// <param name="mapper"></param>
    ///// <param name="_redisClient"></param>
    //public class UpdateLanguageDomainEventHandler(IMapper mapper,
    //    IRedisClient _redisClient) : IDomainEventHandler<UpdateLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 更新语言
    //    /// </summary>
    //    public async Task Handle(UpdateLanguageDomainEvent notification, CancellationToken cancellationToken)
    //    {
    //        await _redisClient.HSetAsync(CacheConst.MultilingualAll, notification.lang.Code, mapper.Map<LanguageInfoDto>(notification.lang));
    //    }
    //}

    ///// <summary>
    ///// 删除语言
    ///// </summary>
    ///// <param name="_redisClient"></param>
    ///// <param name="_repository"></param>
    //public class DeleteLanguageDomainEventHandler(IRedisClient _redisClient,
    //    ILanguageInfoRepository _repository) : IDomainEventHandler<DeleteLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 删除语言
    //    /// </summary>
    //    public async Task Handle(DeleteLanguageDomainEvent notification, CancellationToken cancellationToken)
    //    {
    //        // 删除语种缓存
    //        await _redisClient.HDelAsync(CacheConst.MultilingualAll, notification.lang.Code);
    //        // 删除语种对应的语言文本缓存
    //        var key = string.Format(CacheConst.Multilingual, notification.lang.Code);
    //        await _redisClient.DelAsync(key);
    //    }
    //}
}
