namespace NecManager.Web.Areas.Admin.Settings.Students.ViewModels;

using NecManager.Common.DataEnum;
using System.Collections.Generic;

public sealed class StudentBaseViewModel
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategoryType Category { get; set; }

    public WeaponType Weapon { get; set; }

    public IEnumerable<int> GroupIds { get; set; } = new List<int>();

    public StudentState State { get; set; }

    public string? GroupName { get; set; }
}
