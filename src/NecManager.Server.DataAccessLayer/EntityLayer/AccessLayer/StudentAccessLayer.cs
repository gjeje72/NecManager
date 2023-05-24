namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using NecManager.Common;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;
using NecManager.Server.DataAccessLayer.ServiceExtension;

public class StudentAccessLayer : IStudentAccessLayer
{
    public StudentAccessLayer(NecLiteDbContext context)
    {
        this.Context = context;
        this.ModelSet = this.Context.Set<Student>();
    }

    /// <summary>
    ///     Gets the Db context.
    /// </summary>
    protected DbContext Context { get; }

    /// <summary>
    ///     Gets the Db model set.
    /// </summary>
    protected DbSet<Student> ModelSet { get; }

    /// <inheritdoc/>
    public async Task<string> AddAsync(Student model)
    {
        var result = this.ModelSet.Add(model);
        _ = await this.Context.SaveChangesAsync().ConfigureAwait(false);

        return result.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task<int> AddRangeAsync(IEnumerable<Student> students)
    {
        this.ModelSet.AddRange(students);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IQueryable<Student> GetCollection(Expression<Func<Student, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<Student>, IIncludableQueryable<Student, object>>? navigationProperties = null)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery.Where(filter);

        return trackingEnabled
            ? dbQuery
            : dbQuery.AsNoTracking();
    }

    /// <inheritdoc/>
    public Task<Student?> GetSingleAsync(Expression<Func<Student, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<Student>, IIncludableQueryable<Student, object>>? navigationProperties = null)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        return trackingEnabled
                        ? dbQuery.FirstOrDefaultAsync()
                        : dbQuery.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<int> UpdateAsync(Student model)
    {
        _ = this.ModelSet.Update(model);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> UpdateListAsync(IEnumerable<Student> models)
    {
        this.ModelSet.UpdateRange(models);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PageableResult<Student>> GetPageableCollectionAsync(StudentQuery query, bool isPageable)
    {
        var students = this.GetStudentListingQuery(query, isPageable);

        return await students.GetPageableResultFromQueryAsync(query.CurrentPage, pageSize: query.PageSize);
    }

    private IQueryable<Student> GetStudentListingQuery(StudentQuery query, bool isPageable = true)
    {
        IQueryable<Student> queryable = this.ModelSet.Include(l => l.StudentGroups).ThenInclude(sg => sg.Group);

        if (query.State is not null)
            queryable = queryable.Where(l => l.State == query.State);

        if (query.GroupId is not null)
            queryable = queryable.Where(l => l.StudentGroups.Any() && l.StudentGroups.Any(t => t.GroupId == query.GroupId));

        if (query.WeaponType is not null)
            queryable = queryable.Where(l => l.StudentGroups.Any() && l.StudentGroups.Any(sg => sg.Group != null && sg.Group.Weapon == query.WeaponType));

        if (!string.IsNullOrWhiteSpace(query.Filter))
            queryable = queryable.Where(s => s.Name.Contains(query.Filter)
                                                 || s.FirstName.Contains(query.Filter)
                                                 || s.EmailAddress.Contains(query.Filter)
                                                 || s.StudentGroups.Any(sg => sg.Group != null && sg.Group.Title.Contains(query.Filter))
                                                 );
        queryable = queryable.OrderBy(s => s.Name);
        var collectionInternal = !isPageable ? queryable : queryable.Skip((query.CurrentPage - 1) * query.PageSize).Take(query.PageSize);
        return collectionInternal;
    }

    /// <inheritdoc/>
    public async Task<int> RemoveAsync(Student model)
    {
        _ = this.ModelSet.Remove(model);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<int> RemoveAsync(string id)
    {
        var model = this.ModelSet.FirstOrDefault(model => model.Id == id);
        if (model == null)
        {
            return -1;
        }

        this.ModelSet.Remove(model);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveRangeAsync(IEnumerable<Student> models)
    {
        this.ModelSet.RemoveRange(models);
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveRangeAsync(IEnumerable<string> ids)
    {
        this.ModelSet.RemoveRange(this.ModelSet.Where(model => ids.Contains(model.Id)));
        return await this.Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsAsync(string id)
        => this.GetCollection(filter: o => o.Id == id).AnyAsync();

    /// <inheritdoc/>
    public async Task<bool> ExistsRangeAsync(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            if (!await this.GetCollection(filter: o => o.Id == id).AnyAsync())
                return false;
        }

        return true;
    }
}
