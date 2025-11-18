using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.MenuCommands
{
    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parentId"></param>
    /// <param name="menuType"></param>
    /// <param name="sysMenuType"></param>
    /// <param name="title"></param>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="component"></param>
    /// <param name="rank"></param>
    /// <param name="redirect"></param>
    /// <param name="icon"></param>
    /// <param name="extraIcon"></param>
    /// <param name="enterTransition"></param>
    /// <param name="leaveTransition"></param>
    /// <param name="auths"></param>
    /// <param name="frameSrc"></param>
    /// <param name="frameLoading"></param>
    /// <param name="keepAlive"></param>
    /// <param name="hiddenTag"></param>
    /// <param name="fixedTag"></param>
    /// <param name="showLink"></param>
    /// <param name="showParent"></param>
    /// <param name="remark"></param>
    /// <param name="activePath"></param>
    public record P_UpdateMenuCommand(long id,
        long? parentId,
        MenuTypeEnum menuType,
        SysMenuTypeEnum sysMenuType,
        string title, string name,
        string path, string component,
        int rank, string redirect,
        string icon, string extraIcon,
        string enterTransition,
        string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag,
        bool showLink, bool showParent, string remark,string activePath) : ICommand<bool>;
}
