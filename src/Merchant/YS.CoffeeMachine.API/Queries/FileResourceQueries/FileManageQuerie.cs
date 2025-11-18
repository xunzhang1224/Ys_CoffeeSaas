using Microsoft.EntityFrameworkCore;
using Polly;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.FileResource;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.FileResourceQueries
{
    /// <summary>
    /// 文件管理查询
    /// </summary>
    public class FileManageQuerie(CoffeeMachineDbContext _context) : IFileManageQuerie
    {
        /// <summary>
        /// 获取文件资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<FileManageDto>> GetFileResource(GetFileManageInput input)
        {
            var infos = await _context.FileManage.AsQueryable()
                .WhereIf(!string.IsNullOrEmpty(input.FileName), w => w.FileName.Contains(input.FileName))
                .WhereIf(!string.IsNullOrEmpty(input.FileType), w => w.FileType == input.FileType)
                .WhereIf(!string.IsNullOrEmpty(input.ResorceUsage), w => w.ResourceUsage == input.ResorceUsage)
                .WhereIf(input.DateTimeRange != null && input.DateTimeRange.Count == 2, w => w.CreateTime >= input.DateTimeRange[0] && w.CreateTime <= input.DateTimeRange[1])
                .OrderByDescending(x => x.CreateTime)
                .Select(s => new FileManageDto()
                {
                    Id = s.Id,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    FileType = s.FileType,
                    FileSize = s.FileSize,
                    ResorceUsage = s.ResourceUsage,
                    CreateTime = s.CreateTime,
                })
                .ToPagedListAsync(input);

            var fileIds = infos.Items.Select(s => s.Id).ToList();

            var result = await _context.FileRelation
                .Where(f => fileIds.Contains(f.FileId))
                .GroupBy(f => f.FileId)
                .Select(g => new
                {
                    FileId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(d => d.FileId, d => d.Count);

            foreach (var info in infos.Items)
            {
                info.QuoteCount = result.ContainsKey(info.Id) ? result[info.Id] : 0;
            }

            return infos;
        }
    }
}
