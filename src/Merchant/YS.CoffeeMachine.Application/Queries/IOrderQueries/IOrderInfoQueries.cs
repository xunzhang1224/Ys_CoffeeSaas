using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IOrderQueries
{
    /// <summary>
    /// 订单信息查询接口
    /// </summary>
    public interface IOrderInfoQueries
    {
        /// <summary>
        /// 查询订单总计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrderTotalInfo> GetOrderTotalInfoAsync(OrderInfoInput input);

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

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<OrderRefundDetailListDto>> GetOrderRefundDetailListByOrderIdAsync(long id);

        /// <summary>
        /// 根据主订单Id获取退款分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderRefundPageDto>> GetOrderRefundPageAsync(OrderRefundInput input);

        #region H5

        /// <summary>
        /// H5订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<H5OrderInfoDto>> GetH5OrderInfosPageAsync(H5OrderInfoInput input);

        /// <summary>
        /// 获取H5订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<H5OrderDetailsDto> GetH5OrderRefundDetailsByOrderIdAsync(long id);

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<H5OrderRefundDetailListDto>> GetH5OrderRefundDetailListByOrderIdAsync(long id);
        #endregion
    }
}