using System.Collections.Concurrent;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YSCore.Base.DependencyInjection.Dependencies;

namespace YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.Wrapper;
/// <summary>
/// 集群版Grpc
/// </summary>
public class GrpcClusterIotWrapp
{
    private readonly GrpcClientPool _grpcClientPool;

    /// <summary>
    /// GrpcClusterIotWrapp
    /// </summary>
    /// <param name="grpcClientPool"></param>
    public GrpcClusterIotWrapp(GrpcClientPool grpcClientPool)
    {
        _grpcClientPool = grpcClientPool;
    }

    /// <summary>
    /// 获取或创建针对特定设备的命令发送者
    /// </summary>
    /// <param name="mid">设备号</param>
    /// <returns></returns>
    public async Task<ICommandSender> GetOrCreateCommandSenderAsync(string mid)
    {
        return await _grpcClientPool.GetOrCreatePubClientAsync(mid);
    }

    /// <summary>
    /// GetCommandSenderClientsAsync
    /// </summary>
    /// <returns></returns>
    public async Task<ConcurrentDictionary<string, ICommandSender>> GetCommandSenderClientsAsync()
    {
        return await _grpcClientPool.GetCommandSenderClientsAsync();
    }
}