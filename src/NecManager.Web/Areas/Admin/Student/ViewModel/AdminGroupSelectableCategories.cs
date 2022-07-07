namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using NecManager.Web.Components.Modal;

public class AdminGroupSelectableCategories : ISelectableItem
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public bool IsSelected { get; set; }
}
