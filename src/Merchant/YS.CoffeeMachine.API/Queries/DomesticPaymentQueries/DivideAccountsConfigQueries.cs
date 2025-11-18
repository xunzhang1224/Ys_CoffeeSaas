using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.DivideAccountsConfigDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.DomesticPaymentQueries
{
    /// <summary>
    /// 分账相关查询
    /// </summary>
    public class DivideAccountsConfigQueries(CoffeeMachineDbContext context) : IDivideAccountsConfigQueries
    {
        /// <summary>
        /// 获取当前企业所有可用支付方式
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemPaymentMethodSelect>> GetSystemPaymentMethodSelects()
        {
            return await context.SystemPaymentMethod
                .Select(s => new SystemPaymentMethodSelect
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();
        }

        /// <summary>
        /// 获取当前系统支付方式下所有支付列表
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public async Task<List<M_PaymentMethodSelect>> GetmPaymentMethodSelects(long paymentMethodId)
        {
            if (CommonConst.WechatPaymentId == paymentMethodId || CommonConst.WechatFacePaymentId == paymentMethodId || CommonConst.WechatJSAPIPaymentId == paymentMethodId)
            {
                return await (from a in context.M_PaymentMethod
                              join b in context.M_PaymentWechatApplyments on a.Id equals b.PaymentOriginId into ab
                              from b in ab.DefaultIfEmpty()
                              where a.PaymentEntryStatus == Domain.Shared.Enum.InternalOnboardingStatusEnum.OnboardingSuccess
                              select new M_PaymentMethodSelect
                              {
                                  Id = a.Id,
                                  Name = b.MerchantShortName
                              }).ToListAsync();
            }
            else
            {
                return await (from a in context.M_PaymentMethod
                              join b in context.M_PaymentAlipayApplyments on a.Id equals b.PaymentOriginId into ab
                              from b in ab.DefaultIfEmpty()
                              where a.PaymentEntryStatus == Domain.Shared.Enum.InternalOnboardingStatusEnum.OnboardingSuccess
                              select new M_PaymentMethodSelect
                              {
                                  Id = a.Id,
                                  Name = b.MerchantShortName
                              }).ToListAsync();
            }

        }

        /// <summary>
        /// 获取当前支付方式绑定的设备
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public async Task<List<PaymentMethodBindDeviceSelect>> GetPaymentMethodBindDeviceSelects(long paymentMethodId)
        {
            return await (from a in context.M_PaymentMethodBindDevice
                          join b in context.DeviceInfo on a.DeviceId equals b.Id into ab
                          from b in ab.DefaultIfEmpty()
                          where paymentMethodId == a.PaymentMethodId
                          select new PaymentMethodBindDeviceSelect
                          {
                              Id = b != null ? b.Id : 0,
                              Name = b != null ? b.Name : string.Empty
                          }).ToListAsync();
        }

        /// <summary>
        /// 查询支付分账配置信息分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DivideAccountsConfigOutput>> GetDivideAccountsConfigPageList(DivideAccountsConfigInput input)
        {
            return await (from a in context.DivideAccountsConfig
                          join b in context.SystemPaymentMethod
                              on a.MerchantId equals b.Id into ab
                          from b in ab.DefaultIfEmpty()
                          select new DivideAccountsConfigOutput
                          {
                              Id = a.Id,
                              MerchantId = a.MerchantId,
                              SystemPaymentMethodId = b != null ? b.Id.ToString() : string.Empty,
                              MerchantName = a.MerchantName,
                              Bibliography = a.Bibliography,
                              Remarks = a.Remarks,
                              type = a.type,
                              relation_type = a.relation_type,
                              account = a.account,
                              name = a.name,
                              DeviceIds = a.VendCodes,
                              IsEnabled = a.IsEnabled,
                              CreateTimeText = a.CreateTime.ToString("G")
                          })
              .WhereIf(!string.IsNullOrWhiteSpace(input.MerchantName), w => w.MerchantName == input.MerchantName)
              .WhereIf(!string.IsNullOrWhiteSpace(input.name), w => w.name == input.name)
              .WhereIf(!string.IsNullOrWhiteSpace(input.account), w => w.account == input.account)
              .ToPagedListAsync(input);
        }
    }
}