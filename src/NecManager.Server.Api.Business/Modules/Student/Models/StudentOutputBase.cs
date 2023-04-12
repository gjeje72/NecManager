namespace NecManager.Server.Api.Business.Modules.Student.Models;

using NecManager.Common.DataEnum;

public sealed class StudentOutputBase
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategoryType Categorie { get; set; }

    public WeaponType Weapon { get; set; }

    public IEnumerable<int> GroupIds { get; set; } = new List<int>();

    public StudentState State { get; set; }

    public string? GroupName { get; set; }
}
