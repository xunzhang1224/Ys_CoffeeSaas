using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.Goods
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="Goods"></param>
    public record AddPrivateGoodsCommand(List<Goods> Goods) : ICommand<bool>;

    /// <summary>
    /// 设置禁用/启用
    /// </summary>
    /// <param name="isEnable"></param>
    public record SetPrivateGoodsStatusCommand(List<long> ids, bool isEnable) : ICommand<bool>;

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="sku"></param>
    /// <param name="suggestedPrice"></param>
    /// <param name="isEnable"></param>
    public record UpdatePrivateGoodsCommand(long id,string name, decimal suggestedPrice, bool isEnable) : ICommand<bool>;

    /// <summary>
    /// 添加商品
    /// </summary>
    public class Goods
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [Required]
        public string Sku { get; set; }

        /// <summary>
        /// 建议零售价
        /// </summary>
        public decimal SuggestedPrice { get; set; } = 0;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;
    }
}
