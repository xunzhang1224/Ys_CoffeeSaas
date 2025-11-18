using AutoMapper;
using YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.BasicDtos;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;

namespace YS.CoffeeMachine.Platform.API.Extensions
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
            CreateMap<EnterpriseTypes, P_EnterpriseTypesDto>();//企业信息
            CreateMap<EnterpriseInfo, EnterpriseInfoDto>();//企业信息
            CreateMap<EnterpriseUser, EnterpriseUserDto>();//企业用户信息
            CreateMap<EnterpriseRole, EnterpriseRoleDto>();//企业角色信息
            CreateMap<TaskSchedulingInfo, TaskSchedulingInfoDot>();//任务调度
            CreateMap<ApplicationUser, ApplicationUserDto>();//用户信息
            CreateMap<ApplicationRole, ApplicationRoleDto>().ForMember(d => d.applicationMenuDtos, o => o.MapFrom(s => s.ApplicationRoleMenus));//角色信息
            CreateMap<ApplicationMenu, ApplicationMenuDto>().ForMember(dest => dest.Auth, opt => opt.MapFrom(src => src.Auths)).ForMember(dest => dest.Auths, opt => opt.Ignore());//菜单信息
            CreateMap<IP_ApplicationMenuQueries, P_ApplicationMenuQueries>();//角色菜单信息
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
            CreateMap<FormulaInfo, FormulaInfoDto>().ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs)).ForMember(dest => dest.Specs, opt => opt.Ignore()); ;
            CreateMap<BeverageInfoTemplate, BeverageInfoTemplateDto>();
            CreateMap<FormulaInfoTemplate, FormulaInfoTemplateDto>().ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs)).ForMember(dest => dest.Specs, opt => opt.Ignore());
            CreateMap<BeverageCollection, BeverageCollectionDto>();

            CreateMap<P_BeverageInfo, P_BeverageInfoDto>();
            CreateMap<P_FormulaInfo, P_FormulaInfoDto>().ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs)).ForMember(dest => dest.Specs, opt => opt.Ignore());
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

        }
    }
}
