namespace YS.CoffeeMachine.Provider.Dto.Docment
{
    /// <summary>
    /// 文件上传任务
    /// </summary>
    public class FileExcelUploadTask
    {
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] DataItems { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件中心类型
        /// </summary>
        public int FileCenterType { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public long TenantId { get; set; } = 0;

        /// <summary>
        /// 标识唯一
        /// </summary>
        public string Code { get; set; }
    }
}
