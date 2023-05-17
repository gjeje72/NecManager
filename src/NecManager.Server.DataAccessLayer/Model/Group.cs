namespace NecManager.Server.DataAccessLayer.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public class Group : ADataObject
{
    /// <summary>
    ///     Gets or sets a title.
    /// </summary>
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a collection of categories.
    /// </summary>
    [MaxLength(200)]
    public string? Categories { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Weapon { get; set; }

    /// <summary>
    ///     Gets or sets a collection of students.
    /// </summary>
    public ICollection<StudentGroup> StudentGroups { get; set; } = new HashSet<StudentGroup>();

    public ICollection<Training> Trainings { get; set; } = new HashSet<Training>();
}
