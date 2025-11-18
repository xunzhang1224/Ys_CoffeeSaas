using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos
{
    /// <summary>
    /// 订单详情数据传输对象
    /// </summary>
    public class OrderDetailsDto
    {
        /// <summary>
        /// 饮品
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 子订单商品总额
        /// </summary>
        public decimal Acmount { get { return Quantity * Price; } }

        /// <summary>
        /// 1-出货成功 0-出货失败
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 出货状态描述
        /// </summary>
        public string ResultStr => Result == 1 ? "出货成功" : "出货失败";

        /// <summary>
        /// 错误
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string? ErrorDescription { get; set; } = null;

        /// <summary>
        /// 出货时间
        /// </summary>
        public long ActionTimeSp { get; set; } = 0;

        /// <summary>
        /// 出货时间
        /// </summary>
        public string ActionTimeStr => ActionTimeSp == 0 ? string.Empty : DateTimeOffset.FromUnixTimeMilliseconds(ActionTimeSp).ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 物料使用情况数据传输对象列表
        /// </summary>
        public List<MaterialUsageDto> MaterialUsageDtos { get; set; } = new List<MaterialUsageDto>();
        /// <summary>
        /// 物料使用总量
        /// </summary>
        public string MaterialUsageTotal { get { return GetMaterialUsageSummary(MaterialUsageDtos); } }
        /// <summary>
        /// 历史饮品信息
        /// </summary>
        public string BeverageInfo { get; set; }

        /// <summary>
        /// 统计物料使用情况并按类型汇总（如 "水：10ml，杯子：1个"）
        /// </summary>
        /// <param name="materialUsages">物料使用列表</param>
        /// <returns>格式化后的字符串</returns>
        private string GetMaterialUsageSummary(List<MaterialUsageDto> materialUsages)
        {
            if (materialUsages == null || !materialUsages.Any())
                return "无物料使用记录";

            // 按物料类型分组，计算总使用量
            var grouped = materialUsages
                .GroupBy(m => m.MaterialType)
                .Select(g => new
                {
                    MaterialType = g.Key,
                    TotalUsage = g.Sum(x => x.Usage),
                    Unit = GetUnitByMaterialType(g.Key) // 获取单位
                })
                .Where(x => x.MaterialType != MaterialTypeEnum.Not) // 排除 Not 类型
                .OrderBy(x => x.MaterialType); // 按类型排序（可选）

            // 拼接成字符串
            return string.Join("，", grouped.Select(x =>
                $"{x.MaterialType.GetDescriptionOrValue()}：{x.TotalUsage}{x.Unit}"));
        }

        /// <summary>
        /// 根据物料类型获取单位
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetUnitByMaterialType(MaterialTypeEnum type)
        {
            return type switch
            {
                MaterialTypeEnum.CoffeeBean => "克",
                MaterialTypeEnum.Water => "毫升",
                MaterialTypeEnum.Cassette => "克",
                MaterialTypeEnum.Cup => "个",
                MaterialTypeEnum.CupCover => "个",
                _ => "未知单位"
            };
        }
    }

    /// <summary>
    /// 物料使用情况数据传输对象
    /// </summary>
    public class MaterialUsageDto
    {
        /// <summary>
        /// 物料类型
        /// </summary>
        public MaterialTypeEnum MaterialType { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 使用量
        /// </summary>
        public int Usage { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 是否是糖
        /// </summary>
        public bool IsSugar { get; set; } = false;
    }
}