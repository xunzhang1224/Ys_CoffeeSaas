using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Iot.Application.DownSend.GRPC;

namespace YS.CoffeeMachine.Iot.Application.DownSend.Dto.Input
{
    /// <summary>
    /// 下发请求基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DownSeedRequestBase<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// mid
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        public ICommandSender? Server { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mid"></param>
        public DownSeedRequestBase(T data,string mid)
        {
            Data = data;
            Mid = mid;
        }
    }
}
