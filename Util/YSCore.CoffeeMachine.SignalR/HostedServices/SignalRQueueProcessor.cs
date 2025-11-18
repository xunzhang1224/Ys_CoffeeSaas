using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using YSCore.CoffeeMachine.SignalR.Services;

/// <summary>
/// SignalR 消息队列处理器
/// </summary>
public class SignalRQueueProcessor : IHostedService
{
    private readonly ISignalRService _signalRService;
    private readonly Channel<string> _messageQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// 构造函数注入 ISignalRService
    /// </summary>
    /// <param name="signalRService"></param>
    public SignalRQueueProcessor(ISignalRService signalRService)
    {
        _signalRService = signalRService;
        // 创建消息队列
        _messageQueue = Channel.CreateUnbounded<string>();
        // 创建取消令牌
        _cancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// 启动后台任务，开始处理消息队列
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 在后台线程中运行队列处理任务
        Task.Run(() => ProcessQueueAsync(_cancellationTokenSource.Token), cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 停止后台任务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel(); // 停止队列处理
        return Task.CompletedTask;
    }

    /// <summary>
    /// 异步处理消息队列中的消息
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task ProcessQueueAsync(CancellationToken cancellationToken)
    {
        await foreach (var message in _messageQueue.Reader.ReadAllAsync(cancellationToken))
        {
            // 处理消息并推送到客户端
            await _signalRService.SendMessageToAllAsync(message);
        }
    }

    /// <summary>
    /// 向队列中添加消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task EnqueueMessageAsync(string message)
    {
        await _messageQueue.Writer.WriteAsync(message);
    }
}
