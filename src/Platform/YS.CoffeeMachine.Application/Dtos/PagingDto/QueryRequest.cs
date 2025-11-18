namespace YS.CoffeeMachine.Application.Dtos.PagingDto
{
    /// <summary>
    /// 高级分页
    /// </summary>
    public class QueryRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; } = 1;
        private int _pageSize;
        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize > 100 ? 100 : _pageSize; }
            set { _pageSize = value; }
        }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; } = string.Empty;
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; } = true;
        /// <summary>
        /// 是否开启关联查询
        /// </summary>
        public bool IsIncludeQueries { get; set; } = false;
        /// <summary>
        /// 条件查询参数
        /// </summary>
        public List<FilterDto> Filters { get; set; } = new List<FilterDto>();
        /// <summary>
        /// 不要分页
        /// </summary>
        public bool NotPaginate { get; set; } = false;
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    public class FilterDto
    {
        /// <summary>
        /// 查询字段名
        /// </summary>
        public string Field { get; set; }      // 查询字段名

        /// <summary>
        /// 查询值
        /// </summary>
        public object Value { get; set; }     // 查询值

        /// <summary>
        /// 是否模糊查询
        /// </summary>
        public bool IsFuzzy { get; set; }     // 是否模糊查询
    }
}
