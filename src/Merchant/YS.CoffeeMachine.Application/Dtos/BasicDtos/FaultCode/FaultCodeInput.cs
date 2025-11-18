using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.BasicDtos.FaultCode
{
    /// <summary>
    /// 故障码查询入参
    /// </summary>
    public class FaultCodeInput : QueryRequest
    {
        /// <summary>
        /// 故障码标识
        /// </summary>
        public string? Code { get; set; } = null;

        /// <summary>
        /// 多语言标识
        /// </summary>
        public string? LanCode { get; set; } = null;

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; } = null;
    }
}