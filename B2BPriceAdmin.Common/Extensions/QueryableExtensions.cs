using System.Linq.Expressions;

namespace B2BPriceAdmin.Common.Extensions
{
    /// <summary>
    /// Extension methods for Queryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Dynamic Sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderByMember"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<T> DynamicSort<T>(this IQueryable<T> query, string orderByMember, string direction)
        {
            if (string.IsNullOrEmpty(orderByMember))
            {
                return query;
            }
            var queryElementTypeParam = Expression.Parameter(typeof(T));
            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);
            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == "a" ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));
            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}
