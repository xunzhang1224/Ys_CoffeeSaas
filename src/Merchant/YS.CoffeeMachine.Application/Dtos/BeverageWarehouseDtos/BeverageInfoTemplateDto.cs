using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos
{
    /// <summary>
    /// 饮品信息
    /// </summary>
    public class BeverageInfoTemplateDto : BaseEntity
    {
        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<long>? CategoryIds { get; set; }

        /// <summary>
        /// 分类名称集合
        /// </summary>
        public string CategoryNames { get; set; }

        //public long TransId { get; set; }
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
        /// 是否显示编码
        /// </summary>
        public bool CodeIsShow { get; set; } = false;
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
        /// 配方列表
        /// </summary>
        public List<FormulaInfoTemplateDto> FormulaInfos { get; set; } = [];

        /// <summary>
        /// 配方列表
        /// </summary>
        public string FormulasText { get { return string.Join(",", FormulaInfos.Select(s => s.FormulaType.GetDescriptionOrValue()).ToList()); } }

        /// <summary>
        /// 历史版本
        /// </summary>
        public string VersionText { get { return BeverageTemplateVersions != null ? string.Format($"第{BeverageTemplateVersions.Where(w => w.VersionType == BeverageVersionTypeEnum.Edit || w.VersionType == BeverageVersionTypeEnum.Insert).Count() + 1}版") : "无"; } }
        /// <summary>
        /// 历史记录
        /// </summary>
        public List<BeverageTemplateVersion> BeverageTemplateVersions { get; set; } = [];
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
        /// 售卖信息
        /// </summary>
        public string SellStradgy { get; set; }
    }
}
