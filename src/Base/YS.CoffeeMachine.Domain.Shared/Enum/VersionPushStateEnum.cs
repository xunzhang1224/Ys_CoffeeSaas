using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 版本推送状态
    /// </summary>
    public enum VersionPushStateEnum
    {
        /// <summary>
        /// 已推送
        /// </summary>
        [Description("已推送")]
        Pushed,

        /// <summary>
        /// 推送成功
        /// </summary>
        [Description("推送成功")]
        PushSuccess,

        /// <summary>
        /// 推送失败
        /// </summary>
        [Description("推送失败")]
        PushFail,
    }
}
