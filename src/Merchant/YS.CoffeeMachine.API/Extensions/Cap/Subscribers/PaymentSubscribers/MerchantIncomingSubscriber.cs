using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Pay.SDK.ServicePlatform.Request;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers.PaymentSubscribers
{
    /// <summary>
    /// 商户进件回调订阅
    /// </summary>
    public class MerchantIncomingSubscriber(CoffeeMachineDbContext context) : ICapSubscribe
    {
        /// <summary>
        /// 商户进件回调订阅
        /// </summary>
        [CapSubscribe(CapConst.MerchantIncomingCallback)]
        public async Task Handle(MerchantIncomingChangesDto input)
        {
            var paymentInfo = await context.PaymentConfig.AsQueryable().Where(a => a.MerchantCode == input.MerchantId && a.Email == input.Email).FirstOrDefaultAsync();
            if (paymentInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            paymentInfo.UpdateStatue((PaymentConfigStatueEnum)input.OnboardingStatus);
        }
    }
}