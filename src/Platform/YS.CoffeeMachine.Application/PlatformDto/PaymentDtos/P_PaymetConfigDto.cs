using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.PaymentDtos
{
    /// <summary>
    /// 平台端支付配置dto
    /// </summary>
    public class P_PaymetConfigDto
    {
        /// <summary>
        /// 支付平台支付方式id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上线地区（字典获取国家）
        /// </summary>
        public string Countrys { get; set; }

        /// <summary>
        /// 地区数组
        /// </summary>
        public List<string>? CountryArry { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryNames { get; set; }

        /// <summary>
        /// 支付模式
        /// </summary>
        public PaymentEnum PaymentModel { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; set; }
    }
}
