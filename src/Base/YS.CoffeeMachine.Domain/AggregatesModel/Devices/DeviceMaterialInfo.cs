using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 物料信息
    /// </summary>
    public class DeviceMaterialInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// GetKeys
        /// </summary>
        /// <returns></returns>
        //public override object[] GetKeys() => new object[] { DeviceBaseId, Type, Index };

        /// <summary>
        /// 获取或设置关联的平台设备唯一标识符。
        /// 用于标识该属性所属的设备基础信息。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 类型
        /// 1咖啡豆、2水量、3料盒、4杯盖、5杯子
        /// </summary>
        [Required]
        public MaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 序列号
        /// 从1开始
        /// </summary>
        [Required]
        public int Index { get; set; } = 1;

        /// <summary>
        /// 名字
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get; set; } = 0;

        /// <summary>
        /// 存量
        /// </summary>
        public int Stock { get; set; } = 0;

        /// <summary>
        /// 是否是糖
        /// </summary>
        public bool IsSugar { get; set; } = false;

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceMaterialInfo() { }

        /// <summary>
        /// 1010能力配置上报
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="issugar"></param>
        public DeviceMaterialInfo(long deviceBaseId, MaterialTypeEnum type, int index, string? name, bool issugar = false, int? capacity = null)
        {
            DeviceBaseId = deviceBaseId;
            Type = type;
            Index = index;
            Name = name;
            IsSugar = issugar;
            if (capacity != null)
            {
                Capacity = capacity ?? 0;
            }
        }

        /// <summary>
        /// 9022
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceBaseId"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="capacity"></param>
        /// <param name="stock"></param>
        /// <param name="issugar"></param>
        public DeviceMaterialInfo(long id, long deviceBaseId, MaterialTypeEnum type, int index, string? name, int capacity, int stock,bool issugar = false)
        {
            Id = id;
            DeviceBaseId = deviceBaseId;
            Type = type;
            Index = index;
            Name = name;
            Capacity = capacity;
            Stock = stock;
            IsSugar = issugar;
        }

        /// <summary>
        /// 9022 上报物料库存
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="stock"></param>
        /// <param name="warning"></param>
        public void Update(int capacity, int stock)
        {
            Capacity = capacity;
            Stock = stock;
            LastModifyTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="name"></param>
        public void UpdateName(string? name)
        {
            Name = name;
        }

        /// <summary>
        /// 修改糖
        /// </summary>
        /// <param name="issugar"></param>
        public void UpdateIsSugar(bool issugar = false)
        {
            IsSugar = issugar;
        }

        /// <summary>
         /// 修改容量
         /// </summary>
         /// <param name="issugar"></param>
        public void UpdateCapacity(int capacity)
        {
            Capacity = capacity;
        }
    }
}
