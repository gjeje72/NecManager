namespace NecManager.Server.DataAccessLayer.Model;
using System.ComponentModel.DataAnnotations;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class Student : ADataObject
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
    ///     Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the email address.
    /// </summary>
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategoryType Category { get; set; }

    /// <summary>
    ///     Gets or sets a state.
    /// </summary>
    public StudentState State { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether a student is a master.
    /// </summary>
    public bool IsMaster { get; set; }

    /// <summary>
    ///     Gets or sets a group id.
    /// </summary>
    public ICollection<StudentGroup> StudentGroups { get; set; } = new HashSet<StudentGroup>();
}
