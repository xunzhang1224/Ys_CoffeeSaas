namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备软件信息的聚合根实体。
    /// 用于记录与设备相关的各类程序版本信息，例如安卓应用、单片机程序等。
    /// </summary>
    public class DeviceSoftwareInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的平台设备唯一标识符。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 获取或设置程序类型，表示该软件所属类别：
        /// 1: 安卓应用程序；2: 单片机程序；3: 安卓固件程序。
        /// </summary>
        [Required]
        public int ProgramType { get; private set; } = 0;

        /// <summary>
        /// 获取或设置程序标题，用于展示给用户或日志记录。
        /// </summary>
        [Required]
        public string Title { get; private set; }

        /// <summary>
        /// 获取或设置程序名称/标识符，可用于系统内识别不同程序。
        /// 可为空。
        /// </summary>
        [Required]
        public string? Name { get; private set; }

        /// <summary>
        /// 获取或设置程序的可读版本名称（如 "v1.0.0"）。
        /// 可为空。
        /// </summary>
        public string? VersionName { get; private set; }

        /// <summary>
        /// 获取或设置关联的版本表唯一标识符。
        /// 可为空。
        /// </summary>
        public long? VerId { get; private set; }

        /// <summary>
        /// 获取或设置程序的具体版本号字符串。
        /// 可为空。
        /// </summary>
        public string? Version { get; private set; }

        /// <summary>
        /// 获取或设置附加信息，通常为 JSON 格式的数据，用于扩展存储额外配置。
        /// 可为空。
        /// </summary>
        public string? Extra { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceSoftwareInfo() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceSoftwareInfo 实例。
        /// </summary>
        /// <param name="deviceBaseId">关联的平台设备唯一标识。</param>
        /// <param name="programType">程序类型（1:安卓应用，2:单片机程序，3:安卓固件）。</param>
        /// <param name="title">程序标题。</param>
        /// <param name="name">程序名称/标识。</param>
        /// <param name="versionName">可读版本名称。</param>
        /// <param name="verId">关联的版本表ID。</param>
        /// <param name="version">具体版本号。</param>
        /// <param name="extra">附加信息（JSON格式字符串）。</param>
        public DeviceSoftwareInfo(long deviceBaseId, int programType, string title, string name, string versionName, long? verId, string version, string extra)
        {
            DeviceBaseId = deviceBaseId;
            ProgramType = programType;
            Title = title;
            Name = name;
            VersionName = versionName;
            VerId = verId;
            Version = version;
            Extra = extra;
        }

        /// <summary>
        /// 更新当前实例的版本相关信息。
        /// 同时更新最后修改时间。
        /// </summary>
        /// <param name="versionName">新的可读版本名称。</param>
        /// <param name="verId">新的版本表ID。</param>
        /// <param name="version">新的具体版本号。</param>
        /// <param name="extra">新的附加信息（JSON格式字符串）。</param>
        public void Update(string versionName, long? verId, string version, string extra)
        {
            VersionName = versionName;
            VerId = verId;
            Version = version;
            Extra = extra;
            LastModifyTime = DateTime.UtcNow;
        }
    }
}