namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase
{
    /// <summary>
    /// 任务调度基类
    /// </summary>
    public interface IRecurringTask
    {
        /// <summary>
        /// 所有定时任务都作为一个单独的类，需要继承此类并实现该方法
        /// </summary>
        void Execute();
    }
}
