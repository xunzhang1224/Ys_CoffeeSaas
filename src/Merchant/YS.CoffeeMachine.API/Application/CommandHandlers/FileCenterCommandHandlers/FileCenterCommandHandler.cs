using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.BasicCommands.DocmentCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.FileCenterCommandHandlers
{
    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="_db"></param>
    public class CreateFileCenterCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<CreateFileCenterCommand>
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        public async Task Handle(CreateFileCenterCommand request, CancellationToken cancellationToken)
        {
            var file = new FileCenter(request.fileName,YitIdHelper.NextId().ToString(), request.fileCenterType, request.url, FileStateEnum.Success, request.tenantId, SysMenuTypeEnum.Merchant, request.faildMessage);
            await _db.AddAsync(file);
        }
    }

    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="_db"></param>
    public class CreateFileNewCenterCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<CreateFileNewCenterCommand>
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        public async Task Handle(CreateFileNewCenterCommand request, CancellationToken cancellationToken)
        {
            var file = new FileCenter(request.fileName, request.code, request.fileCenterType, request.tenantId, SysMenuTypeEnum.Merchant);
            await _db.AddAsync(file);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    public class DeleteFileCenterCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<DeleteFileCenterCommand, bool>
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(DeleteFileCenterCommand request, CancellationToken cancellationToken)
        {
            var file = await _db.FileCenter.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == request.id);
            if (file != null)
            {
                file.IsDelete = true;
                _db.FileCenter.Update(file);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="_db"></param>
        public class UpdateFileCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateFileCommand>
        {
            /// <summary>
            /// a
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public async Task Handle(UpdateFileCommand request, CancellationToken cancellationToken)
            {
                var file = await _db.FileCenter.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Code == request.code);
                if (file != null)
                {
                    file.Update(request.state, request.faildMessage, request.url);
                    _db.FileCenter.Update(file);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
