using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 管理输入
    /// </summary>
    public class DeviceVersionManageInput : QueryRequest
    {
        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; } = null;

        /// <summary>
        /// 设备类型(通过字典获取)
        /// </summary>
        public string DeviceType { get; set; } = null;

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; } = null;

        /// <summary>
        /// 程序类型(通过字典获取)
        /// </summary>
        public string? ProgramTypeName { get; set; } = null;

        /// <summary>
        /// 版本类型(通过字典获取)
        /// </summary>
        public int? VersionType { get; set; } = null;

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum? Enabled { get; set; } = null;

        /// <summary>
        /// 更新说明
        /// </summary>
        public string Remark { get; set; } = null;

        /// <summary>
        /// 发布时间范围
        /// </summary>
        public List<DateTime>? CreateTimeRange { get; set; } = null;
    }

    /// <summary>
    /// 更新记录输入
    /// </summary>
    public class DeviceVersionUpdateRecordInput : QueryRequest
    {
        /// <summary>
        /// 设备名称或设备编号
        /// </summary>
        public string? DeviceNameSn { get; set; } = null;

        /// <summary>
        /// 企业id
        /// </summary>
        public long? EnterpriseId { get; set; } = null;

        /// <summary>
        /// 企业名称
        /// </summary>
        public string? EnterpriseName { get; set; } = null;

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool? IsOnline { get; set; } = null;

        /// <summary>
        /// 状态(
        /// </summary>
        public int? State { get; set; } = null;

        /// <summary>
        /// 更新时间范围
        /// </summary>
        public List<DateTime>? UpdateTimeRange { get; set; } = null;
    }

    /// <summary>
    /// 更新记录查询（根据版本）
    /// </summary>
    public class PushDataByVersionInput : QueryRequest
    {
        /// <summary>
        /// 管理Id
        /// </summary>
        public long DeviceVersionManageId { get; set; }

        /// <summary>
        /// 设备名称或设备编号
        /// </summary>
        public string? DeviceNameSn { get; set; } = null;

        /// <summary>
        /// 更新时间范围
        /// </summary>
        public List<DateTime>? PushTimeRange { get; set; } = null;
    }

    /// <summary>
    /// 更新记录查询(根据设备)
    /// </summary>
    public class PushDataByDeviceInput : QueryRequest
    {
        /// <summary>
        /// 管理Id
        /// </summary>
        public long DeviceBaseInfoId { get; set; }

    }
}
