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
        endpoints.MapPost("/groups", async (ApiRequestHeaders headers, [FromBody] GroupInput input, [FromServices] IGroupBusinessModule groupService)
            => Results.Extensions.ApiResponse(await groupService.CreateGroupAsync(headers, input)))
                .WithName("Create a group")
                .WithTags("Groups");

        return endpoints;
    }
}
