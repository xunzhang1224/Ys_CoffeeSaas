using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 饮品版本类型枚举
    /// </summary>
    public enum BeverageVersionTypeEnum
    {
        [Description("新增")]
        Insert = 0,
        [Description("编辑")]
        Edit = 1,
        [Description("删除")]
        Delete = 2,
        [Description("合集")]
        Collection = 3
    }
}
