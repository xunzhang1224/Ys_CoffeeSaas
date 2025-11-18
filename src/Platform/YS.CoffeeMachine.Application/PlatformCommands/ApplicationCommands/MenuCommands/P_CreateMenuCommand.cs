using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.MenuCommands
{
    public record P_CreateMenuCommand(long? parentId, MenuTypeEnum menuType, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark) : ICommand<bool>;
}