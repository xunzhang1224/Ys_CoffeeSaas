using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;

namespace YS.CoffeeMachine.Infrastructure.PlatformRepositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 企业类型
    /// </summary>
    /// <param name="context"></param>
    public class PEnterpriseTypesRepository(CoffeeMachinePlatformDbContext context) : YsRepositoryBase<EnterpriseTypes, long, CoffeeMachinePlatformDbContext>(context), IPEnterpriseTypesRepository
    {
    }
}
