using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public class EnterpriseInfoDto
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        public long? EnterpriseTypeId { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseTypesDto EnterpriseType { get; set; }
        /// <summary>
        /// 企业类型名称
        /// </summary>
        public string EnterpriseTypeText { get; set; }
        /// <summary>
        /// 上级
        /// </summary>
        public long? Pid { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 当前企业拥有的菜单，Id集合
        /// </summary>
        public string MenuIds { get; set; }
        /// <summary>
        /// 当前企业拥有的菜单，Id集合
        /// </summary>
        public List<long> MenuIdsList { get { return !string.IsNullOrWhiteSpace(MenuIds) ? MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList() : []; } }

        /// <summary>
        /// 半菜单Id集合
        /// </summary>
        public List<long> HalfMenuIds { get; set; }

        /// <summary>
        /// 当前企业拥有的H5菜单，Id集合
        /// </summary>
        public string H5MenuIds { get; set; }
        /// <summary>
        /// 当前企业拥有的菜单，Id集合
        /// </summary>
        public List<long> H5MenuIdsList { get { return !string.IsNullOrWhiteSpace(H5MenuIds) ? H5MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList() : []; } }

        /// <summary>
        /// 半菜单Id集合
        /// </summary>
        public List<long> H5HalfMenuIds { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// 管理员用户Id集合
        /// </summary>
        public List<long> AdminUserIds { get; set; }

        /// <summary>
        /// 用户Id集合
        /// </summary>
        public List<long> UserIds { get; set; }

        /// <summary>
        /// 角色Id集合
        /// </summary>
        public List<long> RoleIds { get; set; }

        /// <summary>
        /// 地区关系Id
        /// </summary>
        public long? AreaRelationId { get; set; }

        ///// <summary>
        ///// 关联用户
        ///// </summary>
        //public List<EnterpriseUserDto> Users { get; set; }
        ///// <summary>
        ///// 关联角色
        ///// </summary>
        //public List<EnterpriseRoleDto> Roles { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public List<EnterpriseInfoDto> children { get; set; } = new List<EnterpriseInfoDto>();

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EnterpriseInfoDto()
        {
            //Users = new List<EnterpriseUserDto>();
            //Roles = new List<EnterpriseRoleDto>();
            UserIds = new List<long>();
            RoleIds = new List<long>();
            children = new List<EnterpriseInfoDto>();
            AdminUserIds = new List<long>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseInfoDto(long id, string name, long? enterpriseTypeId, long? pid, string menuIds, string h5MenuIds, bool isDefault, string remark, List<EnterpriseUser>? e_users = null,
            List<EnterpriseRole>? e_roles = null, EnterpriseTypes enterpriseType = null, string? halfMenuIdsStr = null, string? h5HalfMenuIdsStr = null, long? areaRelationId = null)
        {
            Id = id;
            Name = name;
            EnterpriseTypeId = enterpriseTypeId;
            EnterpriseTypeText = enterpriseType != null ? enterpriseType.Name : string.Empty;
            Pid = pid;
            MenuIds = menuIds;
            H5MenuIds = h5MenuIds;
            IsDefault = isDefault;
            Remark = remark;
            if (e_users != null && e_users.Count > 0)
            {
                var users = e_users.Where(w => w.User != null).Select(s => s.User).ToList();
                UserIds = e_users.Select(s => s.UserId).ToList();
                // 获取users中角色Code = "administrator"的userId
                var roleUserIds = users.Where(w => w.ApplicationUserRoles.Select(s => s.Role).Select(s => s.Code).Contains("administrator")).SelectMany(s => s.ApplicationUserRoles.Select(u => u.UserId)).ToList();
                if (users != null && users.Count > 0)
                {
                    //企业管理员
                    var adminUser = users.Where(w => /*UserIds.Contains(w.Id) || */roleUserIds.Contains(w.Id) || w.AccountType == AccountTypeEnum.SuperAdmin || w.IsDefault).ToList();
                    AdminUserName = adminUser != null && adminUser.Count > 0 ? string.Join("，", adminUser.OrderBy(o => o.CreateTime).Select(s => s.NickName)) : "无";
                    AdminUserIds = adminUser?.OrderByDescending(o => o.IsDefault).Select(s => s.Id).ToList();
                }
            }
            if (e_roles != null && e_roles.Count > 0)
            {
                RoleIds = e_roles.Select(s => s.RoleId).ToList();
            }

            if (halfMenuIdsStr != null)
            {
                HalfMenuIds = string.IsNullOrWhiteSpace(halfMenuIdsStr) ? new List<long>() : halfMenuIdsStr.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            }

            if (h5HalfMenuIdsStr != null)
            {
                H5HalfMenuIds = string.IsNullOrWhiteSpace(h5HalfMenuIdsStr) ? new List<long>() : h5HalfMenuIdsStr.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            }

            if (areaRelationId != null)
            {
                AreaRelationId = areaRelationId;
            }
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        public void BindUsers(List<EnterpriseUser> e_users)
        {
            if (e_users.Count > 0)
            {
                UserIds = e_users.Select(s => s.UserId).ToList();
            }
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        public void BindRoles(List<EnterpriseRole> e_roles)
        {
            if (e_roles != null && e_roles.Count > 0)
            {
                RoleIds = e_roles.Select(s => s.RoleId).ToList();
            }
        }
    }

    /// <summary>
    /// 企业信息列表
    /// </summary>
    public class EnterpriseInfoListDto
    {
        /// <summary>
        /// 企业信息
        /// </summary>
        public List<EnterpriseInfoDto> enterpriseInfoDtos { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EnterpriseInfoListDto()
        {
            enterpriseInfoDtos = new List<EnterpriseInfoDto>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseInfoListDto(List<EnterpriseInfo> enterpriseInfos, List<ApplicationUser> users)
        {
            if (enterpriseInfos.Count > 0)
            {
                enterpriseInfoDtos = new List<EnterpriseInfoDto>();
                enterpriseInfos.ForEach(x =>
                {
                    enterpriseInfoDtos.Add(new EnterpriseInfoDto(x.Id, x.Name, x.EnterpriseTypeId, x.Pid, x.MenuIds, x.H5MenuIds, x.IsDefault, x.Remark, x.Users, x.Roles, x.EnterpriseType, x.HalfMenuIds, x.H5HalfMenuIds));
                });
            }
        }
    }
}