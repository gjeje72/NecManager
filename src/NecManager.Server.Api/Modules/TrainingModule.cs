namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Training;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;

public sealed class TrainingModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapGet("/trainings",
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, /*BindAsync*/ TrainingQueryInput query)
                //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, /*BindAsync*/ LessonQueryInput query)
                => Results.Extensions.ApiResponse(await trainingService.SearchAsync(requestHeaders, query)))
                    .ProducesApiResponse<PageableResult<TrainingBase>>()
                    .WithName("Get all trainings")
                    .WithTags("Trainings");

        _ = endpoints.MapGet("/trainings/{trainingId:int}",
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponse(await trainingService.GetTrainingByIdAsync(requestHeaders, trainingId)))
                .ProducesApiResponse<TrainingBase>()
                .WithName("Get a training by its identifier.")
                .WithTags("Trainings");

        _ = endpoints.MapPost("/trainings",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, TrainingCreationInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingCreationInput input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.CreateTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Create a new training")
                .WithTags("Trainings");

        _ = endpoints.MapPost("/trainings/multiple",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, TrainingCreationInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] List<TrainingCreationInput> input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.CreateRangeTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Create some new training")
                .WithTags("Trainings");

        _ = endpoints.MapPut("/trainings/{trainingId:int}",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingUpdateInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId, [FromBody] TrainingUpdateInput input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.UpdateTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing training")
                .WithTags("Trainings");

        _ = endpoints.MapPut("/trainings/{trainingId:int}/add-students",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingUpdateStudentInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingUpdateStudentInput input, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponseEmpty(await trainingService.AddStudentsInTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing training to add student")
                .WithTags("Trainings");

        _ = endpoints.MapDelete("/trainings/{trainingId:int}",
             //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
             async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponseEmpty(await trainingService.DeleteTrainingAsync(requestHeaders, trainingId)))
                .ProducesApiResponseEmpty()
                .WithName("Delete an existing training")
                .WithTags("Trainings");

        return endpoints;
    }

    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        => services;
}
