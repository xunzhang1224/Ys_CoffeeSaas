//using FreeRedis;
//using MediatR;
//using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
//using YS.CoffeeMachine.Domain.Shared.Const;
//using YSCore.Base.DomainEvent;

//namespace YS.CoffeeMachine.API.Application.DomainEventHandlers.BasicEventHandles.Language
//{
//    /// <summary>
//    /// 删除语言文本
//    /// </summary>
//    /// <param name="mediator"></param>
//    /// <param name="_redisClient"></param>
//    public class DeleteLanguageTextDomainEventHandler(IMediator mediator, IRedisClient _redisClient) : IDomainEventHandler<DeleteLanguageTextDomainEvent>
//    {
//        /// <summary>
//        /// 删除语言文本
//        /// </summary>
//        public async Task Handle(DeleteLanguageTextDomainEvent notification, CancellationToken cancellationToken)
//        {
//            string cachekey = string.Format(CacheConst.Multilingual, notification.entitie.LangCode);
//            await _redisClient.HDelAsync(cachekey, notification.entitie.Code);
//        }
//    }

//    /// <summary>
//    /// 批量删除语言文本
//    /// </summary>
//    /// <param name="_redisClient"></param>
//    public class DelRangeLanguageTextDomainEventHandler(IRedisClient _redisClient) : IDomainEventHandler<DelRangeLanguageTextDomainEvent>
//    {
//        /// <summary>
//        /// 批量删除语言文本
//        /// </summary>
//        public async Task Handle(DelRangeLanguageTextDomainEvent notification, CancellationToken cancellationToken)
//        {
//            var fieldsToDelete = new List<string>();
//            string cachekey = string.Format(CacheConst.Multilingual, notification.entities.First().LangCode);
//            foreach (var item in notification.entities)
//            {
//                fieldsToDelete.Add(cachekey);
//            }
//            await _redisClient.HDelAsync(cachekey, fieldsToDelete.ToArray());
//        }
//    }

//    /// <summary>
//    /// 新增语言文本
//    /// </summary>
//    /// <param name="mediator"></param>
//    /// <param name="_redisClient"></param>
//    public class CreateLanguageTextDomainEventHandler(IMediator mediator, IRedisClient _redisClient) : IDomainEventHandler<CreateLanguageTextDomainEvent>
//    {
//        /// <summary>
//        /// 新增语言文本
//        /// </summary>
//        public async Task Handle(CreateLanguageTextDomainEvent notification, CancellationToken cancellationToken)
//        {
//            string cachekey = string.Format(CacheConst.Multilingual, notification.entitie.LangCode);
//            await _redisClient.HSetAsync(cachekey, notification.entitie.Code, notification.entitie.Value);
//            await _redisClient.ExpireAsync(cachekey, TimeSpan.Zero);
//        }
//    }

//    /// <summary>
//    /// 批量新增语言文本
//    /// </summary>
//    /// <param name="_redisClient"></param>
//    public class AddRangeLanguageTextDomainEventHandler(IRedisClient _redisClient) : IDomainEventHandler<AddRangeLanguageTextDomainEvent>
//    {
//        /// <summary>
//        /// 批量新增语言文本
//        /// </summary>
//        public async Task Handle(AddRangeLanguageTextDomainEvent notification, CancellationToken cancellationToken)
//        {
//            foreach (var entity in notification.entities)
//            {
//                string cachekey = string.Format(CacheConst.Multilingual, entity.LangCode);
//                await _redisClient.HSetAsync(cachekey, entity.Code, entity.Value);
//                await _redisClient.ExpireAsync(cachekey, TimeSpan.MaxValue);
//            }
//        }
//    }
//}
