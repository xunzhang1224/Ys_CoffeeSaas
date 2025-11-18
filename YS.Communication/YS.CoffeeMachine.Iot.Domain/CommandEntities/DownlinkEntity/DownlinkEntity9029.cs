using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    /// <summary>
    /// 9029 下发商品顺序
    /// </summary>
    [MessagePackObject(true)]
    public class DownlinkEntity9029 : BaseCmd
    {

        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 9029;

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
