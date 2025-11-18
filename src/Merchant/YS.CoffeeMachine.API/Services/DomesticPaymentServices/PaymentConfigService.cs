using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Application.Dtos.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Services.DomesticPaymentServices
{
    /// <summary>
    /// 微信支付宝支付配置获取工具类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="redisClient"></param>
    /// <param name="_environment"></param>
    public class PaymentConfigService(CoffeeMachineDbContext context, IRedisClient redisClient, IWebHostEnvironment _environment) : IWechatMerchantOptionsProvider, IAlipaySettingProvider
    {
        #region 获取微信支付宝商户配置

        /// <summary>
        /// 获取支付宝服务商配置
        /// </summary>
        /// <param name="serviceProviderId">支付的服务商表表的Id(SystemPaymentServiceProvider表Id)</param>
        /// <returns></returns>
        public async Task<IAlipaySetting> GetAlipaySetting(string serviceProviderId)
        {
            var paymentConfig = await GetPaymentConfig(Convert.ToInt64(serviceProviderId));
            return new AlipaySetting(appid: paymentConfig.AppletAppID, privateKey: paymentConfig.ApiV3Key!, publicKey: paymentConfig.AppKey!);
        }

        /// <summary>
        /// 获取微信商户配置
        /// </summary>
        /// <param name="serviceProviderId">支付的服务商表表的Id(SystemPaymentServiceProvider表Id)</param>
        /// <returns></returns>
        public async Task<MerchantOptions> GetWechatSetting(string serviceProviderId)
        {
            var paymentConfig = await GetPaymentConfig(Convert.ToInt64(serviceProviderId));

            return new MerchantOptions
            {
                ApiV2Key = paymentConfig.AppKey,
                ApiV3Key = paymentConfig.ApiV3Key,
                MerchantCertFileUrl = paymentConfig.CretFileUrl,
                MerchantCertPassWrod = paymentConfig.CretPassWrod,
                MerchantCertType = MerchantCertTypeEnum.FromFile,
                MerchantId = paymentConfig.SPMerchantId,
                WechatCertType = WechatCertTypeEnum.Platform
            };
        }

        /// <summary>
        /// 获取支付配置
        /// </summary>
        /// <param name="serviceProviderId">服务商Id</param>
        /// <returns></returns>
        public async Task<PaymentConfigDto> GetPaymentConfig(long serviceProviderId)
        {
#if DEBUG
            PaymentConfigDto cachedConfig = null;
#else
            var cacheKey = string.Format(CacheConst.SystemPaymentServiceProviderKey, serviceProviderId);
            var cachedConfig = await redisClient.GetAsync<PaymentConfigDto>(cacheKey);
#endif
            if (cachedConfig == null)
            {
                var paymentServiceProviderEntity = await context.SystemPaymentServiceProvider.FirstAsync(a => a.Id == serviceProviderId);

                if (paymentServiceProviderEntity == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0014)]);

                cachedConfig = new PaymentConfigDto()
                {
                    SPMerchantId = paymentServiceProviderEntity.SpMerchantId,
                    AppletAppID = paymentServiceProviderEntity.AppletAppID,
                    AppKey = paymentServiceProviderEntity.AppKey,
                    ApiV3Key = paymentServiceProviderEntity.ApiV3Key,
                    NotifyUrl = paymentServiceProviderEntity.NotifyUrl
                };

                // 获取wwwroot绝对路径
                string wwwrootPath = _environment.WebRootPath;

                // 拼接文件路径
                cachedConfig.CretFileUrl = Path.Combine(wwwrootPath, "cert", "apiclient_cert.p12");
#if DEBUG
#else
                // 缓存
                await redisClient.SetAsync(cacheKey, cachedConfig, TimeSpan.FromHours(1));
#endif
            }

            return cachedConfig;
        }
        #endregion
    }
}