using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AccountTypeEnum
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 0,

        ///// <summary>
        ///// 系统管理员
        ///// </summary>
        //[Description("系统管理员")]
        //SysAdmin = 1,

        /// <summary>
        /// 普通账号
        /// </summary>
        [Description("普通账号")]
        NormalUser = 2,

        ///// <summary>
        ///// 员工
        ///// </summary>
        //[Description("员工")]
        //Member = 3,
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disable = 0
    }

    /// <summary>
    /// 角色状态
    /// </summary>
    public enum RoleStatusEnum
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disable = 0,
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleTypeEnum
    {
        [Description("默认")]
        DefaultRole = 0,
        [Description("普通")]
        RrdinaryRole = 1
    }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuTypeEnum
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 0,
        /// <summary>
        /// Iframe
        /// </summary>
        [Description("Iframe")]
        Iframe = 1,
        /// <summary>
        /// 外链
        /// </summary>
        [Description("外链")]
        Link = 2,
        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Btn = 3
    }

    /// <summary>
    /// 系统菜单类型
    /// </summary>
    public enum SysMenuTypeEnum
    {
        [Description("平台")]
        Platform = 0,
        [Description("商户")]
        Merchant = 1,
        [Description("移动端")]
        H5 = 2,
        [Description("消费者端")]
        Consumer = 3,
    }

    /// <summary>
    /// 菜单状态
    /// </summary>
    //public enum MenuStatusEnum
    //{
    //    /// <summary>
    //    /// 启用
    //    /// </summary>
    //    [Description("启用")]
    //    Enable = 0,

    //    /// <summary>
    //    /// 停用
    //    /// </summary>
    //    [Description("停用")]
    //    Disable = 1,
    //}
}
