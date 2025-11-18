namespace YS.CoffeeMachine.Domain.AggregatesModel.Settings
{
    using System;
    using System.Collections.Generic;
    using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备设置信息的聚合根实体。
    /// 包含设备界面、清洗、个性化、安全等多方面的配置项。
    /// 注意：设备离线状态下无法修改此设置。
    /// </summary>
    public class SettingInfo : BaseEntity, IAggregateRoot
    {
        #region 基础信息

        /// <summary>
        /// 获取或设置关联设备的唯一标识符。
        /// </summary>
        public long DeviceId { get; protected set; }

        /// <summary>
        /// 获取与此设置绑定的设备信息实体对象。
        /// 用于导航至设备详情。
        /// </summary>
        public DeviceInfo DeviceInfo { get; private set; }

        #endregion

        #region 界面设置

        /// <summary>
        /// 获取或设置是否在界面上显示设备编号。
        /// </summary>
        public bool IsShowEquipmentNumber { get; private set; }

        /// <summary>
        /// 获取或设置界面风格的唯一标识符。
        /// 参考 InterfaceStyles 定义。
        /// </summary>
        public long InterfaceStylesId { get; private set; }

        /// <summary>
        /// 获取与此设置绑定的界面风格实体对象。
        /// </summary>
        public InterfaceStyles InterfaceStyles { get; set; }

        #endregion

        #region 冲洗设置

        /// <summary>
        /// 获取或设置冲洗类型（例如：自动、定时、手动）。
        /// 参考 WashEnum 枚举定义。
        /// </summary>
        public WashEnum WashType { get; private set; }

        /// <summary>
        /// 获取或设置定时冲洗时间（格式为 HH:mm）。
        /// </summary>
        public string RegularWashTime { get; private set; }

        /// <summary>
        /// 获取或设置清洗预警阈值（单位为杯数），达到该次数后触发提醒。
        /// 可为空。
        /// </summary>
        public int? WashWarning { get; private set; }

        #endregion

        #region 个性化设置

        /// <summary>
        /// 获取或设置售后联系电话号码。
        /// </summary>
        public string AfterSalesPhone { get; private set; }

        /// <summary>
        /// 获取或设置料盒集合（最多不超过6个）。
        /// 用于管理咖啡机中各类物料容器的信息与预警规则。
        /// </summary>
        public List<MaterialBox> MaterialBoxs { get; private set; }

        /// <summary>
        /// 获取或设置期望安装更新的时间（格式为 HH:mm）。
        /// </summary>
        public string ExpectedUpdateTime { get; private set; }

        /// <summary>
        /// 获取或设置屏幕亮度等级（0~100）。
        /// </summary>
        public int ScreenBrightness { get; private set; }

        /// <summary>
        /// 获取或设置设备声音等级（0~100）。
        /// </summary>
        public int DeviceSound { get; private set; }

        #endregion

        #region 安全与隐私

        /// <summary>
        /// 获取或设置管理员密码（加密存储）。
        /// </summary>
        public string AdministratorPwd { get; private set; }

        /// <summary>
        /// 获取或设置补货员密码（加密存储）。
        /// </summary>
        public string ReplenishmentOfficerPwd { get; private set; }

        #endregion

        #region 定时开、关机配置

        /// <summary>
        /// 获取或设置定时开机时间（格式为 HH:mm）。
        /// </summary>
        public string StartTime { get; private set; }

        /// <summary>
        /// 获取或设置每周开机日集合（使用位掩码表示周一到周日）。
        /// </summary>
        public int StartWeek { get; private set; }

        /// <summary>
        /// 获取或设置定时关机时间（格式为 HH:mm）。
        /// </summary>
        public string EndTime { get; private set; }

        /// <summary>
        /// 获取或设置每周关机日集合（使用位掩码表示周一到周日）。
        /// </summary>
        public int EndWeek { get; private set; }

        #endregion

        #region 高级设置

        /// <summary>
        /// 获取或设置系统语言名称（例如：中文、English）。
        /// </summary>
        public string LanguageName { get; private set; }

        #endregion

        #region 货币信息配置

        /// <summary>
        /// 获取或设置当前币种编码（例如：CNY、USD）
        /// </summary>
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// 货币符号（如 ¥、$）
        /// </summary>
        public string CurrencySymbol { get; private set; }

        /// <summary>
        /// 货币名称（如 人民币、美元）
        /// </summary>
        public string CurrencyName { get; private set; }

        /// <summary>
        /// 货币符号位置（0= 前置、1=后置）
        /// </summary>
        public int CurrencyPosition { get; private set; }

        /// <summary>
        /// 货币小数位数（如 2）
        /// </summary>
        public int CurrencyDecimalDigits { get; private set; }
        #endregion

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected SettingInfo() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 SettingInfo 实例。
        /// </summary>
        /// <param name="deviceInfoId">关联设备ID。</param>
        /// <param name="isShowEquipmentNumber">是否显示设备编号。</param>
        /// <param name="interfaceStylesId">界面风格ID。</param>
        /// <param name="washType">冲洗类型。</param>
        /// <param name="regularWashTime">定时冲洗时间（HH:mm 格式）。</param>
        /// <param name="washWarning">清洗预警阈值（单位为杯数）。</param>
        /// <param name="afterSalesPhone">售后电话号码。</param>
        /// <param name="expectedUpdateTime">期望安装更新时间（HH:mm 格式）。</param>
        /// <param name="screenBrightness">屏幕亮度等级（0~100）。</param>
        /// <param name="deviceSound">设备音量等级（0~100）。</param>
        /// <param name="administratorPwd">管理员密码。</param>
        /// <param name="replenishmentOfficerPwd">补货员密码。</param>
        /// <param name="startTime">定时开机时间（HH:mm 格式）。</param>
        /// <param name="startWeek">开机对应的星期集合（位掩码）。</param>
        /// <param name="endTime">定时关机时间（HH:mm 格式）。</param>
        /// <param name="endWeek">关机对应的星期集合（位掩码）。</param>
        /// <param name="languageName">系统语言名称。</param>
        /// <param name="currencyCode">币种编码（如 CNY、USD）。</param>
        /// <param name="materialBoxs">料盒集合（最多不超过6个）。</param>
        public SettingInfo(
            long deviceInfoId,
            bool isShowEquipmentNumber,
            long interfaceStylesId,
            WashEnum washType,
            string? regularWashTime,
            int? washWarning,
            string afterSalesPhone,
            string expectedUpdateTime,
            int screenBrightness,
            int deviceSound,
            string administratorPwd,
            string replenishmentOfficerPwd,
            string startTime,
            int startWeek,
            string endTime,
            int endWeek,
            string languageName,
            string currencyCode,
            List<MaterialBox> materialBoxs)
        {
            DeviceId = deviceInfoId;
            IsShowEquipmentNumber = isShowEquipmentNumber;
            InterfaceStylesId = interfaceStylesId;
            WashType = washType;
            RegularWashTime = regularWashTime;
            WashWarning = washWarning;
            AfterSalesPhone = afterSalesPhone;
            ExpectedUpdateTime = expectedUpdateTime;
            ScreenBrightness = screenBrightness;
            DeviceSound = deviceSound;
            AdministratorPwd = administratorPwd;
            ReplenishmentOfficerPwd = replenishmentOfficerPwd;
            StartTime = startTime;
            StartWeek = startWeek;
            EndTime = endTime;
            EndWeek = endWeek;
            LanguageName = languageName;
            CurrencyCode = currencyCode;
            MaterialBoxs = new List<MaterialBox>();
            if (materialBoxs != null && materialBoxs.Count <= 6)
            {
                materialBoxs.ForEach(x =>
                {
                    MaterialBoxs.Add(new MaterialBox(Id, x.Name, x.Sort, x.IsActive));
                });
            }
        }

        /// <summary>
        /// 更新当前设置的基本字段信息。
        /// 不包含料盒集合。
        /// </summary>
        /// <param name="deviceInfoId">新的设备ID。</param>
        /// <param name="isShowEquipmentNumber">是否显示设备编号。</param>
        /// <param name="interfaceStylesId">新的界面风格ID。</param>
        /// <param name="washType">新的冲洗类型。</param>
        /// <param name="regularWashTime">新的定时冲洗时间。</param>
        /// <param name="washWarning">新的清洗预警阈值。</param>
        /// <param name="afterSalesPhone">新的售后电话。</param>
        /// <param name="expectedUpdateTime">新的期望更新时间。</param>
        /// <param name="screenBrightness">新的屏幕亮度。</param>
        /// <param name="deviceSound">新的设备音量。</param>
        /// <param name="administratorPwd">新的管理员密码。</param>
        /// <param name="replenishmentOfficerPwd">新的补货员密码。</param>
        /// <param name="startTime">新的定时开机时间。</param>
        /// <param name="startWeek">新的开机周集合。</param>
        /// <param name="endTime">新的定时关机时间。</param>
        /// <param name="endWeek">新的关机周集合。</param>
        /// <param name="languageName">新的系统语言。</param>
        /// <param name="currencyCode">新的币种编码。</param>
        public void Update(
            long deviceInfoId,
            bool isShowEquipmentNumber,
            long interfaceStylesId,
            WashEnum washType,
            string regularWashTime,
            int? washWarning,
            string afterSalesPhone,
            string expectedUpdateTime,
            int screenBrightness,
            int deviceSound,
            string administratorPwd,
            string replenishmentOfficerPwd,
            string startTime,
            int startWeek,
            string endTime,
            int endWeek,
            string languageName,
            string currencyCode)
        {
            DeviceId = deviceInfoId;
            IsShowEquipmentNumber = isShowEquipmentNumber;
            InterfaceStylesId = interfaceStylesId;
            WashType = washType;
            RegularWashTime = regularWashTime;
            WashWarning = washWarning;
            AfterSalesPhone = afterSalesPhone;
            ExpectedUpdateTime = expectedUpdateTime;
            ScreenBrightness = screenBrightness;
            DeviceSound = deviceSound;
            AdministratorPwd = administratorPwd;
            ReplenishmentOfficerPwd = replenishmentOfficerPwd;
            StartTime = startTime;
            StartWeek = startWeek;
            EndTime = endTime;
            EndWeek = endWeek;
            LanguageName = languageName;
            CurrencyCode = currencyCode;
        }

        /// <summary>
        /// 设置币种编码。
        /// </summary>
        /// <param name="code">新的币种编码。</param>
        public void SetCurrencyCode(string code)
        {
            CurrencyCode = code;
        }

        /// <summary>
        /// 设置币种配置信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="currencySymbol"></param>
        /// <param name="currencyName"></param>
        /// <param name="currencyPosition"></param>
        /// <param name="currencyDecimalDigits"></param>
        public void SetCurrencyInfo(string code, string currencySymbol, string currencyName, int currencyPosition, int currencyDecimalDigits)
        {
            CurrencyCode = code;
            CurrencySymbol = currencySymbol;
            CurrencyName = currencyName;
            CurrencyPosition = currencyPosition;
            CurrencyDecimalDigits = currencyDecimalDigits;
        }

        /// <summary>
        /// 设置界面风格ID。
        /// </summary>
        /// <param name="styleId">新的界面风格ID。</param>
        public void SetStyle(long styleId)
        {
            InterfaceStylesId = styleId;
        }

        /// <summary>
        /// 充值设置的基本信息
        /// </summary>
        public void ReSetSettingInfo()
        {
            IsShowEquipmentNumber = false;
            WashType = WashEnum.Automatic;
            RegularWashTime = string.Empty;
            WashWarning = null;
            AfterSalesPhone = string.Empty;
            ExpectedUpdateTime = string.Empty;
            ScreenBrightness = 50; // 默认值
            DeviceSound = 50; // 默认值
            AdministratorPwd = string.Empty;
            ReplenishmentOfficerPwd = string.Empty;
            StartTime = string.Empty;
            StartWeek = 0; // 默认值
            EndTime = string.Empty;
            EndWeek = 0; // 默认值
            LanguageName = string.Empty; // 默认语言
        }
    }
}