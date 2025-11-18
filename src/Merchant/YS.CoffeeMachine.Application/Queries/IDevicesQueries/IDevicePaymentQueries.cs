using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备支付查询
    /// </summary>
    public interface IDevicePaymentQueries
    {
        /// <summary>
        /// 获取指定支付方式下未绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DevicePaymentDto>> GetNotBildDeviceList(DevicePaymentInput input);

        /// <summary>
        /// 获取指定支付方式下绑定的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DevicePaymentDto>> GetBildDeviceList(DevicePaymentInput input);
    }
}
