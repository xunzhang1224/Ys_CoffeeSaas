using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.FileResource
{
    /// <summary>
    /// 文件管理查询
    /// </summary>
    public interface IFileManageQuerie
    {
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FileManageDto>> GetFileResource(GetFileManageInput input);
    }
}
