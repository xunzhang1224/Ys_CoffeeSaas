namespace YS.Provider.OSS.Util
{

    using YS.Provider.OSS.Interface.Base;

    /// <summary>
    /// 提供与OSS服务交互的方法
    /// </summary>
    public class OssUnil
    {
        private readonly IOSSService _iOSSService;

        /// <summary>
        /// 构造函数，初始化IOSSService实例
        /// </summary>
        /// <param name="iOSSService">OSS服务接口实例</param>
        public OssUnil(IOSSService iOSSService)
        {
            _iOSSService = iOSSService;
        }

        /// <summary>
        /// 获取OSS服务的上传和下载链接
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="directory">目录名</param>
        /// <returns>包含上传和下载链接的元组</returns>
        public async Task<(string, string)> OssPutAndGetUrlAsync(string fileName, string directory)
        {
            // 获取文件类型
            var fileType = fileName.Split('.').Last();
            // 从文件名中移除文件类型
            fileName = fileName.Replace($".{fileType}", string.Empty);
            // 生成新的目录名，包含唯一标识符
            string newDirectoryName = $"img/{directory}/{fileName}_{Guid.NewGuid().ToString()}.{fileType}";
            // 获取上传链接
            var Puturl = await _iOSSService.PresignedPutObjectAsync("ourvend-kfj", newDirectoryName, 60 * 30);
            // 获取下载链接
            var Geturl = await _iOSSService.PresignedGetObjectAsync("ourvend-kfj", newDirectoryName, 900);
            // 返回包含上传和下载链接的元组
            return (Puturl, Geturl);
        }
    }
}
