using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Dtos.Files
{
    /// <summary>
    /// 文件管理输入参数
    /// </summary>
    public class FileManageInput
    {
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
    }

    /// <summary>
    /// 文件管理查询参数
    /// </summary>
    public class GetFileManageInput : QueryRequest
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 资源用途(字典) FileDrikType:饮品  FileAdType：广告
        /// </summary>
        public string ResorceUsage { get; set; }

        /// <summary>
        /// 时间范围
        /// </summary>
        public List<DateTime> DateTimeRange { get; set; }
    }
}
