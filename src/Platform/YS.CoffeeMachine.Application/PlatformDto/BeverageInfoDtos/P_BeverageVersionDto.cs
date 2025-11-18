using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos
{
    /// <summary>
    /// 饮品版本dto
    /// </summary>
    public class P_BeverageVersionDto
    {
        /// <summary>
        /// 饮品版本id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 饮品Id
        /// </summary>
        public long BeverageInfoId { get; set; }
        /// <summary>
        /// 版本类型
        /// </summary>
        public BeverageVersionTypeEnum VersionType { get; set; }

        /// <summary>
        /// 饮品历史信息
        /// </summary>
        public string BeverageInfoDataString { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public long? CreateUserId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int VersionNum { get; set; }
    }
}
