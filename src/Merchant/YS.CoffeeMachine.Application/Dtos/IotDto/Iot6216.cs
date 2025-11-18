using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.IotDto
{
    /// <summary>
    /// a
    /// </summary>
    public class Iot6216
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// CapabilityId
        /// </summary>
        public int CapabilityId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public List<string> Parameters { get; set; }
    }

    /// <summary>
    /// a
    /// </summary>
    public class Iot6216_63
    {
        /// <summary>
        /// 1
        /// </summary>
        public bool Lock { get; set; }
    }
}
