using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Commands.InternalMsgCommands;
using YS.CoffeeMachine.Platform.API.Extensions;

namespace YS.CoffeeMachine.Platform.API.Controllers
{
    /// <summary>
    /// 站内信管理
    /// </summary>
    /// <param name="mediator"></param>
    [Authorize]
    [Route("papi/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Platform_v1))]
    public class InternalMsgController(IMediator mediator) : Controller
    {
        #region 系统消息
        /// <summary>
        /// 创建系统消息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateSystemMessages")]
        public Task CreateSystemMessages([FromBody] SystemMessagesCommands command) => mediator.Send(command);

        /// <summary>
        /// 编辑系统消息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UpdateSystemMessages")]
        public Task UpdateSystemMessages([FromBody] UpdateSystemMessagesCommands command) => mediator.Send(command);

        /// <summary>
        /// 取消系统消息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CancelSystemMessages")]
        public Task CancelSystemMessages([FromBody] CancelSystemMessagesCommands command) => mediator.Send(command);
        #endregion

        #region 用户消息
        /// <summary>
        /// 创建用户消息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateUserMessages")]
        public Task CreateUserMessages([FromBody] UserMessagesCommands command) => mediator.Send(command);

        /// <summary>
        /// 标记用户消息为已读
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("MarkAsReadUserMessages")]
        public Task MarkAsReadUserMessages([FromBody] MarkAsReadCommands command) => mediator.Send(command);

        /// <summary>
        /// 标记用户消息为已弹窗
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("MarkAsPopupShownUserMessages")]
        public Task MarkAsPopupShown([FromBody] MarkAsPopupShownCommands command) => mediator.Send(command);
        #endregion

        #region 用户已读全局消息
        /// <summary>
        /// 用户读取全局消息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UserReadGlobalMessages")]
        public Task UserReadGlobalMessages([FromBody] UserReadGlobalMessagesCommands command) => mediator.Send(command);
        #endregion
    }
}