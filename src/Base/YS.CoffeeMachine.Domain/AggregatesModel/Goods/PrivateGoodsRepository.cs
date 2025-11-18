using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Goods
{
    /// <summary>
    /// 私有库
    /// </summary>
    public class PrivateGoodsRepository : EnterpriseBaseEntity
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sku
        /// </summary>
        public string Sku { get; private set; }

        /// <summary>
        /// 建议零售价
        /// </summary>
        public decimal SuggestedPrice { get; private set; } = 0;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; private set; } = true;

        /// <summary>
        /// a
        /// </summary>
        protected PrivateGoodsRepository() { }

        /// <summary>
        /// 新增私有库商品
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sku"></param>
        /// <param name="suggestedPrice"></param>
        /// <param name="isEnable"></param>
        public PrivateGoodsRepository(string name, string sku, decimal suggestedPrice, bool isEnable)
        {
            Name = name;
            Sku = sku;
            SuggestedPrice = suggestedPrice;
            IsEnable = isEnable;
        }

        /// <summary>
        /// 设置价格
        /// </summary>
        /// <param name="suggestedPrice"></param>
        public void SetPrice(decimal suggestedPrice)
        {
            SuggestedPrice = suggestedPrice;
        }

        /// <summary>
        /// 禁用/启用
        /// </summary>
        /// <param name="isEnable"></param>
        public void SetStatus(bool isEnable)
        {
            IsEnable = isEnable;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="name"></param>
        /// <param name="suggestedPrice"></param>
        /// <param name="isEnable"></param>
        public void Update(string name, decimal suggestedPrice, bool isEnable)
        {
            Name = name;
            SuggestedPrice = suggestedPrice;
            IsEnable = isEnable;
        }
    }
}
