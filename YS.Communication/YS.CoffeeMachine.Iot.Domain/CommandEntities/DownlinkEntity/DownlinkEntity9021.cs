using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YS.CoffeeMachine.Iot.Domain.CommandEntities.UplinkEntity9022.Request;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    /// <summary>
    /// 9021: 物料下发
    /// </summary>
    [MessagePackObject(true)]
    public class DownlinkEntity9021 : BaseCmd
    {

        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 9021;

        #region 公共属性

        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 物料集合。
        /// </summary>
        public List<MaterialInfo> Details { get; set; }
        #endregion

        #region 响应实体

        /// <summary>
        /// 接收
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
            /// <summary>
            /// TransId
            /// </summary>
            public string TransId { get; set; }
        }
        #endregion
    }
}
