using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands
{
    /// <summary>
    /// 支付配置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    public record UpdatePaymentConfigEnabledCommand(long id, EnabledEnum enabled) : ICommand;
}
