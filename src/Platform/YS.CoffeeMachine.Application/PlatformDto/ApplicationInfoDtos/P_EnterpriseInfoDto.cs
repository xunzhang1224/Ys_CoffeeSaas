using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos
{
    /// <summary>
    /// 企业信息dto
    /// </summary>
    public class P_EnterpriseInfoDto
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        public long? EnterpriseTypeId { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public P_EnterpriseInfoDto EnterpriseType { get; set; }

        /// <summary>
        /// 企业类型文本
        /// </summary>
        public string? EnterpriseTypeText { get; set; } = null;
        /// <summary>
        /// 企业默认管理员账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 默认管理员昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机区号
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 默认管理员手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 默认管理员邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 设备数量
        /// </summary>
        public int DeviceCount { get; set; }
        /// <summary>
        /// 下级企业数量
        /// </summary>
        public int ChildrenCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 下级企业
        /// </summary>
        public List<P_EnterpriseInfoDto> Children { get; set; }

        /// <summary>
        /// 地区关联id
        /// </summary>
        public long? AreaRelationId { get; set; }

        /// <summary>
        /// 国家key
        /// </summary>
        public string CountryKey { get; set; }

        /// <summary>
        /// 国家value
        /// </summary>
        public string CountryValue { get; set; }
    }

    /// <summary>
    /// 企业选择
    /// </summary>
    public class P_EnterpriseSelect
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
