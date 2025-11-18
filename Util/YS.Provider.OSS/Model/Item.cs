namespace YS.Provider.OSS.Model
{
   /// <summary>
   /// item
   /// </summary>
    public class Item
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// LastModified
        /// </summary>
        public string LastModified { get; internal set; }

        /// <summary>
        /// ETag
        /// </summary>
        public string ETag { get; internal set; }

        /// <summary>
        /// Size
        /// </summary>
        public ulong Size { get; internal set; }

        /// <summary>
        /// IsDir
        /// </summary>
        public bool IsDir { get; internal set; }

        /// <summary>
        /// BucketName
        /// </summary>
        public string BucketName { get; internal set; }

        /// <summary>
        /// VersionId
        /// </summary>
        public string VersionId { get; set; }

        /// <summary>
        /// LastModifiedDateTime
        /// </summary>
        public DateTime? LastModifiedDateTime { get; internal set; }
    }
}
