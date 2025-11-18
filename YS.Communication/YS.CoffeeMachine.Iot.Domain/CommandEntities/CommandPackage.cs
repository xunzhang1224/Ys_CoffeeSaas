namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;

/// <summary>
/// Command包
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public class CommandPackage<T>
    where T : BaseCmd
{
    /// <summary>
    /// 消息头
    /// </summary>
    public CommandHeaders Headers { get; set; }

    /// <summary>
    /// 消息体
    /// </summary>
    public T Body { get; set; }
}
