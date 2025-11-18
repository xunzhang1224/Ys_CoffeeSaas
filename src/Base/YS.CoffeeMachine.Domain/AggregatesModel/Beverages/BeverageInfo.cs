namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 饮品信息
    /// </summary>
    public class BeverageInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; protected set; }

        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<long>? CategoryIds { get; private set; }

        ///// <summary>
        ///// 设备Id
        ///// </summary>
        //public long DeviceInfoId { get; private set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

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
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 是否显示编码
        /// </summary>
        public bool CodeIsShow { get; private set; }

        ///// <summary>
        ///// 自增长编码
        ///// </summary>
        //[Required]
        //public long AutoCode { get; private set; }

        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum Temperature { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; private set; }

        /// <summary>
        /// 配方列表
        /// </summary>
        public List<FormulaInfo> FormulaInfos { get; private set; } = [];

        /// <summary>
        /// 历史记录
        /// </summary>
        public List<BeverageVersion> BeverageVersions { get; private set; } = [];

        /// <summary>
        /// 出品预测
        /// </summary>
        public string? ProductionForecast { get; private set; }

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
        protected BeverageInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceInfoId"></param>
        /// <param name="name"></param>
        /// <param name="beverageIcon"></param>
        /// <param name="temperature"></param>
        /// <param name="remarks"></param>
        /// <param name="productionForecast"></param>
        /// <param name="forecastQuantity"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isDefault"></param>
        /// <param name="id"></param>
        /// <param name="sellStradgy"></param>
        /// <param name="price"></param>
        /// <param name="discountedPrice"></param>
        /// <param name="code"></param>
        /// <param name="productCategoryIds"></param>
        public BeverageInfo(long deviceInfoId, string name, string beverageIcon, TemperatureEnum temperature, string remarks, string productionForecast,
            double? forecastQuantity, bool displayStatus, bool isDefault, long? id = null, string sellStradgy = null, decimal? price = null, decimal? discountedPrice = null, string code = null, List<long>? productCategoryIds = null)
        {
            DeviceId = deviceInfoId;
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
            IsDefault = isDefault;
            CategoryIds = productCategoryIds;
            if (id != null && id != 0)
            {
                Id = id ?? 0;
                Code = Id.ToString();
            }
            SellStradgy = sellStradgy;
            Price = price;
            DiscountedPrice = discountedPrice;
            if (!string.IsNullOrWhiteSpace(code))
                Code = code;
            //if (Code != Id.ToString())
            //    CodeIsShow = true;
            //else
            //    CodeIsShow = false;
            //if (autoCode == null)
            //    AutoCode = YitIdHelper.NextId();
            //else
            //    AutoCode = autoCode ?? 0;
        }

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(long id) { Id = id; }

        /// <summary>
        /// AddCode
        /// </summary>
        /// <param name="code"></param>
        public void AddCode(string code)
        {
            Code = Id + code;
        }

        /// <summary>
        /// AddCodeNotId
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
        public void AddRangeFormulaInfos(List<FormulaInfo> formulaInfos)
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
        public void AddFormulaInfo(FormulaInfo formulaInfo)
        {
            FormulaInfos.Add(formulaInfo);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="name"></param>
        /// <param name="beverageIcon"></param>
        /// <param name="temperature"></param>
        /// <param name="productionForecast"></param>
        /// <param name="price"></param>
        /// <param name="discountedPrice"></param>
        /// <param name="forecastQuantity"></param>
        /// <param name="productCategoryIds"></param>
        public void Update(string name, string beverageIcon, TemperatureEnum temperature, int? productionForecast, decimal? price, decimal? discountedPrice, double? forecastQuantity = null, List<long>? productCategoryIds = null)
        {
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            ProductionForecast = productionForecast.ToString();
            Price = price;
            DiscountedPrice = discountedPrice;
            ForecastQuantity = forecastQuantity;
            CategoryIds = productCategoryIds;
        }

        /// <summary>
        /// 设置售卖时间
        /// </summary>
        /// <param name="sellStradgy"></param>
        public void SetSellStradgy(string sellStradgy)
        {
            SellStradgy = sellStradgy;
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
        /// <param name="sellStradgy"></param>
        /// <param name="productCategoryIds"></param>
        public void Update(string name, string beverageIcon, TemperatureEnum temperature, string remarks, string productionForecast, double? forecastQuantity, bool displayStatus, string sellStradgy = null, List<long>? productCategoryIds = null)
        {
            Name = name;
            BeverageIcon = beverageIcon;
            Temperature = temperature;
            Remarks = remarks;
            ProductionForecast = productionForecast;
            if (forecastQuantity != null)
                ForecastQuantity = forecastQuantity;
            DisplayStatus = displayStatus;
            SellStradgy = sellStradgy;
            CategoryIds = productCategoryIds;
        }
        //public void BindParentId(long? parentId)
        //{
        //    ParentId = parentId;
        //}

        /// <summary>
        /// 添加历史版本
        /// </summary>
        /// <param name="versionType"></param>
        /// <param name="beverageInfoDataString"></param>
        public void InsertBeverageVersions(BeverageVersionTypeEnum versionType, string beverageInfoDataString, long? beverVersionId = null)
        {
            BeverageVersions.Add(new BeverageVersion(Id, versionType, beverageInfoDataString, beverVersionId));
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

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="sort"></param>
        public void UpdateSort(int sort)
        {
            Sort = sort;
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
        /// 更新价格
        /// </summary>
        /// <param name="price"></param>
        /// <param name="discountedPrice"></param>
        public void UpdatePrice(decimal? price, decimal? discountedPrice)
        {
            Price = price;
            DiscountedPrice = discountedPrice;
        }

        /// <summary>
        /// 更新出品预测量
        /// </summary>
        /// <param name="forecastQuantity"></param>
        public void UpdateForecastQuantity(double? forecastQuantity)
        {
            ForecastQuantity = forecastQuantity;
        }

        /// <summary>
        /// 设置分类
        /// </summary>
        /// <param name="categoryIds"></param>
        public void SetCategoryId(List<long>? categoryIds)
        {
            CategoryIds = categoryIds;
        }

        /// <summary>
        /// 饮品删除指定分类
        /// </summary>
        /// <param name="categoryId"></param>
        public void DelCategoryId(long categoryId)
        {
            if (CategoryIds! != null && CategoryIds.Contains(categoryId))
                CategoryIds.Remove(categoryId);
        }
    }
}