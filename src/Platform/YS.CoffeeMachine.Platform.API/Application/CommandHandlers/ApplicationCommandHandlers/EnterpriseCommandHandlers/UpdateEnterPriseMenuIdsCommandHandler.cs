using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 更改企业拥有的菜单Ids
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEnterPriseMenuIdsCommandHandler(CoffeeMachinePlatformDbContext context, IConfiguration configuration) : ICommandHandler<UpdateEnterPriseMenuIdsCommand, bool>
    {
        /// <summary>
        /// 更改企业拥有的菜单Ids
        /// </summary>
        public async Task<bool> Handle(UpdateEnterPriseMenuIdsCommand request, CancellationToken cancellationToken)
        {
            //var info = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == request.id);
            //if (info == null)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            //info.UpdateMenuIds(request.ids);
            ////获取所有下级企业
            //var children = await GetAllSubNodesUsingSqlAsync(info.Id);
            //if (children != null && children.Count > 0)
            //{
            //    foreach (var item in children)
            //    {
            //        var curMenuIds = request.ids.Intersect(item.MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList()).ToList();
            //        item.UpdateMenuIds(curMenuIds);
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 根据企业Id获取所有下级企业
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task<List<EnterpriseInfo>> GetAllSubNodesUsingSqlAsync(long parentId)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            await using var connection = new SqlConnection(connectionString);

            // 确保连接打开
            await connection.OpenAsync();

            // 定义查询，使用递归 CTE
            const string sqlQuery = @"
                                    WITH RecursiveCTE AS (
                                        SELECT * 
                                        FROM EnterpriseInfo 
                                        WHERE Id = @parentId
                                        UNION ALL
                                        SELECT ei.* 
                                        FROM EnterpriseInfo ei
                                        INNER JOIN RecursiveCTE rcte ON ei.Pid = rcte.Id
                                    )
                                    SELECT * 
                                    FROM RecursiveCTE;";

            // 执行查询并读取结果
            var result = (await connection.QueryAsync<EnterpriseInfo>(sqlQuery, new { parentId })).ToList();
            return result;
        }
    }
}