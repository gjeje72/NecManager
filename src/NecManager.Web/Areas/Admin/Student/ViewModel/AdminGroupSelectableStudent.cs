namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using NecManager.Web.Components.Modal;

public class AdminGroupSelectableStudent : ISelectableItem
{
    public string StudentId { get; set; } = string.Empty;

    public string StudentFullName { get; set; } = string.Empty;

    public bool IsSelected { get; set; }
}
