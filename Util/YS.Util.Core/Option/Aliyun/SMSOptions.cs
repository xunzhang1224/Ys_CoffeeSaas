namespace YS.Util.Core.Option.Aliyun
{
    /// <summary>
    /// 短信配置选项
    /// </summary>
    public sealed class SMSOptions
    {
        /// <summary>
        /// Aliyun
        /// </summary>
        public SMSSettings Aliyun { get; set; }
    }

    /// <summary>
    /// 短信服务设置
    /// </summary>
    public sealed class SMSSettings
    {
        /// <summary>
        /// SdkAppId
        /// </summary>
        public string SdkAppId { get; set; }

        /// <summary>
        /// AccessKey ID
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// AccessKey Secret
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// Templates
        /// </summary>
        public List<SmsTemplate> Templates { get; set; }

        /// <summary>
        /// GetTemplate
        /// </summary>
        public SmsTemplate GetTemplate(string code)
        {
            foreach (var template in Templates)
            {
                if (template.TemplateCode == code) { return template; }
            }
            return null;
        }
    }

    /// <summary>
    /// 短信模板
    /// </summary>
    public class SmsTemplate
    {
        /// <summary>
        /// 短信签名名称
        /// </summary>
        public string SignName { get; set; }

        /// <summary>
        /// 短信模板CODE
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短信模板内容
        /// </summary>
        public string Content { get; set; }
    }

}