namespace NecManager.Web.Areas.Admin.CoursCollectifs.ViewModels;

using NecManager.Common.DataEnum;

public class TrainingStudentViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategoryType Category { get; set; }
}
