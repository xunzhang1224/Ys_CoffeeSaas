namespace YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YS.CoffeeMachine.Application.Dtos.PagingDto;
    using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
    using YS.CoffeeMachine.Application.Tools;
    using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;

    /// <summary>
    /// 企业信息数据传输对象（DTO），用于在应用程序层和表现层之间传递企业相关数据
    /// </summary>
    public class EnterpriseInfoDto
    {
        /// <summary>
        /// 企业唯一标识 ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 企业类型 ID，关联企业类型表
        /// </summary>
        public long? EnterpriseTypeId { get; set; }

        /// <summary>
        /// 企业类型 DTO 对象（可为空）
        /// </summary>
        public P_EnterpriseTypesDto EnterpriseType { get; set; }

        /// <summary>
        /// 企业类型名称，从 EnterpriseType 中提取
        /// </summary>
        public string EnterpriseTypeText { get; set; }

        /// <summary>
        /// 上级企业 ID（可为空，表示根节点）
        /// </summary>
        public long? Pid { get; set; }

        /// <summary>
        /// 是否为默认企业
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 当前企业拥有的菜单 ID 集合（逗号分隔字符串）
        /// </summary>
        public string MenuIds { get; set; }

        /// <summary>
        /// 菜单 ID 列表，从 MenuIds 字符串解析而来
        /// </summary>
        public List<long> MenuIdsList
        {
            get
            {
                return !string.IsNullOrWhiteSpace(MenuIds)
                    ? MenuIds.Split(',').Select(s => Convert.ToInt64(s)).ToList()
                    : new List<long>();
            }
        }

        /// <summary>
        /// 备注信息，用于描述企业的额外信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 树形结构中的层级深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 企业管理员用户名列表，多个以中文逗号分隔
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// 关联用户 ID 列表
        /// </summary>
        public List<long> UserIds { get; set; }

        /// <summary>
        /// 关联角色 ID 列表
        /// </summary>
        public List<long> RoleIds { get; set; }

        /// <summary>
        /// 子企业集合，支持树形结构递归
        /// </summary>
        public List<EnterpriseInfoDto> children { get; set; } = new List<EnterpriseInfoDto>();

        /// <summary>
        /// 默认构造函数，初始化空集合
        /// </summary>
        protected EnterpriseInfoDto()
        {
            UserIds = new List<long>();
            RoleIds = new List<long>();
            children = new List<EnterpriseInfoDto>();
        }

        /// <summary>
        /// 带参数的构造函数，用于从实体对象初始化 DTO 对象
        /// </summary>
        /// <param name="id">企业ID</param>
        /// <param name="name">企业名称</param>
        /// <param name="enterpriseTypeId">企业类型ID</param>
        /// <param name="pid">上级企业ID</param>
        /// <param name="menuIds">菜单ID字符串（逗号分隔）</param>
        /// <param name="isDefault">是否为默认企业</param>
        /// <param name="remark">备注信息</param>
        /// <param name="e_users">关联用户集合</param>
        /// <param name="e_roles">关联角色集合</param>
        /// <param name="enterpriseType">企业类型实体</param>
        public EnterpriseInfoDto(
            long id,
            string name,
            long? enterpriseTypeId,
            long? pid,
            string menuIds,
            bool isDefault,
            string remark,
            List<EnterpriseUser>? e_users = null,
            List<EnterpriseRole>? e_roles = null,
            EnterpriseTypes enterpriseType = null)
        {
            Id = id;
            Name = name;
            if (enterpriseTypeId != null && enterpriseTypeId > 0)
                EnterpriseTypeId = enterpriseTypeId;
            EnterpriseTypeText = enterpriseType?.Name ?? string.Empty;
            Pid = pid;
            MenuIds = menuIds;
            IsDefault = isDefault;
            Remark = remark;

            if (e_users != null && e_users.Count > 0)
            {
                var users = e_users.Where(w => w.User != null).Select(s => s.User).ToList();
                UserIds = e_users.Select(s => s.UserId).ToList();

                if (users != null && users.Count > 0)
                {
                    // 获取企业管理员
                    var adminUser = users.Where(w =>
                        UserIds.Contains(w.Id) &&
                        (w.IsDefault || w.AccountType == AccountTypeEnum.SuperAdmin)).ToList();

                    AdminUserName = adminUser.Any()
                        ? string.Join("，", adminUser.Select(s => s.NickName))
                        : "无";
                }
            }

            if (e_roles != null && e_roles.Count > 0)
            {
                RoleIds = e_roles.Select(s => s.RoleId).ToList();
            }
        }

        /// <summary>
        /// 绑定关联用户到 UserIds
        /// </summary>
        /// <param name="e_users">企业用户集合</param>
        public void BindUsers(List<EnterpriseUser> e_users)
        {
            if (e_users.Count > 0)
            {
                UserIds = e_users.Select(s => s.UserId).ToList();
            }
        }

        /// <summary>
        /// 绑定关联角色到 RoleIds
        /// </summary>
        /// <param name="e_roles">企业角色集合</param>
        public void BindRoles(List<EnterpriseRole> e_roles)
        {
            if (e_roles != null && e_roles.Count > 0)
            {
                RoleIds = e_roles.Select(s => s.RoleId).ToList();
            }
        }
    }

    /// <summary>
    /// 企业信息列表数据传输对象，用于封装多个 EnterpriseInfoDto 实例
    /// </summary>
    public class EnterpriseInfoListDto
    {
        /// <summary>
        /// 企业 DTO 列表
        /// </summary>
        public List<EnterpriseInfoDto> enterpriseInfoDtos { get; set; }

        /// <summary>
        /// 受保护的无参构造函数，初始化空的企业 DTO 列表
        /// </summary>
        protected EnterpriseInfoListDto()
        {
            enterpriseInfoDtos = new List<EnterpriseInfoDto>();
        }

        /// <summary>
        /// 根据企业实体列表初始化 DTO 列表
        /// </summary>
        /// <param name="enterpriseInfos">企业实体集合</param>
        /// <param name="users">用户集合（当前未使用）</param>
        public EnterpriseInfoListDto(List<EnterpriseInfo> enterpriseInfos, List<ApplicationUser> users)
        {
            if (enterpriseInfos.Count > 0)
            {
                enterpriseInfoDtos = new List<EnterpriseInfoDto>();
                enterpriseInfos.ForEach(x =>
                {
                    enterpriseInfoDtos.Add(new EnterpriseInfoDto(
                        x.Id,
                        x.Name,
                        x.EnterpriseTypeId,
                        x.Pid,
                        x.MenuIds,
                        x.IsDefault,
                        x.Remark,
                        x.Users,
                        x.Roles,
                        x.EnterpriseType));
                });
            }
        }
    }

    /// <summary>
    /// 企业注册审核列表输入参数
    /// </summary>
    public class P_EnterpriseRegisterInput : QueryRequest
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; } = null;

        /// <summary>
        /// 企业类型 ID
        /// </summary>
        public EnterpriseOrganizationTypeEnum? OrganizationType { get; set; } = null;
        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? RegistrationProgress { get; set; } = null;
    }

    /// <summary>
    /// 企业注册DTO
    /// </summary>
    public class P_EnterpriseRegisterDto
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
        /// 企业资质类型
        /// </summary>
        public EnterpriseOrganizationTypeEnum? OrganizationType { get; set; }
        /// <summary>
        /// 企业类型名称，从 EnterpriseType 中提取
        /// </summary>
        public string OrganizationTypeText { get { return OrganizationType == null ? "--" : OrganizationType.Value.GetDescriptionOrValue(); } }
        /// <summary>
        /// 企业管理员账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 企业管理员昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机区号
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 企业管理员手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 企业管理员邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegistrationTime { get; set; }
        /// <summary>
        /// 注册进度
        /// </summary>
        public RegistrationProgress? registrationProgress
        { get; set; } = null;
        /// <summary>
        /// 注册进度文本
        /// </summary>
        public string RegistrationProgressText
        {
            get
            {
                return registrationProgress switch
                {
                    RegistrationProgress.NotStarted => "未开始",
                    RegistrationProgress.InReview => "审核中",
                    RegistrationProgress.Passed => "审核通过",
                    RegistrationProgress.Failed => "已拒绝",
                    _ => "--"
                };
            }
        }
    }
}