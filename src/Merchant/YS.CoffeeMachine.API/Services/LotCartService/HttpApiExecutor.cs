using System.Text.Json;
using YS.CoffeeMachine.Application.IServices.ILotCartService;

namespace YS.CoffeeMachine.API.Services.LotCartService
{
    /// <summary>
    /// http api 执行器
    /// </summary>
    public class HttpApiExecutor(HttpClient _httpClient, ILogger<HttpApiExecutor> _logger) : IHttpApiExecutor
    {
        /// <summary>
        /// post 方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object body, string? authorization = null)
        {
            _logger.LogInformation("➡️ POST {Url} | Body: {@Body}", url, body);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, body);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("⬅️ Response from {Url}: {Content}", url, content);

                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// get 方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                _logger.LogInformation("➡️ GET {Url}", url);

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("⬅️ Response from {Url}: {Content}", url, content);

                response.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}