namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Lesson;
using NecManager.Server.Api.Business.Modules.Lesson.Models;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;

public sealed class LessonModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapGet("/lessons",
             async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, /*BindAsync*/ LessonQueryInput query)
            //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, /*BindAsync*/ LessonQueryInput query)
                => Results.Extensions.ApiResponse(await lessonService.SearchAsync(requestHeaders, query)))
                    .ProducesApiResponse<PageableResult<LessonBase>>()
                    .WithName("Get all lessons")
                    .WithTags("Lessons");

        _ = endpoints.MapGet("/lessons/{lessonId:int}",
            //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId)
             async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId)
                => Results.Extensions.ApiResponse(await lessonService.GetLessonByIdAsync(requestHeaders, lessonId)))
                    .ProducesApiResponse<LessonBase>()
                    .WithName("Get lesson from its Id")
                    .WithTags("Lessons");

        _ = endpoints.MapPost("/lessons",
            //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, LessonCreationInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, LessonCreationInput input)
                => Results.Extensions.ApiResponseEmpty(await lessonService.CreateLessonAsync(requestHeaders, input)))
                .ProducesApiResponseEmpty()
                .WithName("Create a new lesson")
                .WithTags("Lessons");

        _ = endpoints.MapDelete("/lessons/{lessonId:int}",
            //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId)
             async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId)
                => Results.Extensions.ApiResponseEmpty(await lessonService.DeleteLessonAsync(requestHeaders, lessonId)))
                .ProducesApiResponseEmpty()
                .WithName("Delete an existing lesson")
                .WithTags("Lessons");

        _ = endpoints.MapPut("/lessons/{lessonId:int}",
            //[Authorize] async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId, LessonUpdateInput input)
             async (ApiRequestHeaders requestHeaders, [FromServices] ILessonBusiness lessonService, [FromRoute] int lessonId, LessonUpdateInput input)
                => Results.Extensions.ApiResponseEmpty(await lessonService.UpdateLessonAsync(requestHeaders, lessonId, input)))
                .ProducesApiResponseEmpty()
                .WithName("Update an existing lesson")
                .WithTags("Lessons");

        return endpoints;
    }

    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
