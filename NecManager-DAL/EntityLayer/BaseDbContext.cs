namespace NecManager_DAL.EntityLayer;

using System;
using System.Linq;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NecManager_DAL.Model;

/// <summary>
///     Abstract definition of <seealso cref="DbContext"/>
///     to auto-fill <seealso cref="ADataObject.CreationDate"/> and  <seealso cref="ADataObject.LastModifiedDate"/> when <seealso cref="SaveChanges"/> is called.
/// </summary>
public abstract class BaseDbContext : IdentityDbContext<User>
{
    /// <inheritdoc/>
    public BaseDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    ///     Update <seealso cref="ADataObject.CreationDate"/> and <seealso cref="ADataObject.LastModifiedDate"/> before apply changes in databse.
    /// </summary>
    /// <returns>The number of state entrie written to the database.</returns>
    public override int SaveChanges()
    {
        this.UpdateDateChanges();
        return base.SaveChanges();
    }

    /// <summary>
    ///     Async method to update <seealso cref="ADataObject.CreationDate"/> and <seealso cref="ADataObject.LastModifiedDate"/> before apply changes in databse.
    /// </summary>
    /// <returns>The number of state entrie written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdateDateChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     For each entity to be saved in database, this method fills :
    ///     <seealso cref="ADataObject.CreationDate"/> if entity state is <seealso cref="EntityState.Added"/>
    ///     <seealso cref="ADataObject.LastModifiedDate"/> if entity state is <seealso cref="EntityState.Modified"/>
    /// </summary>
    private void UpdateDateChanges()
    {
        var changes = from entity in this.ChangeTracker.Entries()
                      where entity.State != EntityState.Unchanged
                      select entity;

        foreach (var item in changes)
        {
            if (item.Entity is ADataObject trackedEntity)
            {
                if (item.State == EntityState.Added && trackedEntity.CreationDate == default)
                {
                    trackedEntity.CreationDate = DateTime.UtcNow;
                }
                else if (item.State == EntityState.Modified)
                {
                    trackedEntity.LastModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
