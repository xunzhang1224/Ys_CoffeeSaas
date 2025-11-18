using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IOrderQueries
{
    /// <summary>
    /// 订单信息查询接口
    /// </summary>
    public interface IOrderInfoQueries
    {
        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderInfoDto>> GetOrderInfosPageAsync(OrderInfoInput input);

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<OrderDetailsDto>> GetOrderDetailsByOrderIdAsync(long id);
    }
}