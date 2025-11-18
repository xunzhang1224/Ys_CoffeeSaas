using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos
{
    /// <summary>
    /// 饮品集合入参
    /// </summary>
    public class P_BeverageCollectionInput : QueryRequest
    {
        /// <summary>
        /// 饮品集合名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 语言字典key
        /// </summary>
        public string LanguageKey { get; set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        public string BeverageName { get; set; }
    }
}
