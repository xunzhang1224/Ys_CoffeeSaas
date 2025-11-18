using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Cap
{
    /// <summary>
    /// 用于获取访问令牌的请求模型
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 授权类型，例如："client_credentials"
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// 客户端唯一标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥，用于身份验证
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 请求的权限范围
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// 通用响应结果封装类
    /// </summary>
    public class ResponeResult
    {
        /// <summary>
        /// 状态码，表示操作结果的状态（如200、400、500等）
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 返回的数据对象，具体结构根据接口不同而变化
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 操作是否成功标志，true 表示成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 错误信息描述，当操作失败时包含具体错误内容
        /// </summary>
        public string Errors { get; set; }
    }

    /// <summary>
    /// 绑定设备请求模型
    /// </summary>
    public class BindDevice
    {
        /// <summary>
        /// 设备编号或唯一标识符
        /// </summary>
        public string Mid { get; set; }
    }

    /// <summary>
    /// 获取Token后的返回结果模型
    /// </summary>
    public class TokenOutput
    {
        /// <summary>
        /// 获取到的访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 令牌有效期，单位为秒
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
