using System.ComponentModel.DataAnnotations;

namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 设备基本redisdto
    /// </summary>
    public class DeviceBaseRedisDto
    {
        /// <summary>
        /// 设备通讯编码
        /// </summary>
        public string Mid { get; private set; }

        /// <summary>
        /// 车贴码/生产编号
        /// </summary>
        public string MachineStickerCode { get; set; }
        /// <summary>
        /// 箱体id
        /// </summary>
        public string BoxId { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
    }
}
