namespace YS.CoffeeMachine.Application.IServices
{
    /// <summary>
    /// 清除设备关系接口服务
    /// </summary>
    public interface IClearDeviceRelationshipService
    {
        /// <summary>
        /// 清除设备关系
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        Task<bool> ClearDeviceRelationshipsAsync(List<long> deviceIds);
    }
}
