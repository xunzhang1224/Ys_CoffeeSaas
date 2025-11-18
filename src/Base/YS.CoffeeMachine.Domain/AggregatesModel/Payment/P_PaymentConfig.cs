namespace YS.CoffeeMachine.Domain.AggregatesModel.Payment
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 设备支付设置
    /// </summary>
    public class P_PaymentConfig : BaseEntity
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 上线地区（字典获取国家）
        /// </summary>
        public string Countrys { get; private set; }

        /// <summary>
        /// 支付模式
        /// </summary>
        public PaymentEnum PaymentModel { get; private set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PictureUrl { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; private set; }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="countrys">上线地区</param>
        /// <param name="pictureUrl">图片</param>
        /// <param name="enable">是否开启</param>
        public void Update(string countrys, string pictureUrl, EnabledEnum enable)
        {
            Countrys = countrys;
            PictureUrl = pictureUrl;
            Enabled = enable;
        }
    }
}
