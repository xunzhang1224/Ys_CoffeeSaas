using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.PaymentCommands
{
    /// <summary>
    /// 更新平台支付配置
    /// </summary>
    /// <param name="countrys">上线地区</param>
    /// <param name="pictureUrl">图片</param>
    /// <param name="enable">是否启用</param>
    public record UpdatePaymentConfigCommand(long id, string countrys, string pictureUrl, EnabledEnum enable) : ICommand<bool>;
}
