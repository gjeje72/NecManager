namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common;
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
        endpoints.MapGet("/students",
            async (ApiRequestHeaders header, [FromServices] IStudentBusiness studentService, /*BindAsync*/ StudentQueryInput query)
                //[Authorize] async (ApiRequestHeaders header, [FromServices] IStudentBusiness studentService, /*BindAsync*/ StudentQueryInput query)
                => Results.Extensions.ApiResponse(await studentService.SearchStudents(header, query)))
                .ProducesApiResponse<PageableResult<StudentOutputBase>>()
                .WithName("Get all students")
                .WithTags("Students");

        _ = endpoints.MapGet("/students/{studentId:int}",
             async (ApiRequestHeaders requestHeaders, [FromServices] IStudentBusiness studentService, [FromRoute] string studentId)
                //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponse(await studentService.GetStudentByIdAsync(requestHeaders, studentId)))
                .ProducesApiResponse<StudentOutputBase>()
                .WithName("Get a student by its identifier.")
                .WithTags("Students");

        endpoints.MapPost("/students",
            async (ApiRequestHeaders header, [FromBody] StudentCreationInput createInput, [FromServices] IStudentBusiness studentService)
                => Results.Extensions.ApiResponseEmpty(await studentService.CreateStudent(createInput, header)))
                .ProducesApiResponseEmpty()
                .WithName("Create a new student")
                .WithTags("Students");

        _ = endpoints.MapPut("/students/{studentId:int}",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingUpdateInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] IStudentBusiness studentService, [FromRoute] int studentId, [FromBody] StudentUpdateInput input)
                => Results.Extensions.ApiResponseEmpty(await studentService.UpdateStudentAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing student")
                .WithTags("Students");

        _ = endpoints.MapDelete("/students/{studentId:int}",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] IStudentBusiness studentService, [FromRoute] int studentId)
             async (ApiRequestHeaders requestHeaders, [FromServices] IStudentBusiness studentService, [FromRoute] string studentId)
                => Results.Extensions.ApiResponseEmpty(await studentService.DeleteStudentAsync(requestHeaders, studentId)))
                .ProducesApiResponseEmpty()
                .WithName("Delete an existing student")
                .WithTags("Students");

        return endpoints;
    }
}
