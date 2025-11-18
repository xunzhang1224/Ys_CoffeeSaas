using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands
{
    /// <summary>
    /// 补货
    /// </summary>
    /// <param name="input"></param>
    public record DeviceRestockCommand(Dictionary<long, DeviceRestockDto> dic,long deviceId) : ICommand<bool>;

    /// <summary>
    /// a
    /// </summary>
    public class DeviceRestockDto
    {
        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }
    }
}
