namespace YS.CoffeeMachine.Domain.AggregatesModel
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 任务调度信息
    /// </summary>
    public class TaskSchedulingInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; private set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 任务调度信息
        /// </summary>
        protected TaskSchedulingInfo() { }

        /// <summary>
        /// 添加任务调度信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cron"></param>
        /// <param name="isEnabled"></param>
        public TaskSchedulingInfo(string name, string description, string cron, bool isEnabled = false)
        {
            Name = name;
            Description = description;
            CronExpression = cron;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cron"></param>
        public void Update(string name, string description, string cron, bool isEnabled)
        {
            Name = name;
            Description = description;
            CronExpression = cron;
            IsEnabled = isEnabled;
        }
    }
}
