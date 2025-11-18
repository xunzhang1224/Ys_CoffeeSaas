using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Files
{
    /// <summary>
    /// 获取文件中心入参
    /// </summary>
    public class FileCenterInput : QueryRequest
    {
        /// <summary>
        /// 文件名字
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 导出菜单
        /// 文件类型
        /// </summary>
        public DocmentTypeEnum? DocmentType { get; set; }

        /// <summary>
        /// 系统类型
        /// </summary>
        public SysMenuTypeEnum? SysMenuType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public FileStateEnum? FileState { get; set; }

        /// <summary>
        /// 文件源
        /// 地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 导出时间
        /// </summary>
        public List<DateTime>? Times { get; set; }
    }
}
