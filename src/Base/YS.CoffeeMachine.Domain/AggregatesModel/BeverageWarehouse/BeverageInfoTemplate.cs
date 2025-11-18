namespace YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse
{
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 饮料信息模板
    /// </summary>
    public class BeverageInfoTemplate : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<long>? CategoryIds { get; private set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public long? EnterpriseInfoId { get; private set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 饮品图标
        /// </summary>
        public string BeverageIcon { get; private set; }

        /// <summary>
        /// 编码（商品SKU），同设备内唯一
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 是否显示编码
        /// </summary>
        public bool CodeIsShow { get; private set; }

        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum Temperature { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }

        /// <summary>
        /// 配方列表
        /// </summary>
        public List<FormulaInfoTemplate> FormulaInfoTemplates { get; private set; } = [];

        /// <summary>
        /// 历史记录
        /// </summary>
        public List<BeverageTemplateVersion> BeverageTemplateVersions { get; private set; } = [];

        /// <summary>
        /// 出品预测
        /// </summary>
        public string ProductionForecast { get; private set; }
        /// <summary>
        /// 出杯量预测(ml)
        /// </summary>
        public double? ForecastQuantity { get; private set; }
        /// <summary>
        /// 是否显示状态
        /// </summary>
        public bool DisplayStatus { get; private set; }

        /// <summary>
        /// 售卖时间
        /// UplinkEntity5213.Response.SellStradgyInfo 结构
        /// </summary>
        public string? SellStradgy { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BeverageInfoTemplate() { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="enterpriseInfoId"></param>
        /// <param name="deviceModelId"></param>
        /// <param name="name"></param>
        /// <param name="beverageIcon"></param>
        /// <param name="temperature"></param>
        /// <param name="remarks"></param>
        /// <param name="productionForecast"></param>
        /// <param name="forecastQuantity"></param>
        /// <param name="displayStatus"></param>
        public BeverageInfoTemplate(long enterpriseInfoId, long? deviceModelId, string name, string beverageIcon, TemperatureEnum temperature, string remarks,
            string productionForecast, double? forecastQuantity, bool displayStatus, string sellStradgy = null, long? id = null, List<long>? categoryIds = null)
        {
            //EnterpriseinfoId = enterpriseInfoId;
            EnterpriseInfoId = enterpriseInfoId;
            DeviceModelId = deviceModelId;
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
            SellStradgy = sellStradgy;
            if (id != null)
            {
                Id = id ?? 0;
            }
            CategoryIds = categoryIds;
        }

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="formulaInfos"></param>
        public void AddRangeFormulaInfos(List<FormulaInfoTemplate> formulaInfoTemplates)
        {
            if (FormulaInfoTemplates != null)
                FormulaInfoTemplates.Clear();
            FormulaInfoTemplates = formulaInfoTemplates;
        }

        /// <summary>
        /// 添加编码
        /// </summary>
        /// <param name="code"></param>
        public void AddCode(string code)
        {
            Code = Id + code;
        }

        /// <summary>
        /// 添加编码（无Id）
        /// </summary>
        /// <param name="code"></param>
        public void AddCodeNotId(string code)
        {
            Code = code;
        }

        /// <summary>
        /// 是否显示编码
        /// </summary>
        /// <param name="codeIsShow"></param>
        public void UpdateCodeIsShow(bool codeIsShow)
        {
            CodeIsShow = codeIsShow;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="name"></param>
        /// <param name="beverageIcon"></param>
        /// <param name="temperature"></param>
        /// <param name="remarks"></param>
        /// <param name="productionForecast"></param>
        /// <param name="forecastQuantity"></param>
        /// <param name="displayStatus"></param>
        public void Update(string name, string beverageIcon, TemperatureEnum temperature, string remarks, string productionForecast, double forecastQuantity, bool displayStatus, string sellStradgy = null, List<long>? categoryIds = null)
        {
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
            SellStradgy = sellStradgy;
            CategoryIds = categoryIds;
        }

        /// <summary>
        /// 添加历史版本
        /// </summary>
        /// <param name="versionType"></param>
        /// <param name="beverageInfoDataString"></param>
        public void InsertBeverageTemplateVersions(BeverageVersionTypeEnum versionType, string beverageInfoDataString, long? beverageTemplateVsersionId = null)
        {
            BeverageTemplateVersions.Add(new BeverageTemplateVersion(Id, versionType, beverageInfoDataString, beverageTemplateVsersionId));
        }
    }
}
