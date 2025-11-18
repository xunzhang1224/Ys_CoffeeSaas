using YS.CoffeeMachine.Application.Dtos.EmailDtos;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 邮件相关接口
    /// </summary>
    public interface IEmailServiceProvider
    {
        /// <summary>
        /// 发用邮件接口
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<(bool success, string response)> SendEmailSingleAsync(EmailObject email);

        /// <summary>
        /// 发用邮件接口
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<(bool success, string response)> SendEmailBatchAsync(EmailObject email);
    }
}
