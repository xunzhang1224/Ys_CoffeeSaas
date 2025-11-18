using DotNetCore.CAP;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 邮件通知
    /// </summary>
    /// <param name="_email"></param>
    /// <param name="_redis"></param>
    /// <param name="_log"></param>
    public class EmailSubscriber(IEmailServiceProvider _email, IRedisService _redis, ILogger<EmailSubscriber> _log) : ICapSubscribe
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.Email)]
        public async Task Handle(EmailDto input)
        {
            try
            {
                var emailInfo = new EmailObject
                {
                    ToEmail = input.ToEmail,
                    MessageBody = input.MessageBody,
                    Subject = input.Subject,
                };

                await _email.SendEmailBatchAsync(emailInfo);

                _log.LogInformation($"邮件发送成功：设备{input.DeviceId}，收件人{input.ToEmail}");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "处理邮件通知失败");

                // 发送失败时删除缓存键，允许重试
                var cacheKey = GetEmailCacheKey(input.DeviceId, input.MessageBody);
                await _redis.DelKeyAsync(cacheKey);
                throw;
            }
        }

        private string GetEmailCacheKey(string deviceId, string messageBody)
        {
            var messageHash = YS.Util.Core.Util.GetMD5Hash(messageBody);
            return string.Format(CacheConst.Email, deviceId, messageHash);
        }
        ///// <summary>
        ///// 邮件通知
        ///// </summary>
        //[CapSubscribe(CapConst.Email)]
        //public async Task Handle(EmailDto input)
        //{
        //    var jg = _cfg["Email:SendingInterval"];
        //    var md5 = YS.Util.Core.Util.GetMD5Hash(input.MessageBody);
        //    var key = string.Format(CacheConst.Email, input.DeviceId, md5);
        //    if (int.TryParse(jg, out int day))
        //    {
        //        // 判断是否已经通知
        //        if (await _redis.SetNxAsync(key, day * 24 * 3600))
        //        {
        //            var info = new EmailObject()
        //            {
        //                ToEmail = input.ToEmail,
        //                MessageBody = input.MessageBody,
        //                Subject = input.Subject,
        //            };
        //            await _email.SendEmailBatchAsync(info);
        //            foreach (var email in input.ToEmail.Split(','))
        //            {
        //                var msg = new NotityMsg(input.Subject, input.Type, email, input.MessageBody, input.EnterpriseinfoId);
        //                await _db.AddAsync(msg);
        //            }
        //            await _db.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            _log.LogInformation($"邮件服务分布式锁过滤消息：内容【{input.MessageBody}】,邮箱：【{input.ToEmail}】");
        //        }
        //    }
        //}
    }
}
