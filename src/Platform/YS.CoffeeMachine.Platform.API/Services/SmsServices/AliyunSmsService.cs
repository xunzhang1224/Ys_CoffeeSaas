using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Microsoft.Extensions.Options;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.IServices;
using YS.Provider.OSS;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.Platform.API.Services.SmsServices
{
    /// <summary>
    /// 阿里云短信服务
    /// </summary>
    public class AliyunSmsService : IAliyunSmsService
    {
        private readonly OSSOptions _options;
        private readonly Client _client;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public AliyunSmsService(IOptions<OSSOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_options.AccessKey) || string.IsNullOrWhiteSpace(_options.SecretKey))
                throw ExceptionHelper.AppFriendly("阿里云短信服务配置错误");

            var config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                AccessKeyId = _options.AccessKey,
                AccessKeySecret = _options.SecretKey,
                Endpoint = "dysmsapi.aliyuncs.com"
            };
            _client = new Client(config);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="templateCode"></param>
        /// <param name="paramJson"></param>
        /// <returns></returns>
        public async Task<(bool, string)> SendSmsAsync(string phoneNumber, string templateCode, string paramJson)
        {
            // 组装验证码参数
            var sendSmsRequest = new SendSmsRequest
            {
                PhoneNumbers = phoneNumber, // 待发送手机号, 多个以逗号分隔
                SignName = "湖南云数信息科技有限公司", // 短信签名
                TemplateCode = templateCode, // 短信模板
                TemplateParam = paramJson, // 模板中的变量替换JSON串
                OutId = YitIdHelper.NextId().ToString()
            };

            // 发送短信
            var sendSmsResponse = await _client.SendSmsAsync(sendSmsRequest);

            return ((sendSmsResponse.Body.Code == "OK", sendSmsResponse.Body.Message));

            //if (sendSmsResponse.Body.Code != "OK" && sendSmsResponse.Body.Message != "OK")
            //{
            //    //throw ExceptionHelper.AppFriendly("短信发送失败");
            //}
        }
    }
}
