namespace YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos
{
    /// <summary>
    /// 邮箱验证码
    /// </summary>
    public class EmailVCodeDto
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 验证码状态 0：未验证 1：已验证
        /// </summary>
        public int Status { get; set; }
    }
}
