namespace YS.Provider.OSS
{
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using YS.Provider.OSS.Interface.Base;
    using YS.Provider.OSS.Services;
    using Microsoft.Extensions.Options;
    using YS.Provider.OSS.Enum;

    /// <summary>
    /// OSSServiceFactory
    /// </summary>
    public class OSSServiceFactory : IOSSServiceFactory
    {
        private readonly IOptionsMonitor<OSSOptions> optionsMonitor;
        private readonly ICacheProvider _cache;
        private readonly ILoggerFactory logger;

        /// <summary>
        /// OSSServiceFactory
        /// </summary>
        /// <param name="optionsMonitor"></param>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OSSServiceFactory(IOptionsMonitor<OSSOptions> optionsMonitor
            , ICacheProvider provider
            , ILoggerFactory logger)
        {
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException();
            this._cache = provider ?? throw new ArgumentNullException(nameof(IMemoryCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(ILoggerFactory));
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public IOSSService Create()
        {
            return Create(DefaultOptionName.Name);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public IOSSService Create(string name)
        {
            #region

            if (string.IsNullOrEmpty(name))
            {
                name = DefaultOptionName.Name;
            }
            var options = optionsMonitor.Get(name);
            if (options == null ||
                (options.Provider == OSSProviderEnum.Invalid
                && string.IsNullOrEmpty(options.Endpoint)
                && string.IsNullOrEmpty(options.SecretKey)
                && string.IsNullOrEmpty(options.AccessKey)))
                throw new ArgumentException($"Cannot get option by name '{name}'.");
            if (options.Provider == OSSProviderEnum.Invalid)
                throw new ArgumentNullException(nameof(options.Provider));
            if (string.IsNullOrEmpty(options.SecretKey))
                throw new ArgumentNullException(nameof(options.SecretKey), "SecretKey can not null.");
            if (string.IsNullOrEmpty(options.AccessKey))
                throw new ArgumentNullException(nameof(options.AccessKey), "AccessKey can not null.");
            if ((options.Provider == OSSProviderEnum.Minio
                || options.Provider == OSSProviderEnum.AWS)
                && string.IsNullOrEmpty(options.Region))
            {
                throw new ArgumentNullException(nameof(options.Region), "When your provider is AWS, region can not null.");
            }

            #endregion

            switch (options.Provider)
            {
                case OSSProviderEnum.Aliyun:
                    return new AliyunOSSService(_cache, options);
                case OSSProviderEnum.AWS:
                    return new AwsOssService(_cache, options, logger);
                default:
                    throw new Exception("Unknow provider type");
            }
        }
    }

    /// <summary>
    /// DefaultOptionName
    /// </summary>
    public class DefaultOptionName
    {
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "default";
    }
}
