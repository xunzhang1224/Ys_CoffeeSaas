using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 查询
    /// </summary>
    public class GetDeviceFlushLogInput : QueryRequest
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public FlushComponentTypeEnum? FlushType { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public List<DateTime>? DateTimes { get; set; }
    }

    /// <summary>
    /// 查询
    /// </summary>
    public class GetDeviceRestockLogInput : QueryRequest
    {
        /// <summary>
        /// 名称/编号
        /// </summary>
        public string str { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public List<DateTime>? DateTimes { get; set; }
    }
}
