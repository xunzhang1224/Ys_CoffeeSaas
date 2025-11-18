using SuperSocket.Server.Abstractions.Session;
using System.Text;
using System.Text.Json;
using YS.Application.IoT.Service.Http;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.Domain.IoT.Session;
namespace YS.Application.IoT.Service.ReplyCommand
{
    /// <summary>
    /// 命令回复
    /// </summary>
    public class ReplyCommandService : IReplyCommandService
    {
        private readonly IHttp _http;
        private readonly SessionManager<SocketSession> _sessionManager;
        private readonly ISuperSessionContainer _sessionContainer;
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        /// <summary>
        /// 构造函数
        /// </summary>
        public readonly ILogger<ReplyCommandService> _replyLogger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="http"></param>
        /// <param name="replyLogger"></param>
        /// <param name="superSession"></param>
        /// <param name="sessionManager"></param>
        public ReplyCommandService(
            IHttp http,
            ILogger<ReplyCommandService> replyLogger,
            ISuperSessionContainer superSession,
            SessionManager<SocketSession> sessionManager)
        {
            _http = http;
            _replyLogger = replyLogger;
            _sessionContainer = superSession;
            _sessionManager = sessionManager;
        }
        /// <summary>
        /// To: 可以做收到与回复的日志写入
        /// 通用回复函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">返回信息</param>
        /// <param name="request">请求信息</param>
        /// <param name="newPrvkey">1000协议才传入</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SendAsync<T>(T response, MessageContext request, string newPrvkey = null) where T : BaseCmd
        {
            var body = JsonSerializer.Serialize(response, jsonSerializerOptions);
            var prvkey = await _http.GetPrivKey(request.Mid);
            //设置key
            var key = EcodeKey(request.Key, request.Version, request.Mid, request.MessageId);
            byte[] newbody = [];

            var socketSession = _sessionContainer.GetSessionByID(request.SessionId);

            if (socketSession == null)
            {
                _replyLogger.LogWarning($"设备{request.Mid} 不在线");
                return false;
            }

            if (request.Key == "1000")//初始化的时候特殊处理
            {
                ///设置签名
                newbody = Encoding.GetEncoding("GB2312").GetBytes(CommandExtension.GetAgreement(body, key, newPrvkey, response.Mid, response.TimeSp.ToString()));
                // await SendAsync(request.ClientId, newbody);//没有登录只能客户端Id回复
            }
            else
            {
                if (request.Mid != response.Mid)
                {
                    //请求的设备号不等于返回的设备号
                    _replyLogger.LogError($"request:{request.Mid},response:{response.Mid}Mid不匹配");
                    throw new Exception("Mid不匹配");
                }
                ///设置签名
                newbody = Encoding.UTF8.GetBytes(CommandExtension.GetAgreement(body, key, prvkey.PrivaKey, request.Mid, response.TimeSp.ToString()));

                //await SendAsync(request.Mid, newbody);//登录成功用设备号回复
            }
            if (socketSession == null || socketSession is SocketSession socket == false) return false;
            if (request.Key == "1000")
            {
                socket.IsTmp = true;
            }
            else if (request.Key == "1111")
            {
                socket.IsLogin = true;
                socket.Mid = request.Mid;
                await _sessionManager.TryAddOrUpdateAsync(request.Mid, socket);
            }
            await SendAsync(socketSession, newbody);
            request.SetResponse(DateTime.Now.Ticks.ToString(), body);
            return true;
        }

        /// <summary>
        /// 协议回复
        /// </summary>
        /// <param name="response"></param>
        /// <param name="command"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync<T>(T response, string command, string messageId = null) where T : BaseCmd
        {
            try
            {
                var body = JsonSerializer.Serialize(response, jsonSerializerOptions);
                var prvkey = await _http.GetPrivKey(response.Mid);
                //设置key
                var key = EcodeKey(command, "1", response.Mid, messageId ?? YitIdHelper.NextId().ToString());
                var nowBody = CommandExtension.GetAgreement(body, key, prvkey.PrivaKey, response.Mid, response.TimeSp.ToString());
                var socketSession = await _sessionManager.TryGetAsync(response.Mid) as IAppSession;
                await SendAsync(socketSession, Encoding.UTF8.GetBytes(nowBody));
                _replyLogger.LogWarning($"指令发送:{command}------->{nowBody}");
                Console.WriteLine($"指令发送:{command}------->{nowBody}");
            }
            catch (Exception ex)
            {
                _replyLogger.LogError(ex, $"发送出错:设备编号:{response.Mid}|指令:{command}");
                return false;
            }
            return true;
        }

        private string EcodeKey(string key, string version, string mid, string messageId)
        {

            if (string.IsNullOrEmpty(version))
            {
                return key;
            }
            else
            {
                return $"{key}-{mid}-{messageId}-{1}";
            }
        }

        /// <summary>
        /// 不发送
        /// </summary>
        public async Task<bool> NoSendAsync(MessageContext request)
        {
            request.SetResponse(DateTime.Now.Ticks.ToString(), "0");
            return await Task.FromResult(true);
        }

        /// <summary>
        ///回复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        private async Task SendAsync(IAppSession id, ReadOnlyMemory<byte> memory)
        {
            await id.SendAsync(memory);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public async Task CloseAsync(string sessionId)
        {
            var socketSession = _sessionContainer.GetSessionByID(sessionId);

            if (socketSession == null)
            {
                return;
            }

            await socketSession.CloseAsync(SuperSocket.Connection.CloseReason.ProtocolError);
        }
    }
}
