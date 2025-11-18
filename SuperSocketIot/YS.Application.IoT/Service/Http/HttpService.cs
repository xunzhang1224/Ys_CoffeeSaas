
using Flurl.Http;
using Flurl.Http.Configuration;
using Polly;
using Polly.Timeout;
using System.Text.Json;
using YS.Application.IoT.Service.Http.Dto;
using YS.Application.IoT.Wrapper;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.Domain.IoT.Model;
using YS.Domain.IoT.Util;

namespace YS.Application.IoT.Service.Http
{
    /// <summary>
    /// 要增加重试机制
    /// </summary>
    public class HttpService : IHttp
    {
        private static readonly string _GetVendSecretUrl = AppSettingsHelper.GetContent("IHttp", "Vend", "GetVendSecret");
        private static readonly string _SetVendLineStatusUrl = AppSettingsHelper.GetContent("IHttp", "Vend", "SetVendLineStatus");
        private static readonly string _TestUrl = AppSettingsHelper.GetContent("IHttp", "Vend", "Test");
        private readonly ILogger<HttpService> _logger;
        private readonly IFlurlClient _flurlCli;

        /// <summary>
        /// 创建HttpService
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="logger"></param>
        public HttpService(IFlurlClientCache clients,
            ILogger<HttpService> logger)
        {
            _flurlCli = clients.Get("CoreIntegration");
            _logger = logger;
        }

        /// <summary>
        /// 设置设备线状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SetVendLineStatus(VendLineStatusInput input)
        {
            try
            {
                var vendCode = input.VendCode.ToInt64();//查看设备号是否合法
                if (vendCode != 0)
                {

                    // 创建重试策略
                    var retryPolicy = Policy
                        .Handle<FlurlHttpException>() // 捕获所有 Flurl.Http 异常
                        .Or<TimeoutRejectedException>() // 如果需要处理超时异常
                        .WaitAndRetryAsync(3, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 指数退避算法
                            onRetry: (outcome, timespan, retryCount, context) =>
                            {
                                _logger.LogWarning($"第 {retryCount} 次重试：{outcome.Message}");
                            }
                        );
                    var url = $"{_SetVendLineStatusUrl}?vendCode={input.VendCode}&lineStatus={input.LineStatus}";
                    // 使用重试策略执行请求
                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        await _flurlCli.Request(url).GetStringAsync();
                    });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"修改设备在线状态失败{ex.ToString()}");
                return false;
            }
        }

        /// <summary>
        /// 获取设备加密信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<VendInfoForMqttDto> GetVendSecret(string mid)
        {
            var cacheKey = CacheConst.VendBaciInfo.ToFormat(mid);
            if (await RedisHelper.GetAsync<VendInfoForMqttDto>(cacheKey) is VendInfoForMqttDto cachedResult)
            {
                return cachedResult;
            }
            try
            {
                // 创建重试策略
                var retryPolicy = Policy
                    .Handle<FlurlHttpException>() // 捕获所有 Flurl.Http 异常
                    .Or<TimeoutRejectedException>() // 如果需要处理超时异常
                    .WaitAndRetryAsync(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 指数退避算法
                        onRetry: (outcome, timespan, retryCount, context) =>
                        {
                            _logger.LogWarning($"第 {retryCount} 次重试：{outcome.Message}");
                        }
                    );
                var url = $"{_GetVendSecretUrl}?vendCode={mid}";
                // 使用重试策略执行请求
                string body = string.Empty;
                await retryPolicy.ExecuteAsync(async () =>
                {
                    body = await _flurlCli.Request(url).GetStringAsync();
                });

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var vendSecret = JsonSerializer.Deserialize<UnifyResult<VendInfoForMqttDto>>(body, options);

                if (vendSecret?.Code == 200 && vendSecret.Result != null)
                {
                    await RedisHelper.SetAsync(cacheKey, vendSecret.Result, TimeSpan.FromHours(8));
                    return vendSecret.Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取加密信息失败{ex.ToString()}");
            }
            return null;
        }

        /// <summary>
        /// 获取设备私钥
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<VendToken> GetPrivKey(string mid)
        {
            //var key = await GetVendSecret(mid);
            var key = await GrpcWrapp.Instance.GrpcCommandService.GetDeviceInitByMidAsync(mid);
            if (key == null||string.IsNullOrWhiteSpace(key.Mid)|| string.IsNullOrWhiteSpace(key.PriKey))
            {
                return null;
            }
            else
            {
                var token = new VendToken
                {
                    PrivaKey = key.PriKey,
                    PubKey = SignUtil.MyMd5($"{mid}@@@{key.PubKey}").ToLower(),
                };
                return token;
            }
        }

        /// <summary>
        /// 测试http通信
        /// </summary>
        /// <returns></returns>
        public async Task<string> TestAsync()
        {
            try
            {
                var url = $"{_TestUrl}";
                var body = await _flurlCli.Request(url).GetStringAsync();
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"http通信测试失败{ex.ToString()}");
                return string.Empty;
            }

        }
    }
}
