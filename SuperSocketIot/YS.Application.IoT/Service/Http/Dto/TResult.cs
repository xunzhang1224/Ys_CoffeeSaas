namespace YS.Application.IoT.Service.Http.Dto
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 标准化的 HTTP 响应结果封装类（含业务状态、错误信息及数据）。
    /// </summary>
    /// <typeparam name="T">返回的数据类型。</typeparam>
    public class RsResult<T>
    {
        /// <summary>
        /// 请求是否成功。true 表示成功，false 表示失败。
        /// 注意：当前字段名存在拼写错误，建议改为 IsSuccess。
        /// </summary>
        public bool IsSucess { get; set; }

        /// <summary>
        /// 错误码（可空）。用于标识具体的业务异常或错误类型。
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// 错误消息（可空）。描述请求失败的具体原因。
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 返回的业务数据（可空）。泛型支持多种数据结构。
        /// </summary>
        public T? Data { get; set; }

        #region 静态方法（推荐使用方式）

        /// <summary>
        /// 创建一个表示成功的响应对象。
        /// </summary>
        /// <param name="data">要返回的数据。</param>
        /// <returns>成功的结果实例。</returns>
        public static RsResult<T> Success(T data)
        {
            return new RsResult<T>
            {
                IsSucess = true,
                Data = data
            };
        }

        /// <summary>
        /// 创建一个表示失败的响应对象。
        /// </summary>
        /// <param name="errorCode">错误码。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>失败的结果实例。</returns>
        public static RsResult<T> Fail(string errorCode, string errorMessage)
        {
            return new RsResult<T>
            {
                IsSucess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }

        #endregion
    }

    /// <summary>
    /// 统一格式的通用响应封装类，适用于多系统集成场景。
    /// </summary>
    /// <typeparam name="T">返回的数据类型。</typeparam>
    public class UnifyResult<T>
    {
        /// <summary>
        /// 状态码，表示请求的处理结果（如 200 成功，500 异常等）。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应类型，例如 "success" 或 "error"。
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 描述性消息，通常用于展示给前端用户阅读。
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 返回的业务数据（可空）。
        /// </summary>
        public T? Result { get; set; }

        #region 静态方法（统一构建）

        /// <summary>
        /// 构建一个成功状态的统一响应。
        /// </summary>
        /// <param name="result">返回的数据。</param>
        /// <param name="message">提示信息，默认为空。</param>
        /// <returns>统一响应对象。</returns>
        public static UnifyResult<T> Ok(T result, string message = "")
        {
            return new UnifyResult<T>
            {
                Code = 200,
                Type = "success",
                Message = message,
                Result = result
            };
        }

        /// <summary>
        /// 构建一个失败状态的统一响应。
        /// </summary>
        /// <param name="code">错误码。</param>
        /// <param name="type">错误类型（如 error）。</param>
        /// <param name="message">错误描述。</param>
        /// <returns>统一响应对象。</returns>
        public static UnifyResult<T> Error(int code, string type, string message)
        {
            return new UnifyResult<T>
            {
                Code = code,
                Type = type,
                Message = message
            };
        }

        #endregion
    }
}