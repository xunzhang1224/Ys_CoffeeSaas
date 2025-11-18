namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{

    /// <summary>
    /// 企业类型信息
    /// </summary>
    public class EnterpriseTypesDto
    {
        /// <summary>
        /// 企业类型唯一标识ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 企业类型名称，例如：餐饮、零售等
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否受限标志，true 表示该企业类型有特定限制条件
        /// </summary>
        public bool Astrict { get; set; }

        /// <summary>
        /// 是否为默认类型，true 表示该类型是系统默认的企业类型
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
