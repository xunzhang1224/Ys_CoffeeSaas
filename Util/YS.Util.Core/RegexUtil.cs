using System.Text.RegularExpressions;

namespace YS.Util.Core
{
    /// <summary>
    /// 正则表达式工具类
    /// <remarks>
    /// 此类提供了多种预编译的正则表达式以及常用的正则表达式操作方法。
    /// 包括邮箱、URL、IP地址、手机号码、邮政编码、整数、浮点数、密码等验证。
    /// 此外，还支持动态正则表达式的缓存和管理。
    /// 使用 <see cref="GeneratedRegexAttribute"/> 特性在编译期生成正则表达式，
    /// 以提高性能和减少运行时开销。尽量使用预编译的正则表达式方法，
    /// 如 <see cref="IsValidEmail(string)"/>、<see cref="IsValidUrl(string)"/> 等。
    /// </remarks>
    /// </summary>
    public static partial class RegexUtiil
    {
        #region 编译期生成的正则表达式

        /// <summary>
        /// 通用邮箱正则表达式
        /// </summary>
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex EmailRegex();

        /// <summary>
        /// 通用IP地址正则表达式
        /// </summary>
        [GeneratedRegex(@"^(25[0-5]|2[0-4]\d|[01]?\d\d?)(\.(25[0-5]|2[0-4]\d|[01]?\d\d?)){3}$", RegexOptions.Compiled)]
        private static partial Regex IpAddressRegex();

        /// <summary>
        /// 身份证号码正则表达式
        /// </summary>
        [GeneratedRegex(@"(^\d{15}$)|(^\d{17}[\dXx]$)", RegexOptions.Compiled)]
        private static partial Regex IDCardRegex();

        /// <summary>
        /// 国际手机号码正则表达式（符合 E.164 标准）
        /// </summary>
        [GeneratedRegex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled)]
        private static partial Regex InternationalPhoneRegex();

        /// <summary>
        /// 国内手机号码正则表达式（严格11位，支持最新号段）
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^1[3-9]\d{9}$", RegexOptions.Compiled)]
        private static partial Regex ChinaPhoneRegex();

        /// <summary>
        /// 通用邮政编码正则表达式（3-10 位字母数字组合）
        /// </summary>
        [GeneratedRegex(@"^[A-Za-z0-9\- ]{3,10}$", RegexOptions.Compiled)]
        private static partial Regex PostalCodeRegex();

        /// <summary>
        /// 通用整数数字正则表达式
        /// </summary>
        [GeneratedRegex(@"^[+-]?\d+$", RegexOptions.Compiled)]
        private static partial Regex IntegerRegex();

        /// <summary>
        /// 通用浮点数字正则表达式
        /// </summary>
        [GeneratedRegex(@"^[+-]?\d+(\.\d+)?$", RegexOptions.Compiled)]
        private static partial Regex FloatRegex();

        /// <summary>
        /// 密码正则表达式，8-16 位必须包含数字和字母
        /// </summary>
        [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,16}$", RegexOptions.Compiled)]
        private static partial Regex PasswordRegex();

        /// <summary>
        /// 所属端名称正则表达式，用于匹配 YS.Pay.*.Web.Entry 的程序集名称
        /// </summary>
        [GeneratedRegex(@"YS\.Pay\.(.*)\.Web\.Entry", RegexOptions.Compiled)]
        private static partial Regex EntryModuleNameRegex();

        /// <summary>
        /// OSS文件URL对象名称提取正则表达式
        /// 匹配域名后的路径部分，兼容有无查询参数的URL
        /// 示例：
        /// - https://bucket.oss-cn-beijing.aliyuncs.com/path/file.docx → path/file.docx
        /// - https://bucket.oss-cn-beijing.aliyuncs.com/path/file.docx?param=value → path/file.docx
        /// </summary>
        [GeneratedRegex(@"\.com/(.+?)(?:\?|$)", RegexOptions.Compiled)]
        private static partial Regex OssObjectNameRegex();

        /// <summary>
        /// 文件名提取正则表达式
        /// 从路径中提取最后一个斜杠后的文件名（包含扩展名）
        /// 示例：path/to/file.docx → file.docx
        /// </summary>
        [GeneratedRegex(@"[^/]+$", RegexOptions.Compiled)]
        private static partial Regex FileNameRegex();

        #endregion

        #region 通用方法

        /// <summary>
        /// 判断输入的字符串是否匹配指定的正则表达式
        /// </summary>
        public static bool IsMatch(string input, string pattern)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pattern))
                return false;
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 判断输入的字符串是否匹配预编译的正则表达式
        /// </summary>
        public static bool IsMatch(string input, Regex regex)
        {
            if (string.IsNullOrWhiteSpace(input) || regex == null)
                return false;
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 从输入的字符串中提取所有匹配指定正则表达式的值
        /// </summary>
        public static List<string> GetMatches(string input, string pattern)
        {
            var matches = new List<string>();
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pattern))
                return matches;

            foreach (Match match in Regex.Matches(input, pattern))
            {
                matches.Add(match.Value);
            }
            return matches;
        }

        /// <summary>
        /// 从输入的字符串中提取所有匹配预编译正则表达式的值
        /// </summary>
        public static List<string> GetMatches(string input, Regex regex)
        {
            var matches = new List<string>();
            if (string.IsNullOrWhiteSpace(input) || regex == null)
                return matches;

            foreach (Match match in regex.Matches(input))
            {
                matches.Add(match.Value);
            }
            return matches;
        }

        /// <summary>
        /// 替换输入字符串中匹配指定正则表达式的部分
        /// </summary>
        public static string Replace(string input, string pattern, string replacement)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pattern))
                return input;
            return Regex.Replace(input, pattern, replacement ?? string.Empty);
        }

        /// <summary>
        /// 替换输入字符串中匹配预编译正则表达式的部分
        /// </summary>
        public static string Replace(string input, Regex regex, string replacement)
        {
            if (string.IsNullOrWhiteSpace(input) || regex == null)
                return input;
            return regex.Replace(input, replacement ?? string.Empty);
        }

        #endregion

        #region 业务相关方法

        /// <summary>
        /// 获取程序集名称中的客户端名称部分（如：Platform、Merchant、Provider）
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static string GetClientName(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                return string.Empty;

            var match = EntryModuleNameRegex().Match(assemblyName);

            if (match.Success && match.Groups.Count > 1)
            {
                // 返回第一个捕获组的值，即 (.*) 匹配的内容
                return match.Groups[1].Value;
            }

            return string.Empty; // 如果没有匹配成功，返回空字符串
        }

        #endregion

        #region 验证方法

        /// <summary>
        /// 验证邮箱格式，正则+MailAddress双重验证
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns>是否为有效邮箱格式</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            // 先用正则表达式初步过滤
            if (!EmailRegex().IsMatch(email))
                return false;
            // 再用 MailAddress 解析，确保格式合法
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证身份证号码格式
        /// </summary>
        /// <param name="idCardNo">身份证号码</param>
        /// <returns></returns>
        public static bool IsValidIDCard(string idCardNo)
        {
            if (string.IsNullOrWhiteSpace(idCardNo))
                return false;
            return IDCardRegex().IsMatch(idCardNo);
        }

        /// <summary>
        /// 验证URL格式
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns>是否为有效URL格式</returns>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// 验证IP地址格式
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>是否为有效IP地址格式</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;
            return IpAddressRegex().IsMatch(ipAddress);
        }

        /// <summary>
        /// 验证国际手机号码格式
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>是否为有效手机号码格式</returns>
        public static bool IsValidInternationalPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            return InternationalPhoneRegex().IsMatch(phoneNumber);
        }

        /// <summary>
        /// 验证中国大陆手机号码格式（严格11位，支持最新号段）
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsValidChinaPhone(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            return ChinaPhoneRegex().IsMatch(phoneNumber);
        }

        /// <summary>
        /// 验证邮政编码格式
        /// </summary>
        /// <param name="postalCode">邮政编码</param>
        /// <returns>是否为有效邮政编码格式</returns>
        public static bool IsValidPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;
            return PostalCodeRegex().IsMatch(postalCode);
        }

        /// <summary>
        /// 验证是否为整数
        /// </summary>
        /// <param name="value">数值字符串</param>
        /// <returns>是否为有效整数格式</returns>
        public static bool IsValidInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return IntegerRegex().IsMatch(value);
        }

        /// <summary>
        /// 验证是否为浮点数
        /// </summary>
        /// <param name="value">数值字符串</param>
        /// <returns>是否为有效浮点数格式</returns>
        public static bool IsValidFloat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return FloatRegex().IsMatch(value);
        }

        /// <summary>
        /// 验证密码格式（8-16位，必须包含数字和字母）
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>是否为有效密码格式</returns>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            return PasswordRegex().IsMatch(password);
        }

        /// <summary>
        /// 从OSS文件URL中提取对象名称和文件名
        /// </summary>
        /// <param name="fileUrl">OSS文件URL</param>
        /// <returns>包含对象名称和文件名的元组，如果解析失败返回空字符串</returns>
        public static (string objectName, string fileName) ExtractOssObjectName(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return (string.Empty, string.Empty);

            var match = OssObjectNameRegex().Match(fileUrl);
            if (!match.Success)
                return (string.Empty, string.Empty);

            string objectName = match.Groups[1].Value;

            // 从对象名称中提取文件名
            var fileNameMatch = FileNameRegex().Match(objectName);
            string fileName = fileNameMatch.Success ? fileNameMatch.Value : string.Empty;

            return (objectName, fileName);
        }

        /// <summary>
        /// 验证是否为有效的OSS文件URL
        /// </summary>
        /// <param name="fileUrl">OSS文件URL</param>
        /// <returns>是否为有效的OSS文件URL</returns>
        public static bool IsValidOssFileUrl(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return false;

            return OssObjectNameRegex().IsMatch(fileUrl);
        }
        #endregion
    }
}