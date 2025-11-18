using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos;

namespace YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries
{
    /// <summary>
    /// 任务调度查询
    /// </summary>
    public interface ITaskSchedulingInfoQueries
    {
        /// <summary>
        /// 通过Id获取任务调度信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoAsync(long id);
        /// <summary>
        /// 通过任务名获取任务调度信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoAsync(string name);
        /// <summary>
        /// 获取任务调度列表
        /// </summary>
        /// <returns></returns>
        Task<TaskSchedulingInfoListDot> GetTaskSchedulingInfoListAsync();
    }
}
