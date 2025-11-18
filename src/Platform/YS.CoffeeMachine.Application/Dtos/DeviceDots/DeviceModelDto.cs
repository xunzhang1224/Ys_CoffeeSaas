using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备型号dto
    /// </summary>
    public class DeviceModelDto
    {
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 模型Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 型号名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最多料盒数量
        /// </summary>
        public int MaxCassetteCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
