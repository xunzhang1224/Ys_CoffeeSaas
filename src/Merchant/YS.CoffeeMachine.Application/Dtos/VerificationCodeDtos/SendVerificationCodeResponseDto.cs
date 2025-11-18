namespace YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos
{
    /// <summary>
    /// 发送验证码响应
    /// </summary>
    public class SendVerificationCodeResponseDto
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 验证码有效期（单位：分钟）
        /// </summary>
        public int ExpireTime { get; set; }

        /// <summary>
        /// 邮件/短信发送结果
        /// </summary>
        public string Message { get; set; }
    }
}