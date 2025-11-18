using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;

namespace YS.CoffeeMachine.Application.Queries.FileResourceQueries
{
    /// <summary>
    /// 文件中心
    /// </summary>
    public interface IFileCenterQuerie
    {
        /// <summary>
        /// 文件中心
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FileCenter>> GetFileCenter(FileCenterInput input);
    }
}
