namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment
{

    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 文件中心聚合根
    /// </summary>
    public class FileMBCenter : EnterpriseBaseEntity, IAggregateRoot
    {

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public DocmentTypeEnum FileCenterType { get; private set; }

        /// <summary>
        /// 文件URL
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccessed { get; private set; }

        /// <summary>
        /// 失败信息
        /// </summary>
        public string? FaildMessage { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected FileMBCenter() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileCenterType">文件类型</param>
        /// <param name="url">文件URL</param>
        /// <param name="isSuccessed">是否成功</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="faildMessage">失败信息</param>
        public FileMBCenter(string fileName, DocmentTypeEnum fileCenterType, string url, bool isSuccessed, long tenantId, string? faildMessage = null)
        {
            FileName = fileName;
            FileCenterType = fileCenterType;
            Url = url;
            IsSuccessed = isSuccessed;
            FaildMessage = faildMessage;
            EnterpriseinfoId = tenantId;
        }
    }
}
