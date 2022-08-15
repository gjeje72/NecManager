namespace NecManager.Web.Service.ApiServices;

using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models;
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
}
