namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 阿里云短信服务接口
    /// </summary>
    public interface IAliyunSmsService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="templateCode"></param>
        /// <param name="paramJson"></param>
        /// <returns></returns>
        Task<(bool, string)> SendSmsAsync(string phoneNumber, string templateCode, string paramJson);
    }
}
