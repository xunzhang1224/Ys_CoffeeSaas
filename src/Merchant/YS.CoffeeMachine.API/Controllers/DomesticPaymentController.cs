using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V3;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos.PaymentEventSubscribe;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.DivideAccountsConfigCommands;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.WechatAlipayPaymentCommand;
using YS.CoffeeMachine.Application.Dtos.DomesticPayment;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.DivideAccountsConfigDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YSCore.Base.UnifyResult.Attributes;
using static YS.Cabinet.Payment.WechatPay.Application.V3.Responses.Common.WxBranchBankResponse;
using static YS.Cabinet.Payment.WechatPay.V3.WxBankResponse;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 国内支付控制器
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="systemPaymentInfoQueries"></param>
    /// <param name="divideAccountsConfigQueries"></param>
    /// <param name="wechatPaymentQueries"></param>
    /// <param name="orderInfoQueries"></param>
    /// <param name="_paymentPlatformUtil"></param>
    /// <param name="_logger"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="alipayService"></param>
    /// <param name="_publish"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class DomesticPaymentController(
        IMediator mediator,
        ISystemPaymentInfoQueries systemPaymentInfoQueries,
        IDivideAccountsConfigQueries divideAccountsConfigQueries,
        IWechatPaymentQueries wechatPaymentQueries,
        IOrderInfoQueries orderInfoQueries,
        PaymentPlatformUtil _paymentPlatformUtil,
        ILogger<DomesticPaymentController> _logger,
        IWechatMerchantService _wechatMerchantService,
        IAlipayService alipayService,
        IPublishService _publish) : Controller
    {
        #region 进件相关

        /// <summary>
        /// 验证商户Id是否存在
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("VerifyMerchantId")]
        public async Task<bool> VerifyMerchantId([FromBody] VerifyMerchantIdCommand command) => await mediator.Send(command);

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("MerchantOnboardingSendPhoneCode")]
        public async Task<bool> SendPhoneCode([FromBody] MerchantOnboardingSendPhoneCodeCommand command) => await mediator.Send(command);

        /// <summary>
        /// 添加二级商户支付方式
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("InsertMerchantPaymentMethod")]
        public async Task<bool> InsertMerchantPaymentMethod([FromBody] InsertMerchantPaymentMethodCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更新支付方式备注信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ModifyMerchantPaymentMethodRemark")]
        public async Task<bool> ModifyMerchantPaymentMethodRemarkAsync([FromBody] ModifyMerchantPaymentMethodRemarkCommand command) => await mediator.Send(command);

        /// <summary>
        /// 更改二级商户支付方式状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("ModifyPaymentMethodStatus")]
        public async Task<bool> ModifyPaymentMethodStatusAsync([FromBody] ModifyPaymentMethodStatusCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除二级商户支付方式
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeletePaymentMethod")]
        public async Task<bool> DeletePaymentMethodAsync([FromBody] DeletePaymentMethodCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取系统支付方式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSystemPaymentMethods")]
        public async Task<List<SystemPaymentMethodDto>> GetSystemPaymentMethodsAsync() => await systemPaymentInfoQueries.GetSystemPaymentMethodsAsync();

        /// <summary>
        /// 获取二级商户支付方式分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetMachinePaymentMethods")]
        public async Task<PagedResultDto<M_PaymentMethodDto>> GetMachinePaymentMethodsAsync([FromBody] M_PaymentMethodInput input) => await systemPaymentInfoQueries.GetMachinePaymentMethodsAsync(input);

        /// <summary>
        /// 微信进件信息提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("WechatApplymentSubmit")]
        public async Task<bool> WechatApplymentAsync([FromBody] WechatApplymentCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取微信进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetWechatApplymentsById")]
        public async Task<M_PaymentWechatApplymentsOutput> GetWechatApplymentsByIdAsync(long id) => await systemPaymentInfoQueries.GetWechatApplymentsByIdAsync(id);

        /// <summary>
        /// 阿里进件信息提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AlipayApplymentSubmit")]
        public async Task<bool> AlipayApplymentAsync([FromBody] AlipayApplymentCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取支付宝进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAlipayApplymentsById")]
        public async Task<M_PaymentAlipayApplymentsOutput> GetAlipayApplymentsByIdAsync(long id) => await systemPaymentInfoQueries.GetAlipayApplymentsByIdAsync(id);

        /// <summary>
        /// 当前支付方式未绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetPaymentMethodUnBindDevices")]
        public async Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodUnBindDevicesAsync([FromBody] PaymentMethodBindDeviceInput input) => await systemPaymentInfoQueries.GetPaymentMethodUnBindDevicesAsync(input);

        /// <summary>
        /// 当前支付方式绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetPaymentMethodBindDevices")]
        public async Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodBindDevicesAsync([FromBody] PaymentMethodBindDeviceInput input) => await systemPaymentInfoQueries.GetPaymentMethodBindDevicesAsync(input);

        /// <summary>
        /// 商户支付方式与设备绑定/解绑操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PaymentMethodBindDevices")]
        public async Task<List<DeviceBindPaymentMethodDto>> PaymentMethodBindDevicesAsync([FromBody] PaymentMethodBindDevicesCommand command) => await mediator.Send(command);

        /// <summary>
        /// 设备绑定的支付信息查询
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        [HttpPost("GetDevicesBindPaymentMethod")]
        public async Task<Dictionary<string, string>> GetDevicesBindPaymentMethodAsync([FromBody] List<string> mids) => await systemPaymentInfoQueries.GetDevicesBindPaymentMethodAsync(mids);
        #endregion

        #region 微信支付相关查询

        /// <summary>
        /// 根据银行账号获取银行信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("GetWechatBankByAccount")]
        public async Task<List<WxBanksData>> GetBankByAccount(string account) => await wechatPaymentQueries.GetBankByAccount(account);

        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <param name="type">银行列表类型(1=对公 2=对私)</param>
        /// <returns></returns>
        [HttpGet("GetWechatBankList")]
        public async Task<List<WxBanksData>> GetBankList(int type = 1) => await wechatPaymentQueries.GetBankList(type);

        /// <summary>
        /// 获取银行支行列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetBankBranchList")]
        public async Task<List<BankBranchItem>> GetBankBranchList([FromBody] GetBankBranchListInput input) => await wechatPaymentQueries.GetBankBranchList(input);

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWechatProvinces")]
        public async Task<List<GetProvinceCityOutput>> GetWechatProvinces() => await wechatPaymentQueries.GetProvinces();

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("GetWechatCitys")]
        public async Task<List<GetProvinceCityOutput>> GetWechatCitys(long parentId) => await wechatPaymentQueries.GetCitys(parentId);

        /// <summary>
        /// 获取区县列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("GetWechatAreas")]
        public async Task<List<GetProvinceCityOutput>> GetWechatAreas(long parentId) => await wechatPaymentQueries.GetAreas(parentId);
        #endregion

        #region 发起支付

        /// <summary>
        /// 发起二维码支付-创建订单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("CreateNativeOrder")]
        public async Task<CreateNativeOrderOutput> CreateOrder([FromBody] CreateOrderCommand command) => await mediator.Send(command);
        #endregion

        #region 微信异步回调

        /// <summary>
        /// 微信异步回调
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [NonUnify]
        [AllowAnonymous]
        [HttpPost("WechatCallBack/{serviceId}")]
        public async Task<WechatCallbackRespone> WechatCallBack([FromRoute] long serviceId)
        {
            _logger.LogError($"接收到微信回调-{DateTime.Now}");
            // 判断服务商Id是否存在
            if (serviceId <= 0)
            {
                string errMsg = "微信异步回调]服务商Id无效";
                _logger.LogError(errMsg);
                return new WechatCallbackRespone("FAIL", errMsg);
            }

            var serviceProvider = await _paymentPlatformUtil.GetServiceProviderAsync(serviceId);
            if (serviceProvider == null)
            {
                string errMsg = $"[微信异步回调]服务商Id:{serviceId}不存在";
                _logger.LogError(errMsg);
                return new WechatCallbackRespone("FAIL", errMsg);
            }

            /*
             * 验证签名并且解密回调内容
             * result 为true表示签名验证通过并且解密成功
             * msg 为验证结果信息
             * model 为解密后的回调内容
             */
            try
            {
                var (result, msg, model) = await _wechatMerchantService
                      .BuildMerchant(serviceId.ToString())
                      .CheckSignatureAndDecrypt(HttpContext.Request);

                if (!result)
                {
                    _logger.LogError($"[微信异步回调]验签不通过:{msg},数据包:{JsonConvert.SerializeObject(new WxVerifySignature(HttpContext.Request))}");
                    return new WechatCallbackRespone("FAIL", msg);
                }
                _logger.LogInformation("[微信异步回调]收到参数:" + JsonConvert.SerializeObject(model));

                switch (model.EventType)
                {
                    case WxCallBackEventTypeEnum.TRANSACTION_SUCCESS:
                        _logger.LogError("hahahhahahahahhahahahahahhahah--支付成功回调");
                        var resp = JsonConvert.DeserializeObject<WxQueryTransactionResponse>(model.RawData);
                        if (resp == null)
                            return new WechatCallbackRespone("FAIL", msg);
                        if (resp.TradeState == WxTradeStateEnum.SUCCESS)
                        {
                            var reqDto = new PaymentMessageDto
                            {
                                TransId = model.Id,
                                OrderId = resp.OutTradeNo,
                                OrderNo = resp.TransactionId,
                                PayTime = resp.SuccessTime.Value.ToUniversalTime()
                            };
                            _logger.LogError("hahahhahahahahhahahahahahhahah--支付成功发送cap");
                            // 订单支付成功消息通知(在线支付）,执行具体的操作
                            await _publish.SendMessage(CapConst.WechatPaySuccessCallback, reqDto);

                            //BackgroundJob.Enqueue(() => OrderPaymentSuccessful_OnlinePayment_Operate(reqDto));
                        }
                        break;
                    // 微信退款状态变更回调
                    case WxCallBackEventTypeEnum.REFUND_SUCCESS:
                    case WxCallBackEventTypeEnum.REFUND_CLOSED:
                    case WxCallBackEventTypeEnum.REFUND_ABNORMAL:
                        var refundReq = JsonConvert.DeserializeObject<WxRefundResultNotifyResponse>(model.RawData);
                        if (refundReq == null)
                            return new WechatCallbackRespone("FAIL", msg);
                        // 收到通知,调用一次状态查询逻辑,为了代码复用
                        //await _capPublishUtil.PublishAsync(MQTopicConst.OrderRefundSyncStatusTopic, new OrderRefundSyncStatusDto
                        //{
                        //    OrderId = refundReq.OutTradeNo,
                        //    OutRefundNo = refundReq.OutRefundNo,
                        //    TransId = model.Id
                        //});
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return await Task.FromResult(new WechatCallbackRespone("SUCCESS", "成功"));
        }
        #endregion

        #region 支付宝交易异步回调

        /// <summary>
        /// 支付宝异步回调
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [NonUnify]
        [AllowAnonymous]
        [HttpPost("AlipayCallBack/{serviceId}")]
        public async Task<string> AlipayCallback([FromRoute] long serviceId)
        {
            // 判断服务商Id是否存在
            if (serviceId <= 0)
            {
                _logger.LogError("[支付宝异步回调]服务商Id无效");
            }

            var serviceProvider = await _paymentPlatformUtil.GetServiceProviderAsync(serviceId);
            if (serviceProvider == null)
            {
                _logger.LogError($"[支付宝异步回调]服务商Id:{serviceId}不存在");
                return "fail";
            }

            var form = await HttpContext.Request.ReadFormAsync();
            var parameters = form.ToDictionary(dic => dic.Key.ToString(), dic => dic.Value.ToString());

            _logger.LogInformation($"[支付宝异步回调]收到参数=>{JsonConvert.SerializeObject(parameters)}");

            // 验证签名合法性
            var signature = alipayService.BuildMerchant(serviceId.ToString()).VerifySignature(parameters);

            if (!signature)
            {
                _logger.LogError("[支付宝异步回调]验签不通过");
                return "fail";
            }

            // 优先从 msg_method 获取消息接口标识，若不存在则用 notify_type 作为接口标识
            if (parameters.TryGetValue("msg_method", out string? value) && !string.IsNullOrWhiteSpace(value))
            {
                // 和微信一起使用定时任务来获取进件结果
                // var model = JsonConvert.DeserializeObject<AlipayNotifyCallbackResponse>(JsonConvert.SerializeObject(parameters));
                // if (model == null)
                // {
                //     _logger.LogError($"[支付宝异步回调]反序列化回调数据失败,{JsonConvert.SerializeObject(parameters)}");
                //     return "fail";
                // }
                // var reqDto = new AlipayFormMessage
                // {
                //     TransId = YitIdHelper.NextId().ToString(),
                //     BizContent = model.BizContent,
                // };
                // //为了方便维护，使用if else方式来发布
                // if (model.MsgMethod == MQTopicConst.Alipay.MerchantOnboardingPassed)
                // {
                //     //商户入驻成功
                //     await _capPublishUtil.PublishAsync(MQTopicConst.Alipay.MerchantOnboardingPassed, reqDto);
                // }
                // else if (model.MsgMethod == MQTopicConst.Alipay.MerchantOnboardingRejected)
                // {
                //     //商户入驻失败
                //     await _capPublishUtil.PublishAsync(MQTopicConst.Alipay.MerchantOnboardingRejected, reqDto);
                // }
                // else
                // {
                //     _logger.LogError($"[支付宝异步回调]未找到有效的消息接口标识,{JsonConvert.SerializeObject(parameters)}");
                //     return "fail";
                // }
            }
            else if (parameters.TryGetValue("notify_type", out value) && !string.IsNullOrWhiteSpace(value))
            {
                if (value == "trade_status_sync")
                {
                    var model = JsonConvert.DeserializeObject<AlipayTradeQueryCallBackRespons>(JsonConvert.SerializeObject(parameters));
                    if (model == null)
                    {
                        _logger.LogError($"[支付宝异步回调]反序列化回调数据失败,{JsonConvert.SerializeObject(parameters)}");
                        return "fail";
                    }
                    if (model.Trade_status == TradeEnum.TradeStatusEnum.TRADE_SUCCESS)
                    {
                        var reqDto = new PaymentMessageDto
                        {
                            TransId = model.Notify_id,
                            OrderId = model.Out_trade_no,
                            OrderNo = model.Trade_no,
                            PayTime = (model.Gmt_payment ?? DateTime.UtcNow).ToUniversalTime()
                        };
                        // 订单支付成功消息通知(在线支付）,执行具体的操作
                        await _publish.SendMessage(CapConst.AlipaySuccessCallback, reqDto);
                    }
                }
            }
            else
            {
                _logger.LogError($"[支付宝异步回调]未找到有效的消息接口标识,{JsonConvert.SerializeObject(parameters)}");
                return "fail";
            }
            return "success";
        }
        #endregion

        #region 退款相关

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("GetOrderRefundDetailList")]
        public async Task<List<OrderRefundDetailListDto>> GetOrderRefundDetailListByOrderIdAsync([FromQuery] long orderId) => await orderInfoQueries.GetOrderRefundDetailListByOrderIdAsync(orderId);

        /// <summary>
        /// 国内支付订单退款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("DomesticPaymentOrderRefund")]
        public async Task<bool> DomesticPaymentOrderRefundAsync([FromBody] OrderRefundCommand command) => await mediator.Send(command);

        #endregion

        #region 分账相关

        /// <summary>
        /// 添加分账信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddDivideAccountsConfig")]
        public async Task<bool> AddDivideAccountsConfig([FromBody] DivideAccountsConfigCommand command) => await mediator.Send(command);

        /// <summary>
        /// 获取当前企业所有可用支付方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSystemPaymentMethodSelects")]
        public async Task<List<SystemPaymentMethodSelect>> GetSystemPaymentMethodSelectsAsync() => await divideAccountsConfigQueries.GetSystemPaymentMethodSelects();

        /// <summary>
        /// 获取当前系统支付方式下所有支付列表
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        [HttpGet("GetmPaymentMethodSelects")]
        public async Task<List<M_PaymentMethodSelect>> GetmPaymentMethodSelects([FromQuery] long paymentMethodId) => await divideAccountsConfigQueries.GetmPaymentMethodSelects(paymentMethodId);

        /// <summary>
        /// 获取当前支付方式绑定的设备
        /// </summary>
        /// <param name="mpaymentMethodId"></param>
        /// <returns></returns>
        [HttpGet("GetPaymentMethodBindDeviceSelects")]
        public async Task<List<PaymentMethodBindDeviceSelect>> GetPaymentMethodBindDeviceSelectsAsync([FromQuery] long mpaymentMethodId) => await divideAccountsConfigQueries.GetPaymentMethodBindDeviceSelects(mpaymentMethodId);

        /// <summary>
        /// 查询支付分账配置信息分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetDivideAccountsConfigPageList")]
        public async Task<PagedResultDto<DivideAccountsConfigOutput>> GetDivideAccountsConfigPageList([FromBody] DivideAccountsConfigInput input) => await divideAccountsConfigQueries.GetDivideAccountsConfigPageList(input);

        /// <summary>
        /// 更改分账配置状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDivideAccountsConfigEnabled")]
        public async Task<bool> UpdateDivideAccountsConfigEnabledAsync([FromBody] UpdateDivideAccountsConfigEnabledCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除分账配置信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDivideAccountsConfig")]
        public async Task<bool> DeleteDivideAccountsConfigAsync([FromQuery] DeleteDivideAccountsConfigCommand command) => await mediator.Send(command);

        /// <summary>
        /// 编辑分账配置信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateDivideAccountsConfig")]
        public async Task<bool> UpdateDivideAccountsConfigAsync([FromBody] UpdateDivideAccountsConfigCommand command) => await mediator.Send(command);
        #endregion
    }
}