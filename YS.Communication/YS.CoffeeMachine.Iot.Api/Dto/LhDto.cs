namespace YS.CoffeeMachine.Iot.Api.Dto
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
        /// 标识是否是糖
        /// </summary>
        public bool IsSugar { get; set; } = false;
    }
}
