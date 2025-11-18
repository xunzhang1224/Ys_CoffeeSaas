
using YS.CoffeeMachine.Application.Dtos.IotCardDtos;

namespace YS.CoffeeMachine.Application.IServices.ILotCartService
{
    /// <summary>
    /// 流量卡接口
    /// </summary>
    public interface ILotCardApi
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<HttpLotCardSingleResult<LotCardUserInfo>> SigninAsync(LotCardAccountLoginInput input);

        /// <summary>
        /// 获取最佳策略
        /// </summary>
        /// <param name="iccid"></param>
        /// <param name="account"></param>
        /// <param name="accountFrom"></param>
        /// <returns></returns>
        Task<HttpLotCardResult<PoclicyPackageOut>> GetBestPackage(string iccid, string account = "1", string accountFrom = "1");

        /// <summary>
        /// 获取流量卡充值信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<HttpLotCardResult<List<PoclicyDetailOut>>> GetPoclicyDetail(List<IotCardInput> input);

        /// <summary>
        /// 用户充值续订
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        Task<string> UserRechargeRenewal(List<UserRechargeRenewal> input, string Authorization = default);

        /// <summary>
        /// 查询锁卡状态
        /// </summary>
        /// <param name="iccid"></param>
        /// <returns></returns>
        Task<HttpLotCardSingleResult<CardLockStatusOut>> GetCardLockStatus(string iccid);
    }
}