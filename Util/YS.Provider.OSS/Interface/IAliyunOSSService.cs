namespace YS.Provider.OSS.Interface
{
    using YS.Provider.OSS.Interface.Base;

    /// <summary>
    /// 阿里云oss服务
    /// </summary>
    public interface IAliyunOSSService : IOSSService
    {
        /// <summary>
        /// 获取储存桶地域
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<string> GetBucketLocationAsync(string bucketName);

        /// <summary>
        /// 获取桶外部访问URL
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<string> GetBucketEndpointAsync(string bucketName);
    }
}
