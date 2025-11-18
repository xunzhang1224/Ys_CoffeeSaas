namespace YS.CoffeeMachine.Provider.IServices
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public interface IDocmentService
    {
        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetExcleDataToList(Stream file, string key = null);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task Inserts(object dtos, string key = null);
    }
}
