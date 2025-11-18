using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.TermServiceCommands
{
    /// <summary>
    /// 创建服务条款命令
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="description"></param>
    /// <param name="enabled"></param>
    public record CreateTermServiceCommand(string title, string content, string description, EnabledEnum enabled) : ICommand<bool>;

    /// <summary>
    /// 编辑服务条款命令
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="description"></param>
    /// <param name="enabled"></param>
    public record UpdateTermServiceCommand(long id, string title, string content, string description, EnabledEnum enabled) : ICommand<bool>;

    /// <summary>
    /// 删除服务条款命令
    /// </summary>
    /// <param name="id"></param>
    public record DeleteTermServiceCommand(long id) : ICommand<bool>;

    /// <summary>
    /// 变更服务条款状态命令
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    public record ChangeTermServiceStatusCommand(long id, EnabledEnum enabled) : ICommand<bool>;
}