namespace YS.CoffeeMachine.Application.Dtos.DomesticPayment
{
    /// <summary>
    /// 获取银行分行列表输入参数
    /// </summary>
    public class GetBankBranchListInput
    {
        /// <summary>
        /// 银行别名代码
        /// </summary>
        public string BankAliasCode { get; set; }

        /// <summary>
        /// 城市代码
        /// </summary>
        public int WxCityCode { get; set; }
    }

    /// <summary>
    /// 省份输出
    /// </summary>
    public class GetProvinceCityOutput
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; } = null;

        /// <summary>
        /// 微信编码
        /// </summary>
        public string WxCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
