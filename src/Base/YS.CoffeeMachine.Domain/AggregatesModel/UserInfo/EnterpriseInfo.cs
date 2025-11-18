using System.Data;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.PlatformEvents.ApplicationDomainEvents;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.UserInfo
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public class EnterpriseInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 企业类型Id
        /// </summary>
        public long? EnterpriseTypeId { get; private set; } = null;

        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseTypes EnterpriseType { get; private set; }

        /// <summary>
        /// 地区关联id
        /// </summary>
        public long? AreaRelationId { get; private set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        //public EnterpriseTypeEnum EnterpriseType { get; private set; }

        /// <summary>
        /// 上级
        /// </summary>
        public long? Pid { get; private set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// 当前企业拥有的菜单，Id集合
        /// </summary>
        public string MenuIds { get; private set; } = string.Empty;

        /// <summary>
        /// 半菜单
        /// </summary>
        public string? HalfMenuIds { get; private set; }

        /// <summary>
        /// 当前企业拥有的H5菜单，Id集合
        /// </summary>
        public string H5MenuIds { get; private set; } = string.Empty;

        /// <summary>
        /// 半H5菜单
        /// </summary>
        public string? H5HalfMenuIds { get; private set; }

        /// <summary>
        /// 企业资质类型
        /// </summary>
        public EnterpriseOrganizationTypeEnum? OrganizationType { get; private set; } = null;

        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? RegistrationProgress { get; private set; } = null;

        /// <summary>
        /// 企业管理员
        /// </summary>
        public List<EnterpriseUser> Users { get; private set; }
        /// <summary>
        /// 企业角色
        /// </summary>
        public List<EnterpriseRole> Roles { get; private set; }
        /// <summary>
        /// 下级企业
        /// </summary>
        public List<EnterpriseInfo> EnterpriseInfos { get; private set; }
        /// <summary>
        /// 设备分配
        /// </summary>
        public List<EnterpriseDevices> EnterpriseDevices { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EnterpriseInfo()
        {
            Users = new List<EnterpriseUser>();
            Roles = new List<EnterpriseRole>();
            EnterpriseInfos = new List<EnterpriseInfo>();
        }

        /// <summary>
        /// 添加默认企业
        /// </summary>
        /// <param name="account"></param>
        /// <param name="niceName"></param>
        /// <param name="phone"></param>
        /// <param name="emial"></param>
        public void InsertDefaultEnterprise(string account, string niceName, string areaCode, string phone, string emial)
        {
            IsDefault = true;
            AddDomainEvent(new EnterpriseCreatedDomainEvent(Id, account, niceName, areaCode, phone, emial));
        }

        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enterpriseTypeId"></param>
        /// <param name="pid"></param>
        /// <param name="remark"></param>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <param name="enterpriseInfos"></param>
        /// <param name="id"></param>
        /// <param name="enterpriseType"></param>
        public EnterpriseInfo(string name, long? enterpriseTypeId, long? pid, string? remark,
            List<long>? userIds = null, List<long>? roleIds = null, List<EnterpriseInfo>? enterpriseInfos = null,
            long? id = null, EnterpriseTypes enterpriseType = null, long? areaRelationId = null)
        {
            if (id != null && id > 0)
                Id = id.Value;
            Name = name;
            EnterpriseTypeId = enterpriseTypeId;
            Pid = !pid.HasValue || pid == null ? null : pid;
            IsDefault = false;
            Remark = remark;
            AreaRelationId = areaRelationId;
            Users = new List<EnterpriseUser>();
            userIds?.ForEach(x =>
            {
                if (userIds.Count > 0)
                {
                    Users.Add(new EnterpriseUser(Id, x));
                }
            });
            Roles = new List<EnterpriseRole>();
            roleIds?.ForEach(x =>
            {
                if (roleIds.Count > 0)
                {
                    Roles.Add(new EnterpriseRole(Id, x));
                }
            });

            if (enterpriseInfos != null && enterpriseInfos.Count > 0)
            {
                EnterpriseInfos = new List<EnterpriseInfo>(enterpriseInfos);
            }
            if (enterpriseType != null)
                EnterpriseType = enterpriseType;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void SetInfo(long id, string name, long enterpriseTypeId, long? pid, string? remark, List<long>? userIds = null, List<long>? roleIds = null, List<EnterpriseInfo>? enterpriseInfos = null)
        {
            Id = id;
            Name = name;
            EnterpriseTypeId = enterpriseTypeId;
            Pid = !pid.HasValue || pid == null ? null : pid;
            Remark = remark;
            userIds?.ForEach(x =>
            {
                Users.Add(new EnterpriseUser(Id, x));
            });
            roleIds?.ForEach(x =>
            {
                Roles.Add(new EnterpriseRole(Id, x));
            });
            if (EnterpriseInfos != null && EnterpriseInfos.Count > 0)
            {
                EnterpriseInfos = new List<EnterpriseInfo>(enterpriseInfos);
            }
        }

        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enterpriseTypeId"></param>
        /// <param name="pid"></param>
        /// <param name="remark"></param>
        /// <param name="areaRelationId"></param>
        public void Update(string name, long? enterpriseTypeId, long? pid, string? remark, long? areaRelationId = null)
        {
            Name = name;
            if (enterpriseTypeId != null && enterpriseTypeId > 0)
                EnterpriseTypeId = enterpriseTypeId;
            Pid = !pid.HasValue || pid == null ? null : pid;
            Remark = remark;
            if (areaRelationId != null && areaRelationId > 0)
                AreaRelationId = areaRelationId;
        }

        /// <summary>
        /// 更改企业拥有的菜单Ids
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="halfMenuIds"></param>
        /// <param name="sysMenuType"></param>
        public void UpdateMenuIds(List<long> menuIds, List<long>? halfMenuIds = null, SysMenuTypeEnum? sysMenuType = null)
        {
            if (sysMenuType != null && sysMenuType == SysMenuTypeEnum.H5)
            {
                H5MenuIds = menuIds.Count > 0 ? string.Join(",", menuIds) : string.Empty;
                if (halfMenuIds != null)
                {
                    H5HalfMenuIds = menuIds.Count > 0 ? string.Join(",", halfMenuIds) : string.Empty;
                }
            }
            else
            {
                MenuIds = menuIds.Count > 0 ? string.Join(",", menuIds) : string.Empty;
                if (halfMenuIds != null)
                {
                    HalfMenuIds = menuIds.Count > 0 ? string.Join(",", halfMenuIds) : string.Empty;
                }
            }
        }

        /// <summary>
        /// 更改企业拥有的H5菜单Ids
        /// </summary>
        /// <param name="h5MenuIds"></param>
        /// <param name="h5HalfMenuIds"></param>
        public void UpdateH5MenuIds(List<long> h5MenuIds, List<long>? h5HalfMenuIds = null)
        {
            H5MenuIds = h5MenuIds.Count > 0 ? string.Join(",", h5MenuIds) : string.Empty;
            if (h5HalfMenuIds != null)
            {
                H5HalfMenuIds = h5MenuIds.Count > 0 ? string.Join(",", h5HalfMenuIds) : string.Empty;
            }
        }

        /// <summary>
        /// 修改企业类型
        /// </summary>
        /// <param name="enterpriseType"></param>
        public void UpdateType(long enterpriseTypeId)
        {
            EnterpriseTypeId = enterpriseTypeId;
        }

        /// <summary>
        /// 绑定企业用户
        /// </summary>
        /// <param name="userIds"></param>

        public void UpdateEnterpriseUsers(List<long>? userIds)
        {
            if (userIds != null)
            {
                if (Users != null)
                    Users.Clear();
                else
                    Users = new List<EnterpriseUser>();
                userIds.ForEach(x =>
                {
                    Users.Add(new EnterpriseUser(Id, x));
                });
            }
        }

        /// <summary>
        /// 绑定企业角色
        /// </summary>
        /// <param name="userIds"></param>

        public void UpdateEnterpriseRoles(List<long>? roleIds)
        {
            if (roleIds != null)
            {
                Roles.Clear();
                roleIds.ForEach(x =>
                {
                    Roles.Add(new EnterpriseRole(Id, x));
                });
            }
        }

        /// <summary>
        /// 获取父级数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enterprises"></param>
        /// <returns></returns>
        public List<EnterpriseInfo> GetParentHierarchy(long id, List<EnterpriseInfo> enterprises)
        {
            var hierarchy = new List<EnterpriseInfo>();
            var current = enterprises.FirstOrDefault(e => e.Id == id);

            while (current != null && current.Pid.HasValue)
            {
                hierarchy.Insert(0, current);  // 父级在前
                current = enterprises.FirstOrDefault(e => e.Id == current.Pid.Value);
            }

            return hierarchy;
        }

        /// <summary>
        /// 获取子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enterprises"></param>
        /// <returns></returns>
        public List<EnterpriseInfo> GetChildHierarchy(long id, List<EnterpriseInfo> enterprises)
        {
            var hierarchy = new List<EnterpriseInfo>();

            // 查找当前Id的所有子级
            var children = enterprises.Where(e => e.Pid == id).ToList();
            foreach (var child in children)
            {
                hierarchy.Add(child);
                // 递归查找该子级的所有子级
                hierarchy.AddRange(GetChildHierarchy(child.Id, enterprises));
            }

            return hierarchy;
        }

        /// <summary>
        /// 检查新上级是否为当前企业的下级企业
        /// </summary>
        public bool IsInvalidParent(long newPId, List<EnterpriseInfo> allEnterpriseList)
        {
            while (true)
            {
                // 找到当前企业的上级
                var parent = allEnterpriseList.FirstOrDefault(g => g.Id == newPId);
                if (parent == null) break;
                if (parent.Id == Id)
                    return true; // 新上级是当前企业的子企业
                newPId = parent.Pid ?? 0;
            }
            return false;
        }

        /// <summary>
        /// 设置企业名称
        /// </summary>
        /// <param name="name"></param>
        public void SetEnterpriseName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 设置企业资质类型
        /// </summary>
        /// <param name="organizationType"></param>
        public void SetOrganizationType(EnterpriseOrganizationTypeEnum organizationType)
        {
            OrganizationType = organizationType;
        }

        /// <summary>
        /// 更新注册进度
        /// </summary>
        /// <param name="registrationProgress"></param>
        public void UpdateRegistrationProgress(RegistrationProgress registrationProgress)
        {
            RegistrationProgress = registrationProgress;
        }

        /// <summary>
        /// 绑定地区关联Id
        /// </summary>
        /// <param name="areaRelationId"></param>
        public void BindAreaRelationId(long areaRelationId)
        {
            AreaRelationId = areaRelationId;
        }

        /// <summary>
        /// 更新审核拒绝描述
        /// </summary>
        /// <param name="remark"></param>
        public void UpdateRemark(string remark)
        {
            Remark = remark;
        }
    }
}