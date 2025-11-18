using YS.CoffeeMachine.Application.Dtos.BasicDtos.FaultCode;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Queries.BasicQueries.FaultCode
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

        /// <summary>
        /// 根据故障码类型获取故障码
        /// </summary>
        Task<List<FaultCodeDto>> GetFaultCodeByType(FaultCodeTypeEnum type);
    }
}