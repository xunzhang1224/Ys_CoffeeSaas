using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 应用菜单
    /// </summary>
    public class ApplicationMenuDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum SysMenuType { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Rank { get; set; } = 99;
        /// <summary>
        /// 重定向
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 右侧图标
        /// </summary>
        public string ExtraIcon { get; set; }
        /// <summary>
        /// 进场动画
        /// </summary>
        public string EnterTransition { get; set; }
        /// <summary>
        /// 出场动画
        /// </summary>
        public string LeaveTransition { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public List<string> Auths { get; set; }

        /// <summary>
        /// 按钮权限
        /// </summary>
        public string Auth { get; set; }
        /// <summary>
        /// iframe 链接地址
        /// </summary>
        public string FrameSrc { get; set; }
        /// <summary>
        /// 加载动画
        /// </summary>
        public bool FrameLoading { get; set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; } = false;
        /// <summary>
        /// 是否显示标签页
        /// </summary>
        public bool HiddenTag { get; set; } = false;
        /// <summary>
        /// 是否固定标签页
        /// </summary>
        public bool FixedTag { get; set; } = false;
        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public bool ShowLink { get; set; } = true;
        /// <summary>
        /// 是否显示父级菜单
        /// </summary>
        public bool ShowParent { get; set; } = false;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 菜单激活
        /// </summary>
        public string ActivePath { get; set; }

        /// <summary>
        /// Children
        /// </summary>
        public List<ApplicationMenuDto> Children { get; set; }

        /// <summary>
        /// ApplicationMenuDto
        /// </summary>
        public ApplicationMenuDto() { }

        /// <summary>
        /// ApplicationMenuDto
        /// </summary>
        /// <param name="menu"></param>
        public ApplicationMenuDto(ApplicationMenu menu)
        {
            var menuBtnList = new List<ApplicationMenu>();
            if (menu.ApplicationMenus != null && menu.ApplicationMenus.Count > 0)
                menuBtnList = menu.ApplicationMenus.Where(w => w.MenuType == MenuTypeEnum.Btn).ToList();
            Id = menu.Id;
            ParentId = menu.ParentId;
            MenuType = menu.MenuType;
            SysMenuType = menu.SysMenuType;
            Title = menu.Title;
            Name = menu.Name;
            Path = menu.Path;
            Component = menu.Component;
            Rank = menu.Rank;
            Redirect = menu.Redirect;
            Icon = menu.Icon;
            ExtraIcon = menu.ExtraIcon;
            EnterTransition = menu.EnterTransition;
            LeaveTransition = menu.LeaveTransition;
            FrameSrc = menu.FrameSrc;
            FrameLoading = menu.FrameLoading;
            KeepAlive = menu.KeepAlive;
            HiddenTag = menu.HiddenTag;
            FixedTag = menu.FixedTag;
            ShowLink = menu.ShowLink;
            ShowParent = menu.ShowParent;
            Remark = menu.Remark;
            ActivePath = menu.ActivePath;
            Auth = string.Join(",", menuBtnList.Select(s => s.Auths));
            Auths = menuBtnList.Select(s => s.Auths).ToList();
        }

        /// <summary>
        /// ApplicationMenuDto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pId"></param>
        /// <param name="typeEnum"></param>
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
        public ApplicationMenuDto(long id, long? pId, MenuTypeEnum typeEnum, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark, string activePath)
        {
            Id = id;
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
            Auth = auths;
            FrameSrc = frameSrc;
            FrameLoading = frameLoading;
            KeepAlive = keepAlive;
            HiddenTag = hiddenTag;
            FixedTag = fixedTag;
            ShowLink = showLink;
            ShowParent = showParent;
            Remark = remark;
            ActivePath = activePath;
        }
    }

    /// <summary>
    /// 商户及H5全部菜单选择数据传输对象
    /// </summary>
    public class AllMenuSelectDto
    {
        /// <summary>
        /// PC菜单选择数据传输对象，用于菜单的树形结构展示或选择操作
        /// </summary>
        public List<MenuSelectDto> PCMenuSelectDtos { get; set; }

        /// <summary>
        /// H5菜单选择数据传输对象，用于菜单的树形结构展示或选择操作
        /// </summary>
        public List<MenuSelectDto> H5MenuSelectDtos { get; set; }

        /// <summary>
        /// 构造函数，初始化菜单选择数据传输对象的集合
        /// </summary>
        public AllMenuSelectDto()
        {
            PCMenuSelectDtos = new List<MenuSelectDto>();
            H5MenuSelectDtos = new List<MenuSelectDto>();
        }
    }

    /// <summary>
    /// 菜单选择数据传输对象，用于菜单的树形结构展示或选择操作
    /// </summary>
    public class MenuSelectDto
    {
        /// <summary>
        /// 菜单唯一标识ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级菜单ID（可为空，表示根节点）
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 菜单类型，如：目录、菜单项、按钮等
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }

        /// <summary>
        /// 菜单标题，用于界面显示
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单名称，通常作为唯一标识符使用（如路由名称）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子菜单数量，用于前端展示判断是否有子节点
        /// </summary>
        public int SubCount { get; set; }

        /// <summary>
        /// 子菜单集合，支持树形结构递归
        /// </summary>
        public List<MenuSelectDto> Children { get; set; } = new List<MenuSelectDto>();
    }

    /// <summary>
    /// 应用菜单列表数据传输对象，用于封装多个 ApplicationMenuDto 实例的集合
    /// </summary>
    public class ApplicationMenuListDto
    {
        /// <summary>
        /// 菜单 DTO 列表，包含多个菜单信息
        /// </summary>
        public List<ApplicationMenuDto> ApplicationMenuDtos { get; set; }

        /// <summary>
        /// 受保护的无参构造函数，初始化空的菜单 DTO 列表
        /// </summary>
        protected ApplicationMenuListDto()
        {
            ApplicationMenuDtos = new List<ApplicationMenuDto>();
        }

        /// <summary>
        /// 根据 ApplicationMenu 实体列表初始化菜单 DTO 列表
        /// </summary>
        /// <param name="applicationMenus">应用菜单实体集合</param>
        public ApplicationMenuListDto(List<ApplicationMenu> applicationMenus)
        {
            if (applicationMenus.Count > 0)
            {
                ApplicationMenuDtos = new List<ApplicationMenuDto>();
                applicationMenus.ForEach(x =>
                {
                    ApplicationMenuDtos.Add(new ApplicationMenuDto(x.Id, x.ParentId, x.MenuType, x.SysMenuType, x.Title, x.Name, x.Path, x.Component, x.Rank, x.Redirect, x.Icon, x.ExtraIcon, x.EnterTransition, x.LeaveTransition, x.Auths, x.FrameSrc, x.FrameLoading, x.KeepAlive, x.HiddenTag, x.FixedTag, x.ShowLink, x.ShowParent, x.Remark, x.ActivePath));
                });
            }
        }
    }

    /// <summary>
    /// 应用菜单数据传输对象，用于封装单个 ApplicationMenu 实例的信息
    /// </summary>
    public class UserMenus
    {
        /// <summary>
        /// 菜单Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 菜单标题，用于界面显示
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }

        /// <summary>
        /// Meta
        /// </summary>
        public MetaModel Meta { get; set; }

        /// <summary>
        /// Children
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<UserMenus>? Children { get; set; }

        /// <summary>
        /// MetaModel
        /// </summary>
        public class MetaModel
        {
            //public long Id { get; set; }
            ////[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            //public string Title { get; set; }
            //public string Icon { get; set; }
            //public int Rank { get; set; }
            //public List<long> Roles { get; set; }
            //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            //public List<string>? Auths { get; set; }

            /// <summary>
            /// Id
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// 父Id
            /// </summary>
            public long? ParentId { get; set; }
            /// <summary>
            /// 菜单类型
            /// </summary>
            public MenuTypeEnum MenuType { get; set; }
            /// <summary>
            /// 系统类型
            /// </summary>
            public SysMenuTypeEnum SysMenuType { get; set; }
            /// <summary>
            /// 菜单名称
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 路由名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 路由地址
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// 组件路径
            /// </summary>
            public string Component { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int Rank { get; set; } = 99;
            /// <summary>
            /// 重定向
            /// </summary>
            public string Redirect { get; set; }
            /// <summary>
            /// 图标
            /// </summary>
            public string Icon { get; set; }
            /// <summary>
            /// 右侧图标
            /// </summary>
            public string ExtraIcon { get; set; }
            /// <summary>
            /// 进场动画
            /// </summary>
            public string EnterTransition { get; set; }
            /// <summary>
            /// 出场动画
            /// </summary>
            public string LeaveTransition { get; set; }
            /// <summary>
            /// iframe 链接地址
            /// </summary>
            public string FrameSrc { get; set; }
            /// <summary>
            /// 加载动画
            /// </summary>
            public bool FrameLoading { get; set; }
            /// <summary>
            /// 是否缓存
            /// </summary>
            public bool KeepAlive { get; set; } = false;
            /// <summary>
            /// 是否显示标签页
            /// </summary>
            public bool HiddenTag { get; set; } = false;
            /// <summary>
            /// 是否固定标签页
            /// </summary>
            public bool FixedTag { get; set; } = false;
            /// <summary>
            /// 是否显示菜单
            /// </summary>
            public bool ShowLink { get; set; } = true;
            /// <summary>
            /// 是否显示父级菜单
            /// </summary>
            public bool ShowParent { get; set; } = false;
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// 菜单激活
            /// </summary>
            public string ActivePath { get; set; }

            /// <summary>
            /// 角色
            /// </summary>
            public List<long> Roles { get; set; } = new List<long>();

            /// <summary>
            /// 权限
            /// </summary>
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<string>? Auths { get; set; }
        }
    }
}
