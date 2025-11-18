namespace YS.Provider.OSS.Model
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// StatementItem
    /// </summary>
    public class StatementItem
    {
        /// <summary>
        /// Effect
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Principal
        /// </summary>
        public Principal Principal { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public List<string> Action { get; set; }

        /// <summary>
        /// Resource
        /// </summary>
        public List<string> Resource { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [JsonIgnore]
        public bool IsDelete { get; set; } = false;
    }
}
