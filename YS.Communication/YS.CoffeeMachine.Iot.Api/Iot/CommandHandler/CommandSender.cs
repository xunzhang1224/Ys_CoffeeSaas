using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Api.Services;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO.Base;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;
using YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.Wrapper;
using YSCore.Base.App;

namespace YS.CoffeeMachine.Iot.Api.Iot.CommandHandler;
/// <summary>
///  提供下发指令的服务类。ServiceBase<ICommandSenderService>, ICommandSenderService,
/// </summary>
public class CommandSender(GrpcClusterIotWrapp _grpcClusterIot,DeviceService _deviceService) : ICommandSenderService
{
    /// <summary>
    /// 查询机器是否在线
    /// </summary>
    /// <param name="vendNo">机器编号</param>
    /// <returns>返回是否在线</returns>
    public async Task<bool> IsOnlineAsync(DownSeedRequestBase<string> request)
    {
        if (request.Server == null) return false;
        return await request.Server.IsOnline(request.Mid);
    }

    /// <summary>
    /// 下发三码信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns>返回是否在线</returns>
    public async Task<bool> Downlink1100SendAsync(DownSeedRequestBase<DownlinkEntity1100> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink1100SendAsync(request.Data);
    }

    /// <summary>
    /// 1011服务器向VMC同步能力配置指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    public async Task<bool> Downlink1011SendAsync(DownSeedRequestBase<DownlinkEntity1011> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink1011SendAsync(request.Data);
    }

    /// <summary>
    /// 6216通用控制指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    public async Task<bool> Downlink6216SendAsync(DownSeedRequestBase<DownlinkEntity6216> request)
    {
        if (request.Server == null) return false;
        if (request.Data.CapabilityId == (int)CapabilityIdEnum.LogUpload)
        {
            await _deviceService.LogUploadFileCenterAsync(request.Data);
        }
        return await request.Server.Downlink6216SendAsync(request.Data);
    }

    /// <summary>
    /// 下发物料。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    public async Task<bool> Downlink9021SendAsync(DownSeedRequestBase<DownlinkEntity9021> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink9021SendAsync(request.Data);
    }

    ///// <summary>
    ///// 查询机器是否在线
    ///// </summary>
    ///// <param name="vendNos">机器编号</param>
    ///// <returns>返回是否在线</returns>
    //public async Task<List<KeyValuePair<string, bool>>> IsOnlinesAsync(string[] vendNos)
    //{
    //    List<KeyValuePair<string, bool>> t = new List<KeyValuePair<string, bool>>();
    //    foreach (var item in vendNos)
    //    {
    //        var commandSender = await _grpcClusterIot.GetOrCreateCommandSenderAsync(item);
    //        t.AddRange(await commandSender.IsOnlines(new List<string> { item }.ToArray()));
    //    }
    //    return t;
    //}

    /// <summary>
    /// 查询在线机器集
    /// </summary>
    /// <returns></returns>
    public async Task<List<KeyValuePair<string, string>>> OnlineVends()
    {
        throw new Exception();
    }

    /// <summary>
    /// 1009.服务器向VMC下发能力
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    public async Task<bool> Downlink1009SendAsync(DownSeedRequestBase<DownlinkEntity1009> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink1009SendAsync(request.Data);
    }

    /// <summary>
    ///  9026
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Downlink9026SendAsync(DownSeedRequestBase<DownlinkEntity9026> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink9026SendAsync(request.Data);
    }

    /// <summary>
    ///  5212
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Downlink5212SendAsync(DownSeedRequestBase<DownlinkEntity5212> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink5212SendAsync(request.Data);
    }

    /// <summary>
    ///  9028
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Downlink9028SendAsync(DownSeedRequestBase<DownlinkEntity9028> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink9028SendAsync(request.Data);
    }

    /// <summary>
    ///  9029
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> Downlink9029SendAsync(DownSeedRequestBase<DownlinkEntity9029> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink9029SendAsync(request.Data);
    }

    /// <summary>
    /// Downlink1203SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Downlink1203SendAsync(DownSeedRequestBase<DownlinkEntity1203> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink1203SendAsync(request.Data);
    }

    /// <summary>
    /// 3201发送远程出货指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    public async Task<bool> Downlink3201SendAsync(DownSeedRequestBase<DownlinkEntity3201> request)
    {
        if (request.Server == null) return false;
        return await request.Server.Downlink3201SendAsync(request.Data);
    }

    ///// <summary>
    ///// 6002.远程清除货道故障并测试货道
    ///// </summary>
    ///// <param name="request">入参。</param>
    ///// <returns>返回是否发送成功。</returns>
    //public async Task<bool> Command6002SendAsync(DownlinkSendDto<DownlinkEntity6002> request) => await App.GetRequiredService<Downlink6002Sender>().SendAsync(request);
    ///// <summary>
    ///// 6200下发同步货道指令。
    ///// </summary>
    ///// <param name="request">入参。</param>
    ///// <returns>返回是否发送成功。</returns>
    //public async Task<bool> Command6200SendAsync(DownlinkSendDto<Downlink6200> request) => throw new Exception();

    ///// <summary>
    ///// 6200下发同步货道指令V2。
    ///// </summary>
    ///// <param name="request">入参。</param>
    ///// <returns>返回是否发送成功。</returns>
    //public async Task<bool> Command6200SendV2Async(DownlinkSendDto<DownlinkEntity6200> request) => await App.GetRequiredService<Downlink6200Sender>().SendAsync(request);

    ///// <summary>
    ///// 1203发送远程升级命令。
    ///// </summary>
    ///// <param name="request">入参。</param>
    ///// <returns>返回是否发送成功。</returns>
    //public async Task<bool> Downlink1203SendAsync(DownlinkSendDto<Downlink1203.Request> request) => await App.GetRequiredService<Downlink1203Sender>().SendAsync(request);

    /// <summary>
    /// 10025发送机器质检命令
    /// </summary>
    /// <param name="request">参数</param>
    /// <returns></returns>
    public async Task<bool> Downlink10025SendAsync(DownlinkSendDto<Downlink10025> request) => throw new Exception();

    /// <summary>
    /// 1012 上报设备指标命令
    /// </summary>
    /// <param name="request">参数</param>
    /// <returns></returns>
    public async Task<bool> Command1012SendAsync(UplinkEntity1012.Request request) => throw new Exception();
    /// <summary>
    /// 5204 上报故障命令
    /// </summary>
    /// <param name="request">参数</param>
    /// <returns></returns>
    public async Task<bool> Command5204SendAsync(UplinkEntity5204.Request request) => throw new Exception();

    /// <summary>
    /// ServiceHealthCheck
    /// </summary>
    /// <returns></returns>
    public async Task<List<CommandSendResult>> ServiceHealthCheck()
    {
        List<CommandSendResult> lt = new List<CommandSendResult>();
        var commandSenders = await _grpcClusterIot.GetCommandSenderClientsAsync();
        foreach (var item in commandSenders)
        {
            var res = await item.Value.DownlinkSendAsync("ServiceHealthCheck");
            res.MessageId = res.MessageId + item.Key.ToString();
            lt.Add(res);
        }
        return lt;
    }

    /// <summary>
    /// Downlink1009SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Downlink1009SendAsync(DownlinkSendDto<Downlink1009> request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Command6002SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Command6002SendAsync(DownlinkSendDto<DownlinkEntity6002> request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Command6200SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Command6200SendAsync(DownlinkSendDto<Downlink6200> request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Command6200SendV2Async
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Command6200SendV2Async(DownlinkSendDto<DownlinkEntity6200> request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Command3201SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Command3201SendAsync(DownlinkSendDto<Downlink3201.Request> request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Downlink1100SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Downlink1100SendAsync(DownlinkEntity1100 request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// IsOnlinesAsync
    /// </summary>
    /// <param name="vendNos"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<KeyValuePair<string, bool>>> IsOnlinesAsync(string[] vendNos)
    {
        throw new NotImplementedException();
    }
}
