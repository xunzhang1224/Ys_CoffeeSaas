using YS.CoffeeMachine.Application.Dtos.PaymentDtos;

namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 支付通用创建订单服务接口
    /// </summary>
    public interface ICreateOrderService
    {
        /// <summary>
        /// 创建订单信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderNO"></param>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        Task<bool> CreateOrderInfo(CreateOrderBaseInput input, string orderNO, string payOrderId);
    }
}