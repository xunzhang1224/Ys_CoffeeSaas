using FreeRedis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Services;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers
{
    /// <summary>
    /// 手机短信验证码发送命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="aliyunSmsService"></param>
    /// <param name="redisClient"></param>
    /// <param name="httpContextAccessor"></param>
    public class SendPhoneCodeCommandHandler(CoffeeMachineDbContext context, IAliyunSmsService aliyunSmsService, IRedisClient redisClient, IHttpContextAccessor httpContextAccessor) : ICommandHandler<MerchantOnboardingSendPhoneCodeCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(MerchantOnboardingSendPhoneCodeCommand request, CancellationToken cancellationToken)
        {
            if (request.systemPaymentMethodId <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            // 手机号不能为空
            if (string.IsNullOrWhiteSpace(request.phone))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0105)]);

            // 手机号格式不正确
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.phone, @"^1[3-9]\d{9}$"))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0106)]);

            #region Ip限流

            // 限制每个IP每分钟最多2次请求
            var ip = IpAddressHelper.GetClientIp(httpContextAccessor.HttpContext);
            // 未获取到IP，后续可以做异常提示，这里暂时不判断
            if (ip != "unknown")
            {
                var ipKey = $"phoneverifylimit:ip:{ip}";

                var count = await redisClient.IncrByAsync(ipKey, 1); // 自增
                if (count == 1)
                {
                    await redisClient.ExpireAsync(ipKey, 60); // 第一次时设置过期时间
                }
                if (count > 2)
                {
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0094)]);
                }
            }
            #endregion

            // 验证账号是否存在
            var exists = await context.M_PaymentMethod.AnyAsync(x => x.Phone == request.phone && x.SystemPaymentMethodId == request.systemPaymentMethodId);// 同一个系统支付方式下手机号不能重复
            if (exists)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            // 生成6位随机验证码
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();
            string templateParamJson = JsonConvert.SerializeObject(new
            {
                code = verificationCode
            });

            // 缓存验证码
            var key = string.Format(CacheConst.MerchantOnboardingPhoneVCodeKey, request.phone);
            var res = await redisClient.SetNxAsync(key, verificationCode, TimeSpan.FromMinutes(5));
            if (!res)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0095)]);

            // 发送验证码
            await aliyunSmsService.SendSmsAsync(request.phone, SmsConst.MerchantPaymentOnboardingVerificationCode, templateParamJson);
            return true;
        }
    }

    /// <summary>
    /// 添加二级商户支付方式命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="redisClient"></param>
    /// <param name="_paymentPlatformUtil"></param>
    public class InsertMerchantPaymentMethodCommandHandler(CoffeeMachineDbContext context, IRedisClient redisClient, PaymentPlatformUtil _paymentPlatformUtil) : ICommandHandler<InsertMerchantPaymentMethodCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(InsertMerchantPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            // 验证手机号
            var key = string.Format(CacheConst.MerchantOnboardingPhoneVCodeKey, request.phone);
            var vcode = await redisClient.GetAsync<string>(key);
            if (string.IsNullOrWhiteSpace(vcode) || vcode != request.code)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0035)]);
            await redisClient.DelAsync(key); // 验证通过后删除验证码

            // 验证账号是否存在
            var exists = await context.M_PaymentMethod.AnyAsync(x => x.Phone == request.phone && x.SystemPaymentMethodId == request.systemPaymentMethodId);// 同一个系统支付方式下手机号不能重复
            if (exists)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            // 验证系统支付方式是否存在
            var systemPaymentMethod = await context.SystemPaymentMethod.FirstOrDefaultAsync(x => x.Id == request.systemPaymentMethodId);
            if (systemPaymentMethod == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 验证系统支付服务商是否存在
            var systemPaymentServiceProvider = await context.SystemPaymentServiceProvider.AnyAsync(x => x.Id == systemPaymentMethod.PaymentPlatformId);
            if (!systemPaymentServiceProvider)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            #region 如果是添加已有商户信息，则验证商户信息是否正确
            var internalOnboardingStatus = InternalOnboardingStatusEnum.Pending;
            var result = true;
            var merchantId = string.IsNullOrWhiteSpace(request.MerchantId) ? null : request.MerchantId.Trim();

            if (!string.IsNullOrWhiteSpace(merchantId))
            {
                // 验证商户号是否存在
                var merchantIdExists = await context.M_PaymentMethod.AnyAsync(x => x.MerchantId == merchantId && x.SystemPaymentMethodId == request.systemPaymentMethodId);
                if (merchantIdExists)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

                var res = request.systemPaymentMethodId switch
                {
                    1002 => await _paymentPlatformUtil.VerifyWxNative(merchantId, systemPaymentMethod.PaymentPlatformId),
                    1001 => await _paymentPlatformUtil.VerifyAlipayNative(merchantId, systemPaymentMethod.PaymentPlatformId),
                    _ => throw ExceptionHelper.AppFriendly("不支持的支付方式"),
                };

                if (res) internalOnboardingStatus = InternalOnboardingStatusEnum.OnboardingSuccess;

                result = res;
            }

            if (!result && merchantId != null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0023)]);
            #endregion

            // 添加商户支付方式
            var merchantPaymentMethod = new M_PaymentMethod(request.systemPaymentMethodId, request.phone, internalOnboardingStatus, request.remark, systemPaymentMethod.PaymentPlatformId);

            // 添加商户号
            merchantPaymentMethod.SetMerchantId(merchantId);

            // 数据持久化
            await context.M_PaymentMethod.AddAsync(merchantPaymentMethod);

            return result;
        }
    }

    /// <summary>
    /// 更新商户支付方式备注
    /// </summary>
    /// <param name="context"></param>
    public class ModifyMerchantPaymentMethodRemarkCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ModifyMerchantPaymentMethodRemarkCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ModifyMerchantPaymentMethodRemarkCommand request, CancellationToken cancellationToken)
        {
            var info = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == request.merchantPaymentMethodId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.ModifyRemark(request.remark);

            // 更新备注
            context.M_PaymentMethod.Update(info);

            return true;
        }
    }

    /// <summary>
    /// 更改二级商户支付方式状态
    /// </summary>
    /// <param name="context"></param>
    public class ModifyPaymentMethodStatusCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ModifyPaymentMethodStatusCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ModifyPaymentMethodStatusCommand request, CancellationToken cancellationToken)
        {
            var info = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 只有进件成功才能启用/禁用操作
            if (info.PaymentEntryStatus != InternalOnboardingStatusEnum.OnboardingSuccess)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0024)]);

            await context.M_PaymentMethod.Where(w => w.Id == request.id).ExecuteUpdateAsync(w => w.SetProperty(e => e.IsEnabled, request.enabledEnum));
            return true;
        }
    }

    /// <summary>
    /// 删除二级商户支付方式
    /// </summary>
    /// <param name="context"></param>
    public class DeletePaymentMethodCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeletePaymentMethodCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var info = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 只有待提交状态能删除
            if (info.PaymentEntryStatus != InternalOnboardingStatusEnum.Pending)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0024)]);

            await context.M_PaymentMethod.FakeDeleteAsync(request.id);
            return true;
        }
    }

    /// <summary>
    /// 商户支付方式与设备绑定/解绑操作
    /// </summary>
    /// <param name="context"></param>
    public class PaymentMethodBindDevicesCommandHandler(CoffeeMachineDbContext context, PaymentPlatformUtil paymentPlatformUtil) : ICommandHandler<PaymentMethodBindDevicesCommand, List<DeviceBindPaymentMethodDto>>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<DeviceBindPaymentMethodDto>> Handle(PaymentMethodBindDevicesCommand request, CancellationToken cancellationToken)
        {
            var paymentMethodInfo = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == request.paymentMethodId);
            if (paymentMethodInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            if (paymentMethodInfo.PaymentEntryStatus != InternalOnboardingStatusEnum.OnboardingSuccess)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0024)]);

            // 如果存在则先删除，避免造成冗余数据
            if (await context.M_PaymentMethodBindDevice.AnyAsync(w => request.deviceIds.Contains(w.DeviceId) && w.SystemPaymentMethodId == request.SystemPaymentMethodId))
                await context.M_PaymentMethodBindDevice.Where(w => request.deviceIds.Contains(w.DeviceId) && w.SystemPaymentMethodId == request.SystemPaymentMethodId).ExecuteDeleteAsync();

            if (request.isBind)
            {
                var haveCount = await context.M_PaymentMethodBindDevice.AsQueryable().Where(w => w.SystemPaymentMethodId == request.SystemPaymentMethodId && request.deviceIds.Contains(w.DeviceId)).CountAsync();
                if (haveCount > 0)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0107)]);

                // 绑定设备
                var bindDevices = request.deviceIds.Select(s => new M_PaymentMethodBindDevice(request.paymentMethodId, s, request.SystemPaymentMethodId));
                await context.M_PaymentMethodBindDevice.AddRangeAsync(bindDevices);
            }

            var deviceBaseId = await context.DeviceInfo.AsQueryable().Where(w => request.deviceIds.Contains(w.Id)).Select(s => s.DeviceBaseId).Distinct().ToListAsync();
            var mids = await context.DeviceBaseInfo.Where(w => deviceBaseId.Contains(w.Id)).Select(s => s.Mid).Distinct().ToListAsync();

            // 获取支付设备包含的支付方式
            return await paymentPlatformUtil.GetDeviceBindPaymentMethodDtos(mids);
        }
    }
}