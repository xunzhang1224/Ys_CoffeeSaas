using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageInfoTemplateQueries
{
    /// <summary>
    /// 饮品库查询
    /// </summary>
    public interface IBeverageInfoTemplateQueries
    {
        /// <summary>
        /// 获取饮品库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BeverageInfoTemplateDto> GetBeverageInfoTemplateAsync(long id);

        /// <summary>
        /// 获取饮品库列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="enterpriseInfoId"></param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        Task<PagedResultDto<BeverageInfoTemplateDto>> GetBeverageInfoTemplateListAsync(QueryRequest request, long enterpriseInfoId, FormulaTypeEnum? formulaType);

        /// <summary>
        /// 校验sku根据饮品id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool?> VerifySkuByBeverageIdAsync(long id);
    }
}
