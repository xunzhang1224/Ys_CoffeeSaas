using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.Basics
{
    /// <summary>
    /// 文件存储仓库
    /// </summary>
    public interface IPFileCenterRepository : IYsRepository<FileCenter>
    {
    }
}
