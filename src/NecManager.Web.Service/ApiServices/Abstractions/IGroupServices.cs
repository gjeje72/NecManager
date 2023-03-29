namespace NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Common;
using NecManager.Web.Service.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGroupServices
{
    /// <summary>
    ///     Method used to call api in way to create a group.
    /// </summary>
    /// <param name="groupInput">The group to manage.</param>
    /// <returns>Returns a <see cref="GroupBase"/>.</returns>
    Task<ServiceResult<GroupBase>> CreateGroupAsync(GroupDetails groupInput);

    /// <summary>
    ///     Method used to call api in way to retrieve all groups.
    /// </summary>
    /// <returns>Return a collection of <see cref="GroupBase"/>.</returns>
    Task<ServiceResult<List<GroupBase>>> GetAllGroups();

    /// <summary>
    ///     Method used to call api in way to retrieve a group.
    /// </summary>
    /// <param name="groupId">The id from group to retrieve.</param>
    /// <returns>Return a <see cref="GroupDetails"/>.</returns>
    Task<ServiceResult<GroupDetails>> GetGroupAsync(int groupId);
}
