using YS.Cabinet.Payment.Alipay;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.V2;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 分账工具类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="alipayService"></param>
    /// <param name="logger"></param>
    public class ProfitSharingUnit(CoffeeMachineDbContext context, IWechatMerchantService _wechatMerchantService, IAlipayService alipayService, ILogger<ProfitSharingUnit> logger)
    {
        #region 支付宝分账

        /// <summary>
        /// 分账关系绑定
        /// </summary>
        /// <param name="requey"></param>
        /// <returns></returns>
        public async Task<bool> AliDirectPayAddReceiver(Alipay_TradeRoyaltyRelationBindRequest request, string paymentPlatformId)
        {
            var res = await alipayService.BuildMerchant(paymentPlatformId)
                .AgreemenService.AliDirectPayAddReceiver(request);

            if (!res.Success)
                throw ExceptionHelper.AppFriendly("添加分账接收方失败：" + res.Msg);

            return true;
        }

        /// <summary>
        /// 分账关系解绑
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paymentPlatformId"></param>
        /// <returns></returns>
        public async Task<bool> AliDirectPayRemoveReceiver(Alipay_TradeRoyaltyRelationUnbindRequest request, string paymentPlatformId)
        {
            var res = await alipayService.BuildMerchant(paymentPlatformId)
                .AgreemenService.AliDirectPayRemoveReceiver(request);

            if (!res.Success)
                throw ExceptionHelper.AppFriendly("分账关系解绑失败：" + res.Msg);

            return true;
        }

        /// <summary>
        /// 支付宝分账
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paymentPlatformId"></param>
        /// <returns></returns>
        public async Task<bool> AliDirectPaySettleAsync(Alipay_TradeOrderSettleRequest request, string paymentPlatformId)
        {
            var res = await alipayService.BuildMerchant(paymentPlatformId)
                .ProfitSharingService.AliDirectPaySettleAsync(request);

            if (!res.Success)
                throw ExceptionHelper.AppFriendly("支付宝分账失败：" + res.Msg);

            return true;
        }
        #endregion

        #region 微信分账

        /// <summary>
        /// 添加分账接收方
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="PaymentPlatformId"></param>
        /// <returns></returns>
        public async Task<bool> AddReceiverAsync(WxProfitSharingAddReceiverV2Request wxRequest, string PaymentPlatformId)
        {
            var wxACRes = await _wechatMerchantService
                        .BuildMerchant(PaymentPlatformId)
                        .ProfitSharingV2Service.ToResponseAsync(a => a.AddReceiverAsync(wxRequest));

            if (wxACRes == null)
                throw ExceptionHelper.AppFriendly("添加分账接收方失败");

            if (!wxACRes.Succeeded)
                throw ExceptionHelper.AppFriendly("添加分账接收方失败：" + wxACRes.Message);

            return true;
        }

        /// <summary>
        /// 删除分账接收方
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="PaymentPlatformId"></param>
        /// <returns></returns>
        public async Task<bool> WXProfitSharingRemovereceiver(WxProfitSharingRemoveReceiverV2Request wxRequest, string PaymentPlatformId)
        {
            var wxPSDeleteRes = await _wechatMerchantService
                        .BuildMerchant(PaymentPlatformId)
                        .ProfitSharingV2Service.ToResponseAsync(a => a.RemoveReceiverAsync(wxRequest));

            if (wxPSDeleteRes == null)
                throw ExceptionHelper.AppFriendly("删除分账接收方失败");

            if (!wxPSDeleteRes.Succeeded)
                throw ExceptionHelper.AppFriendly("删除分账接收方失败：" + wxPSDeleteRes.Message);

            return true;
        }

        /// <summary>
        /// 请求单次分账
        /// </summary>
        /// <param name="wxRequest"></param>
        /// <param name="PaymentPlatformId"></param>
        /// <returns></returns>
        public async Task<bool> ApplyProfitSharingAsync(WxProfitSharingApplyV2Request wxRequest, string PaymentPlatformId)
        {
            var wxAPSRes = await _wechatMerchantService
                        .BuildMerchant(PaymentPlatformId)
                        .ProfitSharingV2Service.ToResponseAsync(a => a.ApplyProfitSharingAsync(wxRequest));

            if (wxAPSRes == null)
                logger.LogError("请求单次分账失败");

            if (!wxAPSRes!.Succeeded)
                logger.LogError("请求单次分账失败：" + wxAPSRes.Message);

            return true;
        }
        #endregion
    }
}