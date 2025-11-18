using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.SettingsCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="context"></param>
    public class SetNoticeCfgCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<SetNoticeCfgCommand, bool>
    {
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(SetNoticeCfgCommand request, CancellationToken cancellationToken)
        {
            var olds = await context.NoticeCfg.Where(x => x.Type == request.NoticeCfgs.Type).ToListAsync();
            context.NoticeCfg.RemoveRange(olds);
            foreach (var item in request.NoticeCfgs.NoticeCfgs)
            {
                var cfg = new NoticeCfg(request.NoticeCfgs.Type, item.UserId, item.UserName, item.Status, item.Method);
                await context.AddAsync(cfg);
            }
            return true;
        }
    }
}
