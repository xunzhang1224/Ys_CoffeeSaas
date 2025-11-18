using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.StrategyDtos
{
    /// <summary>
    /// 地区关联dto
    /// </summary>
    public class AreaRelationDto
    {
        /// <summary>
        /// 地区关联id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 地区（字典）
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 地区 值
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 国家（字典）
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 国家 值
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 语言（字典）
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 语言 值
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// 币种Id
        /// </summary>
        public long CurrencyId { get; set; }

        /// <summary>
        /// 币种code
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 币种名称
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZoneName { get; set; }

        /// <summary>
        /// 服务条款Url
        /// </summary>
        public string TermServiceUrl { get; set; }

        /// <summary>
        /// 服务条款名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
