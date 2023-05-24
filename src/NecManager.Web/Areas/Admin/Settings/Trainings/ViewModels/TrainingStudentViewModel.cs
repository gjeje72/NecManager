namespace NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;

using NecManager.Common.DataEnum;

public class TrainingStudentViewModel
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategoryType Category { get; set; }
}
