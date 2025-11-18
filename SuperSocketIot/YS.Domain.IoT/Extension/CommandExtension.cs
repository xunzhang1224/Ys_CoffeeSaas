using System.Text;
using YS.Domain.IoT.Enum;
using YS.Domain.IoT.Util;
namespace YS.Domain.IoT.Extension
{
    /// <summary>
    /// 命令扩展
    /// </summary>
    public static class CommandExtension
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ToFormat(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// 合成一条协议格式的数据,仅仅加密签名
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="protocolId">协议号</param>
        /// <param name="key">加密密钥</param>
        /// <param name="mid">mid</param>
        /// <param name="timesp">timesp</param>
        /// <param name="flowDirection">数据流向枚举(默认为DOWN,eg:UP:IoT设备上传至服务器)</param>
        /// <param name="dataFormatter">数据字符串格式</param>
        /// <returns>返回标准格式的数据</returns>
        public static string GetAgreement(string json, string protocolId, string key, string mid, string timesp, FlowDirection flowDirection = FlowDirection.DOWN, string dataFormatter = "###{0}${1}${2}&&&")
        {
            try
            {
                // .NET CORE 如果要使用GB2312编码必须引用System.Text.Encoding.CodePages，且在使用GB2312之前添加下面这行语句
                // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string temp = string.Empty;
                Encoding encoding = Encoding.GetEncoding("GB2312");
                if (flowDirection == FlowDirection.UP)
                    encoding = Encoding.GetEncoding("UTF8");
                string strSign = SignUtil.GetSign(json, mid, timesp, key, encoding);
                temp = string.Format(dataFormatter, protocolId, json, strSign);
                return temp;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }

}
