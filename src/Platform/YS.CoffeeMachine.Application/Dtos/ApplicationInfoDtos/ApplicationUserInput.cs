namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    using YS.CoffeeMachine.Application.Dtos.PagingDto;
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

    /// <summary>
    /// 用户信息输入参数 DTO，用于接收用户查询或创建时的请求数据
    /// </summary>
    public class ApplicationUserInput : QueryRequest
    {
        /// <summary>
        /// 所属企业ID，标识用户归属于哪个企业
        /// </summary>
        public long enterpriseId { get; set; }

        /// <summary>
        /// 用户昵称，用于界面显示
        /// </summary>
        public string nickName { get; set; }

        /// <summary>
        /// 用户账号，用于登录系统
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 用户邮箱地址
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 账号状态（可为空），例如启用、禁用等
        /// </summary>
        public UserStatusEnum? status { get; set; }

        /// <summary>
        /// 角色ID（可为空），标识用户在企业中的角色
        /// </summary>
        public long? roleId { get; set; }
    }
}