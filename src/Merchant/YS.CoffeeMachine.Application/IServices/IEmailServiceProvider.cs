using YS.CoffeeMachine.Application.Dtos.EmailDtos;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public interface IEmailServiceProvider
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<(bool success, string response)> SendEmailSingleAsync(EmailObject email);

        /// <summary>
        /// 批量发送邮件
        /// </summary>
        Task<(bool success, string response)> SendEmailBatchAsync(EmailObject email);
    }
}