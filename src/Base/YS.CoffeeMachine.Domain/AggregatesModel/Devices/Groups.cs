namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.Collections.Generic;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备分组的聚合根实体。
    /// 用于对设备进行逻辑分组管理，支持多层级结构，并可绑定用户与设备。
    /// </summary>
    public class Groups : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置该分组所属的企业唯一标识符。
        /// </summary>
        public long EnterpriseInfoId { get; private set; }

        /// <summary>
        /// 获取或设置当前分组的名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置上级分组的唯一标识符，可为空表示顶级分组。
        /// </summary>
        public long? PId { get; private set; }

        /// <summary>
        /// 获取或设置当前分组的完整路径。
        /// </summary>
        public string? Path { get; private set; }

        /// <summary>
        /// 获取或设置与此分组关联的用户集合。
        /// 用于实现基于分组的权限控制和人员管理。
        /// </summary>
        public List<GroupUsers> Users { get; private set; }

        /// <summary>
        /// 获取或设置与此分组关联的设备集合。
        /// 用于批量操作或权限控制。
        /// </summary>
        public List<GroupDevices> Devices { get; private set; }

        /// <summary>
        /// 获取或设置当前分组的备注信息。
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，初始化空列表。
        /// 供ORM工具使用。
        /// </summary>
        protected Groups()
        {
            Users = new List<GroupUsers>();
            Devices = new List<GroupDevices>();
        }

        /// <summary>
        /// 使用指定参数初始化一个新的 Groups 实例。
        /// </summary>
        /// <param name="enterpriseInfoId">归属企业的唯一标识。</param>
        /// <param name="name">分组名称。</param>
        /// <param name="pid">上级分组ID（可为空）。</param>
        /// <param name="remark">备注信息。</param>
        /// <param name="userIds">需要绑定的用户ID集合。</param>
        /// <param name="deviceIds">需要绑定的设备ID集合。</param>
        public Groups(
            long enterpriseInfoId,
            string name,
            long? pid,
            string remark,
            List<long>? userIds,
            List<long>? deviceIds,
            string? path = null, long? id = null)
        {
            EnterpriseInfoId = enterpriseInfoId;
            Name = name;
            PId = pid;
            Remark = remark;
            Users = new List<GroupUsers>();
            Devices = new List<GroupDevices>();
            Path = path;
            if (id != null)
            {
                Id = id ?? 0;
            }

            if (userIds != null && userIds.Count > 0)
                Users = userIds.Select(s => new GroupUsers(Id, s)).ToList();

            if (deviceIds != null && deviceIds.Count > 0)
                Devices = deviceIds.Select(s => new GroupDevices(Id, s)).ToList();
        }

        /// <summary>
        /// 更新当前分组的基本信息。
        /// </summary>
        /// <param name="name">新的分组名称。</param>
        /// <param name="pid">新的上级分组ID。</param>
        /// <param name="remark">新的备注信息。</param>
        public void Update(string name, long? pid, string remark)
        {
            Name = name;
            PId = pid;
            Remark = remark;
        }

        /// <summary>
        /// 更新当前分组的路径信息。
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public void UpdatePath(string oldPath, string newPath)
        {
            Path = Path.Replace(oldPath, newPath);
        }

        /// <summary>
        /// 绑定一组用户到当前分组。
        /// 原有绑定将被清空。
        /// </summary>
        /// <param name="userIds">要绑定的用户ID集合。</param>
        public void BindUsers(List<long> userIds)
        {
            Users.Clear();
            Users = userIds.Select(s => new GroupUsers(Id, s)).ToList();
        }

        /// <summary>
        /// 清除当前分组下所有已绑定的用户。
        /// </summary>
        public void ClearUsers() => Users.Clear();

        /// <summary>
        /// 清除当前分组下所有已绑定的设备。
        /// </summary>
        public void ClearDevices() => Devices.Clear();

        /// <summary>
        /// 绑定一组设备到当前分组。
        /// 原有绑定将被清空。
        /// </summary>
        /// <param name="deviceIds">要绑定的设备ID集合。</param>
        public void BindDevices(List<long> deviceIds)
        {
            Devices.Clear();
            Devices = deviceIds.Select(s => new GroupDevices(Id, s)).ToList();
        }

        /// <summary>
        /// 从当前分组中移除指定的设备。
        /// </summary>
        /// <param name="deviceIds">要移除的设备ID集合。</param>
        public void RemoveDevices(List<long> deviceIds)
        {
            Devices = Devices.Where(w => !deviceIds.Contains(w.DeviceInfoId)).ToList();
        }

        /// <summary>
        /// 获取指定分组的父级路径链。
        /// </summary>
        /// <param name="id">当前分组ID。</param>
        /// <param name="enterprises">所有分组集合。</param>
        /// <returns>包含父级路径的分组列表。</returns>
        public List<Groups> GetParentHierarchy(long id, List<Groups> enterprises)
        {
            var hierarchy = new List<Groups>();
            var current = enterprises.FirstOrDefault(e => e.Id == id);

            while (current != null && current.PId.HasValue)
            {
                hierarchy.Insert(0, current); // 父级插入在前
                current = enterprises.FirstOrDefault(e => e.Id == current.PId.Value);
            }

            return hierarchy;
        }

        /// <summary>
        /// 获取指定分组的所有子级分组（递归获取）。
        /// </summary>
        /// <param name="id">当前分组ID。</param>
        /// <param name="enterprises">所有分组集合。</param>
        /// <returns>包含所有子级分组的列表。</returns>
        public List<Groups> GetChildHierarchy(long id, List<Groups> enterprises)
        {
            var hierarchy = new List<Groups>();

            var children = enterprises.Where(e => e.PId == id).ToList();
            foreach (var child in children)
            {
                hierarchy.Add(child);
                hierarchy.AddRange(GetChildHierarchy(child.Id, enterprises));
            }

            return hierarchy;
        }

        /// <summary>
        /// 判断给定的新上级是否为当前分组的下级或子孙分组。
        /// 防止出现循环依赖。
        /// </summary>
        /// <param name="newPId">新上级分组ID。</param>
        /// <param name="allGroups">所有分组集合。</param>
        /// <returns>如果存在循环依赖返回 true，否则返回 false。</returns>
        public bool IsInvalidParent(long newPId, List<Groups> allGroups)
        {
            while (true)
            {
                var parent = allGroups.FirstOrDefault(g => g.Id == newPId);
                if (parent == null) break;
                if (parent.Id == Id)
                    return true; // 新上级是当前分组的子孙，会导致循环
                newPId = parent.PId ?? 0;
            }
            return false;
        }
    }
}