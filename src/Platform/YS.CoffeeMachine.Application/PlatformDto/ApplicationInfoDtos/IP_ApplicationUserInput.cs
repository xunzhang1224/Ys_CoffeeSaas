using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos
{
    /// <summary>
    /// 用户入参
    /// </summary>
    public class IP_ApplicationUserInput : QueryRequest
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatusEnum? Status { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long? RoleId { get; set; }
    }
}
