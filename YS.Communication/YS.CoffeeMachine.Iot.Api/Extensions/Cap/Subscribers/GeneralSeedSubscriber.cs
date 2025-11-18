namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Subscribers
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using DotNetCore.CAP;

    using Newtonsoft.Json;

    using YS.CoffeeMachine.Domain.Shared.Const;
    using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
    using YS.CoffeeMachine.Iot.Api.Iot.Interfaces;
    using YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input;
    using YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO.Base;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities;
    using YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity;

    /// <summary>
    /// 通用下行指令订阅器，用于处理 CAP 中发布的通用指令消息
    /// </summary>
    public class GeneralSeedSubscriber : ICapSubscribe
    {
        private readonly ICommandSenderService _commandSender;
        private readonly Assembly _targetAssembly;

        /// <summary>
        /// 构造函数，注入命令发送服务并加载目标程序集
        /// </summary>
        /// <param name="commandSender">命令发送服务</param>
        public GeneralSeedSubscriber(ICommandSenderService commandSender)
        {
            _commandSender = commandSender;
            // 加载包含下行实体的程序集，用于反射查找类和方法
            _targetAssembly = typeof(DownlinkEntity6216).Assembly;
        }

        /// <summary>
        /// 处理通用指令消息
        /// </summary>
        /// <param name="input">通用下行指令输入 DTO</param>
        /// <returns>异步任务</returns>
        [CapSubscribe(CapConst.GeneralSeed)]
        public async Task Handle(GeneralSeedInput input)
        {
            try
            {
                // 1. 根据 Method 查找对应的实体类型
                Type? entityType = FindEntityTypeByMethod(input?.Method);
                if (entityType == null)
                    throw new InvalidOperationException($"找不到与 Method={input.Method} 对应的实体类");

                // 2. 反序列化 Params 为对应的实体对象
                object? paramObj = JsonConvert.DeserializeObject(input.Params, entityType);
                if (paramObj == null)
                    throw new InvalidOperationException($"无法将 Params 反序列化为 {entityType.Name}");

                // 设置 TransId 属性（如果存在）
                var transIdProperty = paramObj.GetType().GetProperty("TransId");
                if (transIdProperty != null && transIdProperty.CanWrite)
                {
                    transIdProperty.SetValue(paramObj, input.TransId);
                }

                if (paramObj is BaseCmd entity)
                {
                    if (string.IsNullOrWhiteSpace(entity.Mid) || entity.Mid == "0000000000")
                    {
                        var midProperty = paramObj.GetType().GetProperty("Mid");
                        if (midProperty != null && midProperty.CanWrite)
                        {
                            midProperty.SetValue(paramObj, input.Mid);
                        }
                    }
                }

                // 3. 查找 ICommandSenderService 中对应的方法
                MethodInfo? method = FindServiceMethod(input.Method);
                if (method == null)
                    throw new InvalidOperationException($"找不到与 Method={input.Method} 对应的服务方法");

                // 4. 创建请求对象并调用方法
                object? request = CreateRequestObject(method, paramObj, input);
                var result = method.Invoke(_commandSender, new[] { request });

                // 如果是异步方法，等待完成
                if (result is Task task)
                {
                    await task;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理 GeneralSeedInput 时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据 Method 值查找对应的实体类型
        /// </summary>
        /// <param name="methodValue">方法编号，例如：6216</param>
        /// <returns>匹配的实体类型</returns>
        private Type? FindEntityTypeByMethod(string methodValue)
        {
            return _targetAssembly.GetTypes()
                .FirstOrDefault(t => t.Name.Contains(methodValue, StringComparison.OrdinalIgnoreCase)
                                     && !t.IsInterface
                                     && !t.IsAbstract);
        }

        /// <summary>
        /// 在 ICommandSenderService 中查找对应的方法
        /// </summary>
        /// <param name="methodValue">方法编号，例如：6216</param>
        /// <returns>匹配的方法信息</returns>
        private MethodInfo? FindServiceMethod(string methodValue)
        {
            return typeof(ICommandSenderService).GetMethods()
                .FirstOrDefault(m => m.Name.Contains(methodValue, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 根据方法签名创建请求对象（支持泛型 DownSeedRequestBase<T> 和 DownlinkSendDto<T>）
        /// </summary>
        /// <param name="method">要调用的方法信息</param>
        /// <param name="paramObj">反序列化的参数对象</param>
        /// <param name="input">原始输入数据</param>
        /// <returns>创建好的请求对象</returns>
        private object? CreateRequestObject(MethodInfo method, object paramObj, GeneralSeedInput input)
        {
            var parameterType = method.GetParameters().First().ParameterType;

            if (parameterType.IsGenericType)
            {
                var genericTypeDef = parameterType.GetGenericTypeDefinition();

                if (genericTypeDef == typeof(DownSeedRequestBase<>))
                {
                    var requestType = typeof(DownSeedRequestBase<>).MakeGenericType(paramObj.GetType());
                    return Activator.CreateInstance(requestType, paramObj, input.Mid);
                }

                if (genericTypeDef == typeof(DownlinkSendDto<>))
                {
                    var requestType = typeof(DownlinkSendDto<>).MakeGenericType(paramObj.GetType());
                    return Activator.CreateInstance(requestType, paramObj);
                }
            }

            throw new InvalidOperationException($"不支持的方法参数类型: {parameterType.Name}");
        }
    }
}