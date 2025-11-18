using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 分组查询
    /// </summary>
    public interface IGroupsQueries
    {
        /// <summary>
        /// 根据Id获取分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GroupsTreeDto> GetGroupsAsync(long id);
        /// <summary>
        /// 根据分组Id获取设备名称
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceInfoDto>> GetDeviceInfoByGroupId(QueryRequest request, long gid);
        /// <summary>
        /// 获取分组树列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<GroupsTreeDto>> GetGroupTreeListAsync(long enterpriseinfoId);

        /// <summary>
        /// 获取分组分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GroupListDto>> GetGroupPageList(GroupListInput input);
    }
}
