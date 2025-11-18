namespace YS.Util.Core
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 提供一些常用的工具方法
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举描述</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// 根据数字和枚举类型获取匹配的枚举项列表
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="number">输入的数字</param>
        /// <returns>匹配的枚举项列表</returns>
        public static List<TEnum> GetMatchedEnums<TEnum>(int number) where TEnum : Enum
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumeration type.");

            // 获取枚举中所有的值并过滤匹配的值
            var matchedEnums = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(e => (number & Convert.ToInt32(e)) != 0)
                .ToList();

            return matchedEnums;
        }

        // <summary>
        /// 枚举 int 转 枚举名称
        /// </summary>
        /// <typeparam name="T">枚举名</typeparam>
        /// <param name="itemValue">int值，枚举key值</param>
        /// <returns>枚举名称</returns>
        public static string ConvertEnumToString<T>(int itemValue)
        {
            return Enum.Parse(typeof(T), itemValue.ToString()).ToString();
        }

        /// <summary>
        /// 获取枚举值对应的描述，如果没有 Description 特性则返回枚举值的名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionOrValue(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// 根据位运算，获取包含的枚举状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetDescriptionsByInt<TEnum>(int status) where TEnum : Enum
        {
            var descriptions = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(statusEnum => Convert.ToInt32(statusEnum) != 0 && (status & Convert.ToInt32(statusEnum)) == Convert.ToInt32(statusEnum))
                .Select(statusEnum => statusEnum.GetType()
                    .GetField(statusEnum.ToString())
                    .GetCustomAttribute<DescriptionAttribute>()
                    ?.Description)
                .ToList();

            return string.Join("、", descriptions);
        }

        /// <summary>
        /// CreateKey
        /// </summary>
        /// <returns></returns>
        public static string CreateKey()
        {
            byte[] array = new byte[32];
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(random.Next(1, 255));
            }

            return ByteToHexStr(array);
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }

            string text = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                text += bytes[i].ToString("X2");
            }

            return text;
        }

        /// <summary>
        /// 当前时间是当月的第几周
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>当月的第几周</returns>
        public static int GetWeekOfMonth(DateTime date)
        {
            int day = date.Day;
            int dayOfWeek = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek;
            int num = day + ((dayOfWeek == 0) ? 7 : dayOfWeek);
            return (num + 6) / 7;
        }

        /// <summary>
        /// 判断时间差是否>=d
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static bool IsAtLeastHoursApartFrom(this DateTime time1, DateTime time2, int d)
        {
            TimeSpan difference = time1 - time2;
            return Math.Abs(difference.TotalHours) >= d;
        }

        /// <summary>
        /// 判断时间差是否>=d
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static bool IsAtLeastDaysApartFrom(DateTime time1, DateTime time2, int d)
        {
            TimeSpan difference = time1 - time2;
            return Math.Abs(difference.TotalDays) >= d;
        }

        /// <summary>
        /// 时间差是多少天
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static double GetWholeDaysDifference(DateTime time1, DateTime time2)
        {
            TimeSpan difference = time1 - time2;
            return difference.TotalDays;
        }

        /// <summary>
        /// 时间差是多少小时
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static double GetWholeHHDifference(DateTime time1, DateTime time2)
        {
            TimeSpan difference = time1 - time2;
            return Math.Round(difference.TotalHours, 2);
        }

        /// <summary>
        /// 获取4位长度随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNext()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 毫秒级时间戳转datetime
        /// </summary>
        /// <param name="unixTimeMilliseconds"></param>
        /// <returns></returns>
        public static DateTime UnixMillisecondsToDateTime(long unixTimeMilliseconds)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeMilliseconds).ToLocalTime(); // 转为本地时间
            return dateTime;
        }

        /// <summary>
        /// 截断小数位
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static decimal TruncateDecimal(decimal value, int decimalPlaces)
        {
            if (decimalPlaces < 0)
                return value;

            decimal factor = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Truncate(value * factor) / factor;
        }

        /// <summary>
        /// 流量卡状态描述转换
        /// </summary>
        /// <param name="cardState"></param>
        /// <returns></returns>
        public static dynamic GetLotCardDescription(string cardState)
        {
            return cardState switch
            {
                "41" => "待激活",
                "42" => "已激活",
                "44" => "停机",
                "46" => "测试",
                "47" => "库存",
                "48" => "预销户",
                "49" => "已销户",
                "00" => "已激活",
                "02" => "停机",
                "03" => "预销户",
                "04" => "已销户",
                "10" => "测试",
                "11" => "沉默期",
                "12" => "停机",
                "15" => "库存",
                "61" => "测试",
                "62" => "待激活",
                "63" => "库存",
                "64" => "已激活",
                "65" => "停机",
                "66" => "失效",
                "67" => "已销户",
                "68" => "停机",
                "99" => "未知",
                _ => $"未知"
            };
        }

        /// <summary>
        /// 计算两个坐标点之间的距离（公里）
        /// 使用Haversine公式
        /// </summary>
        /// <param name="lat1">起点纬度</param>
        /// <param name="lon1">起点经度</param>
        /// <param name="lat2">终点纬度</param>
        /// <param name="lon2">终点经度</param>
        /// <returns>距离（公里）</returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            if (lat1 == 0 || lon1 == 0 || lat2 == 0 || lon2 == 0)
                return double.MaxValue; // 返回最大值表示距离很远

            const double R = 6371; // 地球半径（公里）

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return distance;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}