﻿namespace NecManager.Web.Service.ApiServices;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models.Groups;
using NecManager.Web.Service.Provider;

internal sealed class GroupServices : ServiceBase, IGroupServices
{
    public GroupServices(RestHttpService restHttpService)
        : base(restHttpService, "GROUP_S001")
    {
    }

    /// <inheritdoc />
    public async Task<ServiceResult<GroupBase>> CreateGroupAsync(GroupDetails groupInput)
    {
        var groupClient = await this.RestHttpService.GroupClient;
        var response = await groupClient.PostAsync("", groupInput.ToStringContent()).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<GroupBase>().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ServiceResult<List<GroupBase>>> GetAllGroups()
    {
        var groupClient = await this.RestHttpService.GroupClient;
        var response = await groupClient.GetAsync("").ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<List<GroupBase>>().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ServiceResult<GroupDetails>> GetGroupAsync(int groupId)
    {
        var groupClient = await this.RestHttpService.GroupClient;
        var response = await groupClient.GetAsync($"{groupId}").ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<GroupDetails>().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> UpdateGroupAsync(GroupUpdateInput groupUpdateInput, CancellationToken cancellationToken = default)
    {
        var groupClient = await this.RestHttpService.GroupClient;
        var response = await groupClient.PutAsync($"{groupUpdateInput.GroupId}", groupUpdateInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> DeleteGroupAsync(int groupId)
    {
        var groupClient = await this.RestHttpService.GroupClient;
        var response = await groupClient.DeleteAsync($"{groupId}").ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }
}
