namespace NecManager.Web.Service.Models.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NecManager.Common.DataEnum;

public sealed class StudentCreationInput
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
    ///     Gets or sets a student state.
    /// </summary>
    public StudentState State { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether a student is a master.
    /// </summary>
    public bool IsMaster { get; set; }

    public List<int> GroupIds { get; set; } = new();
}
