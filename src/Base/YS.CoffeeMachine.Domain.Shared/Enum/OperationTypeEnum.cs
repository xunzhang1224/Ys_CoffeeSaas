using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperationTypeEnum
    {
        /// <summary>
        /// 设置下发
        /// </summary>
        [Description("设置下发")]
        AttributeSend = 3001,

        #region 6216下的能力

        /// <summary>
        /// 重启
        /// </summary>
        [Description("重启")]
        MachineRestart = 11,

        /// <summary>
        /// 饮品制作
        /// </summary>
        [Description("饮品制作")]
        BeverageProduction = 33,

        /// <summary>
        /// 饮品制作
        /// </summary>
        [Description("整机清洗")]
        machineCleaning = 34,

        /// <summary>
        /// 饮品制作
        /// </summary>
        [Description("搅拌器冲洗")]
        BlenderFlushing = 35,

        /// <summary>
        /// 饮品制作
        /// </summary>
        [Description("关机")]
        MachineShutdown = 36,

        /// <summary>
        /// 饮品制作
        /// </summary>
        [Description("重置饮品配方")]
        ResetBeverageRecipe = 37,

        /// <summary>
        /// 恢复出厂设置
        /// </summary>
        [Description("恢复出厂设置")]
        RestoreFactorySettings = 38,

        /// <summary>
        /// 锁机
        /// </summary>
        [Description("锁机")]
        LockMachine = 39,

        /// <summary>
        /// 解锁
        /// </summary>
        [Description("解锁")]
        UnlockMachine = 40,

        /// <summary>
        /// 饮品应用
        /// </summary>
        [Description("饮品应用")]
        BeverageApplication = 41,
        #endregion
    }
}
