using MagicOnion;
using YS.CoffeeMachine.Iot.Application.GRPC.DTO;
using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO;
using YS.CoffeeMachine.Iot.Domain.CommandEntities;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands;

/// <summary>
/// 提供指令的处理接口
/// </summary>
public interface IGrpcCommandService : IService<IGrpcCommandService>
{
    /// <summary>
    /// TestPayOrder
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    UnaryResult<bool> TestPayOrder(MockWechatPaymentDto request);

    /// <summary>
    /// 测试订单
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    UnaryResult<string> TestOrder(UplinkEntity8888.Request request);

    /// <summary>
    /// 测试异步
    /// </summary>
    /// <param name="vendNo"></param>
    /// <returns></returns>
    UnaryResult<string> TestAsync(string vendNo);

    /// <summary>
    /// 获取设备初始化信息
    /// </summary>
    /// <param name="mid"></param>
    /// <returns></returns>
    UnaryResult<SecretInfoOutput> GetDeviceInitByMidAsync(string mid);
    /// <summary>
    /// 1000 初始化
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity1000.Response> Uplink1000HandleAsync(UplinkEntity1000.Request request);
    /// <summary>
    /// 2000.VMC向服务器上报心跳
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink2000HandleAsync(UplinkEntity2000.Request request);

    /// <summary>
    /// 1008.VMC向服务器上报能力。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回是否成功。</returns>
    UnaryResult<bool> Uplink1008HandleAsync(UplinkEntity1008 request);

    /// <summary>
    /// 1010.VMC向服务器上报能力配置
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity1010.Response> Uplink1010HandleAsync(UplinkEntity1010.Request request);

    /// <summary>
    /// 1012.VMC向服务器上报指标
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink1012HandleAsync(UplinkEntity1012.Request request);

    /// <summary>
    /// 9022上报指标
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink9022HandleAsync(UplinkEntity9022.Request request);

    /// <summary>
    /// 9027上报商品
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity9027.Response> Uplink9027HandleAsync(UplinkEntity9027.Request request);

    /// <summary>
    /// 9030
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity9030.Response> Uplink9030HandleAsync(UplinkEntity9030.Request request);

    /// <summary>
    /// 9031清洗部件上报
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity9031.Response> Uplink9031HandleAsync(UplinkEntity9031.Request request);

    /// <summary>
    /// 9032清洗上报
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity9032.Response> Uplink9032HandleAsync(UplinkEntity9032.Request request);

    /// <summary>
    /// 9033清洗部件使用次数上报
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity9033.Response> Uplink9033HandleAsync(UplinkEntity9033.Request request);

    /// <summary>
    /// 1013.VMC向服务器上报属性
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink1013HandleAsync(UplinkEntity1013.Request request);

    /// <summary>
    /// 4201.VMC向服务器上报出货结果
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity4201.Response> Uplink4201HandleAsync(UplinkEntity4201.Request request);

    /// <summary>
    /// 提供5205号指令【VMC向服务器上报货道（机器货道同步）】
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<UplinkEntity5205.Response> Uplink5205HandleAsync(UplinkEntity5205.Request request);

    /// <summary>
    /// 提供5207号指令 VMC向服务器上报货道结构
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns></returns>
    UnaryResult<UplinkEntity5207.Response> Uplink5207HandleAsync(UplinkEntity5207.Request request);

    /// <summary>
    /// 5204.VMC向服务器上报错误
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink5204HandleAsync(UplinkEntity5204.Request request);

    /// <summary>
    /// 1011.服务器向VMC同步能力配置的应答处理
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Downlink1011Handle(DownlinkEntity1011.Response request);

    /// <summary>
    /// 3201.远程出货的应答处理
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Downlink3201HandleAsync(DownlinkEntity3201.Response request);

    /// <summary>
    /// 6216远程下发通用控制指令的应答处理
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Downlink6216HandleAsync(DownlinkEntity6216.Response request);
    /// <summary>
    ///  用于6002号指令的协议实体：远程清除货道故障并测试货道指令.
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Downlink6002HandleAsync(DownlinkEntity6002.Response request);
    /// <summary>
    /// 6200.远程设置货道信息的应答处理
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Downlink6200HandleAsync(DownlinkEntity6200.Response request);

    /// <summary>
    /// 1006.VMC向服务器发送登录指令（订单）
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<bool> Uplink1006HandleAsync(UplinkEntity1006.Request request);

    /// <summary>
    /// 1205.VMC向服务器上报程序信息。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<bool> Uplink1205HandleAsync(UplinkEntity1205.Request request);

    /// <summary>
    /// 1203.远程升级的应答处理。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<bool> Downlink1203HandleAsync(DownlinkEntity1203.Response request);

    /// <summary>
    /// 1204.VMC向服务器询问最新程序信息。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果。</returns>
    UnaryResult<UplinkEntity1204.Response> Uplink1204HandleAsync(UplinkEntity1204.Request request);

    /// <summary>
    /// 7200.VMC向服务器请求初始化刷脸SDK的必要参数。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity7200.Response> Uplink7200HandleAsync(UplinkEntity7200.Request request);

    /// <summary>
    /// 7201.VMC向服务器获取支付平台相关配置信息。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity7201.Response> Uplink7201HandleAsync(UplinkEntity7201.Request request);

    /// <summary>
    /// 7202.VMC向服务器获取支付用户系统信息。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity7202.Response> Uplink7202HandleAsync(UplinkEntity7202.Request request);

    /// <summary>
    /// 1014.VMC向服务器询问能力配置。
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity1014.Response> Uplink1014HandleAsync(UplinkEntity1014.Request request);

    /// <summary>
    /// 7211.交易上报
    /// </summary>
    /// <param name="request">请求参数。</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity7211.Response> Uplink7211HandleAsync(UplinkEntity7211.Request request);

    /// <summary>
    /// 7221.订单附件上报
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<bool> Uplink7221HandleAsync(UplinkEntity7221.Request request);

    /// <summary>
    /// 5213.VMC向服务器请求商品列表
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity5213.Response> Uplink5213HandleAsync(UplinkEntity5213.Request request);

    /// <summary>
    /// 5209.VMC向服务器上报补货信息
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity5209.Response> Uplink5209HandleAsync(UplinkEntity5209.Request request);

    /// <summary>
    /// 1210.VMC向服务器请求HttpApi调用凭证
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity1210.Response> Uplink1210HandleAsync(UplinkEntity1210.Request request);

    /// <summary>
    /// 5206.VMC向服务器上报故障清除
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回参数</returns>
    UnaryResult<bool> Uplink5206HandleAsync(UplinkEntity5206.Request request);

    /// <summary>
    /// 7214.取货码取货
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回结果</returns>
    UnaryResult<UplinkEntity7214.Response> Uplink7214HandleAsync(UplinkEntity7214.Request request);

    /// <summary>
    /// 7212
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    UnaryResult<UplinkEntity7212.Response> Uplink7212HandleAsync(UplinkEntity7212.Request request);

    /// <summary>
    /// 命令处理
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    UnaryResult<string> CommandHandleAsync(string content);
}
