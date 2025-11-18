using YS.CoffeeMachine.Iot.Domain.CommandEntities;

namespace YS.Application.IoT.Service.ReplyCommand
{
    /// <summary>
    /// 命令回复服务
    /// </summary>
    public interface IReplyCommandService
    {
        /// <summary>
        /// 通用回复函数
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <param name="newPrvkey"></param>
        /// <returns></returns>
        Task<bool> SendAsync<T>(T response, MessageContext request, string newPrvkey = null) where T : BaseCmd;

        /// <summary>
        /// 平台调用发送函数 不会记录日志
        /// </summary>
        /// <param name="response"></param>
        /// <param name="command"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<bool> SendAsync<T>(T response, string command, string messageId = null) where T : BaseCmd;

        ///// <summary>
        ///// 通用下发
        ///// </summary>
        ///// <param name="response"></param>
        ///// <param name="command"></param>
        ///// <param name="messageId"></param>
        ///// <returns></returns>
        //Task<bool> SendNewAsync<T>(T response, string command, string messageId = null) where T : class;

        /// <summary>
        /// 不回复 只记录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> NoSendAsync(MessageContext request);

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task CloseAsync(string sessionId);
    }
}
