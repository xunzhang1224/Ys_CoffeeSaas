namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using YS.CoffeeMachine.Application.Tools;
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

    /// <summary>
    /// 配方信息数据传输对象（DTO），用于传递饮品配方相关的参数或返回结果
    /// </summary>
    public class FormulaInfoDto
    {
        ///// <summary>
        ///// 饮品ID，标识该配方所属的饮品
        ///// </summary>
        ////public long BeverageInfoId { get; set; }

        /// <summary>
        /// 料盒 ID（可为空），表示该配方使用的原料料盒编号
        /// </summary>
        public long? MaterialBoxId { get; set; }

        /// <summary>
        /// 料盒名称，用于前端展示
        /// </summary>
        public string MaterialBoxName { get; set; }

        /// <summary>
        /// 配方类型，例如：咖啡、水、牛奶等
        /// </summary>
        public FormulaTypeEnum FormulaType { get; set; }

        /// <summary>
        /// 配方类型的描述文本，从枚举中获取
        /// </summary>
        public string FormulaTypeText => FormulaType.GetDescriptionOrValue();

        /// <summary>
        /// 排序字段，用于控制配方在界面上显示顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 配方规格数据（JSON 对象格式），支持动态配置
        /// </summary>
        public JsonObject Specs { get; set; }

        /// <summary>
        /// 配方规格字符串，便于持久化存储或网络传输
        /// </summary>
        public string SpecsString { get; set; }
    }

    /// <summary>
    /// 配方规格转换工具类，提供序列化/反序列化及验证功能
    /// </summary>
    public static class FormulaSpecsConverter
    {
        /// <summary>
        /// 根据配方类型创建默认的配方参数对象
        /// </summary>
        /// <param name="formulaType">配方类型</param>
        /// <returns>对应类型的默认配方参数对象</returns>
        public static object CreateSpecsObject(FormulaTypeEnum formulaType)
        {
            return formulaType switch
            {
                FormulaTypeEnum.Coffee => new CoffeeSpecs { PowderAmount = 10, WaterAmount = 50, Temperature = TemperatureEnum.High },
                FormulaTypeEnum.Water => new WaterSpecs { WaterAmount = 50, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.MilkPowder => new MilkPowderSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.Sugar => new SugarSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                //FormulaTypeEnum.Tea => new TeaSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                FormulaTypeEnum.Ice => new IceSpecs { IceAmount = 10 },
                FormulaTypeEnum.Lh => new LhSpecs { PowderAmount = 10, WaterAmount = 50, MixingSpeed = 100, Temperature = TemperatureEnum.High },
                _ => null
            };
        }

        private static readonly Dictionary<FormulaTypeEnum, Type> _formulaTypeToSpecTypeMap = new ()
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
        /// 将配方参数对象序列化为 JSON 字符串
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="specs">配方参数对象</param>
        /// <returns>JSON 字符串</returns>
        public static string SerializeSpecs<T>(T specs) where T : class
        {
            return JsonSerializer.Serialize(specs, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为指定配方类型的对象
        /// </summary>
        /// <param name="formulaType">配方类型</param>
        /// <param name="specsString">JSON 字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static object DeserializeSpecs(FormulaTypeEnum formulaType, string specsString)
        {
            if (_formulaTypeToSpecTypeMap.TryGetValue(formulaType, out var specType))
            {
                return JsonSerializer.Deserialize(specsString, specType) ?? "{}";
            }
            throw new InvalidOperationException($"未找到 {formulaType} 对应的配方规格类型。");
        }

        /// <summary>
        /// 验证 JSON 字符串是否符合目标配方类型的结构要求
        /// </summary>
        /// <param name="formulaType">配方类型</param>
        /// <param name="json">JSON 字符串</param>
        /// <returns>验证结果（true/false）</returns>
        public static bool ValidateSpecsJson(FormulaTypeEnum formulaType, string json)
        {
            try
            {
                // 获取对应的模型类型
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

                // 反序列化验证结构
                JsonSerializer.Deserialize(json, modelType);

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
    /// 基础配方参数类，定义通用字段
    /// </summary>
    public class BaseSpecs
    {
        /// <summary>
        /// 粉量（单位：g）
        /// </summary>
        public double PowderAmount { get; set; } // 值

        /// <summary>
        /// 粉量单位，默认 g
        /// </summary>
        public string PowderUnit { get; set; } = "g"; // 单位

        /// <summary>
        /// 粉量范围（最小值, 最大值）
        /// </summary>
        public int[] PowderScope { get; set; } = [6, 16]; // 值范围

        /// <summary>
        /// 水量（单位：ml）
        /// </summary>
        public double WaterAmount { get; set; } // 值

        /// <summary>
        /// 水量单位，默认 ml
        /// </summary>
        public string WaterUnit { get; set; } = "ml"; // 单位

        /// <summary>
        /// 水量范围（最小值, 最大值）
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240]; // 值范围

        /// <summary>
        /// 搅拌速度（单位：%）
        /// </summary>
        public double MixingSpeed { get; set; } // 值

        /// <summary>
        /// 搅拌速度单位，默认 %
        /// </summary>
        public string MixingUnit { get; set; } = "%"; // 单位

        /// <summary>
        /// 搅拌速度范围（最小值, 最大值）
        /// </summary>
        public int[] MixingScope { get; set; } = [50, 100]; // 值范围

        /// <summary>
        /// 温度设置
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }

    /// <summary>
    /// 咖啡配方参数
    /// </summary>
    public class CoffeeSpecs
    {
        /// <summary>
        /// 粉量（单位：g）
        /// </summary>
        public double PowderAmount { get; set; } // 值

        /// <summary>
        /// 粉量单位，默认 g
        /// </summary>
        public string PowderUnit { get; set; } = "g"; // 单位

        /// <summary>
        /// 粉量范围（最小值, 最大值）
        /// </summary>
        public int[] PowderScope { get; set; } = [6, 16]; // 值范围

        /// <summary>
        /// 水量（单位：ml）
        /// </summary>
        public double WaterAmount { get; set; } // 值

        /// <summary>
        /// 水量单位，默认 ml
        /// </summary>
        public string WaterUnit { get; set; } = "ml"; // 单位

        /// <summary>
        /// 水量范围（最小值, 最大值）
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240]; // 值范围

        /// <summary>
        /// 温度设置
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }

    /// <summary>
    /// 水配方参数
    /// </summary>
    public class WaterSpecs
    {
        /// <summary>
        /// 水量（单位：ml）
        /// </summary>
        public double WaterAmount { get; set; } // 值

        /// <summary>
        /// 水量单位，默认 ml
        /// </summary>
        public string WaterUnit { get; set; } = "ml"; // 单位

        /// <summary>
        /// 水量范围（最小值, 最大值）
        /// </summary>
        public int[] WaterScope { get; set; } = [15, 240]; // 值范围

        /// <summary>
        /// 温度设置
        /// </summary>
        public TemperatureEnum Temperature { get; set; }
    }

    /// <summary>
    /// 牛奶粉配方参数（继承基础字段）
    /// </summary>
    public class MilkPowderSpecs : BaseSpecs
    {
    }

    /// <summary>
    /// 糖配方参数（继承基础字段）
    /// </summary>
    public class SugarSpecs : BaseSpecs
    {
    }

    /// <summary>
    /// 茶配方参数（继承基础字段）
    /// </summary>
    public class TeaSpecs : BaseSpecs
    {
    }

    /// <summary>
    /// 料盒
    /// </summary>
    public class LhSpecs : BaseSpecs
    {
    }
    /// <summary>
    /// 冰块配方参数
    /// </summary>
    public class IceSpecs
    {
        /// <summary>
        /// 冰量（单位：g）
        /// </summary>
        public double IceAmount { get; set; } // 值

        /// <summary>
        /// 冰量单位，默认 g
        /// </summary>
        public string IceUnit { get; set; } = "g"; // 单位

        /// <summary>
        /// 冰量范围（最小值, 最大值）
        /// </summary>
        public int[] IceScope { get; set; } = [5, 30]; // 值范围
    }
}