using Aop.Api.Domain;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Numerics;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V3;
using YS.CoffeeMachine.API.Services.DomesticPaymentServices;
using YS.CoffeeMachine.API.Utils.PaymentUtilDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 支付对接帮助类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="redisClient"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="_alipayService"></param>
    /// <param name="_logger"></param>
    /// <param name="_aliyunSmsService"></param>
    /// <param name="_httpClientFactory"></param>
    /// <param name="_paymentConfigService"></param>
    public class PaymentPlatformUtil(CoffeeMachineDbContext context, IRedisClient redisClient,
        IWechatMerchantService _wechatMerchantService,
        IAlipayService _alipayService,
        ILogger<PaymentPlatformUtil> _logger,
        IAliyunSmsService _aliyunSmsService, IHttpClientFactory _httpClientFactory, PaymentConfigService _paymentConfigService)
    {
        /// <summary>
        /// 获取默认服务商Id
        /// </summary>
        /// <param name="systemPaymentMethodId">系统支付方式表的Id（SystemPaymentMethod表的Id）</param>
        /// <returns></returns>
        public async ValueTask<PaymentServiceProviderDto> GetSystemPaymentServiceProvider(long systemPaymentMethodId)
        {
            return await (from a in context.SystemPaymentMethod
                          join b in context.SystemPaymentServiceProvider on a.PaymentPlatformId equals b.Id into ssp
                          from b in ssp.DefaultIfEmpty()
                          where a.Id == systemPaymentMethodId
                          select new PaymentServiceProviderDto
                          {
                              Id = b.Id,
                              SystemPaymentMethodId = a.Id,
                              AppletAppID = b.AppletAppID
                          }).FirstAsync();
        }

        /// <summary>
        /// 根据服务商Id获取服务商信息
        /// </summary>
        /// <param name="serviceProviderId">支付的服务商表(微信|支付宝)的Id（SystemPaymentServiceProvider表的Id）</param>
        /// <returns></returns>
        /// <remarks>使用ValueTask+缓存优化</remarks>
        public async ValueTask<PaymentServiceProviderDto> GetServiceProviderAsync(long serviceProviderId)
        {
            var cacheKey = string.Format(CacheConst.ServiceProviderKey, serviceProviderId);
            var cacheData = await redisClient.GetAsync<PaymentServiceProviderDto>(cacheKey);
            if (cacheData != null)
                return cacheData;

            var info = await (from a in context.SystemPaymentMethod
                              join b in context.SystemPaymentServiceProvider on a.PaymentPlatformId equals b.Id into ssp
                              from b in ssp.DefaultIfEmpty()
                              where b.Id == serviceProviderId
                              select new PaymentServiceProviderDto
                              {
                                  Id = b.Id,
                                  SystemPaymentMethodId = a.Id,
                                  AppletAppID = b.AppletAppID
                              }).FirstAsync();
            // 缓存
            await redisClient.SetAsync(cacheKey, info);

            return info;
        }

        #region 进件服务方法

        /// <summary>
        /// 同步微信进件审核状态
        /// </summary>
        /// <param name="paymentOriginIds">申请记录paymentOriginId</param>
        /// <returns></returns>
        public async Task<bool> SyncWechatApplymentState(params long[] paymentOriginIds)
        {
            if (paymentOriginIds == null || paymentOriginIds.Length <= 0)
                return false;

            var flowStatus = new[] { ApplymentFlowStatusEnum.PlatformReview, ApplymentFlowStatusEnum.TobeSigned, ApplymentFlowStatusEnum.AUDITING, ApplymentFlowStatusEnum.AccountTeedVerify };

            var applyments = await context.M_PaymentWechatApplyments
                .IgnoreQueryFilters()
                .Where(a => paymentOriginIds.Contains(a.PaymentOriginId) && flowStatus.Contains(a.FlowStatus))
                .ToListAsync();

            var smsNotifications = new List<(ApplymentSmsNoticeTypeEnum, string, object)>();
            foreach (var applyment in applyments)
            {
                var paymentMethod = await context.M_PaymentMethod.IgnoreQueryFilters().FirstAsync(a => a.Id == applyment.PaymentOriginId);

                //var result = await _wechatMerchantService
                //    .BuildMerchant(paymentMethod.SystemPaymentServiceProviderId.ToString()).Applyment4SubService
                //    .ToResponseAsync(m => m.GetApplymentByIdAsync(applyment.ApplymentId!));

                var result = await _wechatMerchantService
                    .BuildMerchant(paymentMethod.SystemPaymentServiceProviderId.ToString()).EcommerceService
                    .ToResponseAsync(m => m.GetApplymentByIdAsync(applyment.ApplymentId!));

                if (result.Succeeded)
                {
                    var type = ApplymentSmsNoticeTypeEnum.None;
                    object param = null;

                    var applymentState = result.Data!.Applyment_State;
                    switch (applymentState)
                    {
                        case WxApplymentStateEnum.CHECKING:
                            applyment.FlowStatus = ApplymentFlowStatusEnum.PlatformReview;
                            break;
                        case WxApplymentStateEnum.AUDITING:
                            applyment.FlowStatus = ApplymentFlowStatusEnum.AUDITING;
                            break;
                        case WxApplymentStateEnum.APPLYMENT_STATE_EDITTING: //提交错误重新提交
                        case WxApplymentStateEnum.REJECTED: //驳回
                            applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;

                            type = ApplymentSmsNoticeTypeEnum.Failed;
                            var msg = (result?.Data?.Audit_Detail?.FirstOrDefault())?.Reject_Reason ?? result.Data.Message;
                            param = new { regist_number = "J" + applyment.Id, failure_reason = applyment.RejectReason };
                            break;
                        case WxApplymentStateEnum.CANCELED: //申请单已被撤销
                            applyment.FlowStatus = ApplymentFlowStatusEnum.Canceled;
                            break;
                        case WxApplymentStateEnum.FROZEN:
                            applyment.FlowStatus = ApplymentFlowStatusEnum.Frozen;
                            break;
                        case WxApplymentStateEnum.FINISH: //完成
                            applyment.FlowStatus = ApplymentFlowStatusEnum.Finish;
                            type = ApplymentSmsNoticeTypeEnum.Finished;
                            param = new { payment_method = "微信支付" };
                            break;
                        case WxApplymentStateEnum.NEED_SIGN:
                            type = ApplymentSmsNoticeTypeEnum.TobeSigned;
                            applyment.FlowStatus = ApplymentFlowStatusEnum.TobeSigned;
                            param = new { payment_method = applyment.MerchantName };
                            break;
                        case WxApplymentStateEnum.ACCOUNT_NEED_VERIFY:
                            type = ApplymentSmsNoticeTypeEnum.Confirmed;
                            applyment.FlowStatus = ApplymentFlowStatusEnum.PlatformReview;
                            break;
                    }

                    applyment.ApplymentState = applymentState;
                    applyment.ApplymentStateDesc = result.Data.Message;

                    if (string.IsNullOrEmpty(applyment.ApplymentStateDesc))
                        applyment.ApplymentStateDesc = applymentState.GetEnumDefaultValue();

                    applyment.RejectReason = applyment.ApplymentStateDesc;
                    applyment.SubMchId = result.Data.Sub_Mchid ?? applyment.SubMchId;
                    if (result.Data.Audit_Detail != null && result.Data.Audit_Detail.Count > 0)
                    {
                        applyment.AuditDetail = JsonConvert.SerializeObject(result.Data.Audit_Detail);
                        applyment.RejectReason = result.Data.Audit_Detail.FirstOrDefault().Reject_Reason;
                    }

                    applyment.SignUrl = result.Data.Sign_Url;
                    context.M_PaymentWechatApplyments.Update(applyment);

                    // 同时更新主申请单状态，并且通知商户申请结果
                    var paymentMethodInfo = await context.M_PaymentMethod.IgnoreQueryFilters().FirstAsync(a => a.Id == applyment.PaymentOriginId);
                    paymentMethodInfo.UpdateApplymentStatus(applyment.SubMchId ?? "", ApplymentFlowStatusToOnboardingStatus(applyment.FlowStatus));
                    context.Update(paymentMethodInfo);

                    // 发送短信通知
                    smsNotifications.Add((type, paymentMethod.Phone, param));
                    //await ApplymentSendSmsNotice(type, paymentMethod.Phone, param);
                }
                else
                {
                    _logger.LogInformation($"微信商户进件查询失败_{JsonConvert.SerializeObject(result)}");
                }
            }

            await context.SaveChangesAsync();

            foreach (var item in smsNotifications)
            {
                await ApplymentSendSmsNotice(item.Item1, item.Item2, item.Item3);
            }
            return true;
        }

        #region (支付宝商户进件)

        /// <summary>
        /// 同步支付宝进件申请状态
        /// </summary>
        /// <param name="paymentOriginIds">进件paymentOriginId</param>
        /// <returns></returns>
        public async Task<bool> SyncAlipayApplymentsState(params long[] paymentOriginIds)
        {
            if (paymentOriginIds == null || paymentOriginIds.Length <= 0)
                return false;

            var flowStatus = new[] { ApplymentFlowStatusEnum.PlatformReview, ApplymentFlowStatusEnum.TobeSigned, ApplymentFlowStatusEnum.AUDITING, ApplymentFlowStatusEnum.AccountTeedVerify };

            var applyments = await context.M_PaymentAlipayApplyments
                .IgnoreQueryFilters()
                .Where(a => paymentOriginIds.Contains(a.PaymentOriginId) && flowStatus.Contains(a.FlowStatus))
                .OrderBy(a => a.CreateTime) // 按创建时间排序，确保处理顺序
                .ToListAsync();
            if (applyments == null || applyments.Count <= 0)
            {
                _logger.LogInformation("没有需要同步的支付宝进件申请记录");
                return true;
            }

            // 一次性查出所有主表的paymentMethod
            var paymentMethodDic = (await context.M_PaymentMethod
                .IgnoreQueryFilters()
                .Where(a => paymentOriginIds.Contains(a.Id))
                .ToListAsync())
                .ToDictionary(x => x.Id, x => x);

            // 批量处理applyments，收集需要更新的applyments和通知
            var updateApplyments = new List<M_PaymentAlipayApplyments>();
            var updatePaymentMethodDic = new Dictionary<long, M_PaymentMethod>();
            var smsTasks = new List<Task>();

            foreach (var applyment in applyments)
            {
                if (!paymentMethodDic.TryGetValue(applyment.PaymentOriginId, out M_PaymentMethod? mainPaymentMethod) || mainPaymentMethod == null)
                {
                    _logger.LogWarning($"未找到支付方式记录，PaymentOriginId: {applyment.PaymentOriginId}");
                    continue;
                }
                var applymentService = _alipayService.BuildMerchant(mainPaymentMethod.SystemPaymentServiceProviderId.ToString()).ApplymentService;

                var order = await applymentService.GetMerchantApplyment(new AlipayApplymentQueryStateRequest
                {
                    External_id = applyment.Id.ToString()
                });

                if (applyment.OrderId == null && order.Data.Orders.Any())
                {
                    applyment.OrderId = order.Data.Orders.OrderByDescending(o => o.ApplyTime).FirstOrDefault().OrderId;
                }

                if (!order.Success)
                {
                    applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                    applyment.RejectReason = order.Data?.SubMsg;
                    updateApplyments.Add(applyment);
                    mainPaymentMethod.SetMerchantId(applyment.Smid ?? string.Empty);
                    mainPaymentMethod.SetPaymentEntryStatus(ApplymentFlowStatusToOnboardingStatus(applyment.FlowStatus));
                    updatePaymentMethodDic[applyment.PaymentOriginId] = mainPaymentMethod;

                    // 发送短信通知
                    smsTasks.Add(ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum.Failed, mainPaymentMethod.Phone,
                        new { regist_number = "J" + applyment.Id, failure_reason = applyment.RejectReason }));
                    continue;
                }

                var result = await applymentService.GetMerchantApplymentOrderQuery(new AlipayApplymentQueryStateRequest()
                {
                    OrderId = applyment.OrderId,
                });

                if (!result.Success || result.Data == null || result.Data.Code != ((int)HttpStateCodeEnum.Success).ToString())
                {
                    applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                    applyment.RejectReason = result.Data?.SubMsg ?? result.Msg;
                    updateApplyments.Add(applyment);
                    mainPaymentMethod.SetMerchantId(applyment.Smid ?? string.Empty);
                    mainPaymentMethod.SetPaymentEntryStatus(ApplymentFlowStatusToOnboardingStatus(applyment.FlowStatus));
                    updatePaymentMethodDic[applyment.PaymentOriginId] = mainPaymentMethod;
                    smsTasks.Add(ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum.Failed, mainPaymentMethod.Phone,
                         new { regist_number = "J" + applyment.Id, failure_reason = applyment.RejectReason }));
                    continue;
                }

                //applyment.IpRoleId = JsonConvert.SerializeObject(result.Data.IpRoleId);
                applyment.MerchantName = result.Data.MerchantName;
                applyment.AppAuthToken = result.Data.Sign;
                //applyment.Status = result.Data.Status;
                applyment.ExtInfo = result.Data.ExtInfo;
                if (result.Data.ExtInfo != null)
                {
                    applyment.CardAliasNo = result.Data.Ext_info_model.CardAliasNo;
                    applyment.Smid = result.Data.Ext_info_model.Smid;
                }

                if (result.Data.IpRoleId != null)
                {
                    applyment.Smid = result.Data.IpRoleId.FirstOrDefault() ?? string.Empty;
                }

                var type = ApplymentSmsNoticeTypeEnum.None;
                object? param = null;

                switch (result.Data.Status)
                {
                    case MerchantZdtorderQueryStatusConst.AlipayReview:
                        applyment.FlowStatus = ApplymentFlowStatusEnum.PlatformReview;
                        break;
                    case MerchantZdtorderQueryStatusConst.Finish:
                        applyment.RejectReason = string.Empty;
                        applyment.FlowStatus = ApplymentFlowStatusEnum.Finish;
                        type = ApplymentSmsNoticeTypeEnum.Finished;
                        smsTasks.Add(ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum.Finished, mainPaymentMethod.Phone,
                            new { payment_method = "支付宝支付" }));
                        break;
                    default:
                        applyment.FlowStatus = ApplymentFlowStatusEnum.Failed;
                        var aliOrder = order.Data?.Orders.Where(a => a.OrderId == applyment.OrderId).FirstOrDefault();
                        if (aliOrder != null)
                        {
                            if (aliOrder?.FkAudit == "REJECT")
                                applyment.RejectReason = aliOrder?.FkAuditMemo ?? string.Empty;
                            else if (aliOrder?.KzAudit == "REJECT")
                                applyment.RejectReason = aliOrder?.KzAuditMemo ?? string.Empty;
                            else
                                applyment.RejectReason = aliOrder?.Reason ?? string.Empty;
                        }
                        type = ApplymentSmsNoticeTypeEnum.Failed;
                        param = new { regist_number = "J" + applyment.Id, failure_reason = applyment.RejectReason };
                        break;
                }

                updateApplyments.Add(applyment);
                mainPaymentMethod.SetMerchantId(applyment.Smid ?? string.Empty);
                mainPaymentMethod.SetPaymentEntryStatus(ApplymentFlowStatusToOnboardingStatus(applyment.FlowStatus));
                updatePaymentMethodDic[applyment.PaymentOriginId] = mainPaymentMethod;
                if (type != ApplymentSmsNoticeTypeEnum.None)
                    smsTasks.Add(ApplymentSendSmsNotice(type, mainPaymentMethod.Phone, param));
            }

            // 批量更新applyments
            if (updateApplyments.Count > 0)
            {
                context.M_PaymentAlipayApplyments.UpdateRange(updateApplyments);
            }

            // 批量更新主表状态
            if (updatePaymentMethodDic.Count > 0)
            {
                var updatePaymentMethods = updatePaymentMethodDic.Values.ToList();
                context.M_PaymentMethod.UpdateRange(updatePaymentMethods);
            }

            await context.SaveChangesAsync();

            // 并发发送短信
            if (smsTasks.Count > 0)
                await Task.WhenAll(smsTasks);

            return true;
        }
        #endregion

        /// <summary>
        /// 将进件流程状态转换为商户入驻状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public InternalOnboardingStatusEnum ApplymentFlowStatusToOnboardingStatus(ApplymentFlowStatusEnum? status)
        {
            return status switch
            {
                ApplymentFlowStatusEnum.Initialize => InternalOnboardingStatusEnum.Pending,
                ApplymentFlowStatusEnum.PlatformReview => InternalOnboardingStatusEnum.Onboarding,
                ApplymentFlowStatusEnum.Finish => InternalOnboardingStatusEnum.OnboardingSuccess,
                ApplymentFlowStatusEnum.Canceled => InternalOnboardingStatusEnum.Revoke,
                ApplymentFlowStatusEnum.TobeSigned => InternalOnboardingStatusEnum.Signing,
                ApplymentFlowStatusEnum.AccountTeedVerify => InternalOnboardingStatusEnum.Onboarding,
                ApplymentFlowStatusEnum.AUDITING => InternalOnboardingStatusEnum.Onboarding,
                ApplymentFlowStatusEnum.MerchantCanceled => InternalOnboardingStatusEnum.Revoke,
                _ => InternalOnboardingStatusEnum.OnboardingFail
            };
        }

        /// <summary>
        ///  发送进件通知短信
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="phone">手机号</param>
        /// <param name="param">短信内容变量参数</param>
        /// <returns></returns>
        public async Task ApplymentSendSmsNotice(ApplymentSmsNoticeTypeEnum type, string phone, object? param = null)
        {
            if (type == ApplymentSmsNoticeTypeEnum.None) return;

            if (param == null)
                param = new { };
            var templCode = SmsConst.MerchantApplymentFailed;
            switch (type)
            {
                case ApplymentSmsNoticeTypeEnum.Failed: templCode = SmsConst.MerchantApplymentFailed; break;
                case ApplymentSmsNoticeTypeEnum.Finished: templCode = SmsConst.MerchantApplymentSuccess; break;
                case ApplymentSmsNoticeTypeEnum.TobeSigned: templCode = SmsConst.MerchantApplymentSign; break;
                default: return;
            }

            await _aliyunSmsService.SendSmsAsync(phone, templCode, JsonConvert.SerializeObject(param));
        }

        /// <summary>
        /// 上传文件到微信同时返回MediaId
        /// </summary>
        /// <param name="wechatMerchantService"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception">上传失败抛异常</exception>
        public async Task<string> UploadImageToWechatMediaId(IWechatMerchantService wechatMerchantService, string url)
        {
            var fileName = Path.GetFileName(url).Split("?")[0];
            var fileByte = await DownloadImageToByte(url);

            var fileRequest = new WxFileMetaInfoRequest
            {
                FileName = fileName,
                Sha256 = fileByte.GenerateSHA256(),
            };
            var result = await wechatMerchantService.CommonService
                .ToResponseAsync(a => a.UploadMediaAsync(new Cabinet.HttpDispatchProxy.HdpFromFile
                {
                    FileName = fileName,
                    FileByte = fileByte,
                }, fileRequest));
            if (result.Succeeded)
                return result!.Data!.MediaId ?? string.Empty;
            else
                throw new Exception(result?.Message);
        }

        /// <summary>
        /// 下载文件到字节数组
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadImageToByte(string url)
        {
            using var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            var result = await client.GetByteArrayAsync(url);
            return result;
        }

        /// <summary>
        /// 修改商户支付方式进件结果
        /// </summary>
        /// <param name="originId">支付方式originId</param>
        /// <param name="mid">第三方商户号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<bool> UpdatePaymentMothodResult(long originId, string? mid, ApplymentFlowStatusEnum? status)
        {
            var entryStatus = ApplymentFlowStatusToOnboardingStatus(status);

            var info = await context.M_PaymentMethod.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == originId);
            if (info != null)
            {
                if (!string.IsNullOrEmpty(mid))
                    info.SetMerchantId(mid);
                info.SetPaymentEntryStatus(entryStatus);
            }
            context.M_PaymentMethod.Update(info);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion

        /// <summary>
        /// 获取设备绑定的商户支付方式信息
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="tenantId"></param>
        /// <param name="systemPaymentMethodId"></param>
        /// <returns></returns>
        public async Task<GetDevicePaymentMethodDto> GetDevicePaymentMethod(long deviceBaseId, long tenantId, long systemPaymentMethodId)
        {
            var fathSystemPaymentMethodId = GetSystemPaymentMethodId(systemPaymentMethodId);

            // 获取设备Id
            var deviceInfoId = await (from a in context.DeviceBaseInfo.IgnoreQueryFilters()
                                      join b in context.DeviceInfo.IgnoreQueryFilters() on a.Id equals b.DeviceBaseId
                                      where a.Id == deviceBaseId
                                      select b.Id).FirstAsync();
            if (deviceInfoId == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            var result = await context.M_PaymentMethod.AsNoTracking().IgnoreQueryFilters()
                .Where(w => w.EnterpriseinfoId == tenantId
                && w.SystemPaymentMethodId == systemPaymentMethodId && w.PaymentEntryStatus == InternalOnboardingStatusEnum.OnboardingSuccess
               && w.IsEnabled == EnabledEnum.Enable && context.M_PaymentMethodBindDevice.IgnoreQueryFilters().Where(b => b.DeviceId == deviceInfoId && b.EnterpriseinfoId == tenantId).Any())
                .Select(p => new GetDevicePaymentMethodDto
                {
                    Id = p.Id,
                    SystemPaymentMethodId = systemPaymentMethodId,// 这里没有搞错，不要修改（因为支付宝支付【支付宝JSAPI，支付宝刷脸等所有的支付宝支付产品都是商户进件一次，所有支付都可以使用】。
                                                                  // 微信支付【微信JSAPI，微信刷脸等所有的微信支付产品都是商户进件一次，所有支付都可以使用】。
                                                                  // 所以这里支付的服务商表(微信|支付宝)【SystemPaymentServiceProvider表的SystemPaymentMethodId】
                                                                  // 商户支付方式表【Me_PaymentMethod表的SystemPaymentMethodId】这两个表都存的是父级的系统支付方式表的Id（（SystemPaymentMethod表的Id）））
                    OrgId = p.EnterpriseinfoId,
                    PaymentEntryStatus = p.PaymentEntryStatus,
                    MerchantId = p.MerchantId,
                    IsEnabled = p.IsEnabled,
                    SystemPaymentServiceProviderId = p.SystemPaymentServiceProviderId
                })
                .FirstOrDefaultAsync();
            if (result == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            return result;
        }

        /// <summary>
        /// 获取设备绑定的支付方式，返回给安卓收银台
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceBindPaymentMethodDto> GetDeviceBindPaymentMethodDtos(string mid)
        {
            var query = from a in context.DeviceBaseInfo.IgnoreQueryFilters()
                        join b in context.DeviceInfo.IgnoreQueryFilters() on a.Id equals b.DeviceBaseId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in context.M_PaymentMethodBindDevice.IgnoreQueryFilters() on b.Id equals c.DeviceId into bc
                        from c in bc.DefaultIfEmpty()
                        join d in context.M_PaymentMethod.IgnoreQueryFilters() on c.PaymentMethodId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where a.MachineStickerCode == mid && d.PaymentEntryStatus == InternalOnboardingStatusEnum.OnboardingSuccess
                            && d.IsEnabled == EnabledEnum.Enable && !a.IsDelete && !b.IsDelete
                        select new
                        {
                            a.Mid,
                            d.SystemPaymentMethodId,
                            d.Id
                        };

            var list = await query.ToListAsync();

            var result = new DeviceBindPaymentMethodDto
            {
                Mid = mid,
                PaymentMethods = new List<DPaymentMethod>()
            };

            if (list.Count > 0)
            {
                list.ForEach(e =>
                {
                    var typeInfo = GetOrderPaymentType(e.SystemPaymentMethodId);
                    result.PaymentMethods.Add(new DPaymentMethod()
                    {
                        Title = typeInfo.Item2,
                        PaymentType = typeInfo.Item1.ToString(),
                        MerchantId = e.Id.ToString()
                    });
                });
            }

            return result;
        }

        /// <summary>
        /// 获取设备绑定的支付方式，返回给安卓收银台（支持多个mid）
        /// </summary>
        /// <param name="mids">多个设备mid</param>
        /// <returns></returns>
        public async Task<List<DeviceBindPaymentMethodDto>> GetDeviceBindPaymentMethodDtos(List<string> mids)
        {
            if (mids == null || mids.Count == 0)
                return new List<DeviceBindPaymentMethodDto>();

            var query = from a in context.DeviceBaseInfo
                        join b in context.DeviceInfo on a.Id equals b.DeviceBaseId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in context.M_PaymentMethodBindDevice on b.Id equals c.DeviceId into bc
                        from c in bc.DefaultIfEmpty()
                        join d in context.M_PaymentMethod on c.PaymentMethodId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        where mids.Contains(a.Mid)
                              && d != null
                              && d.PaymentEntryStatus == InternalOnboardingStatusEnum.OnboardingSuccess
                              && d.IsEnabled == EnabledEnum.Enable
                        select new
                        {
                            a.Mid,
                            d.SystemPaymentMethodId,
                            d.Id
                        };

            var list = await query.ToListAsync();

            // 按 mid 分组
            var result = list
                .GroupBy(e => e.Mid)
                .Select(g =>
                {
                    var dto = new DeviceBindPaymentMethodDto
                    {
                        Mid = g.Key,
                        PaymentMethods = new List<DPaymentMethod>()
                    };

                    foreach (var e in g)
                    {
                        var typeInfo = GetOrderPaymentType(e.SystemPaymentMethodId);
                        dto.PaymentMethods.Add(new DPaymentMethod
                        {
                            Title = typeInfo.Item2,
                            PaymentType = typeInfo.Item1.ToString(),
                            MerchantId = e.Id.ToString()
                        });
                    }

                    return dto;
                })
                .ToList();

            // 保证所有 mids 都有返回，即使没有绑定支付方式
            foreach (var mid in mids)
            {
                if (!result.Any(r => r.Mid == mid))
                {
                    result.Add(new DeviceBindPaymentMethodDto
                    {
                        Mid = mid,
                        PaymentMethods = new List<DPaymentMethod>()
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 获取订单支付类型
        /// 微信支付、支付宝支付等
        /// </summary>
        /// <param name="systemPaymentMethodId"></param>
        /// <returns></returns>
        private (int, string) GetOrderPaymentType(long systemPaymentMethodId)
        {
            return systemPaymentMethodId switch
            {
                CommonConst.WechatPaymentId => ((int)OrderPaymentTypeEnum.WxNativePay, "微信支付"),
                CommonConst.AlipaPaymenteId => ((int)OrderPaymentTypeEnum.AlipayJsApi, "支付宝支付")
            };
        }

        /// <summary>
        /// 获取系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// </summary>
        /// <param name="systemPaymentMethodId">系统支付方式表的Id（SystemPaymentMethod表的Id）</param>
        /// <returns></returns>
        private static long GetSystemPaymentMethodId(long systemPaymentMethodId)
        {
            // 这里没有搞错，不要修改（因为支付宝支付【支付宝JSAPI，支付宝刷脸等所有的支付宝支付产品都是商户进件一次，所有支付都可以使用】。
            // 微信支付【微信JSAPI，微信刷脸等所有的微信支付产品都是商户进件一次，所有支付都可以使用】。
            // 所以这里支付的服务商表(微信|支付宝)【SystemPaymentServiceProvider表的SystemPaymentMethodId】
            // 商户支付方式表【Me_PaymentMethod表的SystemPaymentMethodId】这两个表都存的是父级的系统支付方式表的Id（（SystemPaymentMethod表的Id）））
            long retSystemPaymentMethodId = systemPaymentMethodId;
            if (systemPaymentMethodId == CommonConst.WechatJSAPIPaymentId || systemPaymentMethodId == CommonConst.WechatFacePaymentId)
                retSystemPaymentMethodId = CommonConst.WechatPaymentId;
            else if (systemPaymentMethodId == CommonConst.AlipaJSAPIPaymentId || systemPaymentMethodId == CommonConst.AlipaFacePaymentId)
                retSystemPaymentMethodId = CommonConst.AlipaPaymenteId;
            return retSystemPaymentMethodId;
        }

        #region 微信支付相关

        /// <summary>
        /// 创建微信jsapi下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns>前端发起jsapi收银台所需的参数</returns>
        public async Task<WechatRestfulResponse<WxTransactionResponse>> CreateWechatNative(CreateWxpayOrderInput input)
        {
            // 微信支付服务商配置
            var config = await _paymentConfigService.GetPaymentConfig(input.SystemPaymentServiceProviderId);

            var request = new WxTransactionRequest
            {
                Amount = new WxTransactionAmount { Total = (int)(input.Amount * 100) },
                Description = input.Description,
                OutTradeNo = input.OutTradeNo,
                SpAppid = config.AppletAppID, // 服务商appid
                SpMchid = config.SPMerchantId, // 服务商户号
                SubMchid = input.MerchantId, // 子商户号
                NotifyUrl = config.NotifyUrl // 异步通知地址
            };

            if (input.GoodsDetail?.Count > 0)
            {
                request.Detail = new WxTransactionPromotionDetail
                {
                    CostPrice = input.GoodsDetail.Sum(a => a.UnitPrice * a.Quantity),
                    GoodsDetail = input.GoodsDetail
                };
            }

            return await _wechatMerchantService
                .BuildMerchant(input.SystemPaymentServiceProviderId.ToString())
                .TransactionService.ToResponseAsync(a => a.PartnerNativeAsync(request));
        }
        #endregion

        #region 支付宝支付相关

        /// <summary>
        /// 支付宝jsapi下单
        /// </summary>
        /// <param name="input">入参</param>
        /// <returns>返回订单的商户订单号和支付宝流水号</returns>
        public async Task<ResultResponse<Alipay_TradePrecreateResponse>> CreateAlipayJsapi(CreateAlipayOrderInput input)
        {
            // 支付服务商配置
            var config = await _paymentConfigService.GetPaymentConfig(input.SystemPaymentServiceProviderId);

            // 四舍五入保留两位小数
            var totalAmount = Math.Round(input.Amount, 2);

            var request = new Alipay_TradePrecreateRequest()
            {
                Out_trade_no = input.OutTradeNo,
                Total_amount = totalAmount,
                Subject = input.Subject,
                Product_code = "QR_CODE_OFFLINE",
                NotifyUrl = config.NotifyUrl,
                Body = "支付宝扫码支付",
                Goods_detail = input.GoodsDetail,
                SubMerchant = new Cabinet.Payment.Alipay.SubMerchant { MerchantId = input.MerchantId }, //子商户号
                SettleInfo = new Cabinet.Payment.Alipay.SettleInfo
                {
                    SettleDetailInfos = new List<Cabinet.Payment.Alipay.SettleInfo.Settle_DetailInfo>
                    {
                        new Cabinet.Payment.Alipay.SettleInfo.Settle_DetailInfo
                        {
                            Amount = totalAmount,
                            TransInType = TradeEnum.TransInTypeEnum.DefaultSettle, //互联网直付通模式固定传值：defaultSettle。
                        },
                    },
                }
            };
            // 发起请求
            var result = await _alipayService.BuildMerchant(input.SystemPaymentServiceProviderId.ToString()).PayService.TradePrecreate(request);
            return result;

        }
        #endregion

        #region 验证商户Id是否存在

        /// <summary>
        /// 验证微信扫码支付商户Id是否存在
        /// </summary>
        /// <param name="MerchantId"></param>
        /// <param name="SystemPaymentServiceProviderId"></param>
        /// <returns></returns>
        public async Task<bool> VerifyWxNative(string MerchantId, long SystemPaymentServiceProviderId)
        {
            // 订单商品详情
            var goods = new List<WxPromotionGoodsDetail>() { new WxPromotionGoodsDetail
            {
                GoodsName = "测试商品1",
                MerchantGoodsId = "V001",
                Quantity = 1,
                UnitPrice = (int)(0.01 * 100)
            }};

            var result = await CreateWechatNative(new CreateWxpayOrderInput
            {
                SystemPaymentServiceProviderId = SystemPaymentServiceProviderId,
                Amount = 0.01m,
                Description = "测试",
                OutTradeNo = BasicUtils.GenerateOrderNo(),
                MerchantId = MerchantId,
                GoodsDetail = goods
            });

            return result.Succeeded;
        }

        /// <summary>
        /// 验证支付宝扫码支付商户Id是否存在
        /// </summary>
        /// <param name="MerchantId"></param>
        /// <param name="SystemPaymentServiceProviderId"></param>
        /// <returns></returns>
        public async Task<bool> VerifyAlipayNative(string MerchantId, long SystemPaymentServiceProviderId)
        {// 订单商品详情
            var goodDetail = new List<Alipay_GoodsDetail>()
            {
                new Alipay_GoodsDetail
                {
                    GoodsId = "V001",
                    GoodsName = "测试商品1",
                    Price = "0.01",
                    Quantity = 1
                }
            };

            var result = await CreateAlipayJsapi(new CreateAlipayOrderInput
            {
                SystemPaymentServiceProviderId = SystemPaymentServiceProviderId,
                Amount = 0.01m,
                OutTradeNo = BasicUtils.GenerateOrderNo(),
                Subject = "测试商品1",
                OpenId = "001",
                MerchantId = MerchantId,
                GoodsDetail = goodDetail,
                PaymentMethodId = Convert.ToInt64(MerchantId)
            });

            return result.Success;
        }
        #endregion

        #region 订单退款

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderRefunds">退款订单表集合(OrderRefund表)</param>
        /// <param name="refundAmount">退款金额（单位：元）</param>
        /// <param name="reason">退款原因</param>
        /// <param name="outRefundNo">自定义退款编号</param>
        /// <returns></returns>
        public async Task<OrderRefundOutput> OrderRefund(OrderInfo order, List<OrderRefundGoodsDto> orderRefunds, decimal refundAmount, string? reason, string outRefundNo)
        {
            if (order == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0001), nameof(order)]);
            if (refundAmount <= 0)
                throw ExceptionHelper.AppFriendly("退款金额必须大于0");
            //if (order.OrderStatus == OrderStatusEnum.Refunding)
            //    throw ExceptionHelper.AppFriendly("当前订单正在退款中，无法再次发起退款");
            if (order.OrderStatus == OrderStatusEnum.FullRefund)
                throw ExceptionHelper.AppFriendly("没有可退款的子订单");
            if (order.SystemPaymentMethodId == CommonConst.OtherPaymentId)
                throw ExceptionHelper.AppFriendly("现金支付的订单不支持退款");
            if (order.OrderType != OrderTypeEnum.OnlineOrder)
                throw ExceptionHelper.AppFriendly("当前订单不支持退款");
            if (orderRefunds.Count == 0)
                throw ExceptionHelper.AppFriendly("请选择要退款的子订单");
            if (orderRefunds.Count(p => p.RefundAmount > 0) == 0)
                throw ExceptionHelper.AppFriendly("请选择要退款的子订单");

            if (string.IsNullOrEmpty(reason))
                reason = "手动退款";

            // 获取支付配置
            var config = await _paymentConfigService.GetPaymentConfig(order.SystemPaymentServiceProviderId ?? 0);
            var result = new OrderRefundOutput
            {
                OrderId = order.Code,
                OrderNo = order.ThirdOrderNo ?? string.Empty,
                OutRefundNo = outRefundNo
            };

            // 四舍五入，保留两位小数,不然会报错
            if (!decimal.TryParse(Math.Round(refundAmount, 2, MidpointRounding.AwayFromZero).ToString("F2"), out var amount) || amount <= 0)
            {
                _logger.LogError($"[订单退款]退款金额格式错误，orderId={order.Code}，refundAmount={refundAmount}");
                throw ExceptionHelper.AppFriendly("退款金额必须大于0");
            }

            refundAmount = amount;

            switch (order.SystemPaymentMethodId)
            {
                case CommonConst.AlipaPaymenteId:
                    var alipayRefundReq = new AliPayRefundRequest
                    {
                        OutRequestNo = result.OutRefundNo,
                        RefundAmount = refundAmount,
                        RefundReason = reason,
                        TradeNo = order.ThirdOrderNo,
                        OutTradeNo = order.Code,
                        RefundGoodsDetail = orderRefunds.Select(a => new AliPayRefundRequest.RefundGoodsDetailItem
                        {
                            GoodsId = a.Code,
                            RefundAmount = decimal.Parse(Math.Round(a.RefundAmount, 2, MidpointRounding.AwayFromZero).ToString("F2")),
                        }).ToList()
                    };

                    // 支付宝发起退款
                    var alipayRefundRes = await _alipayService.BuildMerchant(order.SystemPaymentServiceProviderId.ToString()).RefundService.TradeRefund(alipayRefundReq);

                    if (!alipayRefundRes.Success)
                    {
                        _logger.LogError($"[订单退款]请求支付宝失败，orderId={order.Code}，req={JsonConvert.SerializeObject(alipayRefundReq)}");
                        throw ExceptionHelper.AppFriendly($"支付宝退款失败：{alipayRefundRes.Msg}");
                    }

                    result.RefundId = result.OutRefundNo;
                    break;
                case CommonConst.WechatPaymentId:
                    var wxRefundReq = new WxRefundRequest
                    {
                        SubMchid = order.PaymentMerchantId,
                        OutTradeNo = order.Code,
                        TransactionId = order.ThirdOrderNo,
                        Amount = new WxRefundRequest.RefundAmountInfo
                        {
                            Total = (int)(order.Amount * 100),
                            Refund = (int)(refundAmount * 100)
                        },
                        NotifyUrl = config.NotifyUrl,
                        OutRefundNo = result.OutRefundNo,
                        Reason = reason,
                        GoodsDetail = orderRefunds.Select(a => new WxRefundRequest.RefundGoodsDetail
                        {
                            MerchantGoodsId = a.Code,
                            GoodsName = a.Name,
                            RefundAmount = (int)(a.RefundAmount * 100),
                            RefundQuantity = a.Quantity,
                            UnitPrice = (int)(a.Price * 100),
                        }).ToList()
                    };

                    var wxRefundRes = await _wechatMerchantService
                        .BuildMerchant(order.SystemPaymentServiceProviderId.ToString())
                        .TransactionService.ToResponseAsync(a => a.RefundAsync(wxRefundReq));

                    if (!wxRefundRes.Succeeded)
                        throw ExceptionHelper.AppFriendly(wxRefundRes?.Data?.Error ?? wxRefundRes.Message);

                    result.RefundId = wxRefundRes.Data.RefundId;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 订单退款查询结果
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="outRefundNo">退款单号</param>
        /// <returns></returns>
        public async Task<OrderRefundQueryOutput> OrderRefundQuery(long orderId, string outRefundNo)
        {
            var order = await context.OrderInfo.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == orderId);
            if (order == null)
                return new OrderRefundQueryOutput { RefundStatus = RefundStatusEnum.Refunding };

            var config = await _paymentConfigService.GetPaymentConfig(order.SystemPaymentServiceProviderId ?? 0);
            var result = new OrderRefundQueryOutput
            {
                RefundStatus = RefundStatusEnum.Refunding
            };
            switch (order.SystemPaymentMethodId)
            {
                // 支付宝
                case CommonConst.AlipaPaymenteId:
                    var alipayRes = await _alipayService
                        .BuildMerchant(order.SystemPaymentServiceProviderId.ToString())
                        .RefundService.TradeRefundQuery(new Alipay_TradeRefundQueryRequest
                        {
                            OutRequestNo = outRefundNo,
                            OutTradeNo = order.Code,
                            QueryOptions = new List<string> { "gmt_refund_pay" }
                        });
                    //请求发起成功,表示查询到了结果
                    if (alipayRes.Success)
                    {
                        result.RefundStatus = alipayRes.Data.RefundStatus == "REFUND_SUCCESS" ? RefundStatusEnum.Success : RefundStatusEnum.Fail;
                        if (DateTime.TryParse(alipayRes.Data.GmtRefundPay, out var successTime))
                        {
                            // 支付宝退款时间是UTC+8，转换成UTC时间，
                            // 线上环境是UTC+0时区直接使用 ToUniversalTime 无法转换成UTC时间,这里直接减去8小时
                            // result.SuccessTime = successTime.AddHours(-8);

                            result.SuccessTime = successTime;
                        }
                        result.Reason = alipayRes.Data.SubMsg;
                    }
                    else
                    {
                        _logger.LogError("[订单退款查询]查询失败" + JsonConvert.SerializeObject(alipayRes));
                    }
                    break;
                // 微信
                case CommonConst.WechatPaymentId:
                    var wxRes = await _wechatMerchantService
                        .BuildMerchant(order.SystemPaymentServiceProviderId.ToString())
                        .TransactionService.ToResponseAsync(a => a.RefundQueryAsync(outRefundNo, order.PaymentMerchantId));

                    if (wxRes.Succeeded)
                    {
                        result.RefundStatus = wxRes.Data.Status switch
                        {
                            WxRefundStateEnum.SUCCESS => RefundStatusEnum.Success,
                            WxRefundStateEnum.CLOSED => RefundStatusEnum.Fail,
                            WxRefundStateEnum.ABNORMAL => RefundStatusEnum.Fail,
                            _ => result.RefundStatus
                        };
                        if (result.RefundStatus == RefundStatusEnum.Success)
                        {
                            if (DateTime.TryParse(wxRes.Data.SuccessTime, out var successTime))
                                result.SuccessTime = successTime.ToUniversalTime();
                            else
                                result.SuccessTime = DateTime.UtcNow;
                        }
                        result.Reason = wxRes.Data.Message;
                    }
                    else if (wxRes.Data != null && wxRes.Data.Code == "RESOURCE_NOT_EXISTS")
                    {
                        result.RefundStatus = RefundStatusEnum.Fail;
                        result.Reason = wxRes.Data.Message;
                    }
                    else
                    {
                        _logger.LogError("[订单退款查询]查询失败" + JsonConvert.SerializeObject(wxRes));
                    }
                    break;
            }
            return result;
        }
        #endregion
    }
}