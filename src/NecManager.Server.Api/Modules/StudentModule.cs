namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common.Security;
using NecManager.Server.Api.Business;
using NecManager.Server.Api.Business.Modules.Student;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;

/// <summary>
///     Class which defines the student module.
/// </summary>
public sealed class StudentModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddStudentBusiness(configuration);
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/students", (ApiRequestHeaders header, [FromServices] IStudentBusinessModule studentService)
            => Results.Extensions.ApiResponse(studentService.GetStudents(header)))
            .ProducesApiResponse<IEnumerable<StudentOutputBase>>()
            .WithName("Get all students")
            .WithTags("Students");

        endpoints.MapPost("/students", async (ApiRequestHeaders header, [FromBody] StudentCreationInput createInput, [FromServices] IStudentBusinessModule studentService)
            => Results.Extensions.ApiResponseEmpty(await studentService.CreateStudent(createInput,header)))
            .ProducesApiResponseEmpty()
            .WithName("Create a new student")
            .WithTags("Students");

        return endpoints;
    }
}
