using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO.Base;

/// <summary>
/// 用于发送指令的数据载体。
/// </summary>
/// <typeparam name="T">指令DTO</typeparam>
public class DownlinkSendDto<T>
{
    /// <summary>
    /// 机器Id。
    /// </summary>
    public long? VendId { get; set; }

    /// <summary>
    /// 机器编号
    /// </summary>
    public string VendCode { get; set; }

    /// <summary>
    /// 操作人UserId
    /// </summary>
    public long? OperatorId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string OperatorName { get; set; }

    /// <summary>
    /// 数据对象
    /// </summary>
    public T Data { get; set; }
}
