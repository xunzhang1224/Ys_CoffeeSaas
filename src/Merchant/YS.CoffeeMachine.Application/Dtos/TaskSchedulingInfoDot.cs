using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;

namespace YS.CoffeeMachine.Application.Dtos
{
    /// <summary>
    /// 任务调度信息
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
        /// 构造函数
        /// </summary>

        public TaskSchedulingInfoDot() { }

        /// <summary>
        /// 构造函数
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
    /// 任务调度信息列表
    /// </summary>
    public class TaskSchedulingInfoListDot
    {
        /// <summary>
        /// 任务调度信息列表
        /// </summary>
        public List<TaskSchedulingInfoDot> DtoList { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskSchedulingInfoListDot()
        {
            DtoList = new List<TaskSchedulingInfoDot>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
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
