using YS.CoffeeMachine.Application.Commands.ApplicationCommands.ServiceAuthorizationRecordCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.ServiceAuthorizationRecordCommandHandlers
{
    /// <summary>
    /// 新增授权
    /// </summary>
    /// <param name="repository"></param>
    public class CreateServiceAuthorizationRecordCommandHandler(IServiceAuthorizationRecordRepository repository) : ICommandHandler<CreateServiceAuthorizationRecordCommand, bool>
    {
        /// <summary>
        /// 新增授权
        /// </summary>
        public async Task<bool> Handle(CreateServiceAuthorizationRecordCommand request, CancellationToken cancellationToken)
        {
            //授权用户
            var authorizationUserInfo = await repository.GetUserByUserId(request.serviceUserId);
            if (authorizationUserInfo == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0020)], request.serviceUserId));
            //服务人员账号
            var serviceUserInfo = await repository.GetUserByUserAccount(request.serviceUserAccount);
            if (serviceUserInfo == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0021)], request.serviceUserAccount));
            //验证授权与服务人员是否在统一企业（服务授权只能授权给不同企业的人员）
            var e_users = await repository.GetEnterpriseByUserIds(new List<long>() { request.serviceUserId, serviceUserInfo.Id });
            if (e_users == null || e_users.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0022)]);
            bool hasDuplicates = e_users.GroupBy(x => x).Any(g => g.Count() > 1);
            if (hasDuplicates)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0023)]);
            //写入数据
            var info = new ServiceAuthorizationRecord(request.name, request.deviceIds, request.serviceUserAccount, serviceUserInfo.Id, request.authorizationEndTime);
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
