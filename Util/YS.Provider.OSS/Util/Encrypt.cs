namespace YS.Provider.OSS.Util
{
    using System.Text;

    /// <summary>
    /// 提供加密相关的方法
    /// </summary>
    static class Encrypt
    {
        /// <summary>
        /// 使用MD5算法对输入字符串进行加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string input)
        {
            // 创建MD5加密算法实例
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 计算输入字符串的哈希值
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 创建StringBuilder对象，用于存储哈希值的十六进制表示
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    // 将每个字节的哈希值转换为十六进制字符串并添加到StringBuilder中
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                // 返回最终的哈希值字符串
                return sb.ToString();
            }
        }
    }
}
