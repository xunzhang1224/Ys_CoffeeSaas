using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;

namespace YS.CoffeeMachine.Application.Dtos.SettringDtos
{
    /// <summary>
    /// 设置dto
    /// </summary>
    public class SettingInfoDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceInfoId { get; set; }

        /// <summary>
        /// 币种code
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo DeviceInfo { get; set; }
        #region 界面设置
        /// <summary>
        /// 是否显示设备编号
        /// </summary>
        public bool IsShowEquipmentNumber { get; set; }
        /// <summary>
        /// 界面风格Id
        /// </summary>
        public long InterfaceStylesId { get; set; }

        /// <summary>
        /// 风格
        /// </summary>
        public InterfaceStyles InterfaceStyles { get; set; }
        #endregion

        #region 冲洗设置
        /// <summary>
        /// 类型
        /// </summary>
        public WashEnum WashType { get; set; }
        /// <summary>
        /// 定时冲洗时间（格式：01:00）
        /// </summary>
        public string RegularWashTime { get; set; }
        /// <summary>
        /// 清洗预警（300）杯
        /// </summary>
        public int? WashWarning { get; set; }
        #endregion

        #region 个性化设置
        /// <summary>
        /// 售后电话
        /// </summary>
        public string AfterSalesPhone { get; set; }
        /// <summary>
        /// 料盒集合（不能为空，最多不超过6个）
        /// </summary>
        public List<MaterialBox> MaterialBoxs { get; set; }
        /// <summary>
        /// 期望安装更新时间（格式：01:00）
        /// </summary>
        public string ExpectedUpdateTime { get; set; }
        /// <summary>
        /// 屏幕亮度
        /// </summary>
        public int ScreenBrightness { get; set; }
        /// <summary>
        /// 设备声音（0-100）
        /// </summary>
        public int DeviceSound { get; set; }
        #endregion

        #region 安全与隐私
        /// <summary>
        /// 管理员密码
        /// </summary>
        public string AdministratorPwd { get; set; }
        /// <summary>
        /// 补货员密码
        /// </summary>
        public string ReplenishmentOfficerPwd { get; set; }

        #endregion

        #region 定时开、关机配置
        /// <summary>
        /// 定时开机
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 开机时间，周集合
        /// </summary>
        public int StartWeek { get; set; }
        /// <summary>
        /// 定时关机
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 关机时间，周集合
        /// </summary>
        public int EndWeek { get; set; }
        #endregion

        #region 高级设置
        /// <summary>
        /// 语言
        /// </summary>
        public string LanguageName { get; set; }
        #endregion
    }
}
