namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.Info;

/// <summary>
/// MQTT信息
/// </summary>
public class MqttInfo
{
    /// <summary>
    /// 频道
    /// </summary>
    public string Channel { get; set; }

    /// <summary>
    /// 实例id
    /// </summary>
    public string InstanceId { get; set; }

    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Secret
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 分组id
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// UTopic
    /// </summary>
    public string UTopic { get; set; }

    /// <summary>
    /// DTopic
    /// </summary>
    public string DTopic { get; set; }
}
