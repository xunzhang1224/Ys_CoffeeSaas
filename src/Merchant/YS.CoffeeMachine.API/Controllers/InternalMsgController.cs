using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.InternalMsgCommands;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 站内信管理
    /// </summary>
    /// <param name="mediator"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class InternalMsgController(IMediator mediator) : Controller
    {

        #region 用户消息
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
