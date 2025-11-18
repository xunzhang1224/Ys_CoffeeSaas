using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos
{
    /// <summary>
    /// BeverageInfoTemplateDto
    /// </summary>
    public class BeverageInfoTemplateDto : BaseEntity
    {
        //public long Id { get; set; }
        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseInfoId { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long DeviceModelId { get; set; }
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
        public List<FormulaInfoTemplateDto> FormulaInfoTemplates { get; set; } = [];

        /// <summary>
        /// FormulaInfos
        /// </summary>
        public List<FormulaInfoTemplateDto> FormulaInfos { get; set; }

        /// <summary>
        /// FormulasText
        /// </summary>
        public string FormulasText { get { return string.Join(",", FormulaInfos.Select(s => s.FormulaType.GetDescriptionOrValue()).ToList()); } }

        /// <summary>
        /// VersionText
        /// </summary>
        public string VersionText { get { return BeverageTemplateVersions != null ? string.Format($"第{BeverageTemplateVersions.Count() + 1}版") : "无"; } }
        /// <summary>
        /// 历史记录
        /// </summary>
        public List<BeverageTemplateVersion> BeverageTemplateVersions { get; private set; } = [];
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
