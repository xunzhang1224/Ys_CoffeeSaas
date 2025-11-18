using MagicOnion;

namespace YS.CoffeeMachine.Iot.Application.DownSend.GRPC
{
    /// <summary>
    /// SocketGrpcService
    /// </summary>
    public interface ISocketGrpcService : IService<ISocketGrpcService>
    {
        /// <summary>
        /// 发送Socket消息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="body"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        UnaryResult<string> SendSocketMessage(string clientId, string body, string mid);

        /// <summary>
        /// 查询Socket客户端
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        UnaryResult<bool> QueryClient(string mid);

        /// <summary>
        /// 发送Socket消息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        UnaryResult<string> SendMessage(string mid, byte[] body);

        /// <summary>
        /// 测试Socket消息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        UnaryResult<string> TestMessage(string mid);
    }
}