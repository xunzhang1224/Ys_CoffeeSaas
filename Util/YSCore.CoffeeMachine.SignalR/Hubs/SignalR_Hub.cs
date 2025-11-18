using Microsoft.AspNetCore.SignalR;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.CoffeeMachine.SignalR.Services;

namespace YSCore.CoffeeMachine.SignalR.Hubs
{
    /// <summary>
    /// SignalR 集成
    /// </summary>
    /// <param name="_redisService"></param>
    /// <param name="_signalRService"></param>
    public class SignalR_Hub(IRedisService _redisService, ISignalRService _signalRService) : Hub
    {
        private static string GetUserConnectionIdKey(string userId) => $"/SignalR/User/connectionId/{userId}";

        /// <summary>
        /// 从 Claims 中获取用户 ID (UId)，如果没有找到则返回 null
        /// </summary>
        /// <returns></returns>
        private string GetUserIdFromClaims() => Context.User?.Claims?.FirstOrDefault(c => c.Type == "UId")?.Value;
        private string GetTenantIdFromClaims() => Context.User?.Claims?.FirstOrDefault(c => c.Type == "EnterpriseId")?.Value;
        /// <summary>
        /// 客户端发送过来的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ReceiveMessage(string message)
        {
            //根据token解析当前用户Id
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return;
            // 打印收到的消息
            Console.WriteLine($"{userId} Received message: {message}");
        }

        /// <summary>
        /// 客户端连接时触发，记录连接信息
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //根据token解析当前用户Id
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return;
            //根据token解析当前租户Id
            var tenantId = GetTenantIdFromClaims();
            if (tenantId == null)
                return;
            Console.WriteLine($"User connected: {Context.ConnectionId}");

            // 将当前连接 ID 保存到 Redis 中
            await _redisService.SetStringAsync(GetUserConnectionIdKey(userId), Context.ConnectionId, TimeSpan.FromDays(7));

            // 添加用户到租户组
            await _signalRService.AddToGroupAsync(Context.ConnectionId, tenantId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端断开连接时触发，删除连接信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //根据token解析当前用户Id
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return;
            //根据token解析当前租户Id
            var tenantId = GetTenantIdFromClaims();
            if (tenantId == null)
                return;

            // 从 Redis 中移除连接 ID
            await _redisService.DelKeyAsync(GetUserConnectionIdKey(userId));

            // 从租户组中移除用户
            await _signalRService.RemoveFromGroupAsync(Context.ConnectionId, tenantId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}