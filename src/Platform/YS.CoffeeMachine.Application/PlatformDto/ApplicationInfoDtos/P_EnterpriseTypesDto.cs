namespace YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos
{
    /// <summary>
    /// 企业类型 DTO，用于在应用程序层和表现层之间传递企业类型数据
    /// </summary>
    public class P_EnterpriseTypesDto
    {
        /// <summary>
        /// 企业类型唯一标识 ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 企业类型名称，例如：集团、门店、代理商等
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否受限制（权限或功能上的限制）
        /// </summary>
        public bool Astrict { get; set; }

        /// <summary>
        /// 是否为默认企业类型
        /// </summary>
        public bool IsDefault { get; set; }
    }
}