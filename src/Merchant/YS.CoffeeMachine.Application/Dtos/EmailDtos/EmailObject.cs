namespace YS.CoffeeMachine.Application.Dtos.EmailDtos
{
    /// <summary>
    /// 邮件对象
    /// </summary>
    public class EmailObject
    {
        /// <summary>
        /// 收件人的电子邮件地址
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// 邮件的签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 邮件的正文内容
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// 额外数据的键值对集合
        /// </summary>
        public IDictionary<string, string> Data { get; set; }

        /// <summary>
        /// 邮件的主题
        /// </summary>
        public string Subject { get; set; }
    }
}
