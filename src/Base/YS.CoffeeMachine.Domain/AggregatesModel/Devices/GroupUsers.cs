namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示分组与用户之间关联的实体类。
    /// 用于实现设备分组与用户之间的多对多关系管理。
    /// </summary>
    public class GroupUsers : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的分组唯一标识符。
        /// </summary>
        public long GroupsId { get; private set; }

        /// <summary>
        /// 获取或设置关联的用户唯一标识符。
        /// </summary>
        public long ApplicationUserId { get; private set; }

        /// <summary>
        /// 获取与此关联绑定的用户实体对象。
        /// 用于导航至用户信息。
        /// </summary>
        public ApplicationUser ApplicationUser { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected GroupUsers() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 GroupUsers 实例。
        /// </summary>
        /// <param name="groupId">分组的唯一标识。</param>
        /// <param name="userId">用户的唯一标识。</param>
        public GroupUsers(long groupId, long userId)
        {
            GroupsId = groupId;
            ApplicationUserId = userId;
        }
    }
}