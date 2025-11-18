namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备软件版本的实体类。
    /// 用于存储特定程序版本的详细信息，包括标题、描述、版本号、发布日期等元数据。
    /// </summary>
    public class DeviceSoftwareVersionInfo : BaseEntity
    {
        /// <summary>
        /// 获取或设置程序的标题，用于展示给用户或日志记录。
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 获取或设置程序的描述信息，通常包含更新内容或功能说明。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 获取或设置程序名称（如包名），用于系统内识别不同程序。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置程序类型，表示该软件所属类别：
        /// 1: 安卓应用程序；2: 单片机程序；3: 安卓固件程序；4: UNIX程序。
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// 获取或设置可读版本名称（例如："v1.0.0"）。
        /// </summary>
        public string VersionName { get; private set; }

        /// <summary>
        /// 获取或设置程序的具体版本编号（例如："1.0.0.1"）。
        /// </summary>
        public string VersionNo { get; private set; }

        /// <summary>
        /// 获取或设置版本迭代码，用于比较版本顺序和升级逻辑。
        /// 可为空。
        /// </summary>
        public int? VersionCode { get; private set; }

        /// <summary>
        /// 获取或设置程序版本的发布时间。
        /// 可为空。
        /// </summary>
        public DateTime? PushDate { get; private set; }

        /// <summary>
        /// 获取或设置程序下载地址或访问路径。
        /// 可为空。
        /// </summary>
        public string? Url { get; private set; }

        /// <summary>
        /// 获取或设置版本类型：
        /// 1: 公测版；2: 内测版；3: 稳定版。
        /// </summary>
        public int VersionType { get; private set; }

        /// <summary>
        /// 获取或设置适用的设备型号 ID 集合，多个值以逗号分隔。
        /// 用于控制该版本软件可部署的设备型号范围。
        /// </summary>
        public string DeviceModelIds { get; private set; }
    }
}