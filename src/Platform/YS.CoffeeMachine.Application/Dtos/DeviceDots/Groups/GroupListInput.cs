using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups
{
    /// <summary>
    /// 分组入参
    /// </summary>
    public class GroupListInput : QueryRequest
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间区间
        /// </summary>
        public List<string> TimeRange { get; set; }
    }
}
