namespace NecManager.Web.Service.Models;

using NecManager.Common.DataEnum;

public sealed class TrainingStudentBase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a categorie.
    /// </summary>
    public CategoryType Category { get; set; }
}
