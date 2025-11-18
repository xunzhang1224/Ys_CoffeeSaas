using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.IPlatformRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories
{
    /// <summary>
    /// 任务调度信息
    /// </summary>
    /// <param name="context"></param>
    public class PTaskSchedulingInfoRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<TaskSchedulingInfo, long, CoffeeMachinePlatformDbContext>(context), IPTaskSchedulingInfoRepository
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
