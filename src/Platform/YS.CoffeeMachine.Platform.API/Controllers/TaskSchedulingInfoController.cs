using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 任务调度
    /// </summary>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class TaskSchedulingInfoController(IMediator mediator, ITaskSchedulingInfoQueries taskSchedulingInfoQueries) : Controller
    {
        #region 任务调度相关接口
        /// <summary>
        /// 创建任务调度信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateTaskSchedulingInfo")]
        public Task<bool> CreateTaskSchedulingInfo([FromBody] CreateTaskSchedulingInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 通过Id获取任务调度信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetTaskSchedulingInfoById")]
        public Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoById(long id)
        {
            return taskSchedulingInfoQueries.GetTaskSchedulingInfoAsync(id);
        }

        /// <summary>
        /// 通过任务名获取任务调度信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("GetTaskSchedulingInfoByTaskName")]
        public Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoByTaskName(string name)
        {
            return taskSchedulingInfoQueries.GetTaskSchedulingInfoAsync(name);
        }

        /// <summary>
        /// 获取任务调度列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllTaskSchedulingInfos")]
        public Task<TaskSchedulingInfoListDot> GetAllTaskSchedulingInfos()
        {
            return taskSchedulingInfoQueries.GetTaskSchedulingInfoListAsync();
        }

        /// <summary>
        /// 编辑任务调度信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateTaskSchedulingInfo")]
        public Task<bool> UpdateTaskSchedulingInfo([FromBody] UpdateTaskSchedulingInfoCommand command)
        {
            return mediator.Send(command);
        }

        /// <summary>
        /// 删除任务调度信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTaskSchedulingInfo")]
        public Task<bool> DeleteTaskSchedulingInfo([FromBody] DeleteTaskSchedulingInfoCommand command)
        {
            return mediator.Send(command);
        }
        #endregion
    }
}
