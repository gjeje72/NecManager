namespace NecManager.Server.Api.Business.Modules.Group.Models;
using System.Collections.Generic;

using NecManager.Server.Api.Business.Modules.Student.Models;

public sealed class GroupOutputDetails : GroupOutputBase
{
    /// <summary>
    ///     Gets or sets a collection of students register to this group.
    /// </summary>
    public List<StudentOutputBase>? Students { get; set; }
}
