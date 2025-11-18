using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 程序类型
    /// </summary>
    public enum ProgramTypeEnum
    {
        /// <summary>
        /// -
        /// </summary>
        [Description("-")]
        Not,

        /// <summary>
        /// 安卓应用程序
        /// </summary>
        [Description("安卓应用程序")]
        AndroidApplications,

        /// <summary>
        /// 单片机程序
        /// </summary>
        [Description("单片机程序")]
        MicrocontrollerProgram,

        /// <summary>
        /// 安卓固件程序
        /// </summary>
        [Description("安卓固件程序")]
        AndroidApplicaAndroidFirmwareProgramtions,

        /// <summary>
        /// UNIX程序
        /// </summary>
        [Description("UNIX程序")]
        UNIX,
    }
}
