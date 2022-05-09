namespace NecManager_DAL;

using Microsoft.EntityFrameworkCore;

/// <summary>
///     Represents the application Database.
/// </summary>
public class SqlServerDbContext : AppDbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SqlServerDbContext" /> class.
    /// </summary>
    /// <param name="options">The db context options.</param>
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
    {
    }
}
