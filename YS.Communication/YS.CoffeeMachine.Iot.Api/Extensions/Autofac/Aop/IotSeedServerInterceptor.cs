using Castle.DynamicProxy;
using MediatR;
using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;
using YS.CoffeeMachine.Platform.API.Extensions.TaskSchedulingBase.Wrapper;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Autofac.Aop
{
    /// <summary>
    /// IotSeedServerInterceptor
    /// </summary>
    /// <param name="_logger"></param>
    /// <param name="_grpcClusterIot"></param>
    public class IotSeedServerInterceptor(ILogger<IotSeedServerInterceptor> _logger, GrpcClusterIotWrapp _grpcClusterIot) : IInterceptor
    {
        /// <summary>
        /// Intercept
        /// </summary>
        /// <param name="invocation"></param>
        public async void Intercept(IInvocation invocation)
        {
            try
            {
                if (!await Verification()) return;
                await BeforeExecuteMethod(invocation);
                //执行调用方法
                invocation.Proceed();
                //当前接口返回值
                var value = invocation.ReturnValue;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Iot下发异常-方法【{invocation.Method.Name}】，异常原因：{ex.Message}----->{ex} ");
                throw;
            }
            finally
            {
                await AfterExecuteMethod();
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="communicationBase"></param>
        /// <returns></returns>
        private async Task<bool> Verification()
        {
            return true;
        }

        /// <summary>
        /// 执行方法前
        /// </summary>
        /// <param name="communicationBase"></param>
        /// <returns></returns>
        private async Task BeforeExecuteMethod(IInvocation invocation)
        {
            // 1. 检查方法参数中是否有 DownSeedRequestBase<T>
            foreach (var argument in invocation.Arguments)
            {
                if (argument != null && argument.GetType().IsGenericType &&
                    argument.GetType().GetGenericTypeDefinition() == typeof(DownSeedRequestBase<>))
                {
                    var serverProperty = argument.GetType().GetProperty("Server");
                    var midProperty = argument.GetType().GetProperty("Mid");
                    var vendNo = midProperty?.GetValue(argument) as string;
                    if (serverProperty != null && serverProperty.CanWrite&&!string.IsNullOrWhiteSpace(vendNo))
                    {
                        var server = await _grpcClusterIot.GetOrCreateCommandSenderAsync(vendNo);
                        serverProperty.SetValue(argument, server);
                    }
                }
            }

        }
        /// <summary>
        /// 执行方法完成/异常后
        /// </summary>
        /// <returns></returns>
        private async Task AfterExecuteMethod()
        {
            //await redis.DelKeyAsync(key);
        }
    }
}
