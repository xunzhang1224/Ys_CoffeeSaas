using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace YS.CoffeeMachine.Platform.API.Queries
{
    /// <summary>
    /// 自定义关联查询
    /// </summary>
    public static class QueryExtensions
    {        /// <summary>
             /// 自定义Include多个关联实体
             /// </summary>
        public static IQueryable<TEntity> Includes<TEntity>(this IQueryable<TEntity> query, params string[] navigationPropertyPaths)
        where TEntity : class
        {
            foreach (var navigationProperty in navigationPropertyPaths)
            {
                query = query.Include(navigationProperty);  // 动态 Include 属性
            }
            return query;
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }
    }
}
