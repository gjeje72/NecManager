namespace NecManager_DAL.ServiceExtension;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NecManager_DAL.EntityLayer.AccessLayer;
using NecManager_DAL.Model;
using NecManager_DAL.Seeder;

/// <summary>
///     Provides extensions methods to register NecManager access layer in DI.
/// </summary>
public static class DataServiceExtension
{
    /// <summary>
    ///     Extension method use to initialize data access.
    /// </summary>
    /// <param name="services">Collection of the available services for the app.</param>
    /// <param name="dbcontextBuilder">The database context options builder.</param>
    /// <returns>Returns edited services collection.</returns>
    public static IServiceCollection AddData(this IServiceCollection services, Action<DbContextOptionsBuilder> dbcontextBuilder, IHostEnvironment environment)
    {
        services.AddDbContext<AppDbContext, SqlServerDbContext>(dbcontextBuilder);
        services.AddIdentityCore<User>(options =>
        {
            options.User.RequireUniqueEmail = false;
        })
        .AddEntityFrameworkStores<AppDbContext>();

        //services.AddHostedService<InitializeDb>();
        //services.AddScoped<UserSeeder>();

        services.AddTransient<StudentAccessLayer>();


        return services;
    }

    /// <summary>
    ///     Extension method use to initialize data access.
    /// </summary>
    /// <param name="services">Collection of the available services for the app.</param>
    /// <param name="builder">The database context options builder.</param>
    public static void AddReportData(this IServiceCollection services, Action<DbContextOptionsBuilder> builder)
    {
        services.AddDbContext<AppDbContext>(builder);


    }
}
