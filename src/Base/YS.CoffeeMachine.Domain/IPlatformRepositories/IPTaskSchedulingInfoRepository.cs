using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories
{
    /// <summary>
    /// 定时任务仓库
    /// </summary>
    public interface IPTaskSchedulingInfoRepository : IYsRepository<TaskSchedulingInfo, long>
    {
        /// <summary>
        /// 通过名称获取任务
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        Task<TaskSchedulingInfo> GetTaskSchedulingInfoByName(string Name);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        Task<List<TaskSchedulingInfo>> GetAllTasks();
    }
}
