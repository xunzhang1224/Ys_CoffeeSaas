using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 管理
    /// </summary>
    public class DeviceVersionManageDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 设备类型(通过字典获取)
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string? DeviceModelName { get; set; }

        /// <summary>
        /// 程序类型(通过字典获取)
        /// </summary>
        public int? ProgramType { get; set; }

        /// <summary>
        /// 程序类型(通过字典获取)
        /// </summary>
        public string? ProgramTypeName { get; set; }

        /// <summary>
        /// 版本类型(通过字典获取)
        /// </summary>
        public int? VersionType { get; set; }

        /// <summary>
        /// 程序oss地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 程序描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum? Enabled { get; set; }

        /// <summary>
        /// 推送次数
        /// </summary>
        public int PushCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 程序版本号
        /// </summary>
        public string VersionNumber { get; set; }
    }

    /// <summary>
    /// 更新记录
    /// </summary>
    public class DeviceVersionUpdateRecordDto
    {
        /// <summary>
        /// 设备基础信息Id
        /// </summary>
        public long DeviceBaseInfoId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string? DeviceModelName { get; set; }

        /// <summary>
        /// 企业名字
        /// </summary>
        public string? EnterpriseName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 版本更新记录
    /// </summary>
    public class PushDataByVersionDto
    {
        /// <summary>
        /// 设备名字
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineStickerCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 推送状态
        /// </summary>
        public VersionPushStateEnum PushState { get; set; }

        /// <summary>
        /// 程序类型
        /// </summary>
        public string ProgramTypeName { get; set; }
    }

    /// <summary>
    /// 设备更新记录
    /// </summary>
    public class PushDataByDeviceDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程序类型
        /// </summary>
        public int ProgramType { get; set; }

        /// <summary>
        /// 版本类型
        /// </summary>
        public int VersionType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 推送状态
        /// </summary>
        public VersionPushStateEnum PushState { get; set; }

        /// <summary>
        /// 程序类型
        /// </summary>
        public string ProgramTypeName { get; set; }
    }

    /// <summary>
    /// 推送版本dto
    /// </summary>
    public class PushVersionDto
    {
        /// <summary>
        /// Mid
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 推送记录id
        /// </summary>
        public long RecordId { get; set; }
    }
}
