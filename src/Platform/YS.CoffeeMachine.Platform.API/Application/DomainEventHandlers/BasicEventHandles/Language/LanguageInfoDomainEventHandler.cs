using AutoMapper;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Platform.API.Application.DomainEventHandlers.BasicEventHandles.Language
{
    /// <summary>
    /// 设置默认语种
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_languageInfoRepository"></param>
    public class SetDefaultLanguageDomainEventHandler(CoffeeMachinePlatformDbContext context,
        IMapper mapper,
        IRedisClient _redisClient,
        IPLanguageInfoRepository _languageInfoRepository) : IDomainEventHandler<SetDefaultLanguageDomainEvent>
    {
        /// <summary>
        /// 设置默认语种
        /// </summary>
        public async Task Handle(SetDefaultLanguageDomainEvent notification, CancellationToken cancellationToken)
        {
            var datas = await context.LanguageInfo
                .Where(s => s.IsDefault == IsDefaultEnum.Yes && s.Code != notification.lang.Code)
                .ToListAsync();

            var dic = new Dictionary<string, LanguageInfoDto>();
            if (datas != null && datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    item.SetNotDefaultLanguage();
                    dic.Add(item.Code, mapper.Map<LanguageInfoDto>(item));
                    //item.Update(item.Name, item.Code, item.IsEnabled, IsDefaultEnum.No);
                }
                await _languageInfoRepository.UpdateRangeAsync(datas);
            }

            //await _redisClient.SetAsync(CacheConst.DefaultLanguage, notification.lang.Code);
            //await _redisClient.ExpireAsync(CacheConst.DefaultLanguage, TimeSpan.MaxValue);
            //if (dic.Count > 0)
            //    await _redisClient.HMSetAsync(CacheConst.MultilingualAll, dic);// 取消缓存中其他默认的语种
        }
    }
    ///// <summary>
    ///// 创建语种
    ///// </summary>
    ///// <param name="mapper"></param>
    ///// <param name="_redisClient"></param>
    //public class CreateLanguageDomainEventHandler(IMapper mapper,
    //    IRedisClient _redisClient) : IDomainEventHandler<CreateLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 创建语种
    //    /// </summary>
    //    public async Task Handle(CreateLanguageDomainEvent notification, CancellationToken cancellationToken)
    //    {
    //        var info = mapper.Map<LanguageInfoDto>(notification.lang);
    //        await _redisClient.HSetAsync(CacheConst.MultilingualAll, notification.lang.Code, info);
    //        await _redisClient.ExpireAsync(CacheConst.MultilingualAll, TimeSpan.MaxValue);
    //    }
    //}
    ///// <summary>
    ///// 更新语种
    ///// </summary>
    ///// <param name="mapper"></param>
    ///// <param name="_redisClient"></param>
    //public class UpdateLanguageDomainEventHandler(IMapper mapper,
    //    IRedisClient _redisClient) : IDomainEventHandler<UpdateLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 更新语种
    //    /// </summary>
    //    public async Task Handle(UpdateLanguageDomainEvent notification, CancellationToken cancellationToken)
    //    {
    //        await _redisClient.HSetAsync(CacheConst.MultilingualAll, notification.lang.Code, mapper.Map<LanguageInfoDto>(notification.lang));
    //    }
    //}
    ///// <summary>
    ///// 删除语种
    ///// </summary>
    ///// <param name="_redisClient"></param>
    //public class DeleteLanguageDomainEventHandler(IRedisClient _redisClient) : IDomainEventHandler<DeleteLanguageDomainEvent>
    //{
    //    /// <summary>
    //    /// 删除语种
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
