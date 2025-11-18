using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.PlatformQueries.IFileResourceQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{

    /// <summary>
    /// 文件资源
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class FileResourceController(IPFileManageQueries _pFileManageQueries) : Controller
    {
        /// <summary>
        /// 获取文件资源列表(平台端)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPFileResourceList")]
        public async Task<List<P_FileManage>> GetPFileResourceListAsync()
        {
            return await _pFileManageQueries.GetPFileResourceList();
        }

        /// <summary>
        /// 获取文件资源列表(商户端)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFileResourceList")]
        public async Task<List<FileManage>> GetFileResourceListAsync()
        {
            return await _pFileManageQueries.GetFileResourceList();
        }
    }
}
