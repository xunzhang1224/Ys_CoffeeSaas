using Aop.Api.Domain;
using YS.CoffeeMachine.Application.Commands.CardCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;
using CardInfo = YS.CoffeeMachine.Domain.AggregatesModel.Card.CardInfo;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.CardCommandHandlers
{
    /// <summary>
    /// 创建卡
    /// </summary>
    /// <param name="_db"></param>
    public class CreateCardCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<CreateCardCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var isHave = await _db.CardInfo.AnyAsync(x => x.CardNumber == request.CardNumber);
            if (isHave)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0019)]);
            var info = new CardInfo(request.CardNumber);
            await _db.AddAsync(info);
            return true;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateCommandCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var info = await _db.CardInfo.FirstOrDefaultAsync(x => x.Id == request.CardId);
            if (info != null)
            {
                if (request.IsEnabled != null)
                    info.UpdateIsEnable(request.IsEnabled ?? true);

                if (!string.IsNullOrWhiteSpace(request.CardNumber))
                    info.UpdateCardNumber(request.CardNumber);
            }
            return true;
        }
    }

    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="_db"></param>
    public class CardBindDeviceCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<CardBindDeviceCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(CardBindDeviceCommand request, CancellationToken cancellationToken)
        {
            var info = await _db.CardInfo.Include(x => x.Assignments).FirstOrDefaultAsync(x => x.Id == request.CardId);
            if (info != null)
            {
                info.AssignDevices(request.DeviceIds);
            }
            return true;
        }
    }
}
