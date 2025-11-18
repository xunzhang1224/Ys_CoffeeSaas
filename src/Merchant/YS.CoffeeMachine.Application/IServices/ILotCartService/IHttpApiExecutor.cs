
namespace YS.CoffeeMachine.Application.IServices.ILotCartService
{
    /// <summary>
    /// http api Ö´ÐÐÆ÷
    /// </summary>
    public interface IHttpApiExecutor
    {
        /// <summary>
        /// post ÇëÇó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string url, object body, string? authorization = null);

        /// <summary>
        /// get ÇëÇó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string url);
    }
}