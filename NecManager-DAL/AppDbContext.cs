namespace NecManager_DAL;

using Microsoft.EntityFrameworkCore;
using NecManager_DAL.EntityLayer;
using NecManager_DAL.Model;

/// <summary>
///     Represents the application Database.
/// </summary>
public class AppDbContext : BaseDbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AppDbContext" /> class.
    /// </summary>
    /// <param name="options">The db context options.</param>
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets the students.
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();
}
