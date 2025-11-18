using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 角色dto
    /// </summary>
    public class EnterpriseRoleDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        //public string Code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public RoleStatusEnum Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EnterpriseRoleDto() { }

        /// <summary>
        /// dto
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="sort"></param>
        /// <param name="remark"></param>
        public EnterpriseRoleDto(string name, RoleStatusEnum status, int sort, string remark)
        {
            Name = name;
            Status = status;
            Sort = sort;
            Remark = remark;
        }
    }
}
