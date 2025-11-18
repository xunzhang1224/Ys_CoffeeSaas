using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.ServiceProviderDtos
{
    /// <summary>
    /// ServiceProviderInfoDto
    /// </summary>
    public class ServiceProviderInfoDto
    {
        /// <summary>
        /// 服务商Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Tel { get; private set; }
    }
}
