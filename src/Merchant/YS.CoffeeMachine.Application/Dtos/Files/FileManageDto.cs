using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.CoffeeMachine.Application.Dtos.Files
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public class FileManageDto
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 资源用途(字典) FileDrikType:饮品  FileAdType：广告
        /// </summary>
        public string ResorceUsage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 引用次数
        /// </summary>
        public int QuoteCount { get; set; }
    }
}
