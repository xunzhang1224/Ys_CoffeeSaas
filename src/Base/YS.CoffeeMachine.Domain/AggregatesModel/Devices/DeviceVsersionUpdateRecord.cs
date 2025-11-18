using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    /// <summary>
    /// 更新记录
    /// </summary>
    public class DeviceVsersionUpdateRecord : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 设备基础信息Id
        /// </summary>
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 管理Id
        /// </summary>
        public long DeviceVersionManageId { get; private set; }

        /// <summary>
        /// 升级类型 1推送（或是发推送消息）  2强制升级
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 程序类型(通过字典获取)
        /// </summary>
        public int? ProgramType { get; private set; }

        /// <summary>
        /// 版本类型(通过字典获取)
        /// </summary>
        public int? VersionType { get; private set; }

        /// <summary>
        /// 程序类型名称
        /// </summary>
        public string? ProgramTypeName { get; private set; }

        /// <summary>
        /// 推送状态
        /// </summary>
        public VersionPushStateEnum PushState { get; private set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DeviceVsersionUpdateRecord() { }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="deviceVersionManageId"></param>
        /// <param name="name"></param>
        /// <param name="programType"></param>
        /// <param name="versionType"></param>
        public DeviceVsersionUpdateRecord(long deviceBaseId, long deviceVersionManageId, string name, int? programType, int? versionType, int type, string programTypeName, long? id = null)
        {
            DeviceBaseId = deviceBaseId;
            DeviceVersionManageId = deviceVersionManageId;
            Name = name;
            ProgramType = programType;
            VersionType = versionType;
            Type = type;
            ProgramTypeName = programTypeName;
            PushState = VersionPushStateEnum.Pushed;
            if (id != null)
            {
                Id = id ?? 0;
            }
        }

        /// <summary>
        /// 设备回复
        /// </summary>
        /// <param name="pushState"></param>
        /// <param name="message"></param>
        public void Update(VersionPushStateEnum pushState, string? message)
        {
            PushState = pushState;
            Message = message;
        }
    }
}
