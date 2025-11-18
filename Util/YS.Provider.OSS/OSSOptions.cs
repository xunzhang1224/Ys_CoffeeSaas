namespace YS.Provider.OSS
{
    using YS.Provider.OSS.Enum;

    /// <summary>
    /// OSSOptions
    /// </summary>
    public class OSSOptions
    {
        /// <summary>
        /// 枚举，OOS提供商
        /// </summary>
        public OSSProviderEnum Provider { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }

        private string _region = "us-east-1";

        /// <summary>
        /// 地域
        /// </summary>
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _region = "us-east-1";
                }
                else
                {
                    _region = value;
                }
            }
        }

        /// <summary>
        /// 上传限制大小
        /// </summary>
        public int UploadLimitSize { get; set; }

        /// <summary>
        /// 是否启用HTTPS
        /// </summary>
        public bool IsEnableHttps { get; set; } = true;

        /// <summary>
        /// 是否启用缓存，默认缓存在MemeryCache中（可使用自行实现的缓存替代默认缓存）
        /// </summary>
        public bool IsEnableCache { get; set; } = false;

        /// <summary>
        /// CDN地址
        /// </summary>
        public string CDNAddress { get; set; }
    }
}
