namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using NecManager.Common.DataEnum;

/// <summary>
///     Class which represents a create user view model.
/// </summary>
public sealed class CreateUserViewModel
{
    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategorieType Categorie { get; set; }

    /// <summary>
    ///     Gets or sets a group.
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Arme { get; set; }

    /// <summary>
    ///     Gets or sets a progression.
    /// </summary>
    public string Progression { get; set; } = string.Empty;
}
