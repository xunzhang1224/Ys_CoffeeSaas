using FreeRedis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using YS.Cabinet.Payment.WechatPay;
using YS.Cabinet.Payment.WechatPay.Application.V3.Responses.Common;
using YS.Cabinet.Payment.WechatPay.V3;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.DomesticPayment;
using YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using static YS.Cabinet.Payment.WechatPay.Application.V3.Responses.Common.WxBranchBankResponse;
using static YS.Cabinet.Payment.WechatPay.V3.WxBankResponse;

namespace YS.CoffeeMachine.API.Queries.DomesticPaymentQueries
{
    /// <summary>
    /// 微信支付相关查询类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_wechatMerchantService"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_paymentPlatformUtil"></param>
    public class WechatPaymentQueries(CoffeeMachineDbContext context, IWechatMerchantService _wechatMerchantService, IRedisClient _redisClient, PaymentPlatformUtil _paymentPlatformUtil) : IWechatPaymentQueries
    {

        /// <summary>
        /// 根据银行账号获取银行信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<List<WxBanksData>> GetBankByAccount(string account)
        {
            var serviceProviderEntity = await _paymentPlatformUtil.GetSystemPaymentServiceProvider(CommonConst.WechatPaymentId);
            var client = _wechatMerchantService.BuildMerchant(serviceProviderEntity.Id.ToString());

            account = client.RSAEncrypt(account);

            var bankInfo = await client.CommonService.ToResponseAsync(a => a.GetBankByBankAccountAsync(account));

            if (bankInfo.Succeeded && bankInfo.Data.Data?.Count > 0)
                return bankInfo.Data.Data;
            return default;
        }

        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <param name="type">银行列表类型(1=对公 2=对私)</param>
        /// <returns></returns>
        public async Task<List<WxBanksData>> GetBankList(int type = 1)
        {
            var key = string.Format(CacheConst.WxBankListCacheKey, nameof(GetBankList), type);
            var result = await _redisClient.GetAsync<List<WxBanksData>>(key);

            if (result == null)
            {
                result = new List<WxBanksData>();
                var hasNext = false;
                var page = 1;
                var pageSize = 200;

                var serviceProviderEntity = await _paymentPlatformUtil.GetSystemPaymentServiceProvider(CommonConst.WechatPaymentId);
                var commonService = _wechatMerchantService.BuildMerchant(serviceProviderEntity.Id.ToString()).CommonService;
                do
                {
                    var offset = (page - 1) * pageSize;
                    WechatRestfulResponse<WxBankResponse> bankResp;
                    if (type == 1)
                    {
                        bankResp = await commonService.ToResponseAsync(a => a.GetCorporateBanksAsync(offset, pageSize));
                    }
                    else
                    {
                        bankResp = await commonService.ToResponseAsync(a => a.GetPersonalBanksAsync(offset, pageSize));
                    }
                    if (bankResp.Succeeded)
                        result.AddRange(bankResp.Data.Data);

                    hasNext = bankResp.Succeeded && bankResp.Data.TotalCount > page * pageSize;
                    page++;
                }
                while (hasNext);
                await _redisClient.SetAsync(key, result);
            }
            return result;
        }

        /// <summary>
        /// 获取银行支行列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<BankBranchItem>> GetBankBranchList(GetBankBranchListInput input)
        {
            var key = string.Format(CacheConst.WxBankListCacheKey, nameof(GetBankBranchList), $"{input.BankAliasCode}_{input.WxCityCode}");
            var result = await _redisClient.GetAsync<List<BankBranchItem>>(key);

            if (result == null)
            {
                result = new List<BankBranchItem>();
                var hasNext = false;
                var page = 1;
                var pageSize = 200;

                var serviceProviderEntity = await _paymentPlatformUtil.GetSystemPaymentServiceProvider(CommonConst.WechatPaymentId);
                var commonService = _wechatMerchantService.BuildMerchant(serviceProviderEntity.Id.ToString()).CommonService;
                do
                {
                    var offset = (page - 1) * pageSize;
                    var bankResp = await commonService.ToResponseAsync(a => a.GetBranchBankAsync(input.BankAliasCode, input.WxCityCode, offset, pageSize));
                    if (bankResp.Succeeded)
                        result.AddRange(bankResp.Data.Data);
                    hasNext = bankResp.Succeeded && bankResp.Data.TotalCount > page * pageSize;
                    page++;
                }
                while (hasNext);
                await _redisClient.SetAsync(key, result);
            }
            return result;
        }

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetProvinceCityOutput>> GetProvinces()
        {
            var list = await _redisClient.GetAsync<List<GetProvinceCityOutput>>(CacheConst.ProvinceCache);
            if (list == null)
            {
                list = await context.CountryRegion.Where(w => w.ParentID == null && w.CountryID == 1)
                    .Select(a => new GetProvinceCityOutput
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.RegionName
                    }).ToListAsync();

                if (list.Any())
                {
                    var serviceProviderEntity = await _paymentPlatformUtil.GetSystemPaymentServiceProvider(CommonConst.WechatPaymentId);
                    var wxDatas = await _wechatMerchantService
                        .BuildMerchant(serviceProviderEntity.Id.ToString())
                        .CommonService.GetAreaProvincesAsync();

                    if (wxDatas != null && wxDatas.TotalCount > 0)
                    {
                        list.ForEach(a =>
                        {
                            a.WxCode = (wxDatas.Data.FirstOrDefault(f => a.Name.StartsWith(f.ProvinceName))?.ProvinceCode) ?? "";
                        });
                    }
                }

                // TODO:后续可以优化，存本地缓存
                await _redisClient.SetAsync(CacheConst.ProvinceCache, list);
            }

            return list;
        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public async Task<List<GetProvinceCityOutput>> GetCitys(long provinceCode)
        {
            var key = string.Format(CacheConst.CityCache, provinceCode);
            var list = await _redisClient.GetAsync<List<GetProvinceCityOutput>>(key);
            if (list == null)
            {
                list = await context.CountryRegion.Where(w => w.ParentID == provinceCode && w.CountryID == 1)
                    .Select(a => new GetProvinceCityOutput
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.RegionName
                    }).ToListAsync();

                if (list.Any())
                {
                    var aaa = (await GetProvinces()).FirstOrDefault(a => a.Id == provinceCode);
                    var wxProvinceCode = (await GetProvinces()).FirstOrDefault(a => a.Id == provinceCode).WxCode;
                    if (!string.IsNullOrEmpty(wxProvinceCode))
                    {
                        var serviceProviderEntity = await _paymentPlatformUtil.GetSystemPaymentServiceProvider(CommonConst.WechatPaymentId);
                        var citys = await _wechatMerchantService
                            .BuildMerchant(serviceProviderEntity.Id.ToString())
                            .CommonService.GetAreaCitiesAsync(wxProvinceCode);

                        if (citys != null && citys.TotalCount > 0)
                        {
                            list.ForEach(a =>
                            {
                                a.WxCode = citys.Data.FirstOrDefault(f => a.Name.StartsWith(f.CityName))?.CityCode;
                            });
                        }
                    }
                }
                await _redisClient.SetAsync(key, list);
            }
            return list;
        }

        /// <summary>
        /// 获取区县列表
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public async Task<List<GetProvinceCityOutput>> GetAreas(long cityCode)
        {
            return await context.CountryRegion.Where(w => w.ParentID == cityCode && w.CountryID == 1)
                .Select(a => new GetProvinceCityOutput
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.RegionName
                }).ToListAsync();
        }
    }
}