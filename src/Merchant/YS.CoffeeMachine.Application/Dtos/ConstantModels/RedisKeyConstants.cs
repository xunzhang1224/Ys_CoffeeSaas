namespace YS.CoffeeMachine.Application.Dtos.ConstantModels
{
    /// <summary>
    /// Redis Key
    /// </summary>
    public static class RedisKeyConstants
    {
        /// <summary>
        /// 邮箱登录验证码Key
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailLoginVCodeKey(string email) => $"/verificationcode/merchantemaillogin/{email}";

        /// <summary>
        /// 短信登录验证码Key
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string GetSmsLoginVCodeKey(string phone) => $"/verificationcode/merchantsmslogin/{phone}";

        /// <summary>
        /// 修改密码验证码Key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetUserVCodeKey(long userId) => $"/verificationcode/merchantUpdatePwd/{userId}";
    }
}
