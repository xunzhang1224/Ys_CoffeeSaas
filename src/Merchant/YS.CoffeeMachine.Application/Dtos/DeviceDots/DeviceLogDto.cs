using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备日志
    /// </summary>
    public class DeviceLogDto
    {
    }

    /// <summary>
    /// 设备日志
    /// </summary>
    public class DeviceOnlineLogDto
    {
        /// <summary>
        /// 设备baseId
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineStickerCode { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// 设备事件日志
    /// </summary>
    public class DeviceEventLogDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperationTime { get; set; }
    }

    /// <summary>
    /// 设备错误日志
    /// </summary>
    public class DeviceErrorLogDto
    {

        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 异常码
        /// </summary>
        public string AbnormalCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 设备补货记录
    /// </summary>
    public class DeviceRestockLogDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 租户设备id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        public string DeviceCode { get; set; }

        /// <summary>
        /// 补货编号
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public string? DeviceDZ { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public RestockTypeEnum Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<DeviceRestockLogSub> DeviceRestockLogSubs { get; set; }

    }
}
