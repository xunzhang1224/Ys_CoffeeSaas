using MagicOnion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using YS.CoffeeMachine.Iot.Api.Dto;

namespace YS.CoffeeMachine.Iot.Api.Extensions.Http
{
    /// <summary>
    /// http服务
    /// </summary>
    /// <param name="_httpClient"></param>
    public class HttpService(IHttpClientFactory _httpClientFactory, ILogger<HttpService> _logger)
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP请求失败: {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var rsp = JsonConvert.DeserializeObject<ApiOutPut<TResponse>>(responseContent);
                if (rsp == null || rsp.StatusCode != 200)
                    throw new Exception($"状态码异常！响应状态码: {rsp?.StatusCode}，错误描述：{rsp?.Errors}");

                return rsp.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP POST请求失败: {Url}", url);
                throw;
            }
        }
    }
}
