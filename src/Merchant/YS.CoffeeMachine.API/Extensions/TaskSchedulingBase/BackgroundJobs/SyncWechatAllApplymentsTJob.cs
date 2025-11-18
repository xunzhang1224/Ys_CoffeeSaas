using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Extensions.TaskSchedulingBase.BackgroundJobs
{
    /// <summary>
    /// 同步所有微信进件申请状态
    /// </summary>
    public class SyncWechatAllApplymentsTJob : IRecurringTask
    {
        private readonly CoffeeMachineDbContext _context;
        private readonly PaymentPlatformUtil _paymentPlatformUtil;
        private readonly ILogger<SyncAlipayAllApplymentsJob> _logger;

        /// <summary>
        /// 给反射用的无参构造函数
        /// </summary>
        public SyncWechatAllApplymentsTJob() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paymentPlatformUtil"></param>
        /// <param name="logger"></param>
        public SyncWechatAllApplymentsTJob(
            CoffeeMachineDbContext context,
            PaymentPlatformUtil paymentPlatformUtil,
            ILogger<SyncAlipayAllApplymentsJob> logger)
        {
            _context = context;
            _paymentPlatformUtil = paymentPlatformUtil;
            _logger = logger;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public async Task Execute()
        {
            Console.WriteLine($"{DateTime.Now.ToString("G")},进入定时轮询微信商户进件状态同步------------------------------------------------------------------------");
            _logger.LogInformation($"进入定时轮询微信商户进件状态同步");

            var paymentOriginIds = await _context.M_PaymentWechatApplyments.IgnoreQueryFilters()
                .Where(a => a.FlowStatus == ApplymentFlowStatusEnum.PlatformReview || a.FlowStatus == ApplymentFlowStatusEnum.TobeSigned || a.FlowStatus == ApplymentFlowStatusEnum.AUDITING || a.FlowStatus == ApplymentFlowStatusEnum.AccountTeedVerify)
                .Select(a => a.PaymentOriginId)
                .ToListAsync();

            if (paymentOriginIds == null || !paymentOriginIds.Any())
            {
                _logger.LogInformation($"结束定时轮询微信商户进件状态同步，没有提交微信进件商户记录");
                return;
            }

            _logger.LogInformation($"定时轮询微信商户进件状态同步：申请进件:{paymentOriginIds.Count}条");
            await _paymentPlatformUtil.SyncWechatApplymentState(paymentOriginIds.ToArray());

            _logger.LogInformation($"结束定时轮询微信商户进件");

        }
    }
}