using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformQueries.IFileResourceQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.FileResourceQueries
{
    /// <summary>
    /// 文件管理查询
    /// </summary>
    public class PFileManageQueries(CoffeeMachinePlatformDbContext _context) : IPFileManageQueries
    {
        /// <summary>
        /// 获取文件资源列表(平台)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<P_FileManage>> GetPFileResourceList()
        {
            return await _context.P_FileManage.AsQueryable().ToListAsync();
        }

        /// <summary>
        /// 获取文件资源列表(商户)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<FileManage>> GetFileResourceList()
        {
            return await _context.FileManage.AsQueryable().ToListAsync();
            throw new NotImplementedException();
        }
    }
}
