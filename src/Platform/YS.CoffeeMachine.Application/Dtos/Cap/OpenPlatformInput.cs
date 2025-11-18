namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 获取访问令牌请求参数 DTO
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 授权类型（例如：client_credentials）
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// 客户端 ID（Client ID）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥（Client Secret）
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 请求的权限范围（Scope）
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// 标准化响应结果 DTO，用于封装接口返回数据
    /// </summary>
    public class ResponeResult
    {
        /// <summary>
        /// HTTP 状态码（如："200", "400"）
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 返回的数据对象（泛型）
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 错误信息（失败时填充）
        /// </summary>
        public string Errors { get; set; }
    }

    /// <summary>
    /// 设备绑定请求 DTO
    /// </summary>
    public class BindDevice
    {
        /// <summary>
        /// 设备编号（Mid）
        /// </summary>
        public string Mid { get; set; }
    }

    /// <summary>
    /// 访问令牌输出 DTO
    /// </summary>
    public class TokenOutput
    {
        /// <summary>
        /// 获取到的访问令牌（Access Token）
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间（单位：秒）
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}