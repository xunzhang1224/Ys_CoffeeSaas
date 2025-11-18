using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseTypesCommandHandlers
{
    /// <summary>
    /// 创建企业类型命令处理程序
    /// </summary>
    /// <param name="repository"></param>
    public class CreateEnterpriseTypesCommandHandler(IPEnterpriseTypesRepository repository) : ICommandHandler<CreateEnterpriseTypesCommand, bool>
    {
        /// <summary>
        /// 创建企业类型命令处理程序
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseTypesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.name))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);
            // 验证企业类型名称是否存在 && x.Astrict == request.astrict
            var isExist = await repository.AnyAsync(x => x.Name == request.name);
            if (isExist)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0080)]);
            var res = await repository.AddAsync(new EnterpriseTypes(request.name, request.astrict));
            return res != null;
        }
    }
}