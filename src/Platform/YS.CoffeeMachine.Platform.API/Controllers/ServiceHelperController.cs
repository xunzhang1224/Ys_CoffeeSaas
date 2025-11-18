using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Platform.API.Extensions;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.Queries.IServiceQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Interface.Base;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 通用服务接口
    /// </summary>
    /// <param name="enumHelperQueries"></param>
    /// <param name="oSSService"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
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
