using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Files
{
    /// <summary>
    /// 文件关系
    /// </summary>
    public class FileRelation : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 文件中心Id
        /// </summary>
        public long FileId { get; protected set; }

        /// <summary>
        /// 目标Id
        /// </summary>
        public long TargetId { get; protected set; }

        /// <summary>
        /// 目标类型(方便定位具体哪个表使用的)
        /// 1:Beverage
        /// 2:Beverage
        /// 3:FileManage
        /// </summary>
        public int TargetType { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected FileRelation()
        {
        }

        /// <summary>
        /// 创建文件关联
        /// </summary>
        public FileRelation(long fileId, long targetId, int targetType)
        {
            FileId = fileId;
            TargetId = targetId;
            TargetType = targetType;
        }

        /// <summary>
        /// 更新文件关联
        /// </summary>
        /// <param name="fileId"></param>
        public void UpdateFileId(long fileId)
        {
            FileId = fileId;
        }
    }
}
