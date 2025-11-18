using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands
{
    public record UpdateApplicationMenuCommand(long id, long? parentId, MenuTypeEnum menuType, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark,string activePath) : ICommand<bool>;
}
