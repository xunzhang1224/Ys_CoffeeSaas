namespace YS.Provider.OSS.Interface.Base
{
    /// <summary>
    /// oss服务工厂
    /// </summary>
    public interface IOSSServiceFactory
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        IOSSService Create();

        /// <summary>
        /// 创建（名字）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        IOSSService Create(string name);
    }
}
