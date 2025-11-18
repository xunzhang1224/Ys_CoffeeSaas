namespace YS.CoffeeMachine.Application.Commands.BasicCommands
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 审计聚合根
    /// </summary>
    public class Audit : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 操作的表名
        /// 作为key存多语言
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 操作数据主键
        /// </summary>
        public string KeyValues { get; private set; }

        /// <summary>
        /// 修改前的值
        /// </summary>
        public string OldValues { get; private set; }

        /// <summary>
        /// 修改后的值
        /// </summary>
        public string NewValues { get; private set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public TrailTypeEnum TrailType { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected Audit() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">操作表名</param>
        /// <param name="keyValues">操作数据主键</param>
        /// <param name="oldValues">修改前的值</param>
        /// <param name="newValues">修改后的值</param>
        /// <param name="trailType">操作类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="NickName">用户昵称</param>
        public Audit(string tableName, string keyValues, string oldValues, string newValues, TrailTypeEnum trailType, long userId, string NickName)
        {
            TableName = tableName;
            KeyValues = keyValues;
            OldValues = oldValues;
            NewValues = newValues;
            TrailType = trailType;
            CreateUserId = userId;
            CreateUserName = NickName;
        }
    }
}
