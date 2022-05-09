namespace NecManager_DAL.Model;

using DataEnum;

/// <summary>
///     Class which represents a student.
/// </summary>
public class Student : ADataObject
{
    /// <summary>
    ///     Gets or sets the name for this student.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the firstname for this student.
    /// </summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the category for this student.
    /// </summary>
    public Categorie Cat { get; set; }

    /// <summary>
    ///     Gets or sets the groupeId for this student. TODO : navigation prop
    /// </summary>
    public int GroupeId { get; set; }

    /// <summary>
    ///     Gets or sets the weapon for this student.
    /// </summary>
    public Weapon Weapon { get; set; }

    /// <summary>
    ///     Gets or sets the progress for this student. TODO
    /// </summary>
    public string Progress { get; set; } = string.Empty;
}
