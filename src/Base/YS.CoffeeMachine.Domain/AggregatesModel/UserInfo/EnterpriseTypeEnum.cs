using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 企业机构类型
    /// </summary>
    public enum EnterpriseOrganizationTypeEnum
    {
        /// <summary>
        /// 个人/小微商户
        /// </summary>
        [Description("个人/小微商户")]
        Personal,

        /// <summary>
        /// 企业
        /// </summary>
        [Description("企业")]
        Company
    }

    /// <summary>
    /// 注册进度
    /// </summary>
    public enum RegistrationProgress
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotStarted,

        ///// <summary>
        ///// 第一步(选择资质类型)
        ///// </summary>
        //StepOne,

        ///// <summary>
        ///// 第二步(填写资质信息)
        ///// </summary>
        //StepTwo,

        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        InReview,

        /// <summary>
        /// 审核失败
        /// </summary>
        [Description("审核失败")]
        Failed,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Passed
    }

    /// <summary>
    /// 企业类型
    /// </summary>
    //public enum EnterpriseTypeEnum
    //{
    //    [Description("服务商")]
    //    ServiceProvider = 0,
    //    [Description("经销商")]
    //    Distributor = 1,
    //    [Description("代理商")]
    //    Agent = 2,
    //    [Description("商业连锁")]
    //    CommercialChain = 3,
    //    [Description("企业客户")]
    //    EnterpriseCustomers = 4,
    //    [Description("其他")]
    //    Other = 5
    //}
}