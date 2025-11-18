using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.CountryInfoDots
{
    /// <summary>
    /// 国家信息
    /// </summary>
    public class CountryInfoDto
    {
        /// <summary>
        /// 国家Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 国家代码，例如“CN”
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// 所在大洲
        /// </summary>
        public string Continent { get; set; }

        /// <summary>
        /// CountryInfoDto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="countryName"></param>
        /// <param name="countryCode"></param>
        /// <param name="continent"></param>
        public CountryInfoDto(long id, string countryName, string countryCode, string continent)
        {
            Id = id;
            CountryName = countryName;
            CountryCode = countryCode;
            Continent = continent;
        }
    }

    /// <summary>
    /// CountryInfoListDto
    /// </summary>
    public class CountryInfoListDto
    {
        /// <summary>
        /// CountryInfoList
        /// </summary>
        public List<CountryInfoDto> CountryInfoList { get; set; }

        /// <summary>
        /// CountryInfoListDto
        /// </summary>
        public CountryInfoListDto() { CountryInfoList = new List<CountryInfoDto>(); }
    }
}
