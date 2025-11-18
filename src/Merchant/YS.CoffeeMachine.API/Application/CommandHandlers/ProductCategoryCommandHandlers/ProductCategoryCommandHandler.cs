using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.ProductCategoryCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ProductCategoryCommandHandlers
{
    /// <summary>
    /// 创建商品分类
    /// </summary>
    public class CreateProductCategoryCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<CreateProductCategoryCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var info = new ProductCategory(request.parentId, request.name, request.imageUrl, request.iconUrl, request.productCategoryType, request.isEnabled, request.sort, request.description);

            if (request.parentId != null)
            {
                var parent = await context.ProductCategory.FindAsync(request.parentId);
                if (parent == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0020)]);

                // 跟父级分类类型保持一致
                info.SetCategoryType(parent.ProductCategoryType);

                info.SetParentId(request.parentId, parent.Level + 1);
            }

            await context.ProductCategory.AddAsync(info);

            return true;
        }
    }

    /// <summary>
    /// 更改商品分类父级
    /// </summary>
    /// <param name="context"></param>
    public class ChangeParentIdCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ChangeParentIdCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>uu
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ChangeParentIdCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ProductCategory.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.parentId != null)
            {
                var parent = await context.ProductCategory.FirstOrDefaultAsync(w => w.Id == request.parentId);
                if (parent == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0020)]);

                // 跟父级分类类型保持一致
                info.SetCategoryType(parent.ProductCategoryType);

                info.SetParentId(request.parentId, parent.Level + 1);
            }
            else
            {
                info.SetParentId(null, 1);
            }
            return true;
        }
    }

    /// <summary>
    /// 编辑商品分类
    /// </summary>
    /// <param name="context"></param>
    public class UpdateProductCategoryCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateProductCategoryCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ProductCategory.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.imageUrl, request.iconUrl, request.productCategoryType, request.isEnabled, request.sort, request.description);
            return true;
        }
    }

    /// <summary>
    /// 变更商品分类启用禁用状态
    /// </summary>
    /// <param name="context"></param>
    public class ChangeProductCategoryStatusCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<ChangeProductCategoryStatusCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ChangeProductCategoryStatusCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ProductCategory.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.SetIsEnabled(request.isEnabled);
            return true;
        }
    }

    /// <summary>
    /// 删除商品分类
    /// </summary>
    /// <param name="context"></param>
    public class DeleteProductCategoryCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeleteProductCategoryCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ProductCategory.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var hasChild = await context.ProductCategory.AnyAsync(w => w.ParentId == request.id);
            if (hasChild)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0021)]);

            // 商品分类删除后，饮品的分类设置为空
            var Beverages = await context.BeverageInfo.Where(w => w.CategoryIds.Contains(request.id)).ToListAsync();
            if (Beverages != null && Beverages.Any())
            {
                Beverages.ForEach(e =>
                    e.DelCategoryId(request.id)
                );
                context.BeverageInfo.UpdateRange(Beverages);
            }
            context.ProductCategory.Remove(info);
            return true;
        }
    }
}