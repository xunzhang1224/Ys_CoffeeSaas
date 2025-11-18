using MediatR;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.GoodsQueries;
using YS.CoffeeMachine.Application.Commands.FileResourceCommands;
using YS.CoffeeMachine.Application.Commands.Goods;
using YS.CoffeeMachine.Application.Queries.IGoods;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.GoodsCommandHandlers
{
    /// <summary>
    /// 新增
    /// </summary>
    public class AddCommandCommandHandler(IGoodsQueries _goodsQueries,CoffeeMachineDbContext _db) : ICommandHandler<AddPrivateGoodsCommand, bool>
    {
        /// <summary>
        /// q
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(AddPrivateGoodsCommand request, CancellationToken cancellationToken)
        {
            var skus = request.Goods.Select(x => x.Sku).ToList();
            await _goodsQueries.CheckSku(skus);
            foreach (var item in request.Goods)
            {
                var model = new PrivateGoodsRepository(item.Name, item.Sku, item.SuggestedPrice, item.IsEnable);
                await _db.AddAsync(model);
            }
            return true;
        }
    }

    /// <summary>
    /// 禁用/启用
    /// </summary>
    public class SetStatusCommandHandler(IGoodsQueries _goodsQueries, CoffeeMachineDbContext _db) : ICommandHandler<SetPrivateGoodsStatusCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(SetPrivateGoodsStatusCommand request, CancellationToken cancellationToken)
        {
            var infos = await _db.PrivateGoodsRepository.Where(x => request.ids.Contains(x.Id)).ToListAsync();
            if (infos != null && infos.Any())
            {
                infos.ForEach(x => x.SetStatus(request.isEnable));
            }
            return true;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    public class UpdateCommandHandler(IGoodsQueries _goodsQueries, CoffeeMachineDbContext _db) : ICommandHandler<UpdatePrivateGoodsCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(UpdatePrivateGoodsCommand request, CancellationToken cancellationToken)
        {
            //var skus = new List<string>() { request.sku };
            //await _goodsQueries.CheckSku(skus);
            var model = await _db.PrivateGoodsRepository.FirstOrDefaultAsync(x => x.Id == request.id);
            model.Update(request.name, request.suggestedPrice, request.isEnable);
            _db.Update(model);
            return true;
        }
    }
}
