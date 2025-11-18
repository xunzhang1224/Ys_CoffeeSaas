namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;

/// <summary>
/// 基础cmd
/// </summary>
public class BaseCmd
{
    //public string TransId { get; set; }

    /// <summary>
    /// mid
    /// </summary>
    public string Mid { get; set; } = "0000000000";

    /// <summary>
    /// TimeSp
    /// </summary>
    public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
}

/// <summary>
/// 基础TrandCmd
/// </summary>
public class BaseTransCmd
{
    /// <summary>
    /// TransId
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// mid
    /// </summary>
    public string Mid { get; set; } = "0000000000";

    /// <summary>
    /// TimeSp
    /// </summary>
    public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
}