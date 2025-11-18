using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos
{
    /// <summary>
    /// 商户支付方式绑定的设备Dto
    /// </summary>
    public class M_PaymentMethodBindDeviceDto
    {
        /// <summary>
        /// Desc:商户支付方式表的Id（Me_PaymentMethod表的Id）
        /// Default:
        /// Nullable:False
        /// </summary>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// DeviceInfo表Id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// DeviceInfo表Id
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? DeviceMid { get; set; } = null;

        /// <summary>
        /// 设备永久编码
        /// </summary>
        public string? MachineStickerCode { get; set; } = null;

        /// <summary>
        /// 分组Id
        /// </summary>
        public List<long>? GroupId { get; set; } = null;

        /// <summary>
        /// 设备分组名称
        /// </summary>
        public string? DeviceGroupName { get; set; } = null;
    }

    /// <summary>
    /// 商户支付方式绑定设备分页列表查询参数
    /// </summary>
    public class PaymentMethodBindDeviceInput : QueryRequest
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// 设备名称或设备编码
        /// </summary>
        public string? DeviceName { get; set; } = null;

        /// <summary>
        /// 设备分组Id
        /// </summary>
        public List<long> DeviceGroupIds { get; set; } = [];

        /// <summary>
        /// 系统支付方式Id
        /// </summary>
        public long? SystemPaymentMethodId { get; set; } = null;
    }
}