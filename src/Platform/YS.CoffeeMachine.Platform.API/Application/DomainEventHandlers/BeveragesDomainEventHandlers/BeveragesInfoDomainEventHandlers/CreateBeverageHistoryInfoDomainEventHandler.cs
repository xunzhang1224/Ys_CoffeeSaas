using AutoMapper;
using MediatR;
using System.Text.Json.Nodes;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Events.BeveragesDomainEvents.BeveragesInfoDomainEvents;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Platform.API.Application.DomainEventHandlers.BeveragesDomainEventHandlers.BeveragesInfoDomainEventHandlers
{
    /// <summary>
    /// 创建饮品编辑历史事件
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="mapper"></param>
    public class CreateBeverageHistoryInfoDomainEventHandler(IMediator mediator, IMapper mapper) : IDomainEventHandler<CreateBeverageHistoryInfoDomainEvent>
    {
        /// <summary>
        /// 创建饮品编辑历史事件
        /// </summary>
        public async Task Handle(CreateBeverageHistoryInfoDomainEvent notification, CancellationToken cancellationToken)
        {
            var info = notification.beverageInfo;
            //初始化饮品历史信息
            var historyInfo = new BeverageInfo(info.DeviceId, info.Name, info.BeverageIcon, info.Temperature, info.Remarks, info.ProductionForecast, info.ForecastQuantity, info.DisplayStatus, info.IsDefault);
            historyInfo.AddCodeNotId(info.Code);
            //绑定配方
            historyInfo.AddRangeFormulaInfos(info.FormulaInfos);
            //绑定主饮品
            //historyInfo.BindParentId(info.Id);
            //对象转换后调用创建饮品信息命令
            var formulaInfoDtos = mapper.Map<List<FormulaInfoDto>>(info.FormulaInfos);
            formulaInfoDtos.ForEach(x =>
            {
                x.Specs = (JsonObject)JsonObject.Parse(x.SpecsString);
            });
            //var command = new CreateBeverageInfoCommand(historyInfo.DeviceBaseId, historyInfo.Name, historyInfo.BeverageIcon, historyInfo.Code, historyInfo.Temperature, historyInfo.Remarks, formulaInfoDtos, historyInfo.ProductionForecast, historyInfo.ForecastQuantity, historyInfo.DisplayStatus);
            ////发送命令
            //await mediator.Send(command);
        }
    }
}
