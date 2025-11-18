using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Domain.BasicDtos
{
    /// <summary>
    /// 配方信息数据传输对象
    /// </summary>
    public class OrderFormulaInfoDto
    {
        /// <summary>
        /// 料盒
        /// </summary>
        public long? MaterialBoxId { get; set; }
        /// <summary>
        /// 料盒名称
        /// </summary>
        public string MaterialBoxName { get; set; }
        /// <summary>
        /// 配方类型
        /// </summary>
        public FormulaTypeEnum FormulaType { get; set; }
        /// <summary>
        /// 配方类型描述
        /// </summary>
        public string FormulaTypeText { get { return FormulaType.GetDescriptionOrValue(); } }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 物料拓展数据
        /// </summary>
        public JsonObject Specs { get; set; }

        /// <summary>
        /// 配方参数
        /// </summary>
        public string SpecsString { get; set; }
    }

    /// <summary>
    /// 根据枚举序列化对应Model
    /// </summary>
    public static class FormulaSpecsConverter
    {

        /// <summary>
        /// 获取枚举的描述或值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionOrValue(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// 根据配方类型获取配料参数实体
        /// </summary>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        public static object CreateSpecsObject(FormulaTypeEnum formulaType)
        {
            return formulaType switch
            {
                FormulaTypeEnum.Coffee => new CoffeeSpecs { PowderAmount = 10, WaterAmount = 50, Temperature = TemperatureEnum.High },
                FormulaTypeEnum.Water => new WaterSpecs { WaterAmount = 50, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.MilkPowder => new MilkPowderSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.Sugar => new SugarSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.Tea => new TeaSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                FormulaTypeEnum.Ice => new IceSpecs { PowderAmount = 10 },
                FormulaTypeEnum.Lh => new LhSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                _ => null
            };
        }

        private static readonly Dictionary<FormulaTypeEnum, Type> _formulaTypeToSpecTypeMap = new Dictionary<FormulaTypeEnum, Type>()
        {
            { FormulaTypeEnum.Coffee, typeof(CoffeeSpecs) },
            { FormulaTypeEnum.Water, typeof(WaterSpecs) },
            //{ FormulaTypeEnum.MilkPowder, typeof(MilkPowderSpecs) },
            //{ FormulaTypeEnum.Sugar, typeof(SugarSpecs) },
            //{ FormulaTypeEnum.Tea, typeof(TeaSpecs) },
            { FormulaTypeEnum.Ice, typeof(IceSpecs) },
            { FormulaTypeEnum.Lh, typeof(LhSpecs) },
        };

        /// <summary>
        /// 根据FormulaTypeEnum和model序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specs"></param>
        /// <returns></returns>
        public static string SerializeSpecs<T>(T specs) where T : class
        {
            return JsonSerializer.Serialize(specs, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }

        /// <summary>
        /// 根据FormulaTypeEnum和json字符串反序列化json
        /// </summary>
        /// <param name="formulaType"></param>
        /// <param name="specsString"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static object DeserializeSpecs(FormulaTypeEnum formulaType, string specsString)
        {
            if (_formulaTypeToSpecTypeMap.TryGetValue(formulaType, out var specType))
            {
                return JsonSerializer.Deserialize(specsString, specType) ?? "{}";
            }
            throw new InvalidOperationException($"未找到 {formulaType} 对应的配方规格类型。");
        }
        /// <summary>
        /// 根据FormulaTypeEnum和json字符串验证json是否与对应的model匹配
        /// </summary>
        /// <param name="formulaType"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool ValidateSpecsJson(FormulaTypeEnum formulaType, string json)
        {
            try
            {
                // 根据枚举类型获取对应的模型类型
                Type modelType = formulaType switch
                {
                    FormulaTypeEnum.Coffee => typeof(CoffeeSpecs),
                    FormulaTypeEnum.Water => typeof(WaterSpecs),
                    //FormulaTypeEnum.MilkPowder => typeof(MilkPowderSpecs),
                    //FormulaTypeEnum.Sugar => typeof(SugarSpecs),
                    //FormulaTypeEnum.Tea => typeof(TeaSpecs),
                    FormulaTypeEnum.Ice => typeof(IceSpecs),
                    FormulaTypeEnum.Lh => typeof(LhSpecs),
                    _ => null
                };

                if (modelType == null)
                    throw new ArgumentException("配方类型不存在");

                // 尝试反序列化 JSON 字符串
                JsonSerializer.Deserialize(json, modelType);

                // 没有异常即为匹配
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"验证失败: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// 配方参数
    /// </summary>
    public class BaseSpecs
    {
        /// <summary>
        /// 粉量
        /// </summary>
        public double PowderAmount { get; set; }//值

        /// <summary>
        /// 粉量单位
        /// </summary>
        public string PowderUnit { get; set; } = "g";//单位

        /// <summary>
        /// 粉量范围
        /// </summary>
        public int[] PowderScope { get; set; } = [6, 16];//值范围
        /// <summary>
        /// 水量
        /// </summary>
        public double WaterAmount { get; set; }//值
        /// <summary>
        /// 水量单位
        /// </summary>
        public string WaterUnit { get; set; } = "ml";//单位
        /// <summary>
        /// 水量范围
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240];//值范围
        /// <summary>
        /// 搅拌速度
        /// </summary>
        public double MixingSpeed { get; set; }//值
        /// <summary>
        /// 搅拌速度单位
        /// </summary>
        public string MixingUnit { get; set; } = "%";//单位
        /// <summary>
        /// 搅拌速度范围
        /// </summary>
        public int[] MixingScope { get; set; } = [50, 100];//值范围
        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }
    /// <summary>
    /// 咖啡
    /// </summary>
    public class CoffeeSpecs
    {
        /// <summary>
        /// 粉量
        /// </summary>
        public double PowderAmount { get; set; }//值

        /// <summary>
        /// 粉量单位
        /// </summary>
        public string PowderUnit { get; set; } = "g";//单位
        /// <summary>
        /// 粉量范围
        /// </summary>
        public int[] PowderScope { get; set; } = [6, 16];//值范围
        /// <summary>
        /// 水量
        /// </summary>
        public double WaterAmount { get; set; }//值
        /// <summary>
        /// 水量单位
        /// </summary>
        public string WaterUnit { get; set; } = "ml";//单位
        /// <summary>
        /// 水量范围
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240];//值范围
        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }
    /// <summary>
    /// 水
    /// </summary>
    public class WaterSpecs
    {
        /// <summary>
        /// 水量
        /// </summary>
        public double WaterAmount { get; set; }//值
        /// <summary>
        /// 水量单位
        /// </summary>
        public string WaterUnit { get; set; } = "ml";//单位
        /// <summary>
        /// 水量范围
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240];//值范围
        /// <summary>
        /// 温度
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }

    /// <summary>
    /// 料盒
    /// </summary>
    public class LhSpecs : BaseSpecs
    {
    }
    /// <summary>
    /// 冰块
    /// </summary>
    public class IceSpecs
    {
        /// <summary>
        /// 冰量
        /// </summary>
        public double PowderAmount { get; set; }//值
        /// <summary>
        /// 冰量单位
        /// </summary>
        public string PowderUnit { get; set; } = "g";//单位
        /// <summary>
        /// 冰量范围
        /// </summary>
        public int[] PowderScope { get; set; } = [5, 30];//值范围
    }
    //public class IceSpecs
    //{
    //    /// <summary>
    //    /// 冰量
    //    /// </summary>
    //    public double IceAmount { get; set; }//值
    //    /// <summary>
    //    /// 冰量单位
    //    /// </summary>
    //    public string IceUnit { get; set; } = "g";//单位
    //    /// <summary>
    //    /// 冰量范围
    //    /// </summary>
    //    public int[] IceScope { get; set; } = [5, 30];//值范围
    //}
}
