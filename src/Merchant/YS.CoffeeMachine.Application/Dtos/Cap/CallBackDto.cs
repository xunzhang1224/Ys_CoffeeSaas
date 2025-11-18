using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// 设备事件批量下发
    /// </summary>
    public class CommandDownSendBydic : CommunicationBase
    {
        /// <summary>
        /// 根据method转json
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
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
    /// 应用信息
    /// </summary>
    public class Yydto
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        public BeverageAppliedType? AppliedType { get; set; }

        /// <summary>
        /// 替换目标
        /// </summary>
        public string? ReplaceTarget { get; set; }

        /// <summary>
        /// 新sku(方式为新增时候使用)
        /// </summary>
        public long? NewSku { get; set; } = null;
    }

    /// <summary>
    /// 批量设备饮品下发
    /// </summary>
    public class DrinkCommandDownSends
    {
        /// <summary>
        /// 需要下发的设备/饮品集合
        /// 空/新增，有值替换
        /// </summary>
        [Required]
        public Dictionary<string, string?> Datas { get; set; }

        /// <summary>
        /// mid匹配对应的应用方式/目标
        /// </summary>
        [Required]
        public Dictionary<string, Yydto> Yys { get; set; }

        /// <summary>
        /// 需要更新部分饮品信息的数据
        /// </summary>
        public Dictionary<string, string?> Datass { get; set; }

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
}