using YS.Cabinet.Payment.WechatPay.Application.V3.Responses.Common;
using YS.CoffeeMachine.Application.Dtos.DomesticPayment;
using static YS.Cabinet.Payment.WechatPay.V3.WxBankResponse;

namespace YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries
{
    /// <summary>
    /// 微信支付相关查询接口类
    /// </summary>
    public interface IWechatPaymentQueries
    {
        /// <summary>
        /// 根据银行账号获取银行信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<List<WxBanksData>> GetBankByAccount(string account);

        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <param name="type">银行列表类型(1=对公 2=对私)</param>
        /// <returns></returns>
        Task<List<WxBanksData>> GetBankList(int type = 1);

        /// <summary>
        /// 获取银行支行列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<WxBranchBankResponse.BankBranchItem>> GetBankBranchList(GetBankBranchListInput input);

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        Task<List<GetProvinceCityOutput>> GetProvinces();

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        Task<List<GetProvinceCityOutput>> GetCitys(long provinceCode);

        /// <summary>
        /// 获取区县列表
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        Task<List<GetProvinceCityOutput>> GetAreas(long cityCode);
    }
}