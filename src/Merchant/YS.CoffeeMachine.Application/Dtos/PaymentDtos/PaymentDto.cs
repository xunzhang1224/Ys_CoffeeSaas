using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.PaymentDtos
{
    /// <summary>
    /// 支付配置
    /// </summary>
    public class PaymentDto
    {

    }

    /// <summary>
    /// 支付配置
    /// </summary>
    public class PaymentConfigDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 支付方式id
        /// </summary>
        public long P_PaymentConfigId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 进件状态
        /// </summary>
        public PaymentConfigStatueEnum PaymentConfigStatue { get; set; }

        /// <summary>
        /// 商户编码
        /// </summary>
        public string MerchantCode { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; set; }
    }

    /// <summary>
    /// 支付配置
    /// </summary>
    public class P_PaymentConfigDto
    {
        /// <summary>
        /// 平台端支付配置id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 平台端支付配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 平台端支付配置图片地址
        /// </summary>
        public string PictureUrl { get; set; }
    }

    /// <summary>
    /// 订单自定义内容
    /// </summary>
    public class OrderCustomContentDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 消费者用户id
        /// </summary>
        public string ConsumerUserId { get; set; }
    }
}
