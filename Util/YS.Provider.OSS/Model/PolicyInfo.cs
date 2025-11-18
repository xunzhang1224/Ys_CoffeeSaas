namespace YS.Provider.OSS.Model
{
    /// <summary>
    /// PolicyInfo
    /// </summary>
    public class PolicyInfo
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Statement
        /// </summary>
        public List<StatementItem> Statement { get; set; }
    }
}
