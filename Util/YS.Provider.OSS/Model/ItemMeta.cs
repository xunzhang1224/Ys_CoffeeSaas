namespace YS.Provider.OSS.Model
{
    /// <summary>
    /// ItemMeta
    /// </summary>
    public class ItemMeta
    {
        /// <summary>
        /// ObjectName
        /// </summary>
        public string ObjectName { get; internal set; }

        /// <summary>
        /// Size
        /// </summary>
        public long Size { get; internal set; }

        /// <summary>
        /// LastModified
        /// </summary>
        public DateTime LastModified { get; internal set; }

        /// <summary>
        /// ETag
        /// </summary>
        public string ETag { get; internal set; }

        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get; internal set; }

        /// <summary>
        /// IsEnableHttps
        /// </summary>
        public bool IsEnableHttps { get; set; }

        /// <summary>
        /// MetaData
        /// </summary>
        public Dictionary<string, string> MetaData { get; internal set; }

    }
}
