namespace NecManager.Server.DataAccessLayer.EntityLayer;

using Microsoft.EntityFrameworkCore;

using NecManager.Server.DataAccessLayer.Model;

/// <summary>
///     Represents the Nec Database.
/// </summary>
public class NecDbContext : DbContext
{
    public NecDbContext(DbContextOptions<NecDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets the student set.
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();

    /// <summary>
    ///     Gets the group set.
    /// </summary>
    public DbSet<Group> Groups => this.Set<Group>();
}
