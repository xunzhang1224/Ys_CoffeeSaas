using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos
{
    /// <summary>
    /// 饮品合集dto
    /// </summary>
    public class P_BeverageCollectionDto
    {
        /// <summary>
        /// 饮品集合id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 语言字典key
        /// </summary>
        public string LanguageKey { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long DeviceModelId { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 合集名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 饮品Id集合
        /// </summary>
        public string BeverageIds { get; set; }
        /// <summary>
        /// 包含饮品
        /// </summary>
        public string BeverageNames { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public long? CreateUserId { get; set; }

        /// <summary>
        /// 创建用户名字
        /// </summary>
        public string CreateUserName { get; set; }
    }
}
