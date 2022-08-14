namespace NecManager.Server.DataAccessLayer.Model;
using System.ComponentModel.DataAnnotations;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public class Student : ADataObject
{
    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the first name.
    /// </summary>
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategorieType Categorie { get; set; }

    /// <summary>
    ///     Gets or sets a group id.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    ///     Getse or sets a group.
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Arme { get; set; }
}
