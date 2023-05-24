namespace NecManager.Server.DataAccessLayer.Model;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using NecManager.Common.DataEnum;

public sealed class Student : IdentityUser
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
    ///     Gets or sets the email address.
    /// </summary>
    [EmailAddress]
    [MaxLength(200)]
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a birthdate.
    /// </summary>
    public DateTime BirthDate { get; set; } = DateTime.MinValue;

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
