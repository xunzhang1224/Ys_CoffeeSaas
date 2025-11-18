using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;

namespace YS.CoffeeMachine.Application.Queries.IGoods
{
    /// <summary>
    /// 商品查询接口
    /// </summary>
    public interface IGoodsQueries
    {
        /// <summary>
        /// 检查sku是否符合
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        Task<bool> CheckSku(List<string> skus);

        /// <summary>
        /// 获取私有库列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResultDto<PrivateGoodsRepository>> GetPrivateGoodsPage(GoodsDto dto);
    }
}
