using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 补货明细
    /// </summary>
    public class DeviceRestockLogSub : Entity<long>
    {
        /// <summary>
        /// 货柜类型
        /// </summary>
        public HGTypeEnum HGType { get;private set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; private set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string MaterialName { get; private set; }

        /// <summary>
        /// OldValue
        /// </summary>
        public int OldValue { get; private set; }

        /// <summary>
        /// Value
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// NewValue
        /// </summary>
        public int NewValue { get; private set; }

        /// <summary>
        /// 主键id
        /// </summary>
        public long DeviceRestockLogId { get; private set; }

        /// <summary>
        /// 1
        /// </summary>
        [JsonIgnore]
        public DeviceRestockLog DeviceRestockLog { get; private set; }

        /// <summary>
        /// 1
        /// </summary>
        protected DeviceRestockLogSub() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hgType">货柜类型</param>
        /// <param name="materialId">物料id</param>
        /// <param name="materialName">物料名称</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="value">值</param>
        /// <param name="newValue">新值</param>
        public DeviceRestockLogSub(
            HGTypeEnum hgType,
            long materialId,
            string materialName,
            int oldValue,
            int value,
            int newValue)
        {
            HGType = hgType;
            MaterialId = materialId;
            MaterialName = materialName;
            OldValue = oldValue;
            Value = value;
            NewValue = newValue;
        }
    }
}
