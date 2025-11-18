namespace YS.CoffeeMachine.Application.IServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 枚举帮助器接口，用于获取系统中所有枚举类型及其值信息
    /// </summary>
    public interface IEnumHelper
    {
        /// <summary>
        /// 获取系统中所有枚举类型的详细信息（包含名称、值、描述）
        /// </summary>
        /// <returns>枚举信息列表</returns>
        Task<List<EnumInfo>> GetAllEnumInfos();
    }

    /// <summary>
    /// 枚举类型信息类，表示一个枚举类型的基本信息
    /// </summary>
    public class EnumInfo
    {
        /// <summary>
        /// 枚举类型的全名（包括命名空间）
        /// </summary>
        public string EnumTypeName { get; set; }

        /// <summary>
        /// 当前枚举类型的所有枚举值集合
        /// </summary>
        public List<EnumValue> Values { get; set; }

        /// <summary>
        /// 默认构造函数，初始化枚举值列表
        /// </summary>
        public EnumInfo()
        {
            Values = new List<EnumValue>();
        }
    }

    /// <summary>
    /// 枚举值信息类，表示某个枚举类型中的具体值项
    /// </summary>
    public class EnumValue
    {
        /// <summary>
        /// 枚举值的名称（字段名）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 枚举值的整数值
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// 枚举值的 Description 特性描述内容（用于前端展示或说明）
        /// </summary>
        public string Description { get; set; }
    }
}