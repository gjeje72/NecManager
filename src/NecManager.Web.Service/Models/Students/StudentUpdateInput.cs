namespace NecManager.Web.Service.Models.Students;

using System;
using System.Collections.Generic;

using NecManager.Common.DataEnum;

public sealed class StudentUpdateInput
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the email address.
    /// </summary>
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a Birthdate.
    /// </summary>
    public DateTime Birthdate { get; set; }

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
