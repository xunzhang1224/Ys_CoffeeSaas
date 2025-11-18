using Microsoft.AspNetCore.SignalR;
using YS.CoffeeMachine.Provider.IServices;
using YSCore.CoffeeMachine.SignalR.Hubs;
using YSCore.CoffeeMachine.SignalR.Services;

/// <summary>
/// SignalRService
/// </summary>
public class SignalRService : ISignalRService
{
    IRedisService _redisService;
    private readonly IHubContext<SignalR_Hub> _hubContext;
    //private readonly IConnectionTracker _connectionTracker;
    private static string GetUserConnectionIdKey(string userId) => $"/SignalR/User/connectionId/{userId}";

    /// <summary>
    /// 构造函数注入 IHubContext
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="redisService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SignalRService(IHubContext<SignalR_Hub> hubContext, IRedisService redisService)
    {
        _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        _hubContext = hubContext;
    }

    /// <summary>
    /// 向所有客户端广播消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public async Task SendMessageToAllAsync(string message, string topic = null)
    {
        await _hubContext.Clients.All.SendAsync(topic == null ? "ReceiveMessage" : topic, message);
    }

    /// <summary>
    /// 向指定组广播消息
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="message"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public async Task SendMessageToGroupAsync(string groupName, string message, string topic = null)
    {
        await _hubContext.Clients.Group(groupName).SendAsync(topic == null ? "ReceiveMessage" : topic, message);
    }

    /// <summary>
    /// 向指定用户发送消息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="message"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public async Task SendMessageToUserAsync(string userId, string message, string topic = null)
    {
        await _hubContext.Clients.User(userId).SendAsync(topic == null ? "ReceiveMessage" : topic, message);
    }

    /// <summary>
    /// 添加用户到组
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public async Task AddToGroupAsync(string userId, string groupName)
    {
        await _hubContext.Groups.AddToGroupAsync(userId, groupName);
    }

    /// <summary>
    /// 从组中移除用户
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public async Task RemoveFromGroupAsync(string userId, string groupName)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(userId, groupName);
    }

    /// <summary>
    /// 断开指定用户连接
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DisconnectUserAsync(string userId)
    {
        var connectionId = await GetConnectionIdByUserId(userId);
        if (connectionId != null)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("Disconnect");
        }
    }

    /// <summary>
    /// 获取指定用户的连接 ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<string> GetConnectionIdByUserId(string userId)
    {
        // 你可以通过连接追踪器从数据库或缓存中获取用户的连接 ID。
        // 这里假设已经有某种方法来管理连接 ID，返回用户的连接 ID。
        // 可以参考 RedisConnectionTracker 或内存追踪等。
        // 示例：
        return await _redisService.GetStringAsync(GetUserConnectionIdKey(userId)) ?? string.Empty;
    }
}