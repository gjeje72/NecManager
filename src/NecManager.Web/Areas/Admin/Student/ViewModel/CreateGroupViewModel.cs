namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using System.Collections.Generic;

using NecManager.Common.DataEnum;

public sealed class CreateGroupViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<CategorieType> Categories { get; set; } = new();

    public WeaponType Weapon { get; set; }

    public List<CreateUserViewModel> Users { get; set; } = new();

    /* TODO */
    public string? Lessons { get; set; } 
}
