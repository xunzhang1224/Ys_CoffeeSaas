using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Infrastructure.Repositories.DevicesRepositories
{
    /// <summary>
    /// 设备分组
    /// </summary>
    /// <param name="context"></param>
    public class GroupsRepository(CoffeeMachineDbContext context) : YsRepositoryBase<Groups, long, CoffeeMachineDbContext>(context), IGroupsRepository
    {
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Groups> GetByIdAsync(long id)
        {
            return await context.Groups.Include(i => i.Users).ThenInclude(i => i.ApplicationUser).Include(i => i.Devices).ThenInclude(i => i.DeviceInfo).FirstAsync(w => w.Id == id);
        }
    }
}
