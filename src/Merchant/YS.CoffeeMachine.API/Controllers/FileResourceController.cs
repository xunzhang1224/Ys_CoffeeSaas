using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.FileResourceCommands;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.FileResource;
using YS.CoffeeMachine.Application.Queries.FileResourceQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 文件资源
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class FileResourceController(IMediator mediator, IFileManageQuerie _fileManageQuerie) : Controller
    {
        /// <summary>
        /// 获取文件资源
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFileResource")]
        public async Task<PagedResultDto<FileManageDto>> GetFileResourceAsync([FromBody] GetFileManageInput input)
        {
            return await _fileManageQuerie.GetFileResource(input);
        }

        /// <summary>
        /// 创建文件资源
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateFileManage")]
        public async Task CreateFileManageAsync([FromBody] CreateFileManageCommand command) => await mediator.Send(command);

        /// <summary>
        /// 修改文件资源
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateFileManage")]
        public async Task UpdateFileManageAsync([FromBody] UpdateFileManageCommand command) => await mediator.Send(command);

        /// <summary>
        /// 删除文件资源
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFileManage")]
        public async Task DeleteFileManageAsync([FromBody] DeleteFileManageCommand command) => await mediator.Send(command);

    }
}
