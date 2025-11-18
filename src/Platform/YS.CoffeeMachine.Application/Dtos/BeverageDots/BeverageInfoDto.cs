using System.ComponentModel;
using System.Reflection;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品信息
    /// </summary>
    public class BeverageInfoDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; set; }
        //public DeviceInfoDto DeviceInfo { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 饮品图标
        /// </summary>
        public string BeverageIcon { get; set; }
        /// <summary>
        /// 编码（商品SKU），同设备内唯一
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum temperature { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 配方列表
        /// </summary>
        public List<FormulaInfoDto> FormulaInfos { get; set; } = [];

        /// <summary>
        /// 配方文本
        /// </summary>
        public string FormulasText { get { return string.Join(",", FormulaInfos.Select(s => GetEnumDescription(s.FormulaType)).ToList()); } }

        /// <summary>
        /// 版本文本
        /// </summary>
        public string VersionText { get; set; }
        //public string VersionText { get { return BeverageVersions != null ? string.Format($"第{BeverageVersions.Where(w => w.VersionType != Domain.Shared.Enum.BeverageVersionTypeEnum.Collection).Count() + 1}版") : "无"; } }
        /// <summary>
        /// 出品预测
        /// </summary>
        public string ProductionForecast { get; set; }
        /// <summary>
        /// 出杯量预测(ml)
        /// </summary>
        public double? ForecastQuantity { get; set; }
        /// <summary>
        /// 是否显示状态
        /// </summary>
        public bool DisplayStatus { get; set; }
        /// <summary>
        /// 历史记录
        /// </summary>
        //public List<BeverageVersion> BeverageVersions { get; private set; } = [];
        private string GetEnumDescription(Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();  // 获取 Description 特性
            return attribute?.Description ?? enumValue.ToString();  // 如果没有 Description 特性，则返回枚举值的名称
        }
    }
}
