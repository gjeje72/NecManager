namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Server.Api.Business.Modules.Group;
using NecManager.Server.Api.Business.Modules.Group.Models;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;

public sealed class GroupModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/groups", async (ApiRequestHeaders headers, [FromBody] CreateGroupInput input, [FromServices] IGroupBusinessModule groupService)
            => Results.Extensions.ApiResponse(await groupService.CreateGroupAsync(headers, input)))
                .WithName("Create a group")
                .WithTags("Groups");

        endpoints.MapGet("/groups", (ApiRequestHeaders headers, [FromServices] IGroupBusinessModule groupService)
            => Results.Extensions.ApiResponse(groupService.GetAllGroups(headers)))
                .WithName("Get all groups")
                .WithTags("Groups");

        endpoints.MapGet("/groups/{groupId}", async (ApiRequestHeaders headers, [FromRoute] int groupId, [FromServices] IGroupBusinessModule groupService)
            => Results.Extensions.ApiResponse(await groupService.GetGroupByIdAsync(headers, groupId)))
                .WithName("Get a group by id.")
                .WithTags("Groups");

        return endpoints;
    }
}
