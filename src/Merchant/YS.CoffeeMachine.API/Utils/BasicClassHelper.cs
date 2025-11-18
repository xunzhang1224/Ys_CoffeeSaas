using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using static YS.CoffeeMachine.API.Extensions.Cap.Dtos.Drinks9026Dto;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 基础帮助类
    /// </summary>
    public class BasicClassHelper
    {
        /// <summary>
        /// 获取9026指令Dto
        /// </summary>
        /// <param name="beverageInfos"></param>
        /// <param name="mid"></param>
        /// <param name="isNewCode"></param>
        /// <returns></returns>
        public Drinks9026Dto GetDrinks9026Dto(List<BeverageInfo> beverageInfos, string mid, bool isNewCode = false)
        {
            return new Drinks9026Dto
            {
                Mid = mid,
                IsApply = true,
                Sku = beverageInfos[0].Code,
                CoffeeInfo = GetBeverages(beverageInfos)
            };

        }

        /// <summary>
        /// 获取下发饮品信息
        /// </summary>
        /// <param name="beverageInfos"></param>
        /// <returns></returns>
        public List<Beverage> GetBeverages(List<BeverageInfo> beverageInfos, bool isNewCode = false)
        {
            var info = new List<Beverage>();
            foreach (var b in beverageInfos)
            {
                info.Add(new Beverage
                {
                    Sku = isNewCode ? YitIdHelper.NextId().ToString() : b.Code,
                    Name = b.Name,
                    ImageUrl = b.BeverageIcon,
                    Price = b.Price ?? 0,
                    DiscountedPrice = b.DiscountedPrice,
                    Cold = (int)b.Temperature,
                    Sort = b.Sort ?? 0,
                    Spec = b.ProductionForecast != null && !string.IsNullOrWhiteSpace(b.ProductionForecast) ? Convert.ToInt32(b.ProductionForecast) : 0,
                    Recipe = b.FormulaInfos.Select(s => new RecipeInfo
                    {
                        Msg = s.SpecsString,
                        MaterialBoxid = (int)(s.MaterialBoxId ?? 0),
                        MaterialBoxName = s.MaterialBoxName,
                        FormulaType = (int)s.FormulaType,
                        Sort = s.Sort
                    }).ToList(),
                });
            }

            return info;
        }

        /// <summary>
        /// 获取9026指令Dto
        /// </summary>
        /// <param name="beverageInfos"></param>
        /// <param name="mid"></param>
        /// <param name="isNewCode"></param>
        /// <returns></returns>
        public Drinks9026Dto GetDrinks9026DtoByTemp(List<BeverageInfoTemplate> beverageInfos, string mid, bool isNewCode = false)
        {
            return new Drinks9026Dto
            {
                Mid = mid,
                IsApply = true,
                Sku = beverageInfos[0].Code,
                CoffeeInfo = GetBeveragesByTemp(beverageInfos)
            };

        }

        /// <summary>
        /// 获取下发饮品信息
        /// </summary>
        /// <param name="beverageInfos"></param>
        /// <returns></returns>
        public List<Beverage> GetBeveragesByTemp(List<BeverageInfoTemplate> beverageInfos, bool isNewCode = false)
        {
            var info = new List<Beverage>();
            foreach (var b in beverageInfos)
            {
                info.Add(new Beverage
                {
                    Sku = isNewCode ? YitIdHelper.NextId().ToString() : b.Code,
                    Name = b.Name,
                    ImageUrl = b.BeverageIcon,
                    Price = 0,
                    DiscountedPrice = null,
                    Cold = (int)b.Temperature,
                    Spec = b.ProductionForecast != null ? Convert.ToInt32(b.ProductionForecast) : 0,
                    Recipe = b.FormulaInfoTemplates.Select(s => new RecipeInfo
                    {
                        Msg = s.SpecsString,
                        MaterialBoxid = (int)(s.MaterialBoxId ?? 0),
                        MaterialBoxName = s.MaterialBoxName,
                        FormulaType = (int)s.FormulaType,
                        Sort = s.Sort
                    }).ToList(),
                });
            }

            return info;
        }
    }
}
