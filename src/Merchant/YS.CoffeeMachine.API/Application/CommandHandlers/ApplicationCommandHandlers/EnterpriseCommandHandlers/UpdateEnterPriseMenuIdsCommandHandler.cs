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

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 更改企业拥有的菜单Ids
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEnterPriseMenuIdsCommandHandler(CoffeeMachineDbContext context, IConfiguration configuration) : ICommandHandler<UpdateEnterPriseMenuIdsCommand, bool>
    {
        /// <summary>
        /// 更改企业拥有的菜单Ids
        /// </summary>
        public async Task<bool> Handle(UpdateEnterPriseMenuIdsCommand request, CancellationToken cancellationToken)
        {
            var info = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            // 获取所有下级企业
            var children = await GetAllSubNodesUsingSqlAsync(info.Id);
            info.UpdateMenuIds(request.ids, request.halfMenuIds, request.SysMenuType);
            if (children != null && children.Count > 0)
            {

                var childIds = children.Where(w => w.Id != request.id).Select(s => s.Id).ToList();
                var newChildren = await context.EnterpriseInfo.Where(w => childIds.Contains(w.Id)).ToListAsync();

                foreach (var item in newChildren)
                {
                    var curMenuIds = request.ids.Intersect((item.MenuIds ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt64(s)).ToList()).ToList();
                    var halfMenuIds = new List<long>();
                    if (request.halfMenuIds != null)
                    {
                        foreach (var halfMenu in request.halfMenuIds)
                        {
                            if (curMenuIds.Contains(halfMenu))
                            {
                                halfMenuIds.Add(halfMenu);
                            }
                        }
                    }
                    if (request.childHalfMenuIds != null && request.childHalfMenuIds.Count() > 0)
                        halfMenuIds.AddRange(request.childHalfMenuIds);

                    if (halfMenuIds.Any())
                        halfMenuIds = halfMenuIds.Distinct().ToList();

                    item.UpdateMenuIds(curMenuIds, halfMenuIds, request.SysMenuType);
                }
            }
            return true;
        }

        /// <summary>
        /// 根据企业Id获取所有下级企业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EnterpriseInfo>> GetAllSubNodesUsingSqlAsync(long id)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            await using var connection = new SqlConnection(connectionString);

            // 确保连接打开
            await connection.OpenAsync();

            //var result = await context.EnterpriseInfo
            //    .FromSqlInterpolated($@"
            //                        WITH RecursiveCTE AS (
            //                            SELECT *
            //                            FROM EnterpriseInfo
            //                            WHERE TransId = {id}
            //                            UNION ALL
            //                            SELECT ei.*
            //                            FROM EnterpriseInfo ei
            //                            INNER JOIN RecursiveCTE rcte ON ei.Pid = rcte.TransId
            //                        )
            //                        SELECT *
            //                        FROM RecursiveCTE;
            //")
            //    .ToListAsync();

            // 定义查询，使用递归 CTE
            const string sqlQuery = @"
                                     WITH RecursiveCTE AS (
                                         SELECT *
                                        FROM EnterpriseInfo
                                        WHERE Id = @id
                                        UNION ALL
                                        SELECT ei.*
                                        FROM EnterpriseInfo ei
                                        INNER JOIN RecursiveCTE rcte ON ei.Pid = rcte.Id
                                                )
                                                SELECT * 
                                                FROM RecursiveCTE;";
            var result = (await connection.QueryAsync<EnterpriseInfo>(sqlQuery, new { id })).ToList();
            return result;
        }
    }
}