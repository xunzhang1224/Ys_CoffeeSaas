namespace YS.Provider.OSS.Model
{
    using YS.Provider.OSS.Enum;

    /// <summary>
    /// PresignedUrlCache
    /// </summary>
    class PresignedUrlCache
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// BucketName
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public PresignedObjectTypeEnum Type { get; set; }
    }
}
