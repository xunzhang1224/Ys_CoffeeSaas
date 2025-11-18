using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.PlatformDto.BasicDtos
{
    /// <summary>
    /// 字典dto
    /// </summary>
    public class DictionaryDto
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get;  set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Value { get;  set; }

        /// <summary>
        /// 父级key
        /// </summary>
        public string ParentKey { get;  set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum IsEnabled { get;  set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 字典dto
    /// </summary>
    public class DicionaryUseDto
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
