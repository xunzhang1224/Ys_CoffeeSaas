namespace YS.CoffeeMachine.Application.Dtos.BasicDtos
{
    /// <summary>
    /// 时区偏移量通用Dto，继承此实体，会自动获取当前客户端时区偏移量（小时）
    /// </summary>
    public class TimezoneOffsetBaseDto
    {
        /// <summary>
        /// 时区偏移量(小时)
        /// </summary>
        public int? TimezoneOffsetHour { get; set; } = 0;
    }
}