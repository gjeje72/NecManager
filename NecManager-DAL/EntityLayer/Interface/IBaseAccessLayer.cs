namespace NecManager_DAL.EntityLayer.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/// <summary>
///     Interface defining base CRUD operation on model data.
/// </summary>
/// <typeparam name="TModel">Object Model that is using this implementation.</typeparam>
public interface IBaseAccessLayer<TModel>
{
    /// <summary>
    ///     Async Method that add new object in Db.
    /// </summary>
    /// <param name="model">Object model to add.</param>
    /// <returns>Returns Id of newly created data object.</returns>
    Task<int> AddAsync(TModel model);

    /// <summary>
    ///     Async Method that add a range of new object in Db.
    /// </summary>
    /// <param name="models">Enumerable of objects model to add.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> AddRangeAsync(IEnumerable<TModel> models);

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
    TModel GetSingle(Expression<Func<TModel, bool>> filter, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null);

    /// <summary>
    ///     Async Method that retrieve first data object matching the given filter.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <param name="filter">filter to apply</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns></returns>
    TResult GetSingle<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null);

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
    IEnumerable<TModel> GetCollection(Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null);

    /// <summary>
    ///     Method that retrieve a collection of data according to the filter.
    /// </summary>
    /// <remarks>
    ///     Tracking on data returned is disabled by default. 
    /// </remarks>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <param name="filter">Expression to filter data to return.</param>
    /// <param name="trackingEnabled">true if tracking is needed on data returned, false otherwise.</param>
    /// <param name="navigationProperties">Navigations properties to include for data to returned.</param>
    /// <returns>Returns Enumerable of <typeparamref name="TResult" />.</returns>
    IEnumerable<TResult> GetCollection<TResult>(Expression<Func<TModel, TResult>> selector, Expression<Func<TModel, bool>> filter = null, bool trackingEnabled = false, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> navigationProperties = null);

    /// <summary>
    ///     Async method using bulk update method to update data object from db context.
    /// </summary>
    /// <param name="models">Enumerable of Data object to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> UpdateRangeAsync(IEnumerable<TModel> models);

    /// <summary>
    ///     Async method that update a specific data object.
    /// </summary>
    /// <param name="model">The object data model to update.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> UpdateAsync(TModel model);

    /// <summary>
    ///     Async Method that remove a specific object in Db.
    /// </summary>
    /// <param name="model">The object data model to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveAsync(TModel model);

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
    Task<int> RemoveRangeAsync(IEnumerable<TModel> models);

    /// <summary>
    ///     Async method using bulk deletion method to remove data object from db context.
    /// </summary>
    /// <param name="ids">Enumerable of ids of Data object to remove.</param>
    /// <returns>Returns number of state entries written to the database.</returns>
    Task<int> RemoveRangeAsync(IEnumerable<int> ids);

    /// <summary>
    ///     Check the existance of an object in a collection from based on its id.
    /// </summary>
    /// <param name="id">Id of object to check.</param>
    /// <returns>Returns true if object exists, false otherwise.</returns>
    bool Exists(int id);
}
