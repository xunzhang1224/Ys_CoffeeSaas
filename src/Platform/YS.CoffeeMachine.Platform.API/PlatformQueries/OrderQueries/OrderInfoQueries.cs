using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IOrderQueries;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Application.Services;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.OrderQueries
{
    /// <summary>
    /// 订单信息查询类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    /// <param name="mapper"></param>
    public class OrderInfoQueries(CoffeeMachineDbContext context, UserHttpContext _user, IMapper mapper) : IOrderInfoQueries
    {
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

            var query01 = from order in context.OrderInfo.IgnoreQueryFilters()
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

            //if (!_user.AllDeviceRole)
            //{
            //    var deviceBaseIds = await GetDeviceBaseIds();
            //    query01 = query01.Where(w => deviceBaseIds.Contains(w.db.Id));
            //}

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
                .Where(w => w.order.CreateTime >= input.CreateTimes[0] && w.order.CreateTime <= input.CreateTimes[1])
                .Select(x => new OrderInfoDto()
                {
                    Id = x.order.Id,
                    DeviceName = x.order.BaseDeviceName,
                    DeviceCode = x.db.MachineStickerCode,
                    DeviceModelName = x.order.BaseDeviceModelName,
                    BeverageName = string.Join(",", x.odGroup.Select(o => o.BeverageName).Where(i => i != null)),
                    EnterpriseId = x.order.EnterpriseinfoId,
                    EnterpriseName = x.order.BaseEnterpriseName,
                    Amount = x.order.Amount,
                    CurrencyCode = x.order.CurrencyCode,
                    CurrencySymbol = x.currency != null ? x.currency.CurrencySymbol : "￥",
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
            // 先查询退款数据，按 OrderDetailId 分组求和
            var refundQuery =
                from ord in context.OrderRefund.IgnoreQueryFilters()
                where ord.OrderDetailId != null
                group ord by ord.OrderDetailId into g
                select new
                {
                    OrderDetailId = g.Key,
                    TotalRefundAmount = (decimal?)g.Sum(x => x.RefundAmount) // 显式声明为可空类型
                };

            var query =
                from od in context.OrderDetails.IgnoreQueryFilters()
                join odm in context.OrderDetaliMaterial
                    on od.Id equals odm.OrderDetailsId into odmGrp
                from odm in odmGrp.DefaultIfEmpty()
                join dmi in context.DeviceMaterialInfo.IgnoreQueryFilters()
                    on odm.DeviceMaterialInfoId equals dmi.Id into dmiGrp
                from dmi in dmiGrp.DefaultIfEmpty()
                join refund in refundQuery
                    on od.Id.ToString() equals refund.OrderDetailId into refundGrp
                from refund in refundGrp.DefaultIfEmpty()
                where od.OrderId == id
                select new
                {
                    OrderDetail = new
                    {
                        od.Id, // 添加 Id 用于分组
                        od.BeverageName,
                        od.ItemCode,
                        od.Price,
                        od.Quantity,
                        od.Result,
                        od.Error,
                        od.ErrorDescription,
                        od.ActionTimeSp,
                        od.BeverageInfoData,
                        ReturnAmount = refund != null && refund.TotalRefundAmount.HasValue ? refund.TotalRefundAmount.Value : 0
                    },
                    MaterialId = (long?)dmi.Id,
                    MaterialName = dmi != null ? dmi.Name : null,
                    MType = dmi != null ? dmi.Type : MaterialTypeEnum.Not,
                    MIndex = dmi != null ? dmi.Index : 0,
                    IsSugar = dmi != null ? dmi.IsSugar : false,
                    Value = odm != null ? odm.Value : 0,
                };

            var items = await query.AsNoTracking().ToListAsync();

            return items
                .GroupBy(x => x.OrderDetail)
                .Select(g => new OrderDetailsDto
                {
                    GoodsName = g.Key.BeverageName,
                    ItemCode = g.Key.ItemCode,
                    Price = g.Key.Price,
                    Quantity = g.Key.Quantity,
                    Result = g.Key.Result,
                    Error = g.Key.Error,
                    ErrorDescription = g.Key.ErrorDescription,
                    ActionTimeSp = g.Key.ActionTimeSp,
                    BeverageInfo = g.Key.BeverageInfoData,
                    ReturnAmount = g.Key.ReturnAmount,
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
    }
}