namespace NecManager.Web.Service.Models;

using System;
using System.Collections.Generic;

using NecManager.Common.DataEnum;

public sealed class StudentBase
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public DateTime Birthdate { get; set; } = DateTime.MinValue;

    public CategoryType Categorie { get; set; }

    public WeaponType Weapon { get; set; }

    public IEnumerable<int> GroupIds { get; set; } = new List<int>();

    public StudentState State { get; set; }

    public string? GroupName { get; set; }

    public string EmailAddress { get; set; } = string.Empty;

    public bool IsMaster { get; set; }
}
