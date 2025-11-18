namespace YS.CoffeeMachine.Application.Dtos.BasicDtos.FaultCode
{
    /// <summary>
    /// 故障码Dto
    /// </summary>
    public class FaultCodeDto
    {
        /// <summary>
        /// 故障码标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 多语言标识
        /// </summary>
        public string LanCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}