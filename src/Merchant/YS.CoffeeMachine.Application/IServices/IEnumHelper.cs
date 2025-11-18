namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 枚举帮助接口
    /// </summary>
    public interface IEnumHelper
    {
        /// <summary>
        /// 获取所有枚举信息
        /// </summary>
        /// <returns></returns>
        Task<List<EnumInfo>> GetAllEnumInfos();
    }

    /// <summary>
    /// EnumInfo
    /// </summary>
    public class EnumInfo
    {
        /// <summary>
        /// EnumTypeName
        /// </summary>
        public string EnumTypeName { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public List<EnumValue> Values { get; set; }

        /// <summary>
        /// EnumInfo
        /// </summary>
        public EnumInfo() { Values = new List<EnumValue>(); }
    }

    /// <summary>
    /// EnumValue
    /// </summary>
    public class EnumValue
    {
        /// <summary>
        /// 枚举值的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 枚举值的值
        /// </summary>
        public long Value { get; set; }
        /// <summary>
        /// 枚举值的 Description 特性值
        /// </summary>
        public string Description { get; set; }
    }
}