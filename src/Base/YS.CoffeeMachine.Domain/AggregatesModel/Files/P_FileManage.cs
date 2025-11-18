using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Files
{
    /// <summary>
    /// 文件管理(平台端)
    /// </summary>
    public class P_FileManage : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; private set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; private set; }

        /// <summary>
        /// 资源用途（读取字典）
        /// </summary>
        public string ResourceUsage { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected P_FileManage()
        {
        }

        /// <summary>
        /// 新增文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileType"></param>
        /// <param name="fileSize"></param>
        /// <param name="resourceUsage"></param>
        public P_FileManage(string fileName, string filePath, string fileType, string fileSize, string resourceUsage, long? id = null)
        {
            FileName = fileName;
            FilePath = filePath;
            FileType = fileType;
            FileSize = fileSize;
            ResourceUsage = resourceUsage;
            if (id != null)
            {
                Id = id ?? 0;
            }
        }

        /// <summary>
        /// 更新文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resourceUsage"></param>
        public void Update(string fileName, string resourceUsage)
        {
            FileName = fileName;
            ResourceUsage = resourceUsage;
        }
    }
}
