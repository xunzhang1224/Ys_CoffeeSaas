using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// InterfaceStylesDto
    /// </summary>
    public class InterfaceStylesDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 风格名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 风格标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 风格预览图
        /// </summary>
        public string Preview { get; set; }
    }
}
