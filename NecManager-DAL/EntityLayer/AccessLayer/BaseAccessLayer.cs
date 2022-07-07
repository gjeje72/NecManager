namespace NecManager_DAL.EntityLayer.AccessLayer;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NecManager_DAL.EntityLayer.Interface;
using NecManager_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/// <summary>
///     Abstraction that provides basic CRUD operations on models data.
/// </summary>
/// <typeparam name="TContext">Data context that is using this implementation.</typeparam>
/// <typeparam name="TModel">Object Model that is using this implementation.</typeparam>
public abstract class BaseAccessLayer<TContext, TModel> : IBaseAccessLayer<TModel>
    where TModel : ADataObject
    where TContext : DbContext
{
    /// <summary>
    ///     Gets the Db context.
    /// </summary>
    protected readonly TContext context;

    /// <summary>
    ///     Gets the Db model set.
    /// </summary>
    protected readonly DbSet<TModel> modelSet;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseAccessLayer{TContext, TModel}" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    protected BaseAccessLayer(TContext context)
    {
        this.context = context;
        this.modelSet = this.context.Set<TModel>();
    }

    /// <summary>
    ///     Async Method that add new object in Db.
    /// </summary>
    /// <param name="model">Object model to add</param>
    /// <returns>Returns the Id of newly created object.</returns>
    public async Task<int> AddAsync(TModel model)
    {
        var result = this.modelSet.Add(model);
        await this.context.SaveChangesAsync().ConfigureAwait(false);

        return result.Entity.Id;
    }

    /// <summary>
    ///     Async Method that add a range of new object in Db.
    /// </summary>
    /// <param name="models">Enumerable of objects model to add.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    public async Task<int> AddRangeAsync(IEnumerable<TModel> models)
    {
        this.modelSet.AddRange(models);
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Method that retrieve a collection of data according to the filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default. 
    /// </remarks>
    /// <param name="filter">Expression to filter data to return.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns Enumerable of <typeparamref name="TModel" />.</returns>
    public IEnumerable<TModel> GetCollection(Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null)
    {
        var dbQuery = this.modelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var collection = trackingEnabled
                        ? dbQuery
                        : dbQuery.AsNoTracking();

        return collection;
    }

    /// <summary>
    ///     Method that retrieve a collection of data according to the filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default. 
    /// </remarks>
    /// <param name="filter">Expression to filter data to return.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns Enumerable of <typeparamref name="TModel" />.</returns>
    public IEnumerable<TResult> GetCollection<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null)
    {
        var dbQuery = this.modelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var collection = trackingEnabled
                        ? dbQuery.Select(selector)
                        : dbQuery.AsNoTracking().Select(selector);

        return collection;
    }

    /// <summary>
    ///     Async Method that retrieve first data object matching the given filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <param name="filter">filter to apply</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns <typeparamref name="TModel" />.</returns>
    public TModel GetSingle(Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null)
    {
        var dbQuery = this.modelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var item = trackingEnabled
                        ? dbQuery.FirstOrDefault()
                        : dbQuery.AsNoTracking().FirstOrDefault();

        return item;
    }

    /// <summary>
    ///     Async Method that retrieve first data object matching the given filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <param name="filter">filter to apply</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns <typeparamref name="TModel" />.</returns>
    public TResult GetSingle<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null)
    {
        var dbQuery = this.modelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var item = trackingEnabled
                        ? dbQuery.Select(selector).FirstOrDefault()
                        : dbQuery.AsNoTracking().Select(selector).FirstOrDefault();

        return item;
    }

    /// <summary>
    ///     Async method that update a specific data object.
    /// </summary>
    /// <param name="model">The object data model to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    public async Task<int> UpdateAsync(TModel model)
    {
        this.modelSet.Update(model);
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Async method that update a list of data object.
    /// </summary>
    /// <param name="models">The object data model to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    public async Task<int> UpdateRangeAsync(IEnumerable<TModel> models)
    {
        this.modelSet.UpdateRange(models);
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveAsync(TModel model)
    {
        this.modelSet.Remove(model);
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<int> RemoveAsync(int id)
    {
        this.modelSet.Remove(this.modelSet.FirstOrDefault(model => model.Id == id));
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveRangeAsync(IEnumerable<TModel> models)
    {
        this.modelSet.RemoveRange(models);
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveRangeAsync(IEnumerable<int> ids)
    {
        this.modelSet.RemoveRange(this.modelSet.Where(model => ids.Contains(model.Id)));
        return await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Check the existance of an object in a collection from based on its id.
    /// </summary>
    /// <param name="id">Id of object to check.</param>
    /// <returns>Returns true if object exists, false otherwise.</returns>
    public bool Exists(int id)
        => this.GetCollection(filter: o => o.Id == id).Any();
}