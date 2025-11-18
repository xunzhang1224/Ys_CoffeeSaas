using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShardingCore.Extensions.ShardingQueryableExtensions;
using YS.CoffeeMachine.Application.Commands.PaymentCommands;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Pay.SDK;
using YS.Pay.SDK.Response;
using YS.Pay.SDK.ServicePlatform.Request;
using YS.Pay.SDK.Top;
using YSCore.Base.App;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers
{
    /// <summary>
    /// 支付平台回调命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="_publish"></param>
    /// <param name="logger"></param>
    public class PaymentCallbackCommandHandler(CoffeeMachineDbContext context, IConfiguration configuration, IPublishService _publish, ILogger<PaymentCallbackCommandHandler> logger) : ICommandHandler<PaymentCallbackCommand, PublicResponse>
    {
        /// <summary>
        /// 支付平台回调
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PublicResponse> Handle([FromForm] PaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.content))
                return new PublicResponse()
                {
                    ErrCode = "400",
                    ErrMsg = "内容不能为空"
                };

            var httpContext = AppHelper.HttpContext ?? throw ExceptionHelper.AppFriendly("HttpContext不能为空");
            #region 验签
            var app_id = httpContext.Request.Query["app_id"].ToString();
            //加验证appid
            if (string.IsNullOrWhiteSpace(app_id))
                return new PublicResponse()
                {
                    ErrCode = "400",
                    ErrMsg = "签名内容缺失"
                };

            bool success = long.TryParse(app_id, out long appid);
            if (!success)
                return new PublicResponse()
                {
                    ErrCode = "400",
                    ErrMsg = "不可用的Appid"
                };

            var appId = long.Parse(configuration["YsPaymentPlatform:AppId"] ?? "0");
            if (appid != appId)
                return new PublicResponse()
                {
                    ErrCode = "400",
                    ErrMsg = "非本程序的Appid"
                };

            var parameters = new TopDictionary()
                            {
                                { "RequestTopic", request.requestTopic },
                                { "Content", request.content }
                            };
            SignatureVerificationUnit.Verification(httpContext, appId, configuration["YsPaymentPlatform:AppKey"] ?? "", parameters);
            #endregion

            if (request.requestTopic == Domain.Shared.Enum.RequestTopicEnum.MerchantIncomingChanges)
            {
                await MerchantIncomingChangesAsync(request.content);
            }

            // 根据请求主题分发处理
            return request.requestTopic switch
            {
                Domain.Shared.Enum.RequestTopicEnum.MerchantIncomingChanges => await MerchantIncomingChangesAsync(request.content),
                //Domain.Shared.Enum.RequestTopicEnum.OnlineOrderPaymentSuccessful=>
            };
        }

        /// <summary>
        /// 商户进件状态回调处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<PublicResponse> MerchantIncomingChangesAsync(string content)
        {
            logger.LogInformation($"商户进件状态回调 content: {content}");
            try
            {
                // 反序列化回调内容
                MerchantIncomingChangesDto requestDto = JsonConvert.DeserializeObject<MerchantIncomingChangesDto>(content) ?? null;
                if (requestDto == null)
                {
                    var result = new PublicResponse()
                    {
                        ErrCode = "400",
                        ErrMsg = "无效的请求内容"
                    };
                    logger.LogError(JsonConvert.SerializeObject(result));
                    return result;
                }

                // CAP发布商户进件状态变更消息
                await _publish.SendMessage(CapConst.MerchantIncomingCallback, requestDto);

                // 返回成功响应
                return new PublicResponse();
            }
            catch (Exception ex)
            {
                var result = new PublicResponse
                {
                    ErrCode = "500",
                    ErrMsg = ex.Message
                };
                logger.LogError(JsonConvert.SerializeObject(result));
                return result;
            }
        }

        /// <summary>
        /// 在线支付成功回调处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<PublicResponse> OnlineOrderPaymentSuccessAsync(string content)
        {
            logger.LogInformation($"在线支付成功回调 content: {content}");
            try
            {
                OnlineOrderPaymentSuccessfulDto requestDto = JsonConvert.DeserializeObject<OnlineOrderPaymentSuccessfulDto>(content);
                if (requestDto == null)
                {
                    var result = new PublicResponse
                    {
                        ErrCode = "400",
                        ErrMsg = "无效的请求内容"
                    };
                    logger.LogError(JsonConvert.SerializeObject(result));
                    return result;
                }

                // CAP发布在线支付成功消息
                await _publish.SendMessage(CapConst.OnlineOrderPaymentSuccess, requestDto);

                // 返回成功响应
                return new PublicResponse();
            }
            catch (Exception ex)
            {
                var result = new PublicResponse
                {
                    ErrCode = "500",
                    ErrMsg = ex.Message
                };
                logger.LogError(JsonConvert.SerializeObject(result));
                return result;
            }
        }
    }
}