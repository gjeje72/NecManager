namespace NecManager.Server.Api.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Common.Security;
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
        endpoints.MapGet("/groups",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders headers, [FromServices] IGroupBusiness groupService)
            => Results.Extensions.ApiResponse(await groupService.GetAllGroupsAsync(headers)))
                .WithName("Get all groups")
                .WithTags("Groups");

        endpoints.MapGet("/groups/{groupId}",
           [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders headers, [FromRoute] int groupId, [FromServices] IGroupBusiness groupService)
            => Results.Extensions.ApiResponse(await groupService.GetGroupByIdAsync(headers, groupId)))
                .WithName("Get a group by id.")
                .WithTags("Groups");

        endpoints.MapPost("/groups",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders headers, [FromBody] CreateGroupInput input, [FromServices] IGroupBusiness groupService)
            => Results.Extensions.ApiResponse(await groupService.CreateGroupAsync(headers, input)))
                .WithName("Create a group")
                .WithTags("Groups");

        endpoints.MapPut("/groups/{groupId:int}",
            [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders headers, [FromRoute] int groupId, [FromBody] GroupUpdateInput input, [FromServices] IGroupBusiness groupService)
            => Results.Extensions.ApiResponseEmpty(await groupService.UpdateGroupAsync(headers, input)))
                .WithName("Update a group")
                .WithTags("Groups");

        endpoints.MapDelete("/groups/{groupId:int}",
           [Authorize(Policy = PolicyName.IsAdmin)] async (ApiRequestHeaders headers, [FromRoute] int groupId, [FromServices] IGroupBusiness groupService)
            => Results.Extensions.ApiResponseEmpty(await groupService.DeleteGroupAsync(headers, groupId)))
                .WithName("Delete a group by id.")
                .WithTags("Groups");

        return endpoints;
    }
}
