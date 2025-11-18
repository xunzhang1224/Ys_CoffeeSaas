using Aop.Api.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using YS.CoffeeMachine.API.Application.Services;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.OrderQueries
{
    /// <summary>
    /// 订单信息查询类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class OrderInfoQueries(CoffeeMachineDbContext context, UserHttpContext _user, IMapper mapper) : IOrderInfoQueries
    {
        #region PC

        /// <summary>
        /// 获取设备基础信息id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<long>> GetDeviceBaseIds()
        {
            var deviceBaseIds = new List<long>();
            //// 如果选定了指定设备

            //if (!_user.AllDeviceRole)
            //{
            var userInfo = await context.ApplicationUser.AsQueryable().Where(w => w.Id == _user.UserId).FirstOrDefaultAsync();
            if (userInfo.EnterpriseId == _user.TenantId)
            {
                deviceBaseIds = await context.DeviceInfo.AsNoTracking().Where(w => w.EnterpriseinfoId == _user.TenantId
            && (w.DeviceUserAssociations.Select(s => s.UserId).Contains(_user.UserId)
            || (context.GroupDevices.Any(b => b.DeviceInfoId == w.Id && context.GroupUsers.Any(g => g.GroupsId == b.GroupsId && g.ApplicationUserId == _user.UserId)))))
                    .Select(s => s.DeviceBaseId).Distinct().ToListAsync();
            }
            //}

            return deviceBaseIds;
        }

        /// <summary>
        /// 查询订单总计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderTotalInfo> GetOrderTotalInfoAsyncOld(OrderTotalInfoInput input)
        {
            if (input == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            if (input.CreateTimes == null || input.CreateTimes.Count != 2)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0084)]);

            return await context.OrderInfo.AsQueryable()
                .WhereIf(!_user.AllDeviceRole, w => GetDeviceBaseIds().Result.Contains(w.DeviceBaseId))
                .Where(w =>
                    w.CreateTime >= input.CreateTimes[0] && w.CreateTime <= input.CreateTimes[1]
                    && ((w.OrderType == OrderTypeEnum.OnlineOrder
                    && w.OrderStatus != OrderStatusEnum.PaymentInProgress
                    && w.OrderStatus != OrderStatusEnum.Fail
                    && w.OrderStatus != OrderStatusEnum.CancelPayment)
                    || (w.OrderType != OrderTypeEnum.Not && w.OrderType != OrderTypeEnum.OnlineOrder))
                )
                .GroupBy(g => 1)
                .Select(s => new OrderTotalInfo()
                {
                    TotalAmount = s.Sum(a => a.Amount),
                    TotalReturnAmount = s.Sum(a => a.ReturnAmount)
                }).FirstOrDefaultAsync() ?? new OrderTotalInfo() { TotalAmount = 0, TotalReturnAmount = 0 };
        }

        /// <summary>
        /// 查询订单总计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderTotalInfo> GetOrderTotalInfoAsync(OrderInfoInput input)
        {
            if (input == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            if (input.CreateTimes == null || input.CreateTimes.Count != 2)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0084)]);

            var query01 = from order in context.OrderInfo
                          join db in context.DeviceBaseInfo on order.DeviceBaseId equals db.Id into dbGroup
                          from db in dbGroup.DefaultIfEmpty()
                          join currency in context.Currency on order.CurrencyCode equals currency.Code into dbCurrency
                          from currency in dbCurrency.DefaultIfEmpty()
                              //join di in context.DeviceInfo on db.Id equals di.DeviceBaseId into diGroup
                              //from di in diGroup.DefaultIfEmpty()

                              //join dm in context.DeviceModel on di.DeviceModelId equals dm.Id into dmGroup
                              //from dm in dmGroup.DefaultIfEmpty()

                              //join ei in context.EnterpriseInfo on order.EnterpriseinfoId equals ei.Id into eiGroup
                              //from ei in eiGroup.DefaultIfEmpty()

                          join od in context.OrderDetails on order.Id equals od.OrderId into odGroup
                          //from od in odGroup.DefaultIfEmpty()

                          select new
                          {
                              order,
                              db,
                              currency,
                              //di,
                              //dm,
                              //ei,
                              odGroup
                          };

            if (!_user.AllDeviceRole)
            {
                var deviceBaseIds = await GetDeviceBaseIds();
                query01 = query01.Where(w => deviceBaseIds.Contains(w.db.Id));
            }

            var resual = await query01
             //.AsEnumerable()
             //.WhereIf(!string.IsNullOrWhiteSpace(input.BeverageName), w => w.odGroup.Select(a => a.BeverageName).Contains(input.BeverageName))
             .WhereIf(!string.IsNullOrWhiteSpace(input.BeverageName), w => w.odGroup.Any(a => a.BeverageName != null && a.BeverageName.Contains(input.BeverageName)))
             .WhereIf(input.ShipmentResult != null, w => w.order.ShipmentResult == input.ShipmentResult)
             .WhereIf(input.SaleResult != null, w => w.order.SaleResult == input.SaleResult)
             .WhereIf(!string.IsNullOrWhiteSpace(input.Provider), w => w.order.Provider == input.Provider)
             .WhereIf(input.PayTimes != null && input.PayTimes.Count == 2, w => w.order.PayTimeSp >= new DateTimeOffset(input.PayTimes![0]).ToUnixTimeMilliseconds() && w.order.PayTimeSp <= new DateTimeOffset(input.PayTimes[1]).ToUnixTimeMilliseconds())
             .WhereIf(!string.IsNullOrWhiteSpace(input.ThirdOrderNo), w => w.order.ThirdOrderNo == input.ThirdOrderNo)
             .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNo), w => w.order.Code == input.OrderNo || w.order.BizNo == input.OrderNo)
             .WhereIf(!string.IsNullOrWhiteSpace(input.EnterpriseName), w => w.order.BaseEnterpriseName.Contains(input.EnterpriseName!))
             .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), w => w.order.BaseDeviceName.Contains(input.DeviceName!))
             .WhereIf(string.IsNullOrWhiteSpace(input.DeviceCode) == false, w => w.db.MachineStickerCode == input.DeviceCode)
             .WhereIf(input.DeviceModelId != null && input.DeviceModelId > 0, w => w.order.BaseDeviceModelId == input.DeviceModelId)
             .WhereIf(input.IsMakeRecord != null && input.IsMakeRecord == true, w => w.order.Provider == "OTHER")
             .WhereIf(input.IsMakeRecord != null && input.IsMakeRecord != true, w => w.order.Provider != "OTHER")
             .Where(w => w.order.CreateTime >= input.CreateTimes[0] && w.order.CreateTime <= input.CreateTimes[1]
              && ((w.order.OrderType == OrderTypeEnum.OnlineOrder
                    && w.order.OrderStatus != OrderStatusEnum.PaymentInProgress
                    && w.order.OrderStatus != OrderStatusEnum.Fail
                    && w.order.OrderStatus != OrderStatusEnum.CancelPayment)
                    || (w.order.OrderType != OrderTypeEnum.Not && w.order.OrderType != OrderTypeEnum.OnlineOrder)))
              .GroupBy(g => 1)
                .Select(s => new OrderTotalInfo()
                {
                    TotalAmount = s.Sum(a => a.order.Amount),
                    TotalReturnAmount = s.Sum(a => a.order.ReturnAmount)
                }).FirstOrDefaultAsync() ?? new OrderTotalInfo() { TotalAmount = 0, TotalReturnAmount = 0 };

            return resual;

            return await context.OrderInfo.AsQueryable()
                .WhereIf(!_user.AllDeviceRole, w => GetDeviceBaseIds().Result.Contains(w.DeviceBaseId))
                .Where(w =>
                    w.CreateTime >= input.CreateTimes[0] && w.CreateTime <= input.CreateTimes[1]
                    && ((w.OrderType == OrderTypeEnum.OnlineOrder
                    && w.OrderStatus != OrderStatusEnum.PaymentInProgress
                    && w.OrderStatus != OrderStatusEnum.Fail
                    && w.OrderStatus != OrderStatusEnum.CancelPayment)
                    || (w.OrderType != OrderTypeEnum.Not && w.OrderType != OrderTypeEnum.OnlineOrder))
                )
                .GroupBy(g => 1)
                .Select(s => new OrderTotalInfo()
                {
                    TotalAmount = s.Sum(a => a.Amount),
                    TotalReturnAmount = s.Sum(a => a.ReturnAmount)
                }).FirstOrDefaultAsync() ?? new OrderTotalInfo() { TotalAmount = 0, TotalReturnAmount = 0 };
        }

        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderInfoDto>> GetOrderInfosPageAsync(OrderInfoInput input)
        {
            if (input == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            if (input.CreateTimes == null || input.CreateTimes.Count != 2)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0084)]);

            var query01 = from order in context.OrderInfo
                          join db in context.DeviceBaseInfo on order.DeviceBaseId equals db.Id into dbGroup
                          from db in dbGroup.DefaultIfEmpty()
                          join currency in context.Currency on order.CurrencyCode equals currency.Code into dbCurrency
                          from currency in dbCurrency.DefaultIfEmpty()
                              //join di in context.DeviceInfo on db.Id equals di.DeviceBaseId into diGroup
                              //from di in diGroup.DefaultIfEmpty()

                              //join dm in context.DeviceModel on di.DeviceModelId equals dm.Id into dmGroup
                              //from dm in dmGroup.DefaultIfEmpty()

                              //join ei in context.EnterpriseInfo on order.EnterpriseinfoId equals ei.Id into eiGroup
                              //from ei in eiGroup.DefaultIfEmpty()

                          join od in context.OrderDetails on order.Id equals od.OrderId into odGroup
                          //from od in odGroup.DefaultIfEmpty()

                          select new
                          {
                              order,
                              db,
                              currency,
                              //di,
                              //dm,
                              //ei,
                              odGroup
                          };

            if (!_user.AllDeviceRole)
            {
                var deviceBaseIds = await GetDeviceBaseIds();
                query01 = query01.Where(w => deviceBaseIds.Contains(w.db.Id));
            }

            var resual = await query01
                //.AsEnumerable()
                //.WhereIf(!string.IsNullOrWhiteSpace(input.BeverageName), w => w.odGroup.Select(a => a.BeverageName).Contains(input.BeverageName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BeverageName), w => w.odGroup.Any(a => a.BeverageName != null && a.BeverageName.Contains(input.BeverageName)))
                .WhereIf(input.ShipmentResult != null, w => w.order.ShipmentResult == input.ShipmentResult)
                .WhereIf(input.SaleResult != null, w => w.order.SaleResult == input.SaleResult)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Provider), w => w.order.Provider == input.Provider)
                .WhereIf(input.PayTimes != null && input.PayTimes.Count == 2, w => w.order.PayTimeSp >= new DateTimeOffset(input.PayTimes![0]).ToUnixTimeMilliseconds() && w.order.PayTimeSp <= new DateTimeOffset(input.PayTimes[1]).ToUnixTimeMilliseconds())
                .WhereIf(!string.IsNullOrWhiteSpace(input.ThirdOrderNo), w => w.order.ThirdOrderNo == input.ThirdOrderNo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNo), w => w.order.Code == input.OrderNo || w.order.BizNo == input.OrderNo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.EnterpriseName), w => w.order.BaseEnterpriseName.Contains(input.EnterpriseName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceName), w => w.order.BaseDeviceName.Contains(input.DeviceName!))
                .WhereIf(string.IsNullOrWhiteSpace(input.DeviceCode) == false, w => w.db.MachineStickerCode == input.DeviceCode)
                .WhereIf(input.DeviceModelId != null && input.DeviceModelId > 0, w => w.order.BaseDeviceModelId == input.DeviceModelId)
                .WhereIf(input.IsMakeRecord != null && input.IsMakeRecord == true, w => w.order.Provider == "OTHER")
                .WhereIf(input.IsMakeRecord != null && input.IsMakeRecord != true, w => w.order.Provider != "OTHER")
                .Where(w => w.order.CreateTime >= input.CreateTimes[0] && w.order.CreateTime <= input.CreateTimes[1])
                .Select(x => new OrderInfoDto()
                {
                    Id = x.order.Id,
                    DeviceName = x.order.BaseDeviceName == null ? x.db.MachineStickerCode : x.order.BaseDeviceName,
                    DeviceCode = x.db.MachineStickerCode,
                    DeviceModelName = x.order.BaseDeviceModelName,
                    BeverageName = string.Join(",", x.odGroup.Select(o => o.BeverageName).Where(i => i != null)),
                    EnterpriseId = x.order.EnterpriseinfoId,
                    EnterpriseName = x.order.BaseEnterpriseName,
                    Amount = x.order.Amount,
                    CurrencyCode = x.order.CurrencyCode,
                    CurrencySymbol = x.currency != null ? x.currency.CurrencySymbol : "￥",
                    CurrencyShowFormat = x.currency != null ? x.currency.CurrencyShowFormat : CurrencyShowFormatEnum.CurrencyBefore,
                    Provider = x.order.Provider,
                    PayTime = x.order.PayTimeSp,
                    ShipmentResult = x.order.ShipmentResult,
                    SaleResult = x.order.SaleResult,
                    OrderType = x.order.OrderType,
                    CreateTime = x.order.CreateTime,
                    OrderNo = x.order.Code,//系统OrderNo
                    ThirdOrderNo = x.order.ThirdOrderNo,
                    ReturnAmount = x.order.ReturnAmount,
                    OrderStatus = x.order.OrderStatus,
                    Count = x.odGroup.Count()
                })
                .OrderByDescending(o => o.CreateTime)
                .ToPagedListAsync(input);

            resual.Items.ForEach(e =>
            {
                e.Provider = EnumHelper.ParseFromString<PayTypeEnum>(e.Provider.ToString()).GetDescriptionOrValue();
            });

            return resual;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<OrderDetailsDto>> GetOrderDetailsByOrderIdAsync(long id)
        {
            var query =
                from od in context.OrderDetails
                join odm in context.OrderDetaliMaterial
                    on od.Id equals odm.OrderDetailsId into odmGrp
                from odm in odmGrp.DefaultIfEmpty()
                join dmi in context.DeviceMaterialInfo
                    on odm.DeviceMaterialInfoId equals dmi.Id into dmiGrp
                from dmi in dmiGrp.DefaultIfEmpty()
                where od.OrderId == id // 添加过滤条件
                select new
                {
                    OrderDetail = new
                    {
                        od.ItemCode,
                        od.Price,
                        od.Quantity,
                        od.Result,
                        od.Error,
                        od.ErrorDescription,
                        od.ActionTimeSp,
                        od.BeverageInfoData,
                    },
                    MaterialId = (long?)dmi.Id,
                    MaterialName = dmi != null ? dmi.Name : null,
                    MType = dmi != null ? dmi.Type : MaterialTypeEnum.Not,
                    MIndex = dmi != null ? dmi.Index : 0,
                    IsSugar = dmi != null ? dmi.IsSugar : false,
                    Value = odm != null ? odm.Value : 0
                };

            var items = await query.AsNoTracking().ToListAsync();

            return items
                .GroupBy(x => x.OrderDetail)
                .Select(g => new OrderDetailsDto
                {
                    ItemCode = g.Key.ItemCode,
                    Price = g.Key.Price,
                    Quantity = g.Key.Quantity,
                    Result = g.Key.Result,
                    Error = g.Key.Error,
                    ErrorDescription = g.Key.ErrorDescription,
                    ActionTimeSp = g.Key.ActionTimeSp,
                    BeverageInfo = g.Key.BeverageInfoData,
                    MaterialUsageDtos = g
                        .Where(x => x.MaterialId.HasValue)
                        .GroupBy(x => new { x.MaterialId, x.MaterialName, x.MType, x.MIndex, x.IsSugar })
                        .Select(s => new MaterialUsageDto()
                        {
                            MaterialType = s.Key.MType,
                            Name = s.Key.MaterialName,
                            Usage = s.Sum(v => v.Value),
                            Unit = GetUnitByMaterialType(s.Key.MType),
                            IsSugar = s.Key.IsSugar,
                        }).ToList()
                })
                .ToList();
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
                MaterialTypeEnum.CoffeeBean => "g",
                MaterialTypeEnum.Water => "ml",
                MaterialTypeEnum.Cassette => "g",
                MaterialTypeEnum.Cup => "个",
                MaterialTypeEnum.CupCover => "个",
                _ => "未知单位"
            };
        }

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<OrderRefundDetailListDto>> GetOrderRefundDetailListByOrderIdAsync(long id)
        {
            // 获取已退款信息列表(不包含退款失败的)
            var orderRefundList = await context.OrderRefund.Where(w => w.OrderId == id.ToString() && w.RefundStatus != RefundStatusEnum.Fail).ToListAsync();

            // 获取子订单信息
            var orderDetailList = await context.OrderDetails.Where(w => w.OrderId == id)
                .Select(w => new OrderRefundDetailListDto()
                {
                    OrderDetailId = w.Id,
                    ItemCode = w.ItemCode,
                    Name = w.BeverageName,
                    Quantity = w.Quantity,
                    Price = w.Price,
                    MainImage = w.Url
                })
                .ToListAsync();

            if (orderDetailList == null || orderDetailList.Count == 0)
                return new List<OrderRefundDetailListDto>();

            var orderInfo = await context.OrderInfo.FirstOrDefaultAsync(w => w.Id == id);

            //var deviceBaseInfo = await context.DeviceBaseInfo.FirstOrDefaultAsync(w => w.Id == orderInfo.DeviceBaseId);

            //var deviceInfo = await context.DeviceInfo.FirstOrDefaultAsync(w => w.DeviceBaseId == deviceBaseInfo.Id);

            // 获取sku集合、饮品信息
            //var skus = orderDetailList.Select(s => s.ItemCode).ToList();
            //var beverageDics = await context.BeverageInfo.Where(w => skus.Contains(w.Code) && w.DeviceId == deviceInfo.Id).Distinct().ToDictionaryAsync(k => k.Code, v => v.BeverageIcon);

            orderDetailList.ForEach(e =>
            {
                //e.MainImage = beverageDics.Any(w => w.Key == e.ItemCode) ? beverageDics[e.ItemCode!] : string.Empty;
                e.RefundAmount = orderRefundList.Any(w => w.OrderDetailId == e.OrderDetailId.ToString()) ? orderRefundList.Where(f => f.OrderDetailId == e.OrderDetailId.ToString())?.Sum(s => s.RefundAmount) ?? 0 : 0;
            });

            return orderDetailList;
        }

        /// <summary>
        /// 根据主订单Id获取退款分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<OrderRefundPageDto>> GetOrderRefundPageAsync(OrderRefundInput input)
        {
            return context.OrderRefund.AsNoTracking()
                .Where(w => w.OrderId == input.orderId.ToString())
                .Select(s => new OrderRefundPageDto()
                {
                    OrderDetailId = s.OrderId,
                    ItemCode = s.ItemCode,
                    Name = s.Name,
                    MainImage = s.MainImage,
                    RefundAmount = s.RefundAmount,
                    RefundReason = s.RefundReason,
                    RefundStatus = s.RefundStatus,
                    HandlingMethod = s.HandlingMethod,
                }).ToPagedListAsync(input);
        }
        #endregion

        #region H5

        /// <summary>
        /// H5订单分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<H5OrderInfoDto>> GetH5OrderInfosPageAsync(H5OrderInfoInput input)
        {
            if (input == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            if (input.CreateTimes == null || input.CreateTimes.Count != 2)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0084)]);

            var query01 = from order in context.OrderInfo
                          join db in context.DeviceBaseInfo on order.DeviceBaseId equals db.Id into dbGroup
                          from db in dbGroup.DefaultIfEmpty()
                          join currency in context.Currency on order.CurrencyCode equals currency.Code into dbCurrency
                          from currency in dbCurrency.DefaultIfEmpty()
                          select new
                          {
                              order,
                              db,
                              currency
                          };

            if (!_user.AllDeviceRole)
            {
                var deviceBaseIds = await GetDeviceBaseIds();
                query01 = query01.Where(w => deviceBaseIds.Contains(w.db.Id));
            }

            var orders = await query01
                .WhereIf(input.ShipmentResult != null, w => w.order.ShipmentResult == input.ShipmentResult)
                .WhereIf(input.SaleResult != null, w => w.order.SaleResult == input.SaleResult)
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNo), w => w.order.Code == input.OrderNo || w.order.BizNo == input.OrderNo)
                .WhereIf(input.BaseDeviceId != null && input.BaseDeviceId > 0, w => w.db.Id == input.BaseDeviceId)
                .Where(w => w.order.CreateTime >= input.CreateTimes[0] && w.order.CreateTime <= input.CreateTimes[1])
                .Select(x => new H5OrderInfoDto()
                {
                    Id = x.order.Id,
                    DeviceName = x.order.BaseDeviceName == null ? x.db.MachineStickerCode : x.order.BaseDeviceName,
                    DeviceCode = x.db.MachineStickerCode,
                    Amount = x.order.Amount,
                    CurrencyCode = x.order.CurrencyCode,
                    CurrencySymbol = x.currency != null ? x.currency.CurrencySymbol : "￥",
                    SaleResult = x.order.SaleResult,
                    CreateTime = x.order.CreateTime,
                    OrderNo = x.order.Code,// 系统OrderNo
                    ThirdOrderNo = x.order.ThirdOrderNo
                })
                .OrderByDescending(o => o.CreateTime)
                .ToPagedListAsync(input);

            if (orders == null || orders.Items == null || orders.Items.Count == 0)
                return orders;

            var orderIds = orders.Items.Select(s => s.Id).ToList();
            var orderDetails = await context.OrderDetails.Where(w => orderIds.Contains(w.OrderId))
                .Select(s => new
                {
                    s.OrderId,
                    s.BeverageName,
                    s.Price,
                    s.Quantity,
                    s.Result,
                    s.Url
                })
                .ToListAsync();

            // 拼装订单饮品信息
            foreach (var item in orders.Items)
            {
                var details = orderDetails.Where(w => w.OrderId == item.Id).ToList();
                if (details.Any())
                {
                    details.ForEach(f =>
                    {
                        item.Products.Add(new OrderProducts()
                        {
                            ProductName = f.BeverageName,
                            ProductIcon = f.Url,
                            Price = f.Price,
                            Quantity = f.Quantity,
                            Result = f.Result
                        });
                    });
                }
            }

            return orders;
        }

        /// <summary>
        /// 获取H5订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<H5OrderDetailsDto> GetH5OrderRefundDetailsByOrderIdAsync(long id)
        {
            var orderQuery = from order in context.OrderInfo
                             join db in context.DeviceBaseInfo on order.DeviceBaseId equals db.Id into dbGroup
                             from db in dbGroup.DefaultIfEmpty()
                             join currency in context.Currency on order.CurrencyCode equals currency.Code into dbCurrency
                             from currency in dbCurrency.DefaultIfEmpty()
                             select new
                             {
                                 order,
                                 db,
                                 currency
                             };

            if (!_user.AllDeviceRole)
            {
                var deviceBaseIds = await GetDeviceBaseIds();
                orderQuery = orderQuery.Where(w => deviceBaseIds.Contains(w.db.Id));
            }

            var orderInfo = await orderQuery
                .Where(w => w.order.Id == id)
                .Select(x => new H5OrderDetailsDto()
                {
                    OrderNo = x.order.Code,// 系统OrderNo
                    ThirdOrderNo = x.order.ThirdOrderNo,
                    EnterpriseId = x.order.EnterpriseinfoId,
                    EnterpriseName = x.order.BaseEnterpriseName,
                    DeviceName = x.order.BaseDeviceName,
                    DeviceCode = x.db.MachineStickerCode,
                    OrderType = x.order.OrderType,
                    Provider = x.order.Provider,
                    SaleResult = x.order.SaleResult,
                    PayAmount = x.order.Amount,
                    CurrencyCode = x.order.CurrencyCode,
                    CurrencySymbol = x.currency != null ? x.currency.CurrencySymbol : "￥",
                    CreateTimeStr = x.order.CreateTime.ToString("G"),
                    PayTime = x.order.PayTimeSp,
                })
                .FirstOrDefaultAsync();

            orderInfo.Provider = EnumHelper.ParseFromString<PayTypeEnum>(orderInfo.Provider.ToString()).GetDescriptionOrValue();

            var orderDetails = await context.OrderDetails.Where(w => w.OrderId == id)
                .Select(f => new OrderProducts
                {
                    ProductName = f.BeverageName,
                    ProductIcon = f.Url,
                    Price = f.Price,
                    Quantity = f.Quantity,
                    Result = f.Result
                })
                .ToListAsync();

            // 拼装订单饮品信息
            if (orderInfo != null && orderDetails.Any())
                orderInfo.Products.AddRange(orderDetails);

            return orderInfo;
        }

        /// <summary>
        /// 根据主订单Id获取订单退款列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<H5OrderRefundDetailListDto>> GetH5OrderRefundDetailListByOrderIdAsync(long id)
        {
            // 获取已退款信息列表(不包含退款失败的)
            var orderRefundList = await context.OrderRefund.Where(w => w.OrderId == id.ToString() && w.RefundStatus != RefundStatusEnum.Fail).ToListAsync();

            // 获取子订单信息
            var orderDetailList = await context.OrderDetails.Where(w => w.OrderId == id)
                .Select(w => new H5OrderRefundDetailListDto()
                {
                    OrderDetailId = w.Id,
                    ItemCode = w.ItemCode,
                    Name = w.BeverageName,
                    Quantity = w.Quantity,
                    Price = w.Price,
                })
                .ToListAsync();

            if (orderDetailList == null || orderDetailList.Count == 0)
                return new List<H5OrderRefundDetailListDto>();

            var orderInfo = await context.OrderInfo.FirstOrDefaultAsync(w => w.Id == id);

            var deviceBaseInfo = await context.DeviceBaseInfo.FirstOrDefaultAsync(w => w.Id == orderInfo.DeviceBaseId);

            var deviceInfo = await context.DeviceInfo.FirstOrDefaultAsync(w => w.DeviceBaseId == deviceBaseInfo.Id);

            // 获取sku集合、饮品信息
            var skus = orderDetailList.Select(s => s.ItemCode).ToList();
            var beverageDics = await context.BeverageInfo.Where(w => skus.Contains(w.Code) && w.DeviceId == deviceInfo.Id).Distinct().ToDictionaryAsync(k => k.Code, v => v.BeverageIcon);

            orderDetailList.ForEach(e =>
            {
                e.MainImage = beverageDics.Any(w => w.Key == e.ItemCode) ? beverageDics[e.ItemCode!] : string.Empty;
                e.RefundAmount = orderRefundList.Any(w => w.OrderDetailId == e.OrderDetailId.ToString()) ? orderRefundList.Where(f => f.OrderDetailId == e.OrderDetailId.ToString())?.Sum(s => s.RefundAmount) ?? 0 : 0;
            });

            return orderDetailList;
        }
        #endregion
    }
}