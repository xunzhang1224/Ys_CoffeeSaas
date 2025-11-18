using Hangfire;
using System.Reflection;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;

namespace YS.CoffeeMachine.API.Extensions.TaskSchedulingBase
{
    /// <summary>
    /// 这个类会扫描程序集中的所有定时任务类，并根据数据库中的配置来注册和管理任务。
    /// </summary>
    public class RecurringTaskLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly ITaskSchedulingInfoQueries _taskSchedulingInfoQueries;

        /// <summary>
        /// 这个类会扫描程序集中的所有定时任务类
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <param name="taskSchedulingInfoQueries"></param>
        public RecurringTaskLoader(IRecurringJobManager recurringJobManager, ITaskSchedulingInfoQueries taskSchedulingInfoQueries, IServiceProvider serviceProvider)
        {
            _recurringJobManager = recurringJobManager;
            _taskSchedulingInfoQueries = taskSchedulingInfoQueries;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 加载并注册所有定时任务
        /// </summary>
        public async Task LoadAndScheduleTasks()
        {
            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRecurringTask).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            var dbTaskInfo = await _taskSchedulingInfoQueries.GetTaskSchedulingInfoListAsync();

            foreach (var taskType in taskTypes)
            {
                var taskInfo = dbTaskInfo.DtoList.FirstOrDefault(w => w.Name == taskType.Name);
                if (taskInfo != null)
                {
                    // 使用Hangfire的JobActivator来正确处理依赖注入
                    _recurringJobManager.AddOrUpdate(
                        taskType.Name,
                        () => ExecuteTask(taskType.Name),
                        taskInfo.CronExpression);
                }
            }
        }

        /// <summary>
        /// 添加这个方法来处理任务执行
        /// </summary>
        /// <param name="taskTypeName"></param>
        /// <returns></returns>
        public async Task ExecuteTask(string taskTypeName)
        {
            var taskType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.Name == taskTypeName && typeof(IRecurringTask).IsAssignableFrom(t));

            if (taskType != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var taskInstance = (IRecurringTask)scope.ServiceProvider.GetRequiredService(taskType);
                await taskInstance.Execute();
            }
        }
        //public async Task LoadAndScheduleTasks()
        //{
        //    // 获取程序集下的所有定时任务类
        //    var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
        //        .Where(t => typeof(IRecurringTask).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        //    // 获取数据库任务调度信息列表
        //    var dbTaskInfo = await _taskSchedulingInfoQueries.GetTaskSchedulingInfoListAsync();
        //    foreach (var taskType in taskTypes)
        //    {
        //        // 从数据库获取该任务的 Cron 表达式
        //        var taskInfo = dbTaskInfo.DtoList.FirstOrDefault(w => w.Name == taskType.Name);
        //        if (taskInfo != null)
        //        {
        //            using var scope = _serviceProvider.CreateScope();
        //            // 注册任务
        //            var taskInstance = (IRecurringTask)scope.ServiceProvider.GetRequiredService(taskType);// (IRecurringTask)Activator.CreateInstance(taskType);
        //            _recurringJobManager.AddOrUpdate(
        //                taskType.Name,
        //                () => taskInstance.Execute(),
        //                taskInfo.CronExpression);
        //        }
        //    }
        //}
    }
}