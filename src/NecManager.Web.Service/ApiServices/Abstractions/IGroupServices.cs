namespace NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Common;
using NecManager.Web.Service.Models;
using System.Threading.Tasks;

public interface IGroupServices
{
    /// <summary>
    ///     Method used to call api in way to create a group.
    /// </summary>
    /// <param name="groupInput">The group to manage.</param>
    /// <returns>Returns a <see cref="GroupBase"/>.</returns>
    Task<ServiceResult<GroupBase>> CreateGroupAsync(GroupDetails groupInput);
}
