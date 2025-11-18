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
    /// 菜单信息
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
        /// 权限标识
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
        /// 菜单信息
        /// </summary>
        public List<ApplicationMenuDto> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationMenuDto() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationMenuDto(ApplicationMenu menu, List<long>? excludeMenuIds = null)
        {
            if (excludeMenuIds == null)
                excludeMenuIds = new List<long>();
            var menuBtnList = new List<ApplicationMenu>();
            if (menu.ApplicationMenus != null && menu.ApplicationMenus.Count > 0)
                menuBtnList = menu.ApplicationMenus.Where(w => w.MenuType == MenuTypeEnum.Btn && !excludeMenuIds.Contains(w.Id)).ToList();
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
            Auth = string.Join(",", menuBtnList.Select(s => s.Auths));
            Auths = menuBtnList.Select(s => s.Auths).ToList();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationMenuDto(long id, long? pId, MenuTypeEnum typeEnum, SysMenuTypeEnum sysMenuType, string title, string name, string path, string component, int rank, string redirect, string icon, string extraIcon, string enterTransition, string leaveTransition, string auths, string frameSrc, bool frameLoading, bool keepAlive, bool hiddenTag, bool fixedTag, bool showLink, bool showParent, string remark)
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
        }
    }

    /// <summary>
    /// 菜单选择信息
    /// </summary>
    public class MenuSelectDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级菜单Id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }

        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子级菜单数量
        /// </summary>
        public int SubCount { get; set; }

        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<MenuSelectDto> Children { get; set; } = new List<MenuSelectDto>();
    }

    /// <summary>
    /// 菜单Dto
    /// </summary>
    public class MenusDto
    {
        /// <summary>
        /// 商户端菜单
        /// </summary>
        public ApplicationMenuListDto ApplicationMenuList { get; set; }

        /// <summary>
        /// H5菜单
        /// </summary>
        public ApplicationMenuListDto ApplicationH5MenuListDto { get; set; }
    }

    /// <summary>
    /// 菜单列表信息
    /// </summary>
    public class ApplicationMenuListDto
    {
        /// <summary>
        /// 菜单信息
        /// </summary>
        public List<ApplicationMenuDto> ApplicationMenuDtos { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ApplicationMenuListDto() { ApplicationMenuDtos = new List<ApplicationMenuDto>(); }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationMenuListDto(List<ApplicationMenu> applicationMenus)
        {
            if (applicationMenus.Count > 0)
            {
                ApplicationMenuDtos = new List<ApplicationMenuDto>();
                applicationMenus.ForEach(x =>
                {
                    ApplicationMenuDtos.Add(new ApplicationMenuDto(x.Id, x.ParentId, x.MenuType, x.SysMenuType, x.Title, x.Name, x.Path, x.Component, x.Rank, x.Redirect, x.Icon, x.ExtraIcon, x.EnterTransition, x.LeaveTransition, x.Auths, x.FrameSrc, x.FrameLoading, x.KeepAlive, x.HiddenTag, x.FixedTag, x.ShowLink, x.ShowParent, x.Remark));
                });
            }
        }
    }

    /// <summary>
    /// 用户菜单信息
    /// </summary>
    public class UserMenus
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public MetaModel Meta { get; set; }

        /// <summary>
        /// 子级菜单
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<UserMenus>? Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public class MetaModel
        {
            //public long TransId { get; set; }
            ////[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            //public string Title { get; set; }
            //public string Icon { get; set; }
            //public int Rank { get; set; }
            //public List<long> Roles { get; set; }
            //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            //public List<string>? Auths { get; set; }

            /// <summary>
            /// 菜单Id
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
            /// 角色Id
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
