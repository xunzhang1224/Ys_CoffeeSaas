using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups
{
    /// <summary>
    /// 群组列表查询参数
    /// </summary>
    public class GroupListInput : QueryRequest
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<string> TimeRange { get; set; }
    }
}
