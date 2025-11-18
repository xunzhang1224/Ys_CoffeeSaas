using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DomesticPaymentCommandHandlers
{
    /// <summary>
    /// 微信商户进件命令处理器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_cap"></param>
    /// <param name="_user"></param>
    public class WechatApplymentCommandHandler(CoffeeMachineDbContext context, IMapper mapper, IPublishService _cap, UserHttpContext _user) : ICommandHandler<WechatApplymentCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(WechatApplymentCommand request, CancellationToken cancellationToken)
        {
            var input = request.input;

            // 入参验证
            if (input.OrganizationType != WxApplymentSubjectTypeEnum.SUBJECT_TYPE_MICRO)
                if (string.IsNullOrEmpty(input.BusinessLicenseCopy) || string.IsNullOrEmpty(input.BusinessLicenseNumber) || string.IsNullOrEmpty(input.MerchantName))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            // 获取二级商户支付方式
            var paymentMethodInfo = await context.M_PaymentMethod.FirstOrDefaultAsync(w => w.Id == input.PaymentOriginId);
            if (paymentMethodInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            if (input.MerchantType == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            paymentMethodInfo.SetDomesticMerchantType(input.MerchantType!.Value);
            context.M_PaymentMethod.Update(paymentMethodInfo); // 二级商户支付方式更新商户类型

            if (await context.M_PaymentWechatApplyments.AnyAsync(w => w.PaymentOriginId == input.Id))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            var isAdd = input.Id == 0;
            long nid = 0;

            if (isAdd)
            {
                // 进件信息
                var info = mapper.Map<M_PaymentWechatApplyments>(input);
                info.SetId(YitIdHelper.NextId());
                info.SetFlowStatus(ApplymentFlowStatusEnum.Initialize);
                info.SetArtificialApplyment(ArtificialApplymentEnum.No);
                info.EnterpriseinfoId = _user.TenantId;
                info.LastModifyTime = DateTime.Now;
                info.LastModifyUserId = _user.UserId;
                info.LastModifyUserName = _user.NickName;
                nid = info.Id;
                await context.M_PaymentWechatApplyments.AddAsync(info);
            }
            else
            {
                var sourceInfo = await context.M_PaymentWechatApplyments
                    .FirstAsync(w => w.Id == input.Id);

                // 将 input 的内容映射到数据库已存在的实体
                mapper.Map(input, sourceInfo);

                // 更新进件状态
                sourceInfo.SetFlowStatus(ApplymentFlowStatusEnum.Initialize);

                // 保持一些字段不被覆盖
                sourceInfo.CreateTime = sourceInfo.CreateTime;
                sourceInfo.LastModifyTime = DateTime.Now;
                sourceInfo.LastModifyUserId = _user.UserId;
                sourceInfo.LastModifyUserName = _user.NickName;
                nid = sourceInfo.Id;

                context.M_PaymentWechatApplyments.Update(sourceInfo);
            }

            // 发送微信商户进件队列消息
            await _cap.SendMessage(CapConst.WechatMerchantIncomingSubmit, nid);

            return true;
        }
    }
}