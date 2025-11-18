using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries
{
    /// <summary>
    /// 系统支付信息查询接口
    /// </summary>
    public interface ISystemPaymentInfoQueries
    {
        /// <summary>
        /// 获取系统支付方式列表
        /// </summary>
        /// <returns></returns>
        Task<List<SystemPaymentMethodDto>> GetSystemPaymentMethodsAsync();

        /// <summary>
        /// 获取二级商户支付方式分页列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<M_PaymentMethodDto>> GetMachinePaymentMethodsAsync(M_PaymentMethodInput input);

        /// <summary>
        /// 获取微信进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<M_PaymentWechatApplymentsOutput> GetWechatApplymentsByIdAsync(long id);

        /// <summary>
        /// 获取支付宝进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<M_PaymentAlipayApplymentsOutput> GetAlipayApplymentsByIdAsync(long id);

        /// <summary>
        /// 当前支付方式绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodBindDevicesAsync(PaymentMethodBindDeviceInput input);

        /// <summary>
        /// 当前支付方式未绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodUnBindDevicesAsync(PaymentMethodBindDeviceInput input);

        /// <summary>
        /// 获取设备绑定的支付方式，返回给安卓收银台（支持多个mid）
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetDevicesBindPaymentMethodAsync(List<string> mids);
    }
}