using Hangfire;
using System.Reflection;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;

namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase
{
    /// <summary>
    /// 这个类会扫描程序集中的所有定时任务类，并根据数据库中的配置来注册和管理任务。
    /// </summary>
    public class RecurringTaskLoader
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly ITaskSchedulingInfoQueries _taskSchedulingInfoQueries;

        /// <summary>
        /// RecurringTaskLoader
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <param name="taskSchedulingInfoQueries"></param>
        public RecurringTaskLoader(IRecurringJobManager recurringJobManager, ITaskSchedulingInfoQueries taskSchedulingInfoQueries)
        {
            _recurringJobManager = recurringJobManager;
            _taskSchedulingInfoQueries = taskSchedulingInfoQueries;
        }
        /// <summary>
        /// 加载并注册所有定时任务
        /// </summary>
        public async Task LoadAndScheduleTasks()
        {
            // 获取程序集下的所有定时任务类
            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRecurringTask).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            //获取数据库任务调度信息列表
            var dbTaskInfo = await _taskSchedulingInfoQueries.GetTaskSchedulingInfoListAsync();
            foreach (var taskType in taskTypes)
            {
                // 从数据库获取该任务的 Cron 表达式
                var taskInfo = dbTaskInfo.DtoList.FirstOrDefault(w => w.Name == taskType.Name);
                if (taskInfo != null)
                {
                    // 注册任务
                    var taskInstance = (IRecurringTask)Activator.CreateInstance(taskType);
                    _recurringJobManager.AddOrUpdate(
                        taskType.FullName,
                        () => taskInstance.Execute(),
                        taskInfo.CronExpression);
                }
            }
        }
    }
}
