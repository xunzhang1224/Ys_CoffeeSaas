using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    /// <summary>
    /// 设备事件下发
    /// </summary>
    public class CommandDownSend : CommunicationBase
    {
        /// <summary>
        /// 根据method转json
        /// </summary>
        public string Params { get; set; }
    }

    /// <summary>
    /// 批量设备事件下发
    /// </summary>
    public class CommandDownSends
    {
        /// <summary>
        /// 需要下发的设备集合
        /// </summary>
        public List<string> Mids { get; set; }

        /// <summary>
        /// 机器编号
        /// 用来展示操作日志主表设备编号
        /// </summary>
        public string Mid { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 服务/命令 唯一标识
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 消息唯一id
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 根据method转json
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsRecordLog { get; set; } = false;
    }

    /// <summary>
    /// 批量设备事件下发
    /// </summary>
    public class CommandSJDownSends
    {
        /// <summary>
        /// dto
        /// </summary>
        public List<SjDto> Dtos { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 服务/命令 唯一标识
        /// </summary>
        public string Method { get; set; }
    }

    /// <summary>
    /// sj
    /// </summary>
    public class SjDto
    {
        /// <summary>
        /// 机器编号
        /// 用来展示操作日志主表设备编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 消息唯一id
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 根据method转json
        /// </summary>
        public string Params { get; set; }
    }
}