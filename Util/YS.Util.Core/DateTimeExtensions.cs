namespace YS.Util.Core
{
    /// <summary>
    /// 时间拓展类
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// DateTime 转秒级时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return (long)(dateTime - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// DateTime 转毫秒级时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            return (long)(dateTime - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// 秒级时间戳转 DateTime
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(this long seconds)
        {
            return UnixEpoch.AddSeconds(seconds).ToLocalTime();
        }

        /// <summary>
        /// 毫秒级时间戳转 DateTime
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(this long milliseconds)
        {
            return UnixEpoch.AddMilliseconds(milliseconds).ToLocalTime();
        }
    }
}