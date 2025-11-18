using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.BasicQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.API.Queries.FileResourceQueries
{
    /// <summary>
    /// 文件中心
    /// </summary>
    /// <param name="_context"></param>
    public class FileCenterQuerie(CoffeeMachinePlatformDbContext _context) : IFileCenterQuerie
    {
        /// <summary>
        /// 查询文件中心列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<FileCenter>> GetFileCenter(FileCenterInput input)
        {
            return await _context.FileCenter.AsQueryable()
                .WhereIf(!string.IsNullOrWhiteSpace(input?.FileName), x => x.FileName.Contains(input.FileName))
                .WhereIf(input?.DocmentType != null, x => x.FileCenterType == input.DocmentType)
                .WhereIf(input?.FileState != null, x => x.FileState == input.FileState)
                .WhereIf(input?.SysMenuType != null, x => x.SysMenuType == input.SysMenuType)
                .WhereIf(!string.IsNullOrWhiteSpace(input?.Url), x => x.Url.Contains(input.Url))
                .WhereIf(input?.Times != null && input.Times.Count == 2, x => x.CreateTime > input.Times[0] && x.CreateTime < input.Times[1])
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);
        }
    }
}
