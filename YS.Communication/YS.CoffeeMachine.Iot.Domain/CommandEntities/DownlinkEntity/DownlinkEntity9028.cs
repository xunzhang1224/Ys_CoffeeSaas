using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    /// <summary>
    /// 9028 删除商品通知设备
    /// </summary>
    [MessagePackObject(true)]
    public class DownlinkEntity9028 : BaseCmd
    {

        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 9028;

        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 。
        /// </summary>
        public List<string> Skus { get; set; }

        #region 响应实体

        /// <summary>
        ///  响应
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
        }
        #endregion

    }
}
