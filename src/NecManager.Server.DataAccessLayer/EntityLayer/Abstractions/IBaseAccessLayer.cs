namespace NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;

/// <summary>
///     Interface defining base CRUD operation on entity data.
/// </summary>
/// <typeparam name="TEntity">Entity Model that is using this implementation.</typeparam>
public interface IBaseAccessLayer<TEntity>
{
    /// <summary>
    ///     Async Method that add new object in Db.
    /// </summary>
    /// <param name="model">Object model to add.</param>
    /// <returns>Returns the Id of newly created object.</returns>
    Task<int> AddAsync(TEntity model);

    /// <summary>
    ///     Async Method that add a range of new object in Db.
    /// </summary>
    /// <param name="models">Enumerable of objects model to add.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> AddRangeAsync(IEnumerable<TEntity> models);

    /// <summary>
    ///     Method that retrieve a collection of data according to the filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <param name="filter">Expression to filter data to return.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns Enumerable of <typeparamref name="TEntity" />.</returns>
    IQueryable<TEntity> GetCollection(Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null);

    /// <summary>
    ///     Method that retrieve a collection of data according to the filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <typeparam name="TResult">The entity type to retrieve.</typeparam>
    /// <param name="selector">Expression to select data to return.</param>
    /// <param name="filter">Expression to filter data to return.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns Enumerable of <typeparamref name="TResult" />.</returns>
    IQueryable<TResult> GetCollection<TResult>(
        Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null);

    /// <summary>
    ///     Async Method that retrieve first data object matching the given filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <param name="filter">filter to apply.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns <typeparamref name="TEntity" />.</returns>
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null);

    /// <summary>
    ///     Async Method that retrieve first data object matching the given filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default.
    /// </remarks>
    /// <typeparam name="TResult">The entity type to retrieve.</typeparam>
    /// <param name="selector">Expression to select properties on <typeparamref name="TEntity" /> object.</param>
    /// <param name="filter">filter to apply.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns <typeparamref name="TResult" />.</returns>
    TResult? GetSingle<TResult>(
        Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null);

    /// <summary>
    ///     Async method that update a specific data object.
    /// </summary>
    /// <param name="model">The object data model to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> UpdateAsync(TEntity model);

    /// <summary>
    ///     Async method that update a list of data object.
    /// </summary>
    /// <param name="models">List of data object to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> UpdateListAsync(IEnumerable<TEntity> models);

    /// <summary>
    ///     Async Method that remove a specific object in Db.
    /// </summary>
    /// <param name="model">The object data model to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveAsync(TEntity model);

    /// <summary>
    ///     Async Method that remove a specific object in Db.
    /// </summary>
    /// <param name="id">The object id of data model to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveAsync(int id);

    /// <summary>
    ///     Async method using bulk deletion method to remove data object from db context.
    /// </summary>
    /// <param name="models">Enumerable of Data object to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveRangeAsync(IEnumerable<TEntity> models);

    /// <summary>
    ///     Async method using bulk deletion method to remove data object from db context.
    /// </summary>
    /// <param name="ids">Enumerable of ids of Data object to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveRangeAsync(IEnumerable<int> ids);

    /// <summary>
    ///     Check the existence of an object in a collection from based on its id.
    /// </summary>
    /// <param name="id">Id of object to check.</param>
    /// <returns>Returns true if object exists, false otherwise.</returns>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    ///     Check the existence of all object in a collection from based on id.
    /// </summary>
    /// <param name="ids">the collection of id object to check.</param>
    /// <returns>Returns true if objects exists, false otherwise.</returns>
    Task<bool> ExistsRangeAsync(IEnumerable<int> ids);
}
