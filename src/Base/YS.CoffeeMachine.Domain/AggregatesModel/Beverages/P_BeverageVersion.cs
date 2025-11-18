namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 版本
    /// </summary>
    public class P_BeverageVersion : BaseEntity
    {
        /// <summary>
        /// 饮品Id
        /// </summary>
        public long BeverageInfoId { get; private set; }

        /// <summary>
        /// 版本类型
        /// </summary>
        public BeverageVersionTypeEnum VersionType { get; private set; }

        private string _extendedData;

        /// <summary>
        /// 忽略
        /// </summary>
        [NotMapped]
        public JsonObject BeverageInfoData
        {
            get
            {
                // 将 JSON 字符串反序列化为 JObject
                return JsonSerializer.Deserialize<JsonObject>(string.IsNullOrWhiteSpace(_extendedData) ? null : _extendedData);
            }
            set
            {
                // 将 JObject 序列化为字符串
                _extendedData = value.ToString();
            }
        }

        /// <summary>
        /// 饮品历史信息
        /// </summary>
        public string BeverageInfoDataString
        {
            get => _extendedData;
            set => _extendedData = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected P_BeverageVersion() { }

        /// <summary>
        /// 新增历史版本
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <param name="versionType"></param>
        /// <param name="beverageInfoData"></param>
        public P_BeverageVersion(long beverageInfoId, BeverageVersionTypeEnum versionType, string beverageInfoDataString)
        {
            BeverageInfoId = beverageInfoId;
            VersionType = versionType;
            BeverageInfoDataString = beverageInfoDataString;
        }
    }
}
