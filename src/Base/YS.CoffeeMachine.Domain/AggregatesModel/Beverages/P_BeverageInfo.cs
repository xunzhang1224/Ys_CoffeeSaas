namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 饮品信息
    /// </summary>
    public class P_BeverageInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 原价
        /// </summary>
        public decimal? Price { get; private set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal? DiscountedPrice { get; private set; }

        //public BeverageInfo Parent { get; private set; }
        //public long? ParentId { get; private set; }
        //public BeverageInfo Parent { get; private set; }

        /// <summary>
        /// 语言字典key
        /// </summary>

        public string LanguageKey { get; private set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public long DeviceModelId { get; private set; }

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
        public List<P_FormulaInfo> FormulaInfos { get; private set; } = [];

        /// <summary>
        /// 历史记录
        /// </summary>
        public List<P_BeverageVersion> BeverageVersions { get; private set; } = [];

        /// <summary>
        /// 出品预测
        /// </summary>
        public string ProductionForecast { get; private set; }

        /// <summary>
        /// 出杯量预测(ml)
        /// </summary>
        public double? ForecastQuantity { get; private set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// 是否显示状态
        /// </summary>
        public bool DisplayStatus { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; private set; }

        /// <summary>
        /// 售卖时间
        /// UplinkEntity5213.Response.SellStradgyInfo 结构
        /// </summary>
        public string? SellStradgy { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected P_BeverageInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceModelId"></param>
        /// <param name="name"></param>
        /// <param name="beverageIcon"></param>
        /// <param name="temperature"></param>
        /// <param name="remarks"></param>
        /// <param name="productionForecast"></param>
        /// <param name="forecastQuantity"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isDefault"></param>
        public P_BeverageInfo(long deviceModelId, string name, string beverageIcon, TemperatureEnum temperature, string remarks, string productionForecast
            , double forecastQuantity, bool displayStatus, bool isDefault, string languageKey)
        {
            DeviceModelId = deviceModelId;
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
            IsDefault = isDefault;
            LanguageKey = languageKey;
            SellStradgy = "{\"SellDateIndex\":\"0:1:2:3:4:5:6\",\"SellStartTime\":\"00:00\",\"SellEndTime\":\"23:59\"}";
        }

        /// <summary>
        /// 添加编码
        /// </summary>
        /// <param name="code"></param>
        public void AddCode(string code)
        {
            Code = /*Id + */code;
        }

        /// <summary>
        /// 添加编码（不包含id）
        /// </summary>
        /// <param name="code"></param>
        public void AddCodeNotId(string code)
        {
            Code = code;
        }

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="formulaInfos"></param>
        public void AddRangeFormulaInfos(List<P_FormulaInfo> formulaInfos)
        {
            if (FormulaInfos != null)
                FormulaInfos.Clear();
            FormulaInfos = formulaInfos;
        }

        /// <summary>
        /// 清空配方
        /// </summary>
        public void ClearFormulaInfos()
        {
            if (FormulaInfos != null)
                FormulaInfos.Clear();
        }

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="formulaInfo"></param>
        public void AddFormulaInfo(P_FormulaInfo formulaInfo)
        {
            FormulaInfos.Add(formulaInfo);
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
        public void Update(string name, string beverageIcon, TemperatureEnum temperature, string remarks, string productionForecast, double forecastQuantity, bool displayStatus)
        {
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
        }

        //public void BindParentId(long? parentId)
        //{
        //    ParentId = parentId;
        //}

        /// <summary>
        /// 添加历史版本
        /// </summary>
        /// <param name="versionType"></param>
        /// <param name="sort"></param>
        /// <param name="beverageInfoData"></param>
        public void InsertBeverageVersions(BeverageVersionTypeEnum versionType, string beverageInfoDataString)
        {
            BeverageVersions.Add(new P_BeverageVersion(Id, versionType, beverageInfoDataString));
        }

        /// <summary>
        /// 更新价格信息
        /// </summary>
        /// <param name="price"></param>
        /// <param name="discountedPrice"></param>
        public void UpdatePriceInfo(decimal? price, decimal? discountedPrice)
        {
            Price = price;
            DiscountedPrice = discountedPrice;
        }
    }
}
