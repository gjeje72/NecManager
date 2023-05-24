namespace NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;

using NecManager.Common.DataEnum;

public class TrainingStudentBaseViewModel
{
    public string Id { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategoryType Categorie { get; set; }

    public WeaponType Weapon { get; set; }

}
