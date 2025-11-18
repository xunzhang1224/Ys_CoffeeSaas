using YS.Pay.SDK.Models;

namespace YS.CoffeeMachine.Application.Dtos.PaymentDtos
{
    /// <summary>
    /// SEVMC创建订单输入参数
    /// </summary>
    public class SEVMC_CreateOrderInput : CreateOrderBaseInput
    {
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 拓展参数
        /// </summary>
        public SEVMCCreateOrderExtra Extra { get; set; } = new SEVMCCreateOrderExtra();
    }
}