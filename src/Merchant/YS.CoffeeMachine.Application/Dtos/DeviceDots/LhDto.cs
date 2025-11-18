namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 料盒
    /// </summary>
    public class LhDto
    {
        /// <summary>
        /// 料盒名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 标识是否是糖
        /// </summary>
        public bool IsSugar { get; set; } = false;
    }
}
