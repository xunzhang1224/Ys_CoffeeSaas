using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.Pagings
{
    /// <summary>
    /// 分页拓展类
    /// </summary>
    public static class IQueryableExtensions
    {

        /// <summary>
        /// 对查询进行分页处理，并支持关联表加载（Include）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">原始查询（可以包含Include）</param>
        /// <param name="request"></param>
        /// <param name="navigationPropertyPaths"></param>
        /// <returns>包含分页结果的对象</returns>
        public static async Task<PagedResultDto<TEntity>> ToPagedListAsync<TEntity>(
            this IQueryable<TEntity> query,
            QueryRequest request, params string[] navigationPropertyPaths) where TEntity : class
        {
            if (!request.NotPaginate)
            {
                if (request.PageNumber <= 0 || request.PageSize <= 0)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0076)]);
            }
            var result = new PagedResultDto<TEntity>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize, // 限制每页的最大记录数
            };

            // 条件查询
            query = ApplyFilters(query, request);
            // 排序
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                var entityType = typeof(TEntity);
                var property = entityType
                    .GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, request.SortField, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                    query = query.OrderByIsAscending(property, request.IsAscending);
            }
            //是否有关联查询
            if (request.IsIncludeQueries && navigationPropertyPaths != null)
            {
                foreach (var navigationProperty in navigationPropertyPaths)
                {
                    query = query.Include(navigationProperty);
                }
            }
            //读取数据
            result.Items = request.NotPaginate ? await query.ToListAsync() : await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
            //获取总记录数
            result.TotalCount = await query.CountAsync();
            return result;
        }

        /// <summary>
        /// 构造过滤条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query, QueryRequest request) where TEntity : class
        {
            if (request.Filters != null && request.Filters.Any())
            {
                var parameter = Expression.Parameter(typeof(TEntity), "e");
                Expression combinedPredicate = null;

                foreach (var filter in request.Filters)
                {
                    if (filter.Value == null && (!(filter.Value is string) || string.IsNullOrWhiteSpace(filter.Value as string))) continue;
                    var property = typeof(TEntity).GetProperty(filter.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                        continue;
                    //throw new InvalidOperationException($"Property '{filter.Field}' not found on type '{typeof(TEntity).Name}'.");

                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                    // 处理 JsonElement 转换为字符串
                    object? value;
                    if (filter.Value is JsonElement jsonElement)
                    {
                        // 根据 JsonElement 的类型安全地解析值
                        switch (jsonElement.ValueKind)
                        {
                            case JsonValueKind.String:
                                value = jsonElement.GetString();
                                break;
                            case JsonValueKind.Number:
                                value = jsonElement.GetInt64(); // 如果需要整数，可以改为 jsonElement.GetInt32() 等
                                break;
                            case JsonValueKind.True:
                            case JsonValueKind.False:
                                value = jsonElement.GetBoolean();
                                break;
                            case JsonValueKind.Null:
                                value = null;
                                break;
                            default:
                                throw new InvalidOperationException($"Unsupported JsonElement type: {jsonElement.ValueKind}");
                        }
                    }
                    else
                    {
                        value = filter.Value; // 如果不是 JsonElement，直接使用原始值
                    }

                    MethodInfo? likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                        "Like",
                        BindingFlags.Static | BindingFlags.Public,
                        null,
                        new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                        null);

                    if (likeMethod == null)
                        throw new InvalidOperationException("EF.Functions.Like method not found.");
                    // 确保 constant 的值类型和 propertyAccess 的类型一致
                    if (value != null)
                    {
                        var propertyType = property.PropertyType;

                        if (propertyType.IsEnum)
                        {
                            // 如果是枚举类型，使用 Enum.ToObject 转换
                            value = Enum.ToObject(propertyType, value);
                        }
                        else
                        {
                            // 其他情况使用 Convert.ChangeType
                            value = Convert.ChangeType(value, propertyType);
                        }
                    }

                    var constant = Expression.Constant(value, propertyAccess.Type);

                    Expression predicate = filter.IsFuzzy
                        ? Expression.Call(
                            likeMethod,
                            Expression.Constant(EF.Functions),
                            propertyAccess,
                            Expression.Constant($"%{value}%"))
                        : Expression.Equal(propertyAccess, constant);

                    combinedPredicate = combinedPredicate == null
                        ? predicate
                        : Expression.AndAlso(combinedPredicate, predicate);
                }

                if (combinedPredicate != null)
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(combinedPredicate, parameter));
            }

            return query;
        }

        /// <summary>
        /// IQueryable 分页扩展方法
        /// </summary>
        public static async Task<PagedResultDto<T>> ToPagedListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            // 1. 计算总数
            var totalCount = await query.CountAsync();

            // 2. 分页查询
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 3. 返回结果
            return new PagedResultDto<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // 动态排序扩展方法
        private static IQueryable<TEntity> OrderByIsAscending<TEntity>(this IQueryable<TEntity> source, PropertyInfo property, bool isAscending)
        {
            return isAscending ? source.OrderBy(e => EF.Property<object>(e, property.Name)) : source.OrderByDescending(e => EF.Property<object>(e, property.Name));
        }
    }
}
