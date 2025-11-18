using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.IRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.Repositories
{
    /// <summary>
    /// 任务调度信息仓储
    /// </summary>
    /// <param name="context"></param>
    public class TaskSchedulingInfoRepository(CoffeeMachineDbContext context) : YsRepositoryBase<TaskSchedulingInfo, long, CoffeeMachineDbContext>(context), ITaskSchedulingInfoRepository
    {
        /// <summary>
        /// 通过名称获取任务调度信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Task<TaskSchedulingInfo> GetTaskSchedulingInfoByName(string Name) => context.TaskSchedulingInfo.FirstOrDefaultAsync(w => !w.IsDelete && w.Name == Name);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public Task<List<TaskSchedulingInfo>> GetAllTasks() => context.TaskSchedulingInfo.Where(w => w.IsEnabled && !w.IsDelete).ToListAsync();
    }
}