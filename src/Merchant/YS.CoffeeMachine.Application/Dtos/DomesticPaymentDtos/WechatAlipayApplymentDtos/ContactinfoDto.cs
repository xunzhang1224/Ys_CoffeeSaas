using Newtonsoft.Json;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayApplymentDtos
{
    /// <summary>
    /// 商户联系人信息
    /// </summary>
    public class ContactinfoDto
    {
        /// <summary>
        /// 电子邮箱。
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 身份证号。
        /// </summary>
        [JsonProperty("id_card_no")]
        public string? Id_card_no { get; set; }

        /// <summary>
        /// 手机号。必填与否参见外层对象描述，无特别说明认为是非必填。
        /// </summary>
        [JsonProperty("mobile")]
        public string? Mobile { get; set; }

        /// <summary>
        /// 联系人名字。
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 电话。
        /// </summary>
        [JsonProperty("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Tag。
        /// </summary>
        [JsonProperty("tag")]
        public List<string>? Tag { get; set; }

        /// <summary>
        /// Type。
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}