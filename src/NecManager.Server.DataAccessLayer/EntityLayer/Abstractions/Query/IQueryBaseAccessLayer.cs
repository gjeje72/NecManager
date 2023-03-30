namespace NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;

/// <summary>
///     Interface which represent the query base access layer.
/// </summary>
/// <typeparam name="TEntity">The entity type to query.</typeparam>
/// <typeparam name="TQueryDto">The query type.</typeparam>
public interface IQueryBaseAccessLayer<TEntity, in TQueryDto> : IBaseAccessLayer<TEntity>
{
    /// <summary>
    ///     Method that retrieve the count of collection of data according to the query.
    /// </summary>
    /// <param name="query">Query object to filter data.</param>
    /// <param name="isPageable">Boolean that determines if the result is paged.</param>
    /// <returns>Returns the result count item according the query.</returns>
    Task<int> CountCollectionAsync(TQueryDto query, bool isPageable = false);

    /// <summary>
    ///     Method that retrieve a collection of data according to the query.
    /// </summary>
    /// <param name="query">Query object to filter data.</param>
    /// <param name="isPageable">Boolean that determines if the result is paged.</param>
    /// <returns>Returns the collection result of query.</returns>
    Task<IEnumerable<TEntity>> GetCollectionAsync(TQueryDto query, bool isPageable = true);

    /// <summary>
    ///     Method that retrieve a collection of data according to the query.
    /// </summary>
    /// <param name="query">Query object to filter data.</param>
    /// <param name="isPageable">Boolean that determines if the result is paged.</param>
    /// <returns>Returns the collection result of query.</returns>
    Task<PageableResult<TEntity>> GetPageableCollectionAsync(TQueryDto query, bool isPageable = true);
}
