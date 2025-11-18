using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos
{
    /// <summary>
    /// 饮品信息dto
    /// </summary>
    public class P_BeverageInfoDto : BaseEntity
    {
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long DeviceModelId { get; set; }
        /// <summary>
        /// 语言字典key
        /// </summary>
        public string LanguageKey { get; private set; }
        /// <summary>
        /// 料盒数量
        /// </summary>
        public int MaterialBoxCount { get; set; }
        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }
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
        public List<P_FormulaInfoDto> FormulaInfoTemplates { get; set; } = [];

        /// <summary>
        /// 物料信息
        /// </summary>
        public List<P_FormulaInfoDto> FormulaInfos { get; set; } = [];

        /// <summary>
        /// 物料文本
        /// </summary>
        public string FormulasText { get { return string.Join(",", FormulaInfos.Select(s => s.FormulaType.GetDescriptionOrValue()).ToList()); } }

        /// <summary>
        /// 版本文本
        /// </summary>
        public string VersionText { get { return BeverageVersions != null ? string.Format($"第{BeverageVersions.Where(w => w.VersionType == BeverageVersionTypeEnum.Edit || w.VersionType == BeverageVersionTypeEnum.Insert).Count()}版") : "无"; } }
        /// <summary>
        /// 历史记录
        /// </summary>
        public List<P_BeverageVersion> BeverageVersions { get; set; } = [];
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
    }
}
