using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 服务授权状态枚举
    /// </summary>
    public enum ServiceAuthorizationStateEnum
    {
        [Description("创建")]
        Create = 0,
        [Description("待答复")]
        Pending = 1,
        [Description("服务中")]
        InService = 2,
        [Description("已完成")]
        Completed = 3,
        [Description("已拒绝")]
        Rejected = 4
    }
}
