namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment
{
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 文件中心聚合根
    /// </summary>
    public class FileCenter : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public DocmentTypeEnum FileCenterType { get; private set; }

        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum SysMenuType { get; private set; }

        /// <summary>
        /// 文件URL
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 是否成功上传oss
        /// </summary>
        public FileStateEnum FileState { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? FaildMessage { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected FileCenter() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileCenterType">文件类型</param>
        /// <param name="url">文件URL</param>
        /// <param name="fileState">状态</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="faildMessage">描述</param>
        public FileCenter(string fileName,string code, DocmentTypeEnum fileCenterType, string url, FileStateEnum fileState, long tenantId, SysMenuTypeEnum sysMenuType, string? faildMessage = null)
        {
            FileName = fileName;
            FileCenterType = fileCenterType;
            Code = code;
            Url = url;
            FileState = fileState;
            FaildMessage = faildMessage;
            EnterpriseinfoId = tenantId;
            SysMenuType = sysMenuType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileCenterType">文件类型</param>
        /// <param name="url">文件URL</param>
        /// <param name="fileState">状态</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="faildMessage">描述</param>
        public FileCenter(string fileName, string code, DocmentTypeEnum fileCenterType, long tenantId, SysMenuTypeEnum sysMenuType)
        {
            Code = code;
            FileName = fileName;
            FileCenterType = fileCenterType;
            EnterpriseinfoId = tenantId;
            SysMenuType = sysMenuType;
            Url = " ";
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="fileState"></param>
        /// <param name="faildMessage"></param>
        /// <param name="url"></param>
        public void Update(FileStateEnum fileState, string? faildMessage = null, string? url = null)
        {
            FileState = fileState;
            if (fileState == FileStateEnum.Success)
            {
                Url = url;
            }
            else if (fileState == FileStateEnum.Fail)
            {
                FaildMessage = faildMessage;
            }
        }
    }
}
