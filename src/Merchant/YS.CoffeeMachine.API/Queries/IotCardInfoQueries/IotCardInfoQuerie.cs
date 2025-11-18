using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.IotCardDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.IServices.ILotCartService;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.IIotCardInfoQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.API.Queries.IotCardInfoQueries
{
    /// <summary>
    /// 物联网卡信息查询
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="_redis"></param>
    /// <param name="_lotCardApi"></param>
    /// <param name="deviceInfoQueries"></param>
    public class IotCardInfoQuerie(CoffeeMachineDbContext dbContext, IRedisService _redis, ILotCardApi _lotCardApi, IDeviceInfoQueries deviceInfoQueries) : IIotCardInfoQuerie
    {
        /// <summary>
        /// 缓存物联网卡id
        /// </summary>
        /// <param name="iccid"></param>
        /// <returns></returns>
        private static string GetIotCardKey(string iccid) => $"/iotcard/{iccid}";

        /// <summary>
        /// 获取物联网卡分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VendIotCardOut>> GetVendIotCardPageAsync(IotCardQueryInput input)
        {
            // 获取当前用户设备信息
            var deviceInfos = await deviceInfoQueries.GetDeviceByUser();
            var deviceIds = deviceInfos.Select(s => s.DeviceId).ToList();

            var query = from a in dbContext.DeviceInfo.AsNoTracking()
                        join b in dbContext.DeviceBaseInfo.AsNoTracking() on a.DeviceBaseId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        join c in dbContext.DeviceAttribute.AsNoTracking() on a.DeviceBaseId equals c.DeviceBaseId into ac
                        from c in ac.DefaultIfEmpty()
                        where c.Key == "ICCID" && c.Value != null
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.DeviceBaseId,
                            c.Value,
                            b.MachineStickerCode
                        };

            var iotInfoRes = await query
                .Where(w => !string.IsNullOrWhiteSpace(w.Value) && deviceIds.Contains(w.Id))
                .WhereIf(!string.IsNullOrWhiteSpace(input.iccid), w => w.Value.Contains(input.iccid!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.deviceName), w => w.Name == input.deviceName)
                .WhereIf(!string.IsNullOrWhiteSpace(input.code), w => w.MachineStickerCode == input.code)
                .Select(s => new VendIotCardOut
                {
                    DeviceId = s.Id,
                    DeviceName = s.Name,
                    ICCID = s.Value!,
                    MachineStickerCode = s.MachineStickerCode,
                })
                .ToPagedListAsync(input);

            // 获取物联网卡套餐信息
            var iotCardInput = new List<IotCardInput>();
            foreach (var item in iotInfoRes.Items)
            {
                // 判断缓存是否存在PolicyID
                var cacheKey = GetIotCardKey(item.ICCID);
                var policyId = await _redis.GetStringAsync(cacheKey);
                if (!string.IsNullOrWhiteSpace(policyId))
                {
                    iotCardInput.Add(new IotCardInput { Iccid = item.ICCID, PolicyID = policyId });
                    continue;
                }

                // 调用物联网卡服务获取PolicyID
                var res = await _lotCardApi.GetBestPackage(item.ICCID);
                if (res != null && res.Succeeded == true && res.Data != null && res.Data.Data != null)
                {
                    iotCardInput.Add(new IotCardInput { Iccid = item.ICCID, PolicyID = res.Data.Data.PolicyId.ToString() });

                    // 缓存PolicyID
                    await _redis.SetStringAsync(cacheKey, res.Data.Data.PolicyId.ToString(), TimeSpan.FromDays(CommonConst.IotCardPolicyCacheDays));
                }
            }

            // 获取物联网卡流量信息
            var iotCardRes = await _lotCardApi.GetPoclicyDetail(iotCardInput);

            // 组装返回结果898604C91024C0447026
            foreach (var item in iotInfoRes.Items)
            {
                var iotCardInfo = iotCardRes?.Data?.Data?.FirstOrDefault(f => f.iccid == item.ICCID.ToUpper());
                if (iotCardInfo != null)
                {
                    item.FlowUsed = iotCardInfo.flowUsed;
                    item.RechargeNum = iotCardInfo.rechargeNum;
                    item.FlowMaxLimited = iotCardInfo.flowMaxLimited;
                    item.PackDiscount = iotCardInfo.packDiscount;
                    item.CardStatus = Util.Core.Util.GetLotCardDescription(iotCardInfo.cardStatus);
                    item.PolicyCode = iotCardInfo.policyCode;
                    //item.PoclicyDetail = iotCardInfo;
                    item.activeDate = iotCardInfo.activeDate;
                    item.expiryDate = iotCardInfo.expiryDate;
                }
            }

            return iotInfoRes;
        }
    }
}