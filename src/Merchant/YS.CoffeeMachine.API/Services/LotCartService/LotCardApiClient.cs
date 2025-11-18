using YS.CoffeeMachine.Application.Dtos.IotCardDtos;
using YS.CoffeeMachine.Application.IServices.ILotCartService;
using YS.CoffeeMachine.Domain.Shared.Const;

namespace YS.CoffeeMachine.API.Services.LotCartService
{
    /// <summary>
    /// 流量卡接口客户端
    /// </summary>
    /// <param name="_executor"></param>
    public class LotCardApiClient(IHttpApiExecutor _executor) : ILotCardApi
    {
        private const string BaseUrl = CommonConst.IotCardApiUrl;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<HttpLotCardSingleResult<LotCardUserInfo>> SigninAsync(LotCardAccountLoginInput input)
        {
            string url = $"{BaseUrl}User/signin";
            var res = await _executor.PostAsync<HttpLotCardSingleResult<LotCardUserInfo>>(url, input);
            return res;
        }

        /// <summary>
        /// 查询最佳策略
        /// </summary>
        /// <param name="iccid"></param>
        /// <param name="account"></param>
        /// <param name="accountFrom"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<HttpLotCardResult<PoclicyPackageOut>> GetBestPackage(string iccid, string account = "1", string accountFrom = "1")
        {
            string url = $"{BaseUrl}/YsIotCard/api/v3/OuterFace/GetBestPackage/?iccid={iccid}&account={account}&accountFrom={accountFrom}";
            var res = await _executor.GetAsync<HttpLotCardResult<PoclicyPackageOut>>(url);
            return res;
        }

        /// <summary>
        /// 查询锁卡状态
        /// </summary>
        /// <param name="iccid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<HttpLotCardSingleResult<CardLockStatusOut>> GetCardLockStatus(string iccid)
        {
            string url = $"{BaseUrl}/YsIotCard/api/v3/OuterFace/GetCardLockStatus/?iccid={iccid}";
            var res = await _executor.GetAsync<HttpLotCardSingleResult<CardLockStatusOut>>(url);
            return res;
        }

        /// <summary>
        /// 查询流量卡充值信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<HttpLotCardResult<List<PoclicyDetailOut>>> GetPoclicyDetail(List<IotCardInput> input)
        {
            string url = $"{BaseUrl}/YsIotCard/api/v3/OuterFace/GetPoclicyDetail";
            var res = await _executor.PostAsync<HttpLotCardResult<List<PoclicyDetailOut>>>(url, input);
            return res;
        }

        /// <summary>
        /// 用户充值续订
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> UserRechargeRenewal(List<UserRechargeRenewal> input, string Authorization = null)
        {
            string url = $"{BaseUrl}/YsIotCard/api/v3/OuterFace/UserRechargeRenewal";
            var res = await _executor.PostAsync<string>(url, input);
            return res;
        }
    }
}