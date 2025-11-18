using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 设备冲洗部件
    /// </summary>
    public class DeviceFlushComponents : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的平台设备唯一标识符。
        /// 用于标识该属性所属的设备基础信息。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 部件类型
        /// </summary>
        [Required]
        public FlushComponentTypeEnum Type { get; set; }

        /// <summary>
        /// 序列号
        /// 从1开始
        /// </summary>
        [Required]
        public int Index { get; set; } = 0;

        /// <summary>
        /// 名字
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceFlushComponents() { }

        /// <summary>
        /// 9031冲洗部件上报
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        public DeviceFlushComponents(long deviceBaseId, FlushComponentTypeEnum type, int index, string? name)
        {
            DeviceBaseId = deviceBaseId;
            Type = type;
            Index = index;
            Name = name;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name)
        {
            Name = name;
        }
    }
}
