using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries
{
    /// <summary>
    /// 企业类型查询
    /// </summary>
    public interface IP_EnterpriseTypesQueries
    {
        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        Task<List<P_EnterpriseTypesDto>> GetEnterpriseTypesAsync();

        /// <summary>
        /// 根据Id获取企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<P_EnterpriseTypesDto> GetEnterpriseTypeByIdAsync(long id);
    }
}
