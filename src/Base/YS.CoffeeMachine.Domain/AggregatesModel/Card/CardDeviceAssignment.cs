using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Card
{
    /// <summary>
    /// 卡与设备绑定关系
    /// </summary>
    public class CardDeviceAssignment : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 卡id
        /// </summary>
        [Required]
        public long CardId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        [Required]
        public long DeviceId { get; set; }

        /// <summary>
        /// 卡信息
        /// </summary>
        public CardInfo Card { get; private set; }

        /// <summary>
        /// 设备
        /// </summary>
        public DeviceInfo Device { get; private set; }

        private CardDeviceAssignment() { }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="deviceId"></param>
        public CardDeviceAssignment(long cardId, long deviceId)
        {
            CardId = cardId;
            DeviceId = deviceId;
        }
    }
}
