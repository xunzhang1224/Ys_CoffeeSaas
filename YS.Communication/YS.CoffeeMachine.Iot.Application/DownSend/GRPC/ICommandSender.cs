using MagicOnion;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Application.DownSend.GRPC;

/// <summary>
/// 命令发送服务
/// </summary>
public interface ICommandSender : IService<ICommandSender>
{
    /// <summary>
    /// 判断机器是否在线
    /// </summary>
    /// <param name="mid">机器编号</param>
    /// <returns>是否在线</returns>
    UnaryResult<bool> IsOnline(string mid);

    /// <summary>
    /// 激活下发生产编号
    /// </summary>
    /// <param name="request">入参</param>
    /// <returns></returns>
    UnaryResult<bool> Downlink1100SendAsync(DownlinkEntity1100 request);
    /// <summary>
    /// 查询指定机器集是否在线
    /// </summary>
    /// <param name="mids">机器编号集</param>
    /// <returns>返回是否在线</returns>
    UnaryResult<List<KeyValuePair<string, bool>>> IsOnlines(string[] mids);

    /// <summary>
    /// 查询在线机器集
    /// </summary>
    /// <param name="mids">机器编号集</param>
    /// <returns>返回是否在线</returns>
    UnaryResult<List<KeyValuePair<string, string>>> OnlineVends();

    /// <summary>
    /// 6216通用控制指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    UnaryResult<bool> Downlink6216SendAsync(DownlinkEntity6216 request);

    /// <summary>
    /// 9021
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    UnaryResult<bool> Downlink9021SendAsync(DownlinkEntity9021 request);

    /// <summary>
    /// 1011服务器向VMC同步能力配置指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink1011SendAsync(DownlinkEntity1011 request);

    /// <summary>
    /// 9026
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink9026SendAsync(DownlinkEntity9026 request);

    /// <summary>
    /// 5212
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink5212SendAsync(DownlinkEntity5212 request);

    /// <summary>
    /// 9028
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink9028SendAsync(DownlinkEntity9028 request);

    /// <summary>
    /// 9029
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink9029SendAsync(DownlinkEntity9029 request);

    /// <summary>
    /// 6002.远程清除货道故障并测试货道
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink6002SendAsync(DownlinkEntity6002 request);

    /// <summary>
    /// 6200下发同步货道指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink6200SendAsync(DownlinkEntity6200 request);

    /// <summary>
    /// 3201发送远程出货指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    UnaryResult<bool> Downlink3201SendAsync(DownlinkEntity3201 request);

    /// <summary>
    /// 1203发送远程升级指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<bool> Downlink1203SendAsync(DownlinkEntity1203 request);

    /// <summary>
    /// 1203发送远程升级指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<bool> Downlink1009SendAsync(DownlinkEntity1009 request);

    /// <summary>
    /// 10025发送机器质检
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<bool> Downlink10025SendAsync(DownlinkEntity10025 request);

    /// <summary>
    /// 10011发送机器语音播报
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns></returns>
    UnaryResult<bool> Downlink10011SendAsync(DownlinkEntity10011.Request request);

    /// <summary>
    /// 通用下行指令发送接口
    /// </summary>
    /// <param name="content">指令内容</param>
    /// <returns></returns>
    UnaryResult<CommandSendResult> DownlinkSendAsync(string content);
}
