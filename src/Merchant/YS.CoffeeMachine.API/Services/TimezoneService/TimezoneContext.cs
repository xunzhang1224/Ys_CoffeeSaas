using YS.CoffeeMachine.Application.IServices;

namespace YS.CoffeeMachine.API.Services.TimezoneService
{
    /// <summary>
    /// 时区上下文
    /// </summary>
    public class TimezoneContext : ITimezoneContext
    {
        private static readonly AsyncLocal<int?> _offset = new AsyncLocal<int?>();

        /// <summary>
        /// 时区偏移小时数
        /// </summary>
        public int? OffsetHour => _offset.Value;

        /// <summary>
        /// 设置时区偏移小时数
        /// </summary>
        /// <param name="hour"></param>
        public static void SetOffset(int? hour) => _offset.Value = hour;

        /// <summary>
        /// 转换为本地时间
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public DateTime ConvertToLocal(DateTime utcTime)
        {
            if (!_offset.Value.HasValue) return utcTime;
            return utcTime.AddHours(_offset.Value.Value);
        }

        /// <summary>
        /// 将日期范围内的所有时间转换为本地时间
        /// </summary>
        /// <param name="dateRange">日期范围（通常包含2个时间：开始与结束）</param>
        public List<DateTime> ConvertToLocal(List<DateTime> dateRange)
        {
            if (dateRange is not { Count: > 0 } || !_offset.Value.HasValue)
                return dateRange;

            for (int i = 0; i < dateRange.Count; i++)
                dateRange[i] = dateRange[i].AddHours(_offset.Value.Value);

            return dateRange;
        }

        /// <summary>
        /// 转换为UTC时间
        /// </summary>
        /// <param name="localTime"></param>
        /// <returns></returns>
        public DateTime ConvertToUtc(DateTime localTime)
        {
            if (!_offset.Value.HasValue) return localTime;
            return localTime.AddHours(-_offset.Value.Value);
        }
    }
}