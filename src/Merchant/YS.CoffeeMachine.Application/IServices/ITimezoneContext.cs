namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 时区上下文
    /// </summary>
    public interface ITimezoneContext
    {
        /// <summary>当前请求的时区偏移量（小时，可能为负）</summary>
        int? OffsetHour { get; }

        /// <summary>将时间从 UTC 转为客户端时间</summary>
        DateTime ConvertToLocal(DateTime utcTime);

        /// <summary>
        /// 将日期范围内的所有时间转换为本地时间
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        List<DateTime> ConvertToLocal(List<DateTime> dateRange);

        /// <summary>将时间从客户端时间转为 UTC</summary>
        DateTime ConvertToUtc(DateTime localTime);
    }
}