namespace NecManager.Server.Api.Business.Modules.Group;
using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Group.Models;

public interface IGroupBusinessModule
{
    /// <summary>
    ///     Method used to retrieve all groups.
    /// </summary>
    /// <param name="monitoringIds">The monitoring Ids.</param>
    /// <returns>Returns a collection of <see cref="GroupOutputBase"/>.</returns>
    ApiResponse<List<GroupOutputBase>> GetAllGroups(ServiceMonitoringDefinition monitoringIds);

    /// <summary>
    ///     Method used to retrive a group by its id.
    /// </summary>
    /// <param name="monitoringIds">The monitoring Ids.</param>
    /// <param name="groupId">The id from group to retrieve.</param>
    /// <returns>Return a <see cref="GroupOutputBase"/>.</returns>
    Task<ApiResponse<GroupOutputDetails>> GetGroupByIdAsync(ServiceMonitoringDefinition monitoringIds, int groupId);

    /// <summary>
    ///     Method used to create a group and update selected student.
    /// </summary>
    /// <param name="monitoringIds">The monitoring Ids.</param>
    /// <param name="groupInput">The group to manage.</param>
    /// <returns>A <see cref="GroupOutputBase"/>.</returns>
    Task<ApiResponse<GroupOutputBase>> CreateGroupAsync(ServiceMonitoringDefinition monitoringIds, CreateGroupInput groupInput);
}
