using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YS.Domain.IoT.Util
{
    /// <summary>
    /// 签名工具
    /// </summary>
    public class SignUtil
    {
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="json"></param>
        /// <param name="mid"></param>
        /// <param name="timeSp"></param>
        /// <param name="signKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetSign(string json, string mid, string timeSp, string signKey, Encoding encoding)
        {
            try
            {
                string sign = string.Empty;
                byte[] aesDatas = new byte[16];
                //json = Regex.Replace(json, "[:\"-',{}\\[\\]]", string.Empty); // 去除Json特殊字符
                json = Regex.Replace(json, "[:\",{}\\[\\]]", string.Empty); // 去除Json特殊字符
                List<byte> listBytes = new List<byte>();
                listBytes.AddRange(BCDToByte(mid));
                listBytes.AddRange(BCDToByte(timeSp));
                long sumVal = GetSumFromBytes(encoding.GetBytes(json)); // 取校验和

                listBytes.AddRange(BCDToByte(sumVal.ToString().PadLeft(10, '0')));

                listBytes.Add(0x00);
                AESEncryptUtil.AesEncrypt(listBytes.ToArray(), StrToHexByte(signKey), ref aesDatas);
                sign = ByteToHexStr(aesDatas.Take(4).ToArray());
                return sign;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] datas)
        {
            string str = string.Empty;
            for (int i = 0; i < datas.Length; i++)
            {
                str += (datas[i].ToString("X").Length == 1 ? "0" : string.Empty) + datas[i].ToString("X");
            }
            return str;
        }

        /// <summary>
        /// 获取字节数组的校验和
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static long GetSumFromBytes(byte[] datas)
        {
            long sum = 0;
            foreach (byte v in datas)
            {
                sum = sum + v;
            }
            return sum;
        }
        /// <summary>
        /// BCD码转换16进制(压缩BCD)
        /// </summary>
        /// <param name="strTemp">strTemp</param>
        /// <returns></returns>
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public static byte[]? BCDToByte(string strTemp)
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        {
            try
            {
                //数字的二进制码最后1位是1则为奇数
                if (Convert.ToBoolean(strTemp.Length & 1))
                {
                    strTemp = "0" + strTemp; // 数位为奇数时前面补0
                }
                byte[] aryTemp = new byte[strTemp.Length / 2];
                for (int i = 0; i < strTemp.Length / 2; i++)
                {
                    aryTemp[i] = (byte)(strTemp[i * 2] - '0' << 4 | strTemp[i * 2 + 1] - '0');
                }
                return aryTemp; // 高位在前
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 时间转换成Unix时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToUnixTimestampByMilliseconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString">hexString</param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            if (hexString == null) return [];
            hexString = hexString.Replace(" ", string.Empty);
            if (hexString.Length % 2 != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string MyMd5(string key)
        {
            string token = string.Empty;
            var md5 = System.Security.Cryptography.MD5.Create(); //实例化一个md5对像
                                                                 // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            foreach (var item in s)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                token = string.Concat(token, item.ToString("x2"));
            }
            return token;
        }
    }
}
