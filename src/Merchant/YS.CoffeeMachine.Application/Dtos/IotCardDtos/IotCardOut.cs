using Newtonsoft.Json;

namespace YS.CoffeeMachine.Application.Dtos.IotCardDtos
{
    /// <summary>
    /// 充值策略详情
    /// </summary>
    public class PoclicyDetailOut
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 卡平台Id
        /// </summary>
        public int iotCardPlatformId { get; set; }
        /// <summary>
        /// carrier
        /// </summary>
        public int carrier { get; set; }
        /// <summary>
        /// 已用流量
        /// </summary>
        public decimal flowUsed { get; set; }
        /// <summary>
        /// 内部到期日期
        /// </summary>
        public string innerExpiryDate { get; set; }
        /// <summary>
        /// 到期日
        /// </summary>
        public string expiryDate { get; set; }
        /// <summary>
        /// 是否有限续订
        /// </summary>
        public bool isLimitedRenewal { get; set; }
        /// <summary>
        /// 是否有限加油
        /// </summary>
        public bool isLimitedRefueling { get; set; }
        /// <summary>
        /// 最大限度流量
        /// </summary>
        public decimal flowMaxLimited { get; set; }
        /// <summary>
        /// 恢复价格
        /// </summary>
        public decimal? renewalPrice { get; set; } = 0;
        /// <summary>
        /// 恢复折扣
        /// </summary>
        public int? renewalDiscount { get; set; } = 0;
        /// <summary>
        /// 包折扣
        /// </summary>
        public int? packDiscount { get; set; } = 0;
        /// <summary>
        /// 包价格
        /// </summary>
        public string packagePrice { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public string cardStatus { get; set; }
        /// <summary>
        /// 充值流量
        /// </summary>
        public int? rechargeNum { get; set; } = 0;
        /// <summary>
        /// 激活时间
        /// </summary>
        public string activeDate { get; set; }
        /// <summary>
        /// 包名称
        /// </summary>
        public string packageName { get; set; }
        /// <summary>
        /// 策略编号
        /// </summary>
        public string policyCode { get; set; }
        /// <summary>
        /// 策略Id
        /// </summary>
        public string policyId { get; set; }

        /// <summary>
        ///续费折扣价
        /// </summary>
        public decimal RenewalDiscountPrice
        {
            get
            {
                var discount = Convert.ToInt32(renewalDiscount);
                var price = Convert.ToDecimal(renewalPrice);
                if (discount == 0 || discount == 100)
                {
                    return price;//原价
                }
                // 确保折扣在 0 到 100 的范围内
                if (discount < 0 || discount > 100)
                {
                    return price;//原价
                }
                // 计算折扣后的金额
                decimal discountedAmount = price * (discount / 100);
                decimal finalPrice = price - discountedAmount;
                return finalPrice;
            }
        }
        /// <summary>
        ///加油包折扣价
        /// </summary>
        public List<PackagePrices> PackageDiscountPriceList
        {
            get
            {
                if (string.IsNullOrEmpty(packagePrice)) return new List<PackagePrices>();
                var list = JsonConvert.DeserializeObject<List<PackagePrices>>(packagePrice);
                foreach (var item in list)
                {

                    var discount = Convert.ToInt32(packDiscount);
                    var price = Convert.ToDecimal(item.Price);
                    if (discount == 0 || discount == 100)
                    {
                        item.DiscountPrice = item.Price;//原价
                        continue;
                    }
                    // 确保折扣在 0 到 100 的范围内
                    if (discount < 0 || discount > 100)
                    {
                        item.DiscountPrice = item.Price;//原价
                        continue;
                    }
                    // 计算折扣后的金额
                    decimal discountedAmount = price * (discount / 100);
                    decimal finalPrice = price - discountedAmount;
                    item.DiscountPrice = finalPrice;
                }
                return list;
            }
        }

        /// <summary>
        /// 流量包价格详情
        /// </summary>
        public class PackagePrices
        {
            /// <summary>
            /// 价格
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// 单位
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// 折扣后价格
            /// </summary>
            public decimal DiscountPrice { get; set; }
        }
    }

    /// <summary>
    /// 策略套餐输出
    /// </summary>
    public class PoclicyPackageOut
    {
        /// <summary>
        /// 策略Id
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// 策略编号
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// 套餐价格
        /// </summary>
        public string PackagePrice { get; set; }
    }

    /// <summary>
    /// 流量卡用户信息
    /// </summary>
    public class LotCardUserInfo
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 流量卡锁卡状态输出
    /// </summary>
    public class CardLockStatusOut
    {
        /// <summary>
        /// 流量卡Iccid
        /// </summary>
        public string Iccid { get; set; }

        /// <summary>
        /// 锁卡状态
        /// </summary>
        public bool LockStatus { get; set; }
    }

    /// <summary>
    /// 设备流量卡信息输出对象
    /// </summary>
    public class VendIotCardOut
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string MachineStickerCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// iccid
        /// </summary>
        public string ICCID { get; set; }

        /// <summary>
        /// 资产编号
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        /// 已用流量
        /// </summary>
        public decimal FlowUsed { get; set; }

        /// <summary>
        /// 充值笔数
        /// </summary>
        public int? RechargeNum { get; set; } = 0;

        /// <summary>
        /// 最大流量
        /// </summary>
        public decimal FlowMaxLimited { get; set; }

        /// <summary>
        /// 卡状态
        /// </summary>
        public string CardStatus { get; set; } = "未知";

        /// <summary>
        /// 折扣系数
        /// </summary>
        public int? PackDiscount { get; set; } = 0;

        /// <summary>
        ///策略编号
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// 充值策略详情
        /// </summary>
        public PoclicyDetailOut PoclicyDetail { get; set; } = new PoclicyDetailOut();

        /// <summary>
        /// 卡状态
        /// </summary>
        public int FlowStatus
        {
            get
            {
                if (FlowMaxLimited != 0)
                {
                    var denominator = FlowMaxLimited + RechargeNum;
                    var ratio = denominator != 0 ? (FlowUsed / denominator) * 100 : 0;
                    if (ratio >= 100)
                    {
                        return 1;
                    }
                    else if (ratio >= 30)
                    {
                        return 2;
                    }
                    else if (ratio <= 30)
                    {
                        return 3;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 激活时间
        /// </summary>
        public string activeDate { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public string expiryDate { get; set; }
    }
}