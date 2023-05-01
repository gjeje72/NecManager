namespace NecManager.Web.Service.Models.Groups;
using NecManager.Common.DataEnum;

using System.Collections.Generic;

/// <summary>
///     Class which represents the information details for a group.
///     Used when group creating.
/// </summary>
public sealed class GroupDetails : GroupBase
{
    /// <summary>
    ///     Gets or sets a collection of students.
    /// </summary>
    public List<StudentBase>? Students { get; set; }
}
