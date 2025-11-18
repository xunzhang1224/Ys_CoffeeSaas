namespace YS.Domain.IoT.Option
{
    /// <summary>
    /// grpc配置
    /// </summary>
    public class GrpcOptions
    {
        /// <summary>
        /// grpc端口
        /// </summary>
        public int GrpcPort { get; set; } = 11006;

        /// <summary>
        /// grpc consul端口
        /// </summary>
        public int GrpcConsulPort { get; set; } = 22222;

        /// <summary>
        /// grpc服务地址
        /// </summary>
        public string GrpcHost { get; set; } = "127.0.0.1";

        /// <summary>
        /// grpc服务名称
        /// </summary>
        public string GrpcName { get; set; } = "YS.Provider.Grpc";

        /// <summary>
        /// 是否开启日志
        /// </summary>
        public bool EnableLog { get; set; } = true;
    }
}
