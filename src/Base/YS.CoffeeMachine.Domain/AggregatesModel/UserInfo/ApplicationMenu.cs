using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    public class ApplicationMenu : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public long? ParentId { get; private set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum MenuType { get; private set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum SysMenuType { get; private set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; private set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Rank { get; private set; } = 99;
        /// <summary>
        /// 重定向
        /// </summary>
        public string Redirect { get; private set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; private set; }
        /// <summary>
        /// 右侧图标
        /// </summary>
        public string ExtraIcon { get; private set; }
        /// <summary>
        /// 进场动画
        /// </summary>
        public string EnterTransition { get; private set; }
        /// <summary>
        /// 出场动画
        /// </summary>
        public string LeaveTransition { get; private set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Auths { get; private set; }
        /// <summary>
        /// iframe 链接地址
        /// </summary>
        public string FrameSrc { get; private set; }
        /// <summary>
        /// 加载动画
        /// </summary>
        public bool FrameLoading { get; private set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; private set; } = false;
        /// <summary>
        /// 是否显示标签页
        /// </summary>
        public bool HiddenTag { get; private set; } = false;
        /// <summary>
        /// 是否固定标签页
        /// </summary>
        public bool FixedTag { get; private set; } = false;
        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public bool ShowLink { get; private set; } = true;
        /// <summary>
        /// 是否显示父级菜单
        /// </summary>
        public bool ShowParent { get; private set; } = false;
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 菜单激活
        /// </summary>
        public string ActivePath { get; private set; }

        /// <summary>
        /// 菜单集合
        /// </summary>
        public List<ApplicationMenu> ApplicationMenus { get; private set; }
        /// <summary>
        /// 私有构造
        /// </summary>
        protected ApplicationMenu()
        {
            ApplicationMenus = new List<ApplicationMenu>();
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        public ApplicationMenu(long? pId, MenuTypeEnum typeEnum, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark, List<ApplicationMenu>? menus = null)
        {
            ParentId = pId;
            MenuType = typeEnum;
            SysMenuType = sysMenuType;
            Title = title;
            Name = name;
            Path = path;
            Component = component;
            Rank = rank;
            Redirect = redirect;
            Icon = icon;
            ExtraIcon = extraIcon;
            EnterTransition = enterTransition;
            LeaveTransition = leaveTransition;
            Auths = auths;
            FrameSrc = frameSrc;
            FrameLoading = frameLoading;
            KeepAlive = keepAlive;
            HiddenTag = hiddenTag;
            FixedTag = fixedTag;
            ShowLink = showLink;
            ShowParent = showParent;
            IsDefault = false;
            Remark = remark;
            if (menus != null && menus.Count > 0)
            {
                ApplicationMenus = menus;
            }
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        public void Update(long? pId, MenuTypeEnum typeEnum, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark,string activePath, List<ApplicationMenu>? menus = null)
        {
            ParentId = pId;
            MenuType = typeEnum;
            SysMenuType = sysMenuType;
            Title = title;
            Name = name;
            Path = path;
            Component = component;
            Rank = rank;
            Redirect = redirect;
            Icon = icon;
            ExtraIcon = extraIcon;
            EnterTransition = enterTransition;
            LeaveTransition = leaveTransition;
            Auths = auths;
            FrameSrc = frameSrc;
            FrameLoading = frameLoading;
            KeepAlive = keepAlive;
            HiddenTag = hiddenTag;
            FixedTag = fixedTag;
            ShowLink = showLink;
            ShowParent = showParent;
            Remark = remark;
            ActivePath = activePath;
            if (menus != null && menus.Count > 0)
            {
                ApplicationMenus = menus;
            }
        }
    }
}
