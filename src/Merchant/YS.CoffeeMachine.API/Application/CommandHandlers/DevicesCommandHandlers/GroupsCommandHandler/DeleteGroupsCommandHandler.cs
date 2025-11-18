using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Properties;
using Polly;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 删除分组
    /// </summary>
    /// <param name="context"></param>
    public class DeleteGroupsCommandHandler(CoffeeMachineDbContext context, IConfiguration configuration) : ICommandHandler<DeleteGroupsCommand, bool>
    {
        /// <summary>
        /// 删除分组
        /// </summary>
        public async Task<bool> Handle(DeleteGroupsCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);

            var info = context.Groups.Include(i => i.Devices).Include(i => i.Users).FirstOrDefault(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            //清除分组用户绑定
            info.ClearUsers();
            //清除分组设备
            info.ClearDevices();

            //获取所有下级分组
            var allSubGroups = await GetAllSubNodesUsingSqlAsync(info.Id);
            foreach (var item in allSubGroups)
            {
                //清除分组用户绑定
                item.ClearUsers();
                //清除分组设备
                item.ClearDevices();
                //物理删除
                //context.Groups.Remove(item);
            }

            //物理删除分组
            var groupIds = allSubGroups.Where(w => w.Id != info.Id).Select(i => i.Id).ToList();
            //context.Groups.FromSqlInterpolated($"delete [Groups] where id in ({string.Join(",", groupIds)})");
            context.Groups.RemoveRange(context.Groups.Where(w => groupIds.Contains(w.Id)));

            //物理删除
            context.Groups.Remove(info);

            return true;
        }

        /// <summary>
        /// 获取指定分组所有字节的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Groups>> GetAllSubNodesUsingSqlAsync(long id)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            await using var connection = new SqlConnection(connectionString);

            // 确保连接打开
            await connection.OpenAsync();

            // 定义查询，使用递归 CTE
            const string sqlQuery = @"
                                    WITH RecursiveCTE AS (
                                                    SELECT * 
                                                    FROM [Groups] 
                                                    WHERE Id = @id
                                                    UNION ALL
                                                    SELECT ei.* 
                                                    FROM [Groups] ei
                                                    INNER JOIN RecursiveCTE rcte ON ei.PId = rcte.Id
                                                )
                                                SELECT * 
                                                FROM RecursiveCTE";
            var result = (await connection.QueryAsync<Groups>(sqlQuery, new { id })).ToList();
            return result;
        }
    }
}
