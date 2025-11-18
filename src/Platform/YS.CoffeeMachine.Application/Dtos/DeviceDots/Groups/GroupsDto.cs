namespace YS.CoffeeMachine.Application.Dtos.DeviceDots.Groups
{
    /// <summary>
    /// 分组树
    /// </summary>
    public class GroupsTreeDto
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上级分组Id
        /// </summary>
        public long? PId { get; set; }
        /// <summary>
        /// 关联人员
        /// </summary>
        public List<long> UserIds { get; set; }

        /// <summary>
        /// 用户文本
        /// </summary>
        public string UsersText { get; set; }
        /// <summary>
        /// 设备Id集合
        /// </summary>
        public List<long> DeviceIds { get; set; }
        /// <summary>
        /// 关联设备
        /// </summary>
        //public List<DeviceInfoDto> Devices { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeText { get; set; }
        /// <summary>
        /// 下级分组
        /// </summary>
        public List<GroupsTreeDto> Children
        { get; set; }
    }

    /// <summary>
    /// 分组dto
    /// </summary>
    public class GroupsDto
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组层数
        /// </summary>
        public string GroupLayers { get; set; }
        /// <summary>
        /// 上级分组Id
        /// </summary>
        public long? PId { get; set; }
        /// <summary>
        /// 关联人员
        /// </summary>
        public List<long> UsersIds { get; set; }
    }

    /// <summary>
    /// 分组列表dto
    /// </summary>
    public class GroupListDto
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public long? Pid { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组层级
        /// </summary>
        public string LayersText { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间文本
        /// </summary>
        public string CreateTimeText
        {
            get { return CreateTime.ToString("G"); }
        }
    }
}
