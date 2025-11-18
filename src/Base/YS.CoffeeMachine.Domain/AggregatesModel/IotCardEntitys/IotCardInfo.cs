using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.IotCardEntitys
{
    /// <summary>
    /// 物联网卡信息
    /// </summary>
    public class IotCardInfo : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; private set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; private set; }

        /// <summary>
        /// iccid
        /// </summary>
        public string ICCID { get; private set; }

        /// <summary>
        /// 资产编号
        /// </summary>
        public string AssetCode { get; private set; }

        /// <summary>
        /// 已用流量
        /// </summary>
        public decimal FlowUsed { get; private set; }

        /// <summary>
        /// 充值流量
        /// </summary>
        public int RechargeNum { get; private set; }

        /// <summary>
        /// 最大流量
        /// </summary>
        public decimal FlowMaxLimited { get; private set; }

        /// <summary>
        /// 卡状态
        /// </summary>
        public string CardStatus { get; private set; }

        /// <summary>
        /// 折扣系数
        /// </summary>
        public string PackDiscount { get; private set; }

        /// <summary>
        /// 策略编号
        /// </summary>
        public string PolicyCode { get; private set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime activeDate { get; private set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public DateTime expiryDate { get; private set; }
    }
}