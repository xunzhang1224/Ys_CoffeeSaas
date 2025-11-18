namespace YS.Domain.IoT
{
    using System;

    /// <summary>
    /// 表示IoT模块中自定义的异常类型。
    /// 用于封装与物联网设备通信、指令处理、状态校验等相关的异常信息。
    /// </summary>
    public class IotException : Exception
    {
        /// <summary>
        /// 初始化 <see cref="IotException"/> 类的新实例。
        /// </summary>
        public IotException()
        { }

        /// <summary>
        /// 使用指定的错误消息初始化 <see cref="IotException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的字符串。</param>
        public IotException(string message)
            : base(message)
        { }

        /// <summary>
        /// 使用指定的错误消息和对导致此异常的内部异常的引用来初始化 <see cref="IotException"/> 类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="innerException">导致当前异常的异常。</param>
        public IotException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}