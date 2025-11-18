using AutoMapper;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDomesticPaymentQueries;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.DomesticPaymentQueries
{
    /// <summary>
    /// 系统支付信息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="redisClient"></param>
    /// <param name="mapper"></param>
    /// <param name="userHttp"></param>
    /// <param name="_commonHelper"></param>
    /// <param name="paymentPlatformUtil"></param>
    public class SystemPaymentInfoQueries(CoffeeMachineDbContext context, IRedisClient redisClient, IMapper mapper, UserHttpContext userHttp, CommonHelper _commonHelper, PaymentPlatformUtil paymentPlatformUtil) : ISystemPaymentInfoQueries
    {
        /// <summary>
        /// 获取系统支付方式列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemPaymentMethodDto>> GetSystemPaymentMethodsAsync()
        {
            // 先从缓存获取
            var list = await redisClient.GetAsync<List<SystemPaymentMethodDto>>(CacheConst.SystemPaymentMethodsKey);
            if (list != null && list.Count > 0)
                return list;

            // 缓存没有则从数据库获取
            list = await (from a in context.SystemPaymentMethod
                          join b in context.SystemPaymentServiceProvider on a.PaymentPlatformId equals b.Id into ab
                          from b in ab.DefaultIfEmpty()
                          where a.IsEnabled == EnabledEnum.Enable
                          select new SystemPaymentMethodDto
                          {
                              Id = a.Id,
                              Name = a.Name,
                              PaymentImage = a.PaymentImage,
                              OnlinePayment = a.OnlinePayment,
                              OfflinePayment = a.OfflinePayment,
                              Country = a.Country,
                              PaymentPlatformId = a.PaymentPlatformId,
                              PaymentPlatformType = b.PaymentPlatformType
                          })
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

            // 存入缓存，设置永不过期，平台端修改之后会更新缓存
            await redisClient.SetAsync(CacheConst.SystemPaymentMethodsKey, list);

            return list;
        }

        /// <summary>
        /// 获取二级商户支付方式分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<M_PaymentMethodDto>> GetMachinePaymentMethodsAsync(M_PaymentMethodInput input)
        {
            var paymentPlatformType = await (from method in context.SystemPaymentMethod
                                             join service in context.SystemPaymentServiceProvider
                                             on method.PaymentPlatformId equals service.Id
                                             where method.Id == input.SystemPaymentMethodId
                                             select new { service.PaymentPlatformType })
                   .FirstOrDefaultAsync();
            if (paymentPlatformType == null) return null;

            return paymentPlatformType!.PaymentPlatformType switch
            {
                PaymentPlatformTypeEnum.Wechat => await GetMachineWechatPaymentMethodPageList(input),
                PaymentPlatformTypeEnum.Alipay => await GetMachineAlipayPaymentMethodPageList(input),
                _ => null
            };
        }

        /// <summary>
        /// 获取二级商户微信支付方法分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<M_PaymentMethodDto>> GetMachineWechatPaymentMethodPageList(M_PaymentMethodInput input)
        {
            return await (from a in context.M_PaymentMethod
                          join b in context.M_PaymentWechatApplyments on a.Id equals b.PaymentOriginId into ab
                          from b in ab.DefaultIfEmpty()
                          where a.SystemPaymentMethodId == input.SystemPaymentMethodId
                          select new M_PaymentMethodDto
                          {
                              Id = a.Id,
                              ApplymentId = b != null ? b.Id : null,
                              SystemPaymentMethodId = a.SystemPaymentMethodId,
                              Phone = a.Phone,
                              MerchantType = a.DomesticMerchantType,
                              PaymentMode = a.PaymentMode,
                              PaymentEntryStatus = a.PaymentEntryStatus,
                              BindType = a.BindType,
                              IsEnabled = a.IsEnabled,
                              Remark = a.Remark,
                              MerchantId = a.MerchantId,
                              SystemPaymentServiceProviderId = a.SystemPaymentServiceProviderId,
                              FlowStatus = b == null ? null : b.FlowStatus,
                              RejectReason = b.RejectReason,
                              SignUrl = b.SignUrl,
                              CreateTime = b == null ? a.CreateTime : b.CreateTime
                          }
                          )
                          .OrderByDescending(o => o.CreateTime)
                          .ToPagedListAsync(input);
        }

        /// <summary>
        /// 获取二级商户支付宝支付方法分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<M_PaymentMethodDto>> GetMachineAlipayPaymentMethodPageList(M_PaymentMethodInput input)
        {
            return await (from a in context.M_PaymentMethod
                          join b in context.M_PaymentAlipayApplyments on a.Id equals b.PaymentOriginId into ab
                          from b in ab.DefaultIfEmpty()
                          where a.SystemPaymentMethodId == input.SystemPaymentMethodId
                          select new M_PaymentMethodDto
                          {
                              Id = a.Id,
                              ApplymentId = b != null ? b.Id : null,
                              SystemPaymentMethodId = a.SystemPaymentMethodId,
                              Phone = a.Phone,
                              MerchantType = a.DomesticMerchantType,
                              PaymentMode = a.PaymentMode,
                              PaymentEntryStatus = a.PaymentEntryStatus,
                              BindType = a.BindType,
                              IsEnabled = a.IsEnabled,
                              Remark = a.Remark,
                              MerchantId = a.MerchantId,
                              SystemPaymentServiceProviderId = a.SystemPaymentServiceProviderId,
                              FlowStatus = b == null ? null : b.FlowStatus,
                              RejectReason = b.RejectReason,
                              CreateTime = b == null ? a.CreateTime : b.CreateTime
                          }
                          )
                          .OrderByDescending(o => o.CreateTime)
                          .ToPagedListAsync(input);
        }

        /// <summary>
        /// 获取微信进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<M_PaymentWechatApplymentsOutput> GetWechatApplymentsByIdAsync(long id)
        {
            var info = await context.M_PaymentWechatApplyments.FirstOrDefaultAsync(x => x.Id == id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 获取商户支付方式信息
            var mPaymentMethod = await context.M_PaymentMethod.FirstAsync(w => w.Id == info.PaymentOriginId);

            // 商户类型加到进件详情一起回显
            var data = mapper.Map<M_PaymentWechatApplymentsOutput>(info);
            data.MerchantType = mPaymentMethod.DomesticMerchantType;

            return data;
        }

        /// <summary>
        /// 获取支付宝进件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<M_PaymentAlipayApplymentsOutput> GetAlipayApplymentsByIdAsync(long id)
        {
            var info = await context.M_PaymentAlipayApplyments.FirstOrDefaultAsync(x => x.Id == id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 获取商户支付方式信息
            var mPaymentMethod = await context.M_PaymentMethod.FirstAsync(w => w.Id == info.PaymentOriginId);

            // 商户类型加到进件详情一起回显
            var data = mapper.Map<M_PaymentAlipayApplymentsOutput>(info);
            if (mPaymentMethod.DomesticMerchantType == DomesticMerchantTypeEnum.Individual)
                data.MerchantType = MerchantTypeEnum.IndividualBusiness;
            else
                data.MerchantType = MerchantTypeEnum.Enterprise;

            return data;
        }

        /// <summary>
        /// 当前支付方式未绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodUnBindDevicesAsync(PaymentMethodBindDeviceInput input)
        {
            var hasFilter = false;
            var hasDeviceIds = new List<long>();
            if (!userHttp.AllDeviceRole)
            {
                hasFilter = true;
                hasDeviceIds = await _commonHelper.GetDeviceIdsByUserId(userHttp.UserId);
            }

            var devices = await context.DeviceInfo
                    .WhereIf(input.DeviceGroupIds.Count > 0, a => context.GroupDevices.Any(b => a.Id == b.DeviceInfoId && input.DeviceGroupIds.Contains(b.GroupsId)))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), a => a.Name.Contains(input.DeviceName) || context.DeviceBaseInfo.Any(d => d.Id == a.DeviceBaseId && d.MachineStickerCode == input.DeviceName))
                    .WhereIf(hasFilter && hasDeviceIds != null, w => hasDeviceIds.Contains(w.Id))
                    .Where(di => !context.M_PaymentMethodBindDevice
                        .Any(pbd => (pbd.DeviceId == di.Id && pbd.PaymentMethodId == input.PaymentMethodId)
                        || (pbd.DeviceId == di.Id && pbd.SystemPaymentMethodId == input.SystemPaymentMethodId && pbd.PaymentMethodId != input.PaymentMethodId)) && di.DeviceBaseId > 0)
                     .Select(a => new M_PaymentMethodBindDeviceDto
                     {
                         DeviceId = a.Id,
                         PaymentMethodId = input.PaymentMethodId,
                         DeviceName = a.Name
                     }).ToPagedListAsync(input);

            // 获取设备ids
            var deviceIds = devices.Items.Select(a => a.DeviceId).ToList();

            // 获取设备列表
            var deviceInfos = await context.DeviceInfo.AsQueryable().Where(a => deviceIds.Contains(a.Id)).ToListAsync();

            // 获取设备BaseInfoIds
            var baseInfoIds = deviceInfos.Select(s => s.DeviceBaseId).ToList();

            // 获取设备基础信息
            var deviceBaseInfos = await context.DeviceBaseInfo.Where(w => baseInfoIds.Contains(w.Id)).ToDictionaryAsync(d => d.Id, d => new { d.Mid, d.MachineStickerCode, d.IsOnline });

            // 获取设备对应的Mid
            var deviceMids = new Dictionary<long, string>();
            var deviceCodes = new Dictionary<long, string>();
            var deviceIsOnlines = new Dictionary<long, bool>();
            foreach (var deviceInfo in deviceInfos)
            {
                if (deviceBaseInfos != null && deviceBaseInfos.ContainsKey(deviceInfo.DeviceBaseId))
                {
                    deviceMids.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].Mid);
                    deviceCodes.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].MachineStickerCode);
                    deviceIsOnlines.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].IsOnline);
                }
            }

            // 获取设备名字字典
            var deviceDic = deviceInfos.ToDictionary(a => a.Id, a => a.Name);

            // 获取设备分组信息字典
            var groupData = await (
                    from a in context.GroupDevices
                    join b in context.Groups on a.GroupsId equals b.Id into bJoin
                    from b in bJoin.DefaultIfEmpty()
                    where deviceIds.Contains(a.DeviceInfoId)
                    select new { a.DeviceInfoId, b }
                ).ToListAsync();
            var groupDict = groupData
            .GroupBy(x => x.DeviceInfoId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    GroupIds = g.Where(x => x.b != null).Select(x => x.b.Id).ToList(),
                    GroupNames = string.Join(",", g.Where(x => x.b != null).Select(x => x.b.Name))
                }
            );
            // 组装数据
            foreach (var item in devices.Items)
            {
                if (groupDict != null && groupDict.ContainsKey(item.DeviceId))
                {
                    item.GroupId = groupDict[item.DeviceId].GroupIds;
                    item.DeviceGroupName = groupDict[item.DeviceId].GroupNames;
                }

                if (deviceMids != null && deviceMids.ContainsKey(item.DeviceId))
                {
                    item.DeviceMid = deviceMids[item.DeviceId];
                    item.IsOnline = deviceIsOnlines[item.DeviceId];
                }

                if (deviceCodes != null && deviceCodes.ContainsKey(item.DeviceId))
                {
                    if (string.IsNullOrWhiteSpace(item.DeviceName))
                        item.DeviceName = deviceCodes[item.DeviceId];
                    item.MachineStickerCode = deviceCodes[item.DeviceId];
                }
            }
            return devices;
        }

        /// <summary>
        /// 当前支付方式绑定的设备分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<M_PaymentMethodBindDeviceDto>> GetPaymentMethodBindDevicesAsync(PaymentMethodBindDeviceInput input)
        {
            var hasFilter = false;
            var hasDeviceIds = new List<long>();
            if (!userHttp.AllDeviceRole)
            {
                hasFilter = true;
                hasDeviceIds = await _commonHelper.GetDeviceIdsByUserId(userHttp.UserId);
            }
            // 关联表分页
            var devicePyamentSettingInfo = await context.M_PaymentMethodBindDevice.AsQueryable()
                .WhereIf(input.DeviceGroupIds.Count > 0, a => context.GroupDevices.Any(b => a.DeviceId == b.DeviceInfoId && input.DeviceGroupIds.Contains(b.GroupsId)))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName),
                    a => context.DeviceInfo.Any(b => b.Id == a.Id && b.Name.Contains(input.DeviceName))
                    || context.DeviceInfo.Any(di => di.Id == a.DeviceId && context.DeviceBaseInfo.Any(d => d.Id == di.DeviceBaseId && d.MachineStickerCode == input.DeviceName)))
                .WhereIf(hasFilter && hasDeviceIds != null, w => hasDeviceIds.Contains(w.DeviceId))
                .Where(a => a.PaymentMethodId == input.PaymentMethodId && context.DeviceInfo.AsQueryable().AsNoTracking().Where(dw => dw.Id == a.DeviceId).Any())
                .Select(a => new M_PaymentMethodBindDeviceDto
                {
                    DeviceId = a.DeviceId,
                    PaymentMethodId = a.PaymentMethodId,
                }).ToPagedListAsync(input);

            // 获取设备ids
            var deviceIds = devicePyamentSettingInfo.Items.Select(a => a.DeviceId).ToList();

            // 获取设备列表
            var deviceInfos = await context.DeviceInfo.AsQueryable().AsNoTracking().Where(a => deviceIds.Contains(a.Id) && a.DeviceBaseId > 0).ToListAsync();

            // 获取设备BaseInfoIds
            var baseInfoIds = deviceInfos.Select(s => s.DeviceBaseId).ToList();

            // 获取设备基础信息
            var deviceBaseInfos = await context.DeviceBaseInfo.Where(w => baseInfoIds.Contains(w.Id)).ToDictionaryAsync(d => d.Id, d => new { d.Mid, d.MachineStickerCode, d.IsOnline });

            // 获取设备对应的Mid
            var deviceMids = new Dictionary<long, string>();
            var deviceCodes = new Dictionary<long, string>();
            var deviceIsOnlines = new Dictionary<long, bool>();
            foreach (var deviceInfo in deviceInfos)
            {
                if (deviceBaseInfos != null && deviceBaseInfos.ContainsKey(deviceInfo.DeviceBaseId))
                {
                    deviceMids.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].Mid);
                    deviceCodes.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].MachineStickerCode);
                    deviceIsOnlines.Add(deviceInfo.Id, deviceBaseInfos[deviceInfo.DeviceBaseId].IsOnline);
                }
            }

            // 获取设备名字字典
            var deviceDic = deviceInfos.ToDictionary(a => a.Id, a => a.Name);

            // 获取设备分组信息字典
            var groupData = await (
                    from a in context.GroupDevices
                    join b in context.Groups on a.GroupsId equals b.Id into bJoin
                    from b in bJoin.DefaultIfEmpty()
                    where deviceIds.Contains(a.DeviceInfoId)
                    select new { a.DeviceInfoId, b }
                ).ToListAsync();
            var groupDict = groupData
            .GroupBy(x => x.DeviceInfoId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    GroupIds = g.Where(x => x.b != null).Select(x => x.b.Id).ToList(),
                    GroupNames = string.Join(",", g.Where(x => x.b != null).Select(x => x.b.Name))
                }
            );

            // 组装数据
            foreach (var item in devicePyamentSettingInfo.Items)
            {
                if (deviceDic != null && deviceDic.ContainsKey(item.DeviceId) && deviceDic[item.DeviceId] != null)
                {
                    item.DeviceName = deviceDic[item.DeviceId];
                }
                if (groupDict != null && groupDict.ContainsKey(item.DeviceId))
                {
                    item.GroupId = groupDict[item.DeviceId].GroupIds;
                    item.DeviceGroupName = groupDict[item.DeviceId].GroupNames;
                }

                if (deviceMids != null && deviceMids.ContainsKey(item.DeviceId))
                {
                    item.DeviceMid = deviceMids[item.DeviceId];
                    item.IsOnline = deviceIsOnlines[item.DeviceId];
                }

                if (deviceCodes != null && deviceCodes.ContainsKey(item.DeviceId))
                {
                    if (string.IsNullOrWhiteSpace(item.DeviceName))
                        item.DeviceName = deviceCodes[item.DeviceId];
                    item.MachineStickerCode = deviceCodes[item.DeviceId];
                }
            }

            return devicePyamentSettingInfo;
        }

        /// <summary>
        /// 设备绑定的支付信息查询
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetDevicesBindPaymentMethodAsync(List<string> mids)
        {
            var resDict = new Dictionary<string, string>();
            var payList = await paymentPlatformUtil.GetDeviceBindPaymentMethodDtos(mids);
            if (payList != null && payList.Count > 0)
            {
                foreach (var item in payList)
                {
                    if (!resDict.ContainsKey(item.Mid))
                        resDict.Add(item.Mid, JsonConvert.SerializeObject(item.PaymentMethods));
                }
            }
            return resDict;
        }
    }
}