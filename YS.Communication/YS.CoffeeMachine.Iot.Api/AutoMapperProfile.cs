using AutoMapper;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.BasicDtos;
using YS.CoffeeMachine.Iot.Api.Dto;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
using YS.CoffeeMachine.Iot.Application.GRPC.DTO;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api
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
            CreateMap<DeviceInitialization, SecretInfoOutput>();
            CreateMap<DeviceBaseInfo, DeviceBaseRedisDto>();
            CreateMap<BeverageInfo, OrderBeverageInfoDto>();
            CreateMap<FormulaInfo, OrderFormulaInfoDto>()
                .ForMember(dest => dest.SpecsString, opt => opt.MapFrom(src => src.Specs))
                .ForMember(dest => dest.Specs, opt => opt.Ignore());
            CreateMap<DeviceOnlineLog, CreateDeviceOnlineLogDto>();
        }
    }
}
