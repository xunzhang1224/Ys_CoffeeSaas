namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Newtonsoft.Json;
    using YS.CoffeeMachine.Application.Commands.BasicCommands;
    using YS.CoffeeMachine.Domain.Shared.Enum;

    /// <summary>
    /// 实体变更审计条目类。
    /// 用于记录 Entity Framework Core 中实体的变更信息，并生成审计日志。
    /// </summary>
    public class AuditEntry
    {
        /// <summary>
        /// 初始化一个新的 <see cref="AuditEntry"/> 实例。
        /// </summary>
        /// <param name="entry">当前实体的变更条目。</param>
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// 获取当前实体的变更条目。
        /// </summary>
        public EntityEntry Entry { get; }

        /// <summary>
        /// 获取或设置被修改实体对应的数据库表名。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 获取或设置执行操作的用户ID。
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 获取或设置执行操作的用户昵称或用户名。
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置本次操作的类型（如新增、更新、删除等）。
        /// </summary>
        public TrailTypeEnum TrailType { get; set; }

        /// <summary>
        /// 获取主键值字典，用于唯一标识被修改的实体。
        /// </summary>
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 获取修改前的属性值集合。
        /// </summary>
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 获取修改后的属性值集合。
        /// </summary>
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 获取临时属性集合（尚未保存到数据库的属性）。
        /// </summary>
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        /// <summary>
        /// 指示是否包含临时属性。
        /// </summary>
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        /// <summary>
        /// 将当前审计条目转换为可持久化的 <see cref="Audit"/> 对象。
        /// </summary>
        /// <returns>一个包含完整审计信息的 <see cref="Audit"/> 实例。</returns>
        public Audit ToAudit()
        {
            return new Audit(
                TableName,
                JsonConvert.SerializeObject(KeyValues),
                OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                TrailType,
                UserId,
                NickName
            );
        }
    }
}