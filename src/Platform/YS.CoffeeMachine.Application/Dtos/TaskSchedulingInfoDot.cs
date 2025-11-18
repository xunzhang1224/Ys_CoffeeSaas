using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;

namespace YS.CoffeeMachine.Application.Dtos
{
    /// <summary>
    /// 任务调度dto
    /// </summary>
    public class TaskSchedulingInfoDot
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
        /// TaskSchedulingInfoDot
        /// </summary>
        public TaskSchedulingInfoDot() { }

        /// <summary>
        /// TaskSchedulingInfoDot
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cronExpression"></param>
        public TaskSchedulingInfoDot(string name, string description, string cronExpression)
        {
            Name = name;
            Description = description;
            CronExpression = cronExpression;
        }
    }

    /// <summary>
    /// TaskSchedulingInfoListDot
    /// </summary>
    public class TaskSchedulingInfoListDot
    {
        /// <summary>
        /// DtoList
        /// </summary>
        public List<TaskSchedulingInfoDot> DtoList { get; private set; }

        /// <summary>
        /// TaskSchedulingInfoListDot
        /// </summary>
        public TaskSchedulingInfoListDot()
        {
            DtoList = new List<TaskSchedulingInfoDot>();
        }

        /// <summary>
        /// TaskSchedulingInfoListDot
        /// </summary>
        /// <param name="dblist"></param>
        public TaskSchedulingInfoListDot(List<TaskSchedulingInfo> dblist)
        {
            DtoList = new ();
            dblist.ForEach(x =>
            {
                DtoList.Add(new TaskSchedulingInfoDot(x.Name, x.Description, x.CronExpression));
            });
        }
    }
}
