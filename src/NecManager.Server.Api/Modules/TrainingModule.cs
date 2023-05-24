namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common;
using NecManager.Common.Security;
using NecManager.Server.Api.Business.Modules.Training;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.Api.Business.Modules.Training.Models.History;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;
using NecManager.Server.DataAccessLayer.Model;

public sealed class TrainingModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapGet("/trainings",
             [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, /*BindAsync*/ TrainingQueryInput query)
                => Results.Extensions.ApiResponse(await trainingService.SearchAsync(requestHeaders, query)))
                    .ProducesApiResponse<PageableResult<TrainingBase>>()
                    .WithName("Get all trainings")
                    .WithTags("Trainings");

        _ = endpoints.MapGet("/trainings/{trainingId:int}",
             [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponse(await trainingService.GetTrainingByIdAsync(requestHeaders, trainingId)))
                .ProducesApiResponse<TrainingDetails>()
                .WithName("Get a training by its identifier.")
                .WithTags("Trainings");

        _ = endpoints.MapGet("/trainings/history",
           [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, /*BindAsync*/ TrainingHistoryQuery input)
                => Results.Extensions.ApiResponse(await trainingService.GetTrainingHistoryAsync(requestHeaders, input.Id, input.StudentId)))
                .ProducesApiResponse<TrainingsHistory>()
                .WithName("Get trainings history.")
                .WithTags("Trainings");

        _ = endpoints.MapPost("/trainings",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingCreationInput input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.CreateTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Create a new training")
                .WithTags("Trainings");

        _ = endpoints.MapPost("/trainings/multiple",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] List<TrainingCreationInput> input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.CreateRangeTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Create some new training")
                .WithTags("Trainings");

        _ = endpoints.MapPut("/trainings/{trainingId:int}",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId, [FromBody] TrainingUpdateInput input)
                => Results.Extensions.ApiResponseEmpty(await trainingService.UpdateTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing training")
                .WithTags("Trainings");

        _ = endpoints.MapPut("/trainings/{trainingId:int}/add-students",
             [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromBody] TrainingUpdateStudentInput input, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponseEmpty(await trainingService.AddStudentsInTrainingAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing training to add student")
                .WithTags("Trainings");

        _ = endpoints.MapDelete("/trainings/{trainingId:int}",
             [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders requestHeaders, [FromServices] ITrainingBusiness trainingService, [FromRoute] int trainingId)
                => Results.Extensions.ApiResponseEmpty(await trainingService.DeleteTrainingAsync(requestHeaders, trainingId)))
                .ProducesApiResponseEmpty()
                .WithName("Delete an existing training")
                .WithTags("Trainings");

        return endpoints;
    }

    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        => services;
}
