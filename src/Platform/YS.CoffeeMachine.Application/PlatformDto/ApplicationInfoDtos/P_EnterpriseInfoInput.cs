using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos
{
    /// <summary>
    /// 企业信息入参
    /// </summary>
    public class P_EnterpriseInfoInput : QueryRequest
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string enterpriseName { get; set; }
        /// <summary>
        /// 管理员姓名或账号
        /// </summary>
        public string userName_account { get; set; }

        /// <summary>
        /// 企业id
        /// </summary>
        public string? id { get; set; }
    }
}
