using Castle.Core.Logging;
using SuperSocket.ProtoBase;
using System.Buffers;
using System.Text;
using YS.Domain.IoT;
using YS.Domain.IoT.Receive;

namespace YS.Core.IoT.Socket
{
    /// <summary>
    /// 命令协议过滤
    /// </summary>
    public class IotCommandPackageFilter : BeginEndMarkPipelineFilter<MessageContext>
    {
        private static readonly ReadOnlyMemory<byte> BeginMark = Encoding.ASCII.GetBytes("###");
        private static readonly ReadOnlyMemory<byte> EndMark = Encoding.ASCII.GetBytes("&&&");
        // $
        private static readonly byte SpMark = 0x24;

        /// <summary>
        /// IotCommandPackageFilter 构造函数
        /// </summary>
        public IotCommandPackageFilter() : base(BeginMark, EndMark)
        {
        }

        /// <summary>
        /// 将数据包转换成 IotCommandContext 的实例
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="IotException"></exception>
        protected override MessageContext DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            //long lenth = buffer.Length;

            //var keyindex = buffer.PositionOf(SpMark);
            //if (!keyindex.HasValue)
            //{
            //    throw new IotException($"bad request of head keyindex HasnoValue");
            //}
            ////try
            ////{
            //string sign = Encoding.UTF8.GetString(buffer.Slice(lenth - 8, 8)); // 签名认证
            //var mainbody = buffer.Slice(keyindex.Value); // 消息主体
            //var mainlen = mainbody.Length;

            //var keygroup = DecodeKey(Encoding.UTF8.GetString(buffer.Slice(0, keyindex.Value)));

            //var jsbody = Encoding.UTF8.GetString(mainbody.Slice(1, mainlen - 10));
            //var eventId=$"{keygroup.Key}{(string.IsNullOrEmpty(keygroup.Version) ? string.Empty : string.Format(":{0}", keygroup.Version))}";
            //var context = new MessageContext(keygroup.Key, keygroup.Mid, eventId, keygroup.CommandId, keygroup.Version, keygroup.ConvertType, jsbody, sign, keygroup.Mid);

            //return context;
            //}
            //catch (Exception ex)
            //{
            //    // _logger.LogError(ex, $"解析失败:{ex.Message}");
            //    return null;
            //}

            var body = buffer;

            var keyindex = body.PositionOf(SpMark);
            if (!keyindex.HasValue)
            {
                Console.WriteLine($"头部要求违法");
            }
            var mainbody = body.Slice(keyindex.Value);
            var mainlen = mainbody.Length;
            var keygroup = DecodeKey(Encoding.UTF8.GetString(body.Slice(0, keyindex.Value)));
            var jsbody = Encoding.UTF8.GetString(mainbody.Slice(1, mainlen - 10));

            var sign = Encoding.UTF8.GetString(mainbody.Slice(mainlen - 8, 8));
            MessageContext requestInfo = new MessageContext(
            key: keygroup.Key,
            mid: keygroup.Mid,
            eventId: $"{keygroup.Key}{(string.IsNullOrEmpty(keygroup.Version) ? string.Empty : string.Format(":{0}", keygroup.Version))}",
            // messageId: string.IsNullOrEmpty(keygroup.Version) ? bodyext.Id : keygroup.CommandId,
            messageId: keygroup.CommandId,
            version: keygroup.Version,
            body: jsbody,
            sign: sign,
            clientId: keygroup.Mid,
            convertType: keygroup.ConvertType
            );
            return requestInfo;

        }

        /// <summary>
        /// 消息头 解析
        /// </summary>
        /// <param name="keystring"></param>
        /// <returns></returns>
        /// <exception cref="IotException"></exception>
        public KeyGroup DecodeKey(string keystring)
        {
            if (string.IsNullOrEmpty(keystring))
            {
                throw new IotException("连接已拒绝!");
            }
            var keylist = keystring.Split('-');

            if (keylist.Length > 4)
            {
                throw new IotException("错误的关键字串长度!");
            }
            if (keylist.Length == 4)
            {
                return new KeyGroup
                {
                    Key = keylist[0],
                    Mid = keylist[1],
                    CommandId = keylist[2],// 消息Id 设备上报
                    Version = "V1",
                    ConvertType = 1,
                };
            }
            else
            {
                return new KeyGroup
                {
                    Key = keylist[0],
                    Mid = "0000000000",
                    CommandId = Guid.NewGuid().ToString(),
                    Version = string.Empty,
                    ConvertType = 1
                };
            }
        }
    }
}
