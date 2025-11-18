namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.BackgroundJobs
{
    /// <summary>
    /// testTask2
    /// </summary>
    public class testTask2 : IRecurringTask
    {
        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now.ToString("G")},测试定时任务执行22222222222222");
            Console.WriteLine("-------------------------------------------------------------");
        }
    }
}
