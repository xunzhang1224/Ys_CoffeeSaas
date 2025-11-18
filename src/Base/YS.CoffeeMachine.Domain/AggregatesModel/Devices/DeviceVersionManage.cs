using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 管理
    /// </summary>
    public class DeviceVersionManage : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; private set; }

        /// <summary>
        /// 设备类型(通过字典获取)
        /// </summary>
        public string DeviceType { get; private set; }

        /// <summary>
        /// 程序版本名称
        /// </summary>
        public string? VersionNumber { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; private set; }

        /// <summary>
        /// 安卓应用程序1(主程序版本,皮肤插件版本,驱动程序版本)，单片机程序2，安卓固件程序3
        /// </summary>
        public int? ProgramType { get; private set; }

        /// <summary>
        /// 程序类型名称(com.tcn.drivers,com.tcn.tcnstand,,com.tcn.vending,drive,system)
        /// </summary>
        public string? ProgramTypeName { get; private set; }

        /// <summary>
        /// 版本类型 1=公测版 2=内测版 3=稳定版
        /// </summary>
        public int? VersionType { get; private set; }

        /// <summary>
        /// 程序oss地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 程序描述
        /// </summary>
        public string? Remark { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum? Enabled { get; private set; } = EnabledEnum.Enable;

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceVersionManage() { }

        /// <summary>
        /// 创建管理
        /// </summary>
        public DeviceVersionManage(
        string name,
        string deviceType,
        string? versionNumber,
        long? deviceModelId,
        int programType,
        int versionType,
        string url,
        string? remark,
        string? programTypeName)
        {
            Name = name;
            DeviceType = deviceType;
            VersionNumber = versionNumber;
            DeviceModelId = deviceModelId;
            ProgramType = programType;
            VersionType = versionType;
            Url = url;
            Remark = remark;
            Enabled = EnabledEnum.Enable;
            ProgramTypeName = programTypeName;
        }

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="enabled"></param>
        public void UpdateState(EnabledEnum enabled)
        {
            Enabled = enabled;
        }
    }
}
