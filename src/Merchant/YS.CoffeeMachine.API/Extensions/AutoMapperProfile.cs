using AutoMapper;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Extensions.IExecl.Dto.Docment.Exporter;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Application.Dtos.OrderDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.DomesticPayment;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.BasicDtos;

namespace YS.CoffeeMachine.API.Extensions
{
    /// <summary>
    /// 自动映射器配置文件
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 构造内创建映射对象
        /// </summary>
        public AutoMapperProfile()
        {
            #region 企业相关
            CreateMap<EnterpriseInfo, EnterpriseInfoDto>();// 企业信息
            CreateMap<EnterpriseUser, EnterpriseUserDto>();// 企业用户信息
            CreateMap<EnterpriseRole, EnterpriseRoleDto>();// 企业角色信息
            CreateMap<EnterpriseTypes, EnterpriseTypesDto>();// 企业类型信息
            CreateMap<TaskSchedulingInfo, TaskSchedulingInfoDot>();// 任务调度
            CreateMap<ApplicationUser, ApplicationUserDto>();// 用户信息
            CreateMap<ApplicationRole, ApplicationRoleDto>()
                .ForMember(d => d.applicationMenuDtos, o => o.MapFrom(s => s.ApplicationRoleMenus))
                .ForMember(d => d.applicationH5MenuDtos, o => o.MapFrom(s => s.ApplicationRoleMenus))
                ;// 角色信息
            CreateMap<ApplicationMenu, ApplicationMenuDto>().ForMember(dest => dest.Auth, opt => opt.MapFrom(src => src.Auths)).ForMember(dest => dest.Auths, opt => opt.Ignore());// 菜单信息
            #endregion

            #region 设备相关
            CreateMap<DeviceInfo, DeviceInfoDto>();
            CreateMap<GroupDevices, GroupDevicesDto>();
            CreateMap<EnterpriseDevices, EnterpriseDevicesDto>();
            CreateMap<DeviceModel, DeviceModelDto>();
            CreateMap<AdvertisementInfo, AdvertisementInfoDto>();
            CreateMap<EarlyWarningConfig, EarlyWarningConfigDto>();
            CreateMap<SettingInfo, SettingInfoDto>();
            CreateMap<MaterialBox, MaterialBoxDto>();
            CreateMap<DeviceServiceProviders, DeviceServiceProvidersDto>();
            #endregion

            #region 设置相关
            CreateMap<SettingInfo, SettingInfoDto>();
            CreateMap<TimeZoneInfos, TimeZoneInfoDto>();
            CreateMap<InterfaceStyles, InterfaceStylesDto>();
            CreateMap<EarlyWarningConfig, EarlyWarningConfigDto>();
            #endregion

            #region 饮品相关
            CreateMap<BeverageInfo, BeverageInfoDto>();
            CreateMap<FormulaInfo, FormulaInfoDto>()
                .ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs))
                .ForMember(dest => dest.Specs, opt => opt.Ignore());

            CreateMap<BeverageInfo, OrderBeverageInfoDto>();
            CreateMap<FormulaInfo, OrderFormulaInfoDto>()
                .ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs))
                .ForMember(dest => dest.Specs, opt => opt.Ignore());

            CreateMap<FormulaInfoTemplate, FormulaInfoTemplateDto>()
                .ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.SpecsString))
                .ForMember(dest => dest.Specs, opt => opt.Ignore());

            CreateMap<BeverageInfoTemplate, BeverageInfoTemplateDto>()
                .ForMember(dest => dest.FormulaInfos, opt => opt.MapFrom(src => src.FormulaInfoTemplates));

            CreateMap<BeverageCollection, BeverageCollectionDto>();
            #endregion

            #region 基础服务
            CreateMap(typeof(PagedResultDto<>), typeof(PagedResultDto<>));
            CreateMap<LanguageTextEntity, LanguageTextDto>().ForMember(x => x.Language, y => y.MapFrom(z => z.Lang.Name));
            CreateMap<LanguageInfo, LanguageInfoDto>();
            #endregion

            #region Iot
            CreateMap<OperationLog, OperationLogDto>();
            CreateMap<OperationLogDto, OperationLog>();
            CreateMap<OperationLog, OperationLogQueriesDto>();
            CreateMap<OperationSubLog, OperationSubLogDto>();
            #endregion

            #region 国内支付相关
            CreateMap<M_PaymentWechatApplymentsDto, M_PaymentWechatApplyments>();
            CreateMap<M_PaymentWechatApplyments, M_PaymentWechatApplymentsDto>();

            CreateMap<M_PaymentWechatApplymentsOutput, M_PaymentWechatApplyments>();
            CreateMap<M_PaymentWechatApplyments, M_PaymentWechatApplymentsOutput>();

            CreateMap<M_PaymentAlipayApplymentsDto, M_PaymentAlipayApplyments>();
            CreateMap<M_PaymentAlipayApplyments, M_PaymentAlipayApplymentsDto>();
            CreateMap<M_PaymentAlipayApplyments, M_PaymentAlipayApplymentsOutput>();
            #endregion

            CreateMap<Promotion, PromotionOutput>();
            CreateMap<CreateNotityMsgDto, NotityMsg>()
    .ForMember(dest => dest.ContactAddress, opt => opt.MapFrom(src => src.Account))
    .ReverseMap()
    .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.ContactAddress));

            CreateMap<DeviceOnlineLogDto, DeviceOnlineExporter>()
                            .ForMember(dest => dest.DateTime, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {
                                var offset = Convert.ToInt32(context.Items["TimeZoneOffset"] ?? 0);
                                if (offset != 0)
                                {
                                    TimeSpan timeOffset = TimeSpan.FromHours(offset);
                                    src.DateTime += timeOffset;
                                }

                                return src.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }))
                            .ForMember(dest => dest.IsOnline, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {

                                return src.IsOnline ? "在线" : "离线";
                            }));

            CreateMap<DeviceEventLogDto, DeviceEventLogExport>()
                            .ForMember(dest => dest.LocalOperationTime, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {
                                var offset = Convert.ToInt32(context.Items["TimeZoneOffset"] ?? 0);

                                if (!src.OperationTime.HasValue)
                                    return string.Empty;

                                DateTime operationTime = src.OperationTime.Value;

                                if (offset != 0)
                                {
                                    TimeSpan timeOffset = TimeSpan.FromHours(offset);
                                    operationTime += timeOffset;
                                }

                                return operationTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }));

            CreateMap<DeviceErrorLogDto, DeviceErrorLogExport>()
                            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {
                                var offset = Convert.ToInt32(context.Items["TimeZoneOffset"] ?? 0);
                                if (offset != 0)
                                {
                                    TimeSpan timeOffset = TimeSpan.FromHours(offset);
                                    src.CreateTime += timeOffset;
                                }

                                return src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }))
                            .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {

                                return src.Status.GetValueOrDefault() ? "恢复" : "故障";
                            }));

            CreateMap<OrderInfoDto, OrderInfoExport>()
                            .ForMember(dest => dest.PayTime, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {
                                var offset = Convert.ToInt32(context.Items["TimeZoneOffset"] ?? 0);
                                if (string.IsNullOrWhiteSpace(src.PayTimeStr) || src.PayTimeStr.Contains("1970"))
                                    return string.Empty;

                                var utc = Convert.ToDateTime(src.PayTimeStr);

                                if (offset != 0)
                                {
                                    TimeSpan timeOffset = TimeSpan.FromHours(offset);
                                    utc += timeOffset;
                                }

                                return utc.ToString("yyyy-MM-dd HH:mm:ss");
                            }))
                            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom((src, dest, destMember, context) =>
                            {
                                var offset = Convert.ToInt32(context.Items["TimeZoneOffset"] ?? 0);

                                if (offset != 0)
                                {
                                    TimeSpan timeOffset = TimeSpan.FromHours(offset);
                                    src.CreateTime += timeOffset;
                                }

                                return src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }));
        }
    }
}
