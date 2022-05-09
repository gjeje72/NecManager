namespace NecManager_DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NecManager_DAL.Seeder;

/// <summary>
///     This class is used to initialize the database.
/// </summary>
public class InitializeDb : IHostedService
{
    private readonly IHostEnvironment env;

    private readonly IServiceScopeFactory scopeFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InitializeDb" /> class.
    /// </summary>
    /// <param name="env">Hosting environment info from DI.</param>
    /// <param name="scopeFactory">Database context.</param>
    public InitializeDb(IHostEnvironment env, IServiceScopeFactory scopeFactory)
    {
        this.env = env;
        this.scopeFactory = scopeFactory;
    }

    /// <summary>
    ///     Gets called when webHost is being started and before pipeline is initiated.
    /// </summary>
    /// <param name="cancellationToken">token use to control async task cancellation.</param>
    /// <returns>Task completion status.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = this.scopeFactory.CreateScope();
        if (!this.env.IsDevelopment())
        {
            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
        }

        await scope.ServiceProvider.GetRequiredService<UserSeeder>().EnsureSeedDataAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Gets called when webHost is being stopped.
    /// </summary>
    /// <param name="cancellationToken">token use to control async task cancellation.</param>
    /// <returns>Task completion status.</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
