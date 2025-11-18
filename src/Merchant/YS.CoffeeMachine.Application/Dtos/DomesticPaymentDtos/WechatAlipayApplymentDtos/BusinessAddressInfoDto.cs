using Newtonsoft.Json;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayApplymentDtos
{
    /// <summary>
    /// 经营地址信息
    /// </summary>
    public class BusinessAddressInfoDto
    {
        /// <summary>地址。商户详细经营地址或人员所在地点。</summary>
        [JsonProperty("address")]
        public string? Address { get; set; }

        /// <summary>城市编码。请按照https://gw.alipayobjects.com/os/basement_prod/253c4dcb-b8a4-4a1e-8be2-79e191a9b6db.xlsx 表格中内容填写。（参考资料：http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/）。</summary>
        [JsonProperty("city_code")]
        public string? City_code { get; set; }

        /// <summary>区县编码。请按照https://gw.alipayobjects.com/os/basement_prod/253c4dcb-b8a4-4a1e-8be2-79e191a9b6db.xlsx 表格中内容填写。（参考资料：http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/）。</summary>
        [JsonProperty("district_code")]
        public string? District_code { get; set; }

        /// <summary>纬度，浮点型,小数点后最多保留6位如需要录入经纬度，请以高德坐标系为准，录入时请确保经纬度参数准确。高德经纬度查询：http://lbs.amap.com/console/show/picker。</summary>
        [JsonProperty("latitude")]
        public string? Latitude { get; set; }

        /// <summary>经度。</summary>
        [JsonProperty("longitude")]
        public string? Longitude { get; set; }

        /// <summary>高德poiid。</summary>
        [JsonProperty("poiid")]
        public string? Poiid { get; set; }

        /// <summary>省份编码。请按照https://gw.alipayobjects.com/os/basement_prod/253c4dcb-b8a4-4a1e-8be2-79e191a9b6db.xlsx 表格中内容填写。（参考资料：http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/）。</summary>
        [JsonProperty("province_code")]
        public string? Province_code { get; set; }

        /// <summary>Type。</summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}