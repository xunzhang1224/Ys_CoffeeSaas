using Microsoft.EntityFrameworkCore;
using Polly;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 删除企业信息
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteEnterpriseInfoCommandHandler(IEnterpriseInfoRepository repository, CoffeeMachineDbContext _context) : ICommandHandler<DeleteEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 删除企业信息
        /// </summary>
        public async Task<bool> Handle(DeleteEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);

            var allNodes = await _context.EnterpriseInfo.IgnoreQueryFilters()
            .AsNoTracking().Where(w => !w.IsDelete)
            .ToListAsync();

            var allIds = GetAllChildrenIds(allNodes, request.id);
            allIds.Add(request.id);

            var deviceCount = await _context.DeviceInfo.AsQueryable().IgnoreQueryFilters().Where(w => allIds.Contains(w.EnterpriseinfoId) && !w.IsDelete).CountAsync();
            if (deviceCount > 0)
            {
                throw ExceptionHelper.AppFriendly("当前企业树下，存在未解绑设备" + deviceCount + "台");
            }

            //var res = await repository.FakeDeleteByIdAsync(request.id);

            // 软删除当前企业树
            await _context.EnterpriseInfo.IgnoreQueryFilters().Where(w => allIds.Contains(w.Id)).ExecuteUpdateAsync(u => u.SetProperty(s => s.IsDelete, s => true));

            // 删除当前企业树下的所有用户
            await _context.ApplicationUser.IgnoreQueryFilters().Where(w => allIds.Contains(w.EnterpriseId)).ExecuteUpdateAsync(u => u.SetProperty(s => s.IsDelete, s => true));

            //var allUser = await context.ApplicationUser.Where(w => w.EnterpriseId == request.id).ToListAsync();
            //if (allUser != null)
            //{
            //    allUser.ForEach(x => x.IsDelete = true);
            //    await context.SaveChangesAsync();
            //}

            return true;
        }

        /// <summary>
        /// 根据指定id获取 所有子节点
        /// </summary>
        /// <param name="allNodes"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<long> GetAllChildrenIds(List<EnterpriseInfo> allNodes, long parentId)
        {
            var result = new List<long>();
            void Recurse(long id)
            {
                var children = allNodes.Where(x => x.Pid == id).ToList();
                foreach (var child in children)
                {
                    result.Add(child.Id);
                    Recurse(child.Id); // 递归继续找子节点
                }
            }

            Recurse(parentId);
            return result;
        }

    }
}
