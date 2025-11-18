using Aop.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys
{
    /// <summary>
    /// 营销活动
    /// </summary>
    public class PromotionOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>活动名称</summary>
        public string Name { get; set; }

        /// <summary>
        /// 发放的数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>活动开始时间</summary>
        public DateTime StartTime { get; set; }

        /// <summary>活动结束时间</summary>
        public DateTime EndTime { get; set; }

        /// <summary>优惠券类型（满减券、折扣券等）</summary>
        public PromotionType CouponType { get; set; }

        /// <summary>
        /// 总发放数量
        /// </summary>
        public int TotalLimit { get; set; }

        /// <summary>
        /// 限领次数
        /// 0：不限次数
        /// </summary>
        public int LimitedCount { get; set; } = 0;

        /// <summary>
        /// 参与用户
        /// 为空标识全部用户
        /// </summary>
        public List<long>? ParticipatingUsers { get; set; }

        /// <summary>
        /// 优惠券面值配置
        /// </summary>
        public CouponValue Value { get; set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        public UsageRules UsageRules { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public PromotionStatusEnum PromotionStatus { get; set; }

        /// <summary>
        /// 优惠券列表
        /// </summary>
        public List<Coupon> Coupons { get; set; } = new List<Coupon>();

        /// <summary>
        /// 已被领取的数量
        /// </summary>
        public int HaveCount
        {
            get
            {
                return Coupons.Count();
            }
        }

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseCount
        {
            get
            {
                return Coupons.Count(x => x.Status == CouponStatusEnum.Used);
            }
        }

        /// <summary>
        /// 使用占比
        /// </summary>
        public string UseZb
        {
            get
            {
                if (HaveCount <= 0)
                {
                    return "0%";
                }
                else
                {
                    double percentage = (double)UseCount / HaveCount * 100;
                    return $"{percentage:F2}%";
                }
            }
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public class GetDecentlyDevicePageListInput : QueryRequest
        {
            /// <summary>
            /// 设备信息
            /// </summary>
            public string? DeviceName { get; set; }

            /// <summary>
            /// 经度
            /// </summary>
            [Required]
            public double Lng { get; set; }

            /// <summary>
            /// 维度
            /// </summary>
            [Required]
            public double Lat { get; set; }
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public class GetDecentlyDevicePageListOutput
        {
            /// <summary>
            /// 是否在线
            /// </summary>
            public bool IsOnline { get; set; }

            /// <summary>
            /// 设备名称
            /// </summary>
            public string DeviceName { get; set; }

            /// <summary>
            /// 设备地址
            /// </summary>
            public string DeviceAddress { get; set; }

            /// <summary>
            /// 设备图片
            /// </summary>
            public string DeviceImageUrl { get; set; }

            /// <summary>
            /// 运营时间
            /// </summary>
            public string OperatingHours { get; set; }

            /// <summary>
            /// 距离
            /// </summary>
            public double Distance { get; set; }
        }
    }
}
