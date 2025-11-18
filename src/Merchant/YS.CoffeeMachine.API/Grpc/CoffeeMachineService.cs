using Grpc.Core;
using MediatR;

namespace YS.CoffeeMachine.API.Grpc
{
    /// <summary>
    /// CoffeeMachineService
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    public class CoffeeMachineService(IMediator mediator, ILogger<CoffeeMachineService> logger) : DefaultgRPC.DefaultgRPCBase
    {
        /// <summary>
        /// GetDefaultInfo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<DefaultInfoResponse> GetDefaultInfo(GetDefaultRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DefaultInfoResponse { Msg = "ok" + request.Name });
        }
    }
}
