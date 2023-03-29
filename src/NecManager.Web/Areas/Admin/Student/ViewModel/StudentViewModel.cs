namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using NecManager.Common.DataEnum;

public class StudentViewModel
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategoryType Categorie { get; set; }

    public WeaponType Arme { get; set; }
}
