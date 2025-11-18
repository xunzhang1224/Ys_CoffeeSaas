using DotNetCore.CAP;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Services.SmsServices;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.IServices;
using YS.Provider.OSS.Services;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 短信通知
    /// </summary>
    /// <param name="_email"></param>
    /// <param name="_redis"></param>
    /// <param name="_db"></param>
    /// <param name="_log"></param>
    public class SmsSubscriber(IRedisService _redis, ILogger<EmailSubscriber> _log,
        IAliyunSmsService aliyunSmsService) : ICapSubscribe
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.Sms)]
        public async Task Handle(SmsDto input)
        {
            var smsdic = input.MessageBodyDic.FirstOrDefault();
            try
            {
                input.PhoneNumbers.ForEach(async x => await aliyunSmsService.SendSmsAsync(x, smsdic.Key, smsdic.Value));
                _log.LogInformation($"短信发送成功：设备{input.DeviceId}，收件人{JsonConvert.SerializeObject(input.PhoneNumbers)}");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "处理短信通知失败");

                // 发送失败时删除缓存键，允许重试
                var cacheKey = GetEmailCacheKey(input.DeviceId, smsdic.Value);
                await _redis.DelKeyAsync(cacheKey);
                throw;
            }
        }

        private string GetEmailCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Email, deviceId, messageHash);
        }
    }
}
