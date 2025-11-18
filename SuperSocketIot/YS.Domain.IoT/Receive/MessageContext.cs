namespace YS.Domain.IoT.Receive
{
    using Jaina;
    using Masuit.Tools;
    using System.Text;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.Domain.IoT.Util;
    using static YS.Domain.IoT.Util.ProjectUtil;

    /// <summary>
    /// 消息体下文
    /// </summary>
    public class MessageContext : IEventSource
    {
        /// <summary>
        /// 初始化一个空实例，供序列化或框架调用。
        /// </summary>
        public MessageContext() { }

        /// <summary>
        /// 设置会话唯一标识符。
        /// </summary>
        /// <param name="sessionId">会话ID。</param>
        public void SetSessionId(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <summary>
        /// 设置签名密钥，用于后续校验。
        /// </summary>
        /// <param name="signKey">签名密钥。</param>
        public void SetSignKey(string signKey)
        {
            SignKey = signKey;
        }

        /// <summary>
        /// 签名认证
        /// </summary>
        /// <returns></returns>
        public bool CheckSign(BaseCmd mainInfo, string signKey, out string sysSign)
        {
            bool result = false;
            var sign2 = SignUtil.GetSign(Body, Mid, mainInfo.TimeSp.ToString(), signKey, Encoding.GetEncoding("GB2312"));
            result = Sign == sign2;
            sysSign = sign2;
            return result;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mid"></param>
        /// <param name="eventId"></param>
        /// <param name="version"></param>
        /// <param name="body"></param>
        /// <param name="sign"></param>
        public MessageContext(string key, string mid, string eventId, string messageId, string version, int convertType, string body, string sign, string clientId)
        {
            try
            {
                Key = key;
                Mid = mid;
                EventId = eventId;
                MessageId = messageId;
                Version = version;
                ConvertType = convertType;
                Body = body;
                Sign = sign;
                ClientId = clientId;
                if (!string.IsNullOrEmpty(Body))
                {
                    MainInfo = Body.FromJson<BaseCmd>();
                }
                else
                {
                    MainInfo = new BaseCmd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
        #region 请求内容

        /// <summary>
        ///客户端Id
        /// </summary>
        public string ClientId { get; private set; }
        /// <summary>
        ///协议号
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        ///设备号
        /// </summary>
        public string Mid { get; private set; }
        /// <summary>
        ///事件Id
        /// </summary>
        public string EventId { get; private set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 消息主体方式 1 JSON 2 MessagePage
        /// </summary>
        public int ConvertType { get; private set; } = 1;
        /// <summary>
        ///版本号
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// 主体信息
        /// </summary>
        public BaseCmd MainInfo { get; private set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; private set; }

        /// <summary>
        /// 会话
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string SignKey { get; private set; }
        #endregion

        /// <summary>
        /// 负载信息
        /// </summary>
        public object? Payload { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 取消任务 Token
        /// </summary>
        /// <remarks>用于取消本次消息处理</remarks>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// 是否只消费一次
        public bool IsConsumOnce { get; }

        #region 返回内容

        /// <summary>
        /// 消息发布时间
        /// </summary>
        public long PublishTime { get; set; }
        /// <summary>
        /// 处理服务名称
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public short ConsumedTimes { get; set; }
        // 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public string ResponseId { get; private set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ResponseBody { get; private set; }

        /// <summary>
        /// 结束处理
        /// </summary>
        public virtual void End()
        {
            try
            {
                HostName = $"{AppConfigCache.GrpcIp}:{AppConfigCache.GrpcPort}";
                BeginTime = new DateTimeOffset(CreatedTime).ToUnixTimeMilliseconds();
                EndTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                PublishTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            }
            catch (Exception)
            {
            }

            SessionId = null; // 日志信息干扰
        }

        /// <summary>
        /// 设置回复信息
        /// </summary>
        public void SetResponse(string responseId, string responseBody)
        {
            ResponseId = responseId;
            ResponseBody = responseBody;
        }
        #endregion
    }

    /// <summary>
    /// 协议键组结构，用于封装协议关键字段。
    /// </summary>
    public struct KeyGroup
    {
        /// <summary>
        /// 获取或设置协议号。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置协议版本。
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置命令ID。
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// 获取或设置消息转换类型。
        /// </summary>
        public int ConvertType { get; set; }

        /// <summary>
        /// 获取或设置设备编号。
        /// </summary>
        public string Mid { get; set; }
    }

    /// <summary>
    /// 虚拟机器登录命令类，继承自 BaseCmd。
    /// </summary>
    public class VMCLoginCmd : BaseCmd
    {
        /// <summary>
        /// 获取或设置设备附加信息。
        /// </summary>
        public string MidInfo { get; set; } = string.Empty;
    }
}
