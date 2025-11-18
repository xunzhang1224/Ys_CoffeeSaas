using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Queries.ITaskSchedulingInfoQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.Queries.TaskSchedulingInfoQueries
{
    /// <summary>
    /// 任务调度相关查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class TaskSchedulingInfoQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : ITaskSchedulingInfoQueries
    {
        /// <summary>
        /// 通过Id获取任务调度信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.TaskSchedulingInfo.FirstOrDefaultAsync(w => w.Id == id);
            if (info is null)
                throw new KeyNotFoundException();
            TaskSchedulingInfoDot dto = new ();
            dto = mapper.Map<TaskSchedulingInfoDot>(info);
            return dto;
        }
        /// <summary>
        /// 通过任务名获取任务调度信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TaskSchedulingInfoDot> GetTaskSchedulingInfoAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));
            var info = await context.TaskSchedulingInfo.FirstOrDefaultAsync(w => w.Name == name);
            if (info is null)
                throw new KeyNotFoundException();
            TaskSchedulingInfoDot dto = new ();
            dto = mapper.Map<TaskSchedulingInfoDot>(info);
            return dto;
        }
        /// <summary>
        /// 获取任务调度列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TaskSchedulingInfoListDot> GetTaskSchedulingInfoListAsync()
        {
            var infos = await context.TaskSchedulingInfo.Where(w => w.IsEnabled && !w.IsDelete).ToListAsync();
            TaskSchedulingInfoListDot dto = new (infos);
            return dto;
        }
    }
}
