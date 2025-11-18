using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;

namespace YS.CoffeeMachine.Application.PlatformQueries.IFileResourceQueries
{
    /// <summary>
    /// 文件资源查询
    /// </summary>
    public interface IPFileManageQueries
    {
        /// <summary>
        /// 获取文件资源列表(平台)
        /// </summary>
        /// <returns></returns>
        Task<List<P_FileManage>> GetPFileResourceList();

        /// <summary>
        /// 获取文件资源列表(商户)
        /// </summary>
        /// <returns></returns>
        Task<List<FileManage>> GetFileResourceList();
    }
}
