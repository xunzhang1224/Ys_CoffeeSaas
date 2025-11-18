using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;

namespace YS.CoffeeMachine.Application.Queries.ISettingQueries
{
    /// <summary>
    /// 接口风格查询
    /// </summary>
    public interface IInterfaceStyleQueries
    {
        /// <summary>
        /// 获取所有接口风格
        /// </summary>
        /// <returns></returns>
        Task<List<InterfaceStylesDto>> GetAllInterfaceStylesAsync();
    }
}
