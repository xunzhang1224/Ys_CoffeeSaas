using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class NotityMsg : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long DeviceId { get; private set; }
        /// <summary>
        /// 消息名称
        /// </summary>
        [Required]
        public string MsgName { get; private set; }

        /// <summary>
        /// 通知类型
        /// 0：预警 1：异常
        /// </summary>
        [Required]
        public int Type { get; private set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Required]
        public string ContactAddress { get; private set; }

        /// <summary>
        /// 消息
        /// </summary>
        [Required]
        public string Msg { get; private set; }

        /// <summary>
        /// a
        /// </summary>
        protected NotityMsg() { }

        /// <summary>
        /// a
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="type"></param>
        /// <param name="contactAddress"></param>
        /// <param name="msg"></param>
        /// <param name="enterpriseinfoId"></param>
        public NotityMsg(string msgName, int type, string contactAddress, string msg, long enterpriseinfoId)
        {
            MsgName = msgName;
            Type = type;
            ContactAddress = contactAddress;
            Msg = msg;
            EnterpriseinfoId = enterpriseinfoId;
        }
    }
}
