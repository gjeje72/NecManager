﻿namespace NecManager.Server.DataAccessLayer.ServiceExtension;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Server.DataAccessLayer.EntityLayer;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

public static class DataServiceExtension
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NecDbContext>(option =>
        option.UseSqlServer(configuration.GetConnectionString("Main"), opt => opt.MigrationsAssembly("NecManager.Server.DataAccessLayer"))
        );

        services.AddHostedService<InitializeDb>();

        services.AddTransient<IGroupAccessLayer, GroupAccessLayer>();

        services.AddTransient<IStudentAccessLayer, StudentAccessLayer>();

        return services;
    }
}
