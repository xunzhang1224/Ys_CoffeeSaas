namespace YS.Provider.OSS.Interface
{
    using YS.Provider.OSS.Interface.Base;
    using YS.Provider.OSS.Model;

    /// <summary>
    /// mini oss 服务
    /// </summary>
    public interface IMinioOSSService : IOSSService
    {
        /// <summary>
        /// RemoveIncompleteUploadAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Task<bool> RemoveIncompleteUploadAsync(string bucketName, string objectName);

        /// <summary>
        /// ListIncompleteUploads
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<List<ItemUploadInfo>> ListIncompleteUploads(string bucketName);

        /// <summary>
        /// GetPolicyAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<PolicyInfo> GetPolicyAsync(string bucketName);

        /// <summary>
        /// SetPolicyAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        Task<bool> SetPolicyAsync(string bucketName, List<StatementItem> statements);

        /// <summary>
        /// RemovePolicyAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<bool> RemovePolicyAsync(string bucketName);

        /// <summary>
        /// PolicyExistsAsync
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        Task<bool> PolicyExistsAsync(string bucketName, StatementItem statement);
    }
}
