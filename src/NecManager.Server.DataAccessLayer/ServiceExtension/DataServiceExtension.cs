namespace NecManager.Server.DataAccessLayer.ServiceExtension;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Server.DataAccessLayer.EntityLayer;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;
using NecManager.Server.DataAccessLayer.Seeders;

public static class DataServiceExtension
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NecDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("Main"), opt => opt.MigrationsAssembly("NecManager.Server.DataAccessLayer"))
        );

        services.AddDbContext<NecLiteDbContext>(option =>
            option.UseSqlite(configuration.GetConnectionString("MainLite"), opt => opt.MigrationsAssembly("NecManager.Server.DataAccessLayer"))
        );

        services.AddScoped<UserSeeder>();
        services.AddHostedService<InitializeDb>();

        services.AddTransient<IGroupAccessLayer, GroupAccessLayer>();

        services.AddTransient<IStudentAccessLayer, StudentAccessLayer>();

        services.AddTransient<ITrainingAccessLayer, TrainingAccessLayer>();

        services.AddTransient<ILessonAccessLayer, LessonAccessLayer>();

        return services;
    }
}
