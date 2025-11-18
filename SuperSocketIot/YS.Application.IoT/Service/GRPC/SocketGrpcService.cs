namespace YS.Application.IoT.Service.GRPC
{
    using MagicOnion;
    using MagicOnion.Server;
    using System.Threading.Tasks;
    using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;

    /// <summary>
    /// Socket gRPC 服务实现类。
    /// 提供与设备通信的基础接口，如消息发送、客户端查询等。
    /// </summary>
    public class SocketGrpcService : ServiceBase<ISocketGrpcService>, ISocketGrpcService
    {
        /// <summary>
        /// 初始化一个新的 SocketGrpcService 实例。
        /// </summary>
        public SocketGrpcService()
        {
        }

        /// <summary>
        /// 查询指定设备是否处于连接状态。
        /// </summary>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>异步操作结果，指示设备是否在线。</returns>
        public async UnaryResult<bool> QueryClient(string mid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 向指定设备发送二进制格式的消息。
        /// </summary>
        /// <param name="mid">目标设备的唯一标识。</param>
        /// <param name="body">要发送的二进制数据内容。</param>
        /// <returns>异步操作结果，表示是否成功发送。</returns>
        public async UnaryResult<string> SendMessage(string mid, byte[] body)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送字符串格式的消息到指定客户端。
        /// </summary>
        /// <param name="clientId">客户端唯一标识。</param>
        /// <param name="body">消息内容。</param>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>操作结果信息。</returns>
        public UnaryResult<string> SendSocketMessage(string clientId, string body, string mid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 测试用方法，模拟异步返回测试字符串。
        /// </summary>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>固定返回字符串 "a"。</returns>
        public async UnaryResult<string> TestMessage(string mid)
        {
            await Task.Delay(100);
            return "a";
        }
    }
}