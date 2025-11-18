namespace YS.CoffeeMachine.API.Extensions.Cap.Dtos
{
    /// <summary>
    /// 邮件通知
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// 设备id
        /// 判断设备是否已通知该类型邮件
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public long EnterpriseinfoId { get; set; }

        /// <summary>
        /// 收件人的电子邮件地址
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// 邮件的正文内容
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// 邮件的主题
        /// </summary>
        public string Subject { get; set; }
    }

    /// <summary>
    /// 短信
    /// </summary>
    public class SmsDto
    {
        /// <summary>
        /// 邮件的主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public long EnterpriseinfoId { get; set; }

        /// <summary>
        /// 设备id
        /// 判断设备是否已通知该类型邮件
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public List<string> PhoneNumbers { get; set; }

        /// <summary>
        /// 短信
        /// </summary>
        public Dictionary<string, string> MessageBodyDic { get; set; }
    }
}
