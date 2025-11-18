using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// MaterialBoxDto
    /// </summary>
    public class MaterialBoxDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设置id
        /// </summary>
        public long SettingInfoId { get; set; }
        /// <summary>
        /// 料盒名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
