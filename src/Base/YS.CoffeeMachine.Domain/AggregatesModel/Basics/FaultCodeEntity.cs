using System.ComponentModel.DataAnnotations;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    /// <summary>
    /// 故障码实体
    /// </summary>
    public class FaultCodeEntity : Entity, IAggregateRoot
    {
        /// <summary>
        /// 故障码标识
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 多语言标识
        /// </summary>
        [Required]
        public string LanCode { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; private set; }

        /// <summary>
        /// 故障码类型
        /// </summary>
        public FaultCodeTypeEnum? Type { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 备注,可用来维护解决方案
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// GetKeys
        /// </summary>
        /// <returns></returns>
        public override object[] GetKeys() => new object[] { Code };

        /// <summary>
        /// 保护构造
        /// </summary>
        protected FaultCodeEntity()
        {
        }

        /// <summary>
        /// 添加故障码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="lanCode"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public FaultCodeEntity(string code, string lanCode, FaultCodeTypeEnum type, string name, string description, string remark)
        {
            Code = code;
            LanCode = lanCode;
            Type = type;
            Name = name;
            Description = description;
            Remark = remark;
        }

        /// <summary>
        /// 编辑故障码
        /// </summary>
        /// <param name="lanCode"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void Update(string lanCode, string name, string description, string remark)
        {
            LanCode = lanCode;
            Name = name;
            Description = description;
            Remark = remark;
        }
    }
}