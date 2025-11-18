using System.Text.Json.Serialization;
using System.Text.Json;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Application.Queries.IBeverageQueries;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.BeverageQueries
{
    /// <summary>
    /// 配方查询
    /// </summary>
    public class FormulaInfosQueries : IFormulaInfosQueries
    {
        /// <summary>
        /// 获取配方参数字典集合
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<FormulaTypeEnum, string>> GetAllSpecsJson()
        {
            var specsJsonCollection = new Dictionary<FormulaTypeEnum, string>();

            foreach (FormulaTypeEnum formulaType in Enum.GetValues(typeof(FormulaTypeEnum)))
            {
                // 获取对应的实体类型并创建示例对象
                var specsObject = FormulaSpecsConverter.CreateSpecsObject(formulaType);
                if (specsObject != null)
                {
                    // 序列化为 JSON
                    string json = JsonSerializer.Serialize(specsObject, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                    specsJsonCollection.Add(formulaType, json);
                }
            }

            return specsJsonCollection;
        }
    }
}
