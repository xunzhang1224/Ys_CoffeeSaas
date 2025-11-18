using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IServiceQueries;
using YS.Provider.OSS.Interface.Base;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 通用服务接口
    /// </summary>
    /// <param name="enumHelperQueries"></param>
    /// <param name="oSSService"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class ServiceHelperController(IEnumHelperQueries enumHelperQueries, IOSSService oSSService) : Controller
    {
        /// <summary>
        /// 获取项目所有枚举信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllEnumInfos")]
        public Task<List<EnumInfo>> GetAllEnumInfos()
        {
            return enumHelperQueries.GetAllEnumInfos();
        }
    }
}