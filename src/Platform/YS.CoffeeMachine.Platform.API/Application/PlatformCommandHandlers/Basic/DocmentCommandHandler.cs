using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.Basic
{
    ///// <summary>
    ///// 添加文件
    ///// </summary>
    ///// <param name="_repository"></param>
    //public class CreateFileCenterCommandHandler(CoffeeMachinePlatformDbContext _db) : ICommandHandler<CreateFileCenterCommand>
    //{
    //    /// <summary>
    //    /// 添加文件
    //    /// </summary>
    //    public async Task Handle(CreateFileCenterCommand request, CancellationToken cancellationToken)
    //    {
    //        var file = new FileCenter(request.fileName, request.fileCenterType, request.url, FileStateEnum.Success, request.tenantId, SysMenuTypeEnum.Platform, request.faildMessage);
    //        await _db.AddAsync(file);
    //    }
    //}

    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="_db"></param>
    public class CreateFileNewCenterCommandHandler(CoffeeMachinePlatformDbContext _db) : ICommandHandler<CreateFileNewCenterCommand>
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        public async Task Handle(CreateFileNewCenterCommand request, CancellationToken cancellationToken)
        {
            var file = new FileCenter(request.fileName, request.code, request.fileCenterType, request.tenantId, SysMenuTypeEnum.Platform);
            await _db.AddAsync(file);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    public class DeleteFileCenterCommandHandler(CoffeeMachinePlatformDbContext _db) : ICommandHandler<DeleteFileCenterCommand, bool>
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
            var file = await _db.FileCenter.FirstOrDefaultAsync(x => x.Id == request.id);
            if (file != null)
            {
                file.IsDelete = true;
                _db.FileCenter.Update(file);
            }
            return true;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateFileCommandHandler(CoffeeMachinePlatformDbContext _db) : ICommandHandler<UpdateFileCommand>
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
            var file = await _db.FileCenter.FirstOrDefaultAsync(x => x.Code == request.code);
            if (file != null)
            {
                file.Update(request.state, request.faildMessage, request.url);
                _db.FileCenter.Update(file);
            }
        }
    }
}
