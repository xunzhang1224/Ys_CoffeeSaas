using System.ComponentModel.DataAnnotations;

namespace YS.CoffeeMachine.API.Extensions.Cap.Dtos
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class CreateNotityMsgDto
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 消息名称
        /// </summary>
        public string MsgName { get; set; }

        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 联系人账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public long EnterpriseinfoId { get; set; }
    }
}
