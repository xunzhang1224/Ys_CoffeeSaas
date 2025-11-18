namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 下行指令 10025 的数据模型。
    /// 用于向IoT设备发送硬件配置信息，包括柜、门、层、托盘、传感器等组件定义。
    /// </summary>
    public class Downlink10025
    {
        /// <summary>
        /// 获取或设置机器编号（售货机唯一标识）。
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// 获取或设置设备类型标识符。
        /// 用于区分不同种类的IoT设备。
        /// </summary>
        public string MType { get; set; }

        /// <summary>
        /// 获取或设置机器型号（Model Version）。
        /// 用于指定设备的具体硬件版本。
        /// </summary>
        public string MV { get; set; }

        /// <summary>
        /// 获取或设置“柜”列表，表示设备中的柜体单元。
        /// </summary>
        public List<IdAndInclude> Counters { get; set; }

        /// <summary>
        /// 获取或设置“门”列表，表示设备中各门的状态与配置。
        /// </summary>
        public List<IdAndInclude> Doors { get; set; }

        /// <summary>
        /// 获取或设置“层”列表，表示每个柜中的分层结构。
        /// </summary>
        public List<IdAndInclude> Layers { get; set; }

        /// <summary>
        /// 获取或设置“托盘”列表，表示可出货的物理位置。
        /// </summary>
        public List<IdAndInclude> Trays { get; set; }

        /// <summary>
        /// 获取或设置“传感器”列表，记录每个传感器ID及其模式。
        /// </summary>
        public List<IdAndModel> Snos { get; set; }

        /// <summary>
        /// 获取或设置“门与传感器”的关联关系列表。
        /// 用于绑定门控与对应传感器。
        /// </summary>
        public List<IdAndGSon> DoorAndSnos { get; set; }

        /// <summary>
        /// 获取或设置额外配置信息（JSON格式字符串），不同机型支持不同的扩展字段。
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 获取或设置硬件启用配置字符串。
        /// 值按位顺序表示硬件是否启用，例如 "01" 表示 LED 无，主摄像头有。
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 表示带有 ID 和是否包含标志的对象。
        /// 用于柜、门、层、托盘等结构。
        /// </summary>
        public class IdAndInclude
        {
            /// <summary>
            /// 获取或设置对象的唯一标识。
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 获取或设置是否包含该对象（"0"/"1" 或布尔值字符串）。
            /// </summary>
            public string Include { get; set; }
        }

        /// <summary>
        /// 表示带有 ID 和模式描述的对象。
        /// 用于传感器配置。
        /// </summary>
        public class IdAndModel
        {
            /// <summary>
            /// 获取或设置对象的唯一标识。
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 获取或设置对象的模式/类型描述。
            /// </summary>
            public string Mode { get; set; }
        }

        /// <summary>
        /// 表示门与多个传感器的映射关系。
        /// </summary>
        public class IdAndGSon
        {
            /// <summary>
            /// 获取或设置门的唯一标识。
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 获取或设置该门所关联的传感器ID集合（以逗号分隔的字符串）。
            /// </summary>
            public string GSnos { get; set; }
        }
    }
}