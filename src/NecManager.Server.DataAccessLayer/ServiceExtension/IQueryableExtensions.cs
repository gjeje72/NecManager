namespace NecManager.Server.DataAccessLayer.ServiceExtension;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NecManager.Common;


/// <summary>
///     Provides extensions methods to call query.
/// </summary>
internal static class IQueryableExtensions
{
    /// <summary>
    ///     Extension method used to get <see cref="PageableQuery" /> from query.
    /// </summary>
    /// <typeparam name="TItem">Type of model.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="currentPage">The current page of the result.</param>
    /// <param name="pageSize">The page size of the result.</param>
    /// <returns>A <see cref="PageableResult{TItem}" /> from the query.</returns>
    public static async Task<PageableResult<TItem>> GetPageableResultFromQueryAsync<TItem>(this IQueryable<TItem> query, int currentPage, int pageSize)
        => new()
        {
            Items = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync().ConfigureAwait(false),
            TotalElements = await query.CountAsync().ConfigureAwait(false)
        };
}
