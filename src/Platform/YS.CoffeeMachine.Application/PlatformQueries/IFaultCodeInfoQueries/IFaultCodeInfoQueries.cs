using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IFaultCodeInfoQueries
{
    /// <summary>
    /// 故障码查询
    /// </summary>
    public interface IFaultCodeInfoQueries
    {
        /// <summary>
        /// 查询故障码列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FaultCodeDto>> GetFalutCodeList(FaultCodeInput input);

        /// <summary>
        /// 根据code获取故障码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<FaultCodeDto> GetFaultCode(string code);

        /// <summary>
        /// 根据故障码获取多语言code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetFaultLanCodeByCode(string code);
    }
}
