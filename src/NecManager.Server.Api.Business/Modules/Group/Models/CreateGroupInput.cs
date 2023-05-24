namespace NecManager.Server.Api.Business.Modules.Group.Models;
using System.Collections.Generic;

using NecManager.Common.DataEnum;
using NecManager.Server.Api.Business.Modules.Student.Models;

public sealed class CreateGroupInput
{
    /// <summary>
    ///     Gets or sets the title of the group.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a collection of categories.
    /// </summary>
    public List<int>? Categories { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Weapon { get; set; }

    /// <summary>
    ///     Gets or sets a collection of students.
    /// </summary>
    public List<StudentOutputBase> Students { get; set; } = new();

    public string MasterId { get; set; } = string.Empty;
}
