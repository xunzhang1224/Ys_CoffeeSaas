namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.BackgroundJobs
{
    /// <summary>
    /// 测试定时任务
    /// </summary>
    public class testTask : IRecurringTask
    {
        /// <summary>
        /// 具体业务执行
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now.ToString("G")},测试定时任务执行111111");
            Console.WriteLine("-------------------------------------------------------------");
        }
    }
}
