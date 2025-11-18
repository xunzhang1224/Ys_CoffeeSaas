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
    /// 企业设备查询
    /// </summary>
    public interface IEnterpriseDevicesQueries
    {
        /// <summary>
        /// 通过Id获取设备分配信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EnterpriseDevicesDto> GetEnterpriseDevicesAsync(long id);

        /// <summary>
        /// 获取设备分配列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<EnterpriseDevicesDto>> GetEnterpriseDevicesListAsync(EnterpriseDevicesInput request);
    }
}
