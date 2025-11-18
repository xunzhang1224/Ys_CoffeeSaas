
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO.Base;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
/// <summary>
/// 提供下发指令的服务接口类。
/// </summary>
public interface ICommandSenderService
{
    /// <summary>
    /// 查询机器是否在线(测试修改提交)
    /// </summary>
    /// <param name="vendNo">机器编号</param>
    /// <returns>返回是否在线</returns>
    Task<bool> IsOnlineAsync(DownSeedRequestBase<string> requset);

    /// <summary>
    /// 激活下发生产编号
    /// </summary>
    /// <param name="request">入参</param>
    /// <returns></returns>
    Task<bool> Downlink1100SendAsync(DownSeedRequestBase<DownlinkEntity1100> request);
    /// <summary>
    /// 查询机器是否在线
    /// </summary>
    /// <param name="vendNos">机器编号</param>
    /// <returns>返回是否在线</returns>
    Task<List<KeyValuePair<string, bool>>> IsOnlinesAsync(string[] vendNos);
    /// <summary>
    /// 查询在线机器集
    /// </summary>
    /// <returns></returns>
    Task<List<KeyValuePair<string, string>>> OnlineVends();

    /// <summary>
    /// 6216通用控制指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    Task<bool> Downlink6216SendAsync(DownSeedRequestBase<DownlinkEntity6216> request);

    /// <summary>
    /// 9028。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    Task<bool> Downlink9028SendAsync(DownSeedRequestBase<DownlinkEntity9028> request);

    /// <summary>
    /// 下发物料。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    Task<bool> Downlink9021SendAsync(DownSeedRequestBase<DownlinkEntity9021> request);

    /// <summary>
    /// 1009.服务器向VMC下发能力
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否成功。</returns>
    Task<bool> Downlink1009SendAsync(DownSeedRequestBase<DownlinkEntity1009> request);

    /// <summary>
    /// 1011服务器向VMC同步能力配置指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink1011SendAsync(DownSeedRequestBase<DownlinkEntity1011> request);

    /// <summary>
    /// 1203发送远程升级命令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink1203SendAsync(DownSeedRequestBase<DownlinkEntity1203> request);

    /// <summary>
    /// 9026服务器向VMC同步商品。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink9026SendAsync(DownSeedRequestBase<DownlinkEntity9026> request);

    /// <summary>
    /// 5212.服务器向VMC下发商品价格信息
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink5212SendAsync(DownSeedRequestBase<DownlinkEntity5212> request);

    /// <summary>
    /// 9029
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink9029SendAsync(DownSeedRequestBase<DownlinkEntity9029> request);

    /// <summary>
    /// 3201发送远程出货指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Downlink3201SendAsync(DownSeedRequestBase<DownlinkEntity3201> request);

    /// <summary>
    /// Command6002SendAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> Command6002SendAsync(DownlinkSendDto<DownlinkEntity6002> request);

    /// <summary>
    /// 6200下发同步货道指令。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Command6200SendAsync(DownlinkSendDto<Downlink6200> request);

    /// <summary>
    /// 6200下发同步货道指令V2。
    /// </summary>
    /// <param name="request">入参。</param>
    /// <returns>返回是否发送成功。</returns>
    Task<bool> Command6200SendV2Async(DownlinkSendDto<DownlinkEntity6200> request);

    /// <summary>
    /// 10025发送机器质检命令
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> Downlink10025SendAsync(DownlinkSendDto<Downlink10025> request);

    /// <summary>
    /// 1012 上报设备指标记录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> Command1012SendAsync(UplinkEntity1012.Request request);

    /// <summary>
    /// 5204 上报故障命令
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> Command5204SendAsync(UplinkEntity5204.Request request);

    /// <summary>
    /// ServiceHealthCheck
    /// </summary>
    /// <returns></returns>
    Task<List<CommandSendResult>> ServiceHealthCheck();
}