namespace NecManager.Server.Api.Business.Modules.Group;
using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Group.Models;

public interface IGroupBusinessModule
{
    /// <summary>
    ///     Method used to create a group and update selected student.
    /// </summary>
    /// <param name="monitoringIds">The monitoring Ids.</param>
    /// <param name="groupInput">The group to manage.</param>
    /// <returns>A <see cref="GroupOutputBase"/>.</returns>
    Task<ApiResponse<GroupOutputBase>> CreateGroupAsync(ServiceMonitoringDefinition monitoringIds, GroupInput groupInput);
}
