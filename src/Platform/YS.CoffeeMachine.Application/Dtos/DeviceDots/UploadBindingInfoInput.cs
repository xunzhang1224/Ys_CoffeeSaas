using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 上报绑定信息输入参数
    /// </summary>
    public class UploadBindingInfoInput
    {
        /// <summary>
        ///归属系统
        /// </summary>
        public int SubordinateSystem { get; set; }

        /// <summary>
        /// sn，设备唯一标识
        /// mid
        /// </summary>
        [Required]
        public string SN { get; set; }

        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// ICCID
        /// </summary>
        public string ICCID { get; set; }

        /// <summary>
        ///下位机版号
        /// </summary>
        public string LowerComputerCode { get; set; }

        /// <summary>
        /// 箱体ID
        /// </summary>
        [Required]
        public string BoxId { get; set; }

        /// <summary>
        /// 箱体型号
        /// </summary>
        [Required]
        public string BoxType { get; set; }

        /// <summary>
        /// 车贴码，无屏时必填
        /// </summary>
        public string CarStickerCode { get; set; }
    }

    /// <summary>
    /// 12
    /// </summary>
    public class UploadBindingInfoOut
    {
        /// <summary>
        /// 1
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 2
        /// </summary>
        public string Err { get; set; }

        /// <summary>
        /// 3
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 3
        /// </summary>
        public string Product_code { get; set; }
    }
}
