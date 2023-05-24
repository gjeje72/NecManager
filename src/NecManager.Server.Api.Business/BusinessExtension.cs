namespace NecManager.Server.Api.Business;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using NecManager.Server.Api.Business.Modules.Group;
using NecManager.Server.Api.Business.Modules.Identity;
using NecManager.Server.Api.Business.Modules.Lesson;
using NecManager.Server.Api.Business.Modules.Student;
using NecManager.Server.Api.Business.Modules.Training;
using NecManager.Server.DataAccessLayer.ServiceExtension;

public static class BusinessExtension
{
    /// <summary>
    ///     Extension method which add dependencies of <see cref="StudentService" />.
    /// </summary>
    /// <param name="services">The application service collection for DI.</param>
    /// <returns>Returns the services collection.</returns>
    public static IServiceCollection AddStudentBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddData(configuration);

        services.TryAddTransient<IStudentBusiness, StudentBusiness>();
        services.TryAddTransient<IGroupBusiness, GroupBusiness>();
        services.TryAddTransient<ILessonBusiness, LessonBusiness>();
        services.TryAddTransient<ITrainingBusiness, TrainingBusiness>();

        return services;
    }

    public static IServiceCollection AddIdentityBusiness(this IServiceCollection services)
    {
        services.TryAddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
