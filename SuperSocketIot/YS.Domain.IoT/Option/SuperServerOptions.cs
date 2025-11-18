namespace YS.Domain.IoT.Option
{
    /// <summary>
    /// grpc配置
    /// </summary>
    public class SuperServerOptions
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        /// </summary>
        public int IdleSessionTimeOut { get; set; } = 300;

        /// <summary>
        /// 清理超时连接间隔
        /// </summary>
        public int ClearIdleSessionInterval { get; set; } = 120;

        /// <summary>
        /// 包处理超时
        /// </summary>
        public int PackageHandlingTimeOut { get; set; } = 30;

        /// <summary>
        /// 监听地址
        /// </summary>
        public List<ListenOptions> Listeners { get; set; }

        /// <summary>
        /// 监听地址
        /// </summary>
        public class ListenOptions
        {
            /// <summary>
            /// 监听地址
            /// </summary>
            public string Ip { get; set; }

            /// <summary>
            /// 监听端口
            /// </summary>
            public int Port { get; set; }
        }
    }
}
