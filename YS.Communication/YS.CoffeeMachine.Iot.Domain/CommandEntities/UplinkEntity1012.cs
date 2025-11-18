namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;
using System.Collections.Generic;

/// <summary>
/// 用于1012号指令的协议实体：VMC向服务器上报指标.
/// </summary>
public class UplinkEntity1012
{
    /// <summary>
    /// 请求体
    /// </summary>
    public static readonly int KEY = 1012;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// Metrics
        /// </summary>
        public IEnumerable<Detail> Metrics { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套详情
        /// </summary>
        [MessagePackObject(true)]
        public class Detail
        {
            /// <summary>
            /// 货柜编号。
            /// </summary>
            public int CounterNo { get; set; }

            /// <summary>
            /// 指标
            /// </summary>
            public int Metric { get; set; }

            /// <summary>
            /// 指标序号
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 指标值
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// 指标状态(0:正常 1:告警 2:故障)
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 描述信息
            /// </summary>
            public string Description { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 响应实体.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}