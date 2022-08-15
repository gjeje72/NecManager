namespace NecManager.Web.Service.Models;

using NecManager.Common.DataEnum;

public sealed class StudentBase
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategorieType Categorie { get; set; }

    public WeaponType Arme { get; set; }
}
