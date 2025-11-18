using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public class ApplicationUserInput : QueryRequest
    {
        /// <summary>
        /// 企业ID，标识用户所属的企业
        /// </summary>
        public long enterpriseId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickName { get; set; }

        /// <summary>
        /// 用户账号，用于登录系统
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 用户状态（可为空），例如启用、禁用等
        /// </summary>
        public UserStatusEnum? status { get; set; }

        /// <summary>
        /// 角色ID（可为空），标识用户在企业中的角色
        /// </summary>
        public long? roleId { get; set; }
    }
}