using YS.CoffeeMachine.Application.Dtos.IotCardDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IIotCardInfoQueries
{
    /// <summary>
    /// 物联网卡信息查询接口
    /// </summary>
    public interface IIotCardInfoQuerie
    {
        /// <summary>
        /// 获取物联网卡分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VendIotCardOut>> GetVendIotCardPageAsync(IotCardQueryInput input);
    }
}