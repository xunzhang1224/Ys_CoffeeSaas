using System.ComponentModel.DataAnnotations;

namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 绑定入参
    /// </summary>
    public class BindInput
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        public string Mid { get; set; }

        /// <summary>
        /// 箱体
        /// </summary>
        [Required]
        public string BoxId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? MachineStickerCode { get; set; }
    }
}
