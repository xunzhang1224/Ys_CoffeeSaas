using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using YS.AMP.SDK.Factory;
using YS.AMP.SDK.Options;
using YS.AMP.Shared.Enum;
using YS.AMP.Shared.Request;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto
{
    /// <summary>
    /// SeedYYPTSubscriber
    /// </summary>
    /// <param name="_coffeeMachinePlatformDb"></param>
    public class SeedYYPTSubscriber(CoffeeMachinePlatformDbContext _coffeeMachinePlatformDb,ILogger<SeedYYPTSubscriber> _logger, IConfiguration _cfg) : ICapSubscribe
    {
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CapSubscribe(CapConst.SeedYYPSoftUpdate)]
        public async Task Handle(UpdateResultInput input)
        {
            var url = _cfg["AmpSdk:BaseUrl"];
            var accessKey = _cfg["AmpSdk:AccessKey"];
            var accessSecret = _cfg["AmpSdk:AccessSecret"];
            var client = AmpClientFactory.GetClient(new AmpClientOptions
            {
                BaseUrl = url,
                AccessKey = accessKey,
                AccessSecret = accessSecret
            });
            var res = await client.Machine.UpdateResultAsync(input);
            _logger.LogInformation($"推送升级结果到应用平台。内容：{JsonConvert.SerializeObject(input)}");
        }
    }
}
