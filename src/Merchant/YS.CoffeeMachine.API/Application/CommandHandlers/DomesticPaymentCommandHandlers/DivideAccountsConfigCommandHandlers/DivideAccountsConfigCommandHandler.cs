using Aop.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V2;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.DivideAccountsConfigCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers.DivideAccountsConfigCommandHandlers
{
    /// <summary>
    /// 支付分账配置信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="_paymentPlatformUtil"></param>
    /// <param name="_logger"></param>
    public class DivideAccountsConfigCommandHandler(CoffeeMachineDbContext context,
        IWechatMerchantService _wechatMerchantService, PaymentPlatformUtil _paymentPlatformUtil,
        ILogger<DivideAccountsConfigCommandHandler> _logger, ProfitSharingUnit profitSharingUnit) : ICommandHandler<DivideAccountsConfigCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DivideAccountsConfigCommand request, CancellationToken cancellationToken)
        {
            var input = request.input;
            if (input == null)
                throw ExceptionHelper.AppFriendly("参数异常");

            var configs = await context.DivideAccountsConfig.IgnoreQueryFilters().Where(w => w.MerchantId == input.MerchantId).ToListAsync();
            if (configs != null && configs.Any())
            {
                if (configs != null && configs.Count > 5)
                    throw ExceptionHelper.AppFriendly($"商户{input.MerchantId}:{input.MerchantName}，配置的接收方账户数量已达上限");

                if (configs!.Any(w => w.name == input.name && w.account == input.account))
                    throw ExceptionHelper.AppFriendly($"商户{input.MerchantId}:{input.MerchantName}，配置的接收方账户和全称配置已经存在");

                var bibliography = configs!.Sum(x => x.Bibliography);
                if (bibliography + input.Bibliography > 100)
                {
                    throw ExceptionHelper.AppFriendly($"商户{input.MerchantName},已配置分账比列{bibliography}%,本次配置{input.Bibliography}%,大于100%");
                }
            }

            var mPaymentMethod = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == input.MerchantId);
            if (mPaymentMethod == null)
                throw ExceptionHelper.AppFriendly("支付方式参数异常");
            var systemPaymentMethodName = await context.SystemPaymentMethod.Where(w => w.Id == mPaymentMethod.SystemPaymentMethodId).Select(s => s.Name).FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(systemPaymentMethodName))
                throw ExceptionHelper.AppFriendly("商户参数异常");
            var systemPaymentServiceProvider = await context.SystemPaymentServiceProvider.FirstOrDefaultAsync(w => w.Id == mPaymentMethod.SystemPaymentServiceProviderId);
            if (systemPaymentServiceProvider == null)
                throw ExceptionHelper.AppFriendly("商户参数异常");

            var systemPaymentServiceProviderId = mPaymentMethod.SystemPaymentServiceProviderId.ToString();
            switch (input.SysPaymentMethodId)
            {
                case CommonConst.AlipaPaymenteId:
                    Alipay_TradeRoyaltyRelationBindRequest alipayRequest = new Alipay_TradeRoyaltyRelationBindRequest()
                    {
                        OutRequestNo = YitIdHelper.NextId().ToString(),
                        ReceiverList = new List<Alipay_RoyaltyRequest>()
                        {
                            // 这里暂时单个绑定，后面可优化为批量绑定
                            new Alipay_RoyaltyRequest()
                            {
                                Account = input.account,
                                AccountOpenId = input.account,
                                BindLoginName =input.account,
                                LoginName = input.account,
                                Memo = input.Remarks,
                                Name =input.name,
                                Type = input.alipayRoyaltyType.ToString(),
                            }
                        }
                    };

                    // 分账关系绑定
                    await profitSharingUnit.AliDirectPayAddReceiver(alipayRequest, systemPaymentServiceProviderId);
                    break;
                case CommonConst.WechatPaymentId:
                    WxProfitSharingAddReceiverV2Request wxRequest = new WxProfitSharingAddReceiverV2Request()
                    {
                        AppId = systemPaymentServiceProvider.AppletAppID,
                        MchId = systemPaymentServiceProvider.SpMerchantId,
                        SubMchId = mPaymentMethod.MerchantId,
                        Receiver = new WxProfitSharingReceiverInfo()
                        {
                            Type = input.type,
                            Account = input.account,
                            Name = input.name,
                            RelationType = input.relation_type.Value
                        }
                    };

                    // 添加分账接收方
                    await profitSharingUnit.AddReceiverAsync(wxRequest, systemPaymentServiceProviderId);
                    break;
            }

            // 持久化分账信息
            var divideAccountsConfigInfo = new DivideAccountsConfig(input.SysPaymentMethodId, input.MerchantId, systemPaymentMethodName, input.Bibliography, input.Remarks, input.type, input.account, input.name, input.relation_type, input.DeviceIds);
            await context.DivideAccountsConfig.AddAsync(divideAccountsConfigInfo);

            return true;
        }
    }

    /// <summary>
    /// 更改分账配置状态
    /// </summary>
    /// <param name="context"></param>
    public class UpdateDivideAccountsConfigEnabledCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateDivideAccountsConfigEnabledCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateDivideAccountsConfigEnabledCommand request, CancellationToken cancellationToken)
        {
            await context.DivideAccountsConfig
                .Where(w => w.Id == request.id)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.IsEnabled, request.enabled));
            return true;
        }
    }

    /// <summary>
    /// 编辑分账配置信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="profitSharingUnit"></param>
    public class DeleteDivideAccountsConfigEnabledCommandHandler(CoffeeMachineDbContext context, ProfitSharingUnit profitSharingUnit) : ICommandHandler<UpdateDivideAccountsConfigCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateDivideAccountsConfigCommand request, CancellationToken cancellationToken)
        {
            var input = request.input;
            var info = await context.DivideAccountsConfig.FirstOrDefaultAsync(w => w.Id == input.Id);
            if (info == null)
                throw ExceptionHelper.AppFriendly("数据不存在");

            var mPaymentMethod = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == input.MerchantId);
            if (mPaymentMethod == null)
                throw ExceptionHelper.AppFriendly("支付方式参数异常");
            var systemPaymentMethod = await context.SystemPaymentMethod.FirstOrDefaultAsync(w => w.Id == info.SysPaymentMethodId);
            if (systemPaymentMethod == null)
                throw ExceptionHelper.AppFriendly("商户参数异常");
            var systemPaymentServiceProvider = await context.SystemPaymentServiceProvider.FirstOrDefaultAsync(w => w.Id == systemPaymentMethod.PaymentPlatformId);
            if (systemPaymentServiceProvider == null)
                throw ExceptionHelper.AppFriendly("商户参数异常");

            var systemPaymentServiceProviderId = systemPaymentMethod.PaymentPlatformId.ToString();
            var res = false;
            switch (info.MerchantId)
            {
                case CommonConst.AlipaPaymenteId:
                    Alipay_TradeRoyaltyRelationUnbindRequest delRequest = new Alipay_TradeRoyaltyRelationUnbindRequest()
                    {
                        OutRequestNo = YitIdHelper.NextId().ToString(),
                        ReceiverList = new List<Alipay_RoyaltyRequest>()
                        {
                            // 这里暂时单个绑定，后面可优化为批量绑定
                            new Alipay_RoyaltyRequest()
                            {
                                Account = input.account,
                                AccountOpenId = input.account,
                                BindLoginName =input.account,
                                LoginName = input.account,
                                Memo = input.Remarks,
                                Name =input.name,
                                Type = input.alipayRoyaltyType.ToString(),
                            }
                        }
                    };

                    // 解除关系绑定
                    var alipayDelRes = await profitSharingUnit.AliDirectPayRemoveReceiver(delRequest, systemPaymentServiceProviderId);

                    // 删除成功再添加新的
                    if (alipayDelRes)
                    {
                        Alipay_TradeRoyaltyRelationBindRequest alipayRequest = new Alipay_TradeRoyaltyRelationBindRequest()
                        {
                            OutRequestNo = YitIdHelper.NextId().ToString(),
                            ReceiverList = new List<Alipay_RoyaltyRequest>()
                        {
                            // 这里暂时单个绑定，后面可优化为批量绑定
                            new Alipay_RoyaltyRequest()
                            {
                                Account = input.account,
                                AccountOpenId = input.account,
                                BindLoginName =input.account,
                                LoginName = input.account,
                                Memo = input.Remarks,
                                Name =input.name,
                                Type = input.alipayRoyaltyType.ToString(),
                            }
                        }
                        };

                        // 分账关系绑定
                        await profitSharingUnit.AliDirectPayAddReceiver(alipayRequest, systemPaymentServiceProviderId);
                    }
                    break;
                case CommonConst.WechatPaymentId:
                    // 先删除原先的
                    WxProfitSharingRemoveReceiverV2Request deleteWxRequest = new WxProfitSharingRemoveReceiverV2Request()
                    {
                        AppId = systemPaymentServiceProvider.AppletAppID,
                        MchId = systemPaymentServiceProvider.SpMerchantId,
                        SubMchId = mPaymentMethod.MerchantId,
                        Receiver = new WxProfitSharingReceiverInfo()
                        {
                            Type = input.type,
                            Account = input.account,
                            Name = input.name,
                            RelationType = input.relation_type!.Value
                        }
                    };
                    var delRes = await profitSharingUnit.WXProfitSharingRemovereceiver(deleteWxRequest, systemPaymentServiceProviderId);

                    // 删除成功再添加新的
                    if (delRes)
                    {
                        WxProfitSharingAddReceiverV2Request wxRequest = new WxProfitSharingAddReceiverV2Request()
                        {
                            AppId = systemPaymentServiceProvider.AppletAppID,
                            MchId = systemPaymentServiceProvider.SpMerchantId,
                            SubMchId = mPaymentMethod.MerchantId,
                            Receiver = new WxProfitSharingReceiverInfo()
                            {
                                Type = input.type,
                                Account = input.account,
                                Name = input.name,
                                RelationType = input.relation_type.Value
                            }
                        };

                        // 添加分账接收方
                        await profitSharingUnit.AddReceiverAsync(wxRequest, systemPaymentMethod.PaymentPlatformId.ToString());

                        res = true;
                    }
                    break;
            }

            if (res)
            {
                // 更新分账信息
                info.Update(input.SysPaymentMethodId, input.MerchantId, systemPaymentMethod.Name, input.Bibliography, input.Remarks, input.type, input.account, input.name, input.relation_type, input.DeviceIds);
                context.DivideAccountsConfig.Update(info);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除分账配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="profitSharingUnit"></param>
        public class UpdateDivideAccountsConfigEnabledCommandHandler(CoffeeMachineDbContext context, ProfitSharingUnit profitSharingUnit) : ICommandHandler<DeleteDivideAccountsConfigCommand, bool>
        {
            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> Handle(DeleteDivideAccountsConfigCommand request, CancellationToken cancellationToken)
            {
                var info = await context.DivideAccountsConfig.FirstOrDefaultAsync(w => w.Id == request.id);
                if (info == null)
                    throw ExceptionHelper.AppFriendly("数据不存在");

                var mPaymentMethod = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == info.MerchantId);
                if (mPaymentMethod == null)
                    throw ExceptionHelper.AppFriendly("支付方式参数异常");
                var systemPaymentMethod = await context.SystemPaymentMethod.FirstOrDefaultAsync(w => w.Id == info.SysPaymentMethodId);
                if (systemPaymentMethod == null)
                    throw ExceptionHelper.AppFriendly("商户参数异常");
                var systemPaymentServiceProvider = await context.SystemPaymentServiceProvider.FirstOrDefaultAsync(w => w.Id == systemPaymentMethod.PaymentPlatformId);
                if (systemPaymentServiceProvider == null)
                    throw ExceptionHelper.AppFriendly("商户参数异常");

                var systemPaymentServiceProviderId = mPaymentMethod.SystemPaymentServiceProviderId.ToString();
                var res = false;
                switch (info.MerchantId)
                {
                    // TODO：待开发
                    case CommonConst.AlipaPaymenteId:
                        Alipay_TradeRoyaltyRelationUnbindRequest delRequest = new Alipay_TradeRoyaltyRelationUnbindRequest()
                        {
                            OutRequestNo = YitIdHelper.NextId().ToString(),
                            ReceiverList = new List<Alipay_RoyaltyRequest>()
                        {
                            // 这里暂时单个绑定，后面可优化为批量绑定
                            new Alipay_RoyaltyRequest()
                            {
                                Account = info.account,
                                AccountOpenId = info.account,
                                BindLoginName =info.account,
                                LoginName = info.account,
                                Memo = info.Remarks,
                                Name =info.name,
                                Type = info.AlipayRoyaltyType.ToString(),
                            }
                        }
                        };

                        var alipayDelRes = await profitSharingUnit.AliDirectPayRemoveReceiver(delRequest, systemPaymentServiceProviderId);
                        break;
                    case CommonConst.WechatPaymentId:
                        // 删除原先的
                        WxProfitSharingRemoveReceiverV2Request deleteWxRequest = new WxProfitSharingRemoveReceiverV2Request()
                        {
                            AppId = systemPaymentServiceProvider.AppletAppID,
                            MchId = systemPaymentServiceProvider.SpMerchantId,
                            SubMchId = mPaymentMethod.MerchantId,
                            Receiver = new WxProfitSharingReceiverInfo()
                            {
                                Type = info.type,
                                Account = info.account,
                                Name = info.name,
                                RelationType = info.relation_type!.Value
                            }
                        };
                        res = await profitSharingUnit.WXProfitSharingRemovereceiver(deleteWxRequest, systemPaymentServiceProviderId);
                        break;
                }

                if (res)
                {
                    // 删除分账信息
                    await context.DivideAccountsConfig.FakeDeleteAsync(info.Id);
                    return true;
                }

                return false;
            }
        }
    }
}