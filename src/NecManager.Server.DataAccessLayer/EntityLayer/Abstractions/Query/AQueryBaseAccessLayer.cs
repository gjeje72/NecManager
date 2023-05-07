namespace NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NecManager.Common;
using NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.Model.Abstraction;

/// <summary>
///     Abstraction that provides basic CRUD operations on models data.
/// </summary>
/// <typeparam name="TContext">The context to query.</typeparam>
/// <typeparam name="TEntity">The entity type to query.</typeparam>
/// <typeparam name="TQueryDto">The query type.</typeparam>
public abstract class AQueryBaseAccessLayer<TContext, TEntity, TQueryDto> : BaseAccessLayer<TContext, TEntity>, IQueryBaseAccessLayer<TEntity, TQueryDto>
    where TEntity : ADataObject
    where TContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AQueryBaseAccessLayer{TContext, TEntity,TQueryDto}" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    protected AQueryBaseAccessLayer(TContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetCollectionAsync(TQueryDto query, bool isPageable = true)
        => await this.GetCollectionInternal(query, isPageable).ToListAsync();

    /// <inheritdoc />
    public async Task<PageableResult<TEntity>> GetPageableCollectionAsync(TQueryDto query, bool isPageable = true)
        => new()
        {
            Items = await this.GetCollectionInternal(query, isPageable).ToListAsync(),
            TotalElements = await this.GetCollectionInternal(query, false).CountAsync()
        };

    /// <inheritdoc />
    public async Task<int> CountCollectionAsync(TQueryDto query, bool isPageable = false)
        => await this.GetCollectionInternal(query, false).CountAsync();

    /// <summary>
    ///     Internal method that retrieve a queryable collection of data according to the query.
    /// </summary>
    /// <param name="query">Query object to filter data.</param>
    /// <param name="isPageable">Boolean that determines if the result is paged.</param>
    /// <returns>Returns the queryable list of entity.</returns>
    protected abstract IQueryable<TEntity> GetCollectionInternal(TQueryDto query, bool isPageable = true);
}
