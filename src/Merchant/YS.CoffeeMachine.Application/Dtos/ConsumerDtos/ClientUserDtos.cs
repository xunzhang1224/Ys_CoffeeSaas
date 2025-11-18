namespace YS.CoffeeMachine.Application.Dtos.ConsumerDtos
{
    /// <summary>
    /// 消费者端登录响应
    /// </summary>
    public class ClientLoginResponseDto
    {
        /// <summary>
        /// 用户的访问令牌 (Access Token)
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户的刷新令牌 (Refresh Token)
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 访问令牌的过期时间 (UTC 时间)
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string? Phone { get; set; } = null;
    }
}