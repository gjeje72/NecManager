namespace NecManager.Web.Areas.Admin.Student;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NecManager.Common.DataEnum;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Components.Modal;

public sealed partial class AdminGroup
{
    private bool creatingInProgress;
    private CreateGroupViewModel CreateGroupModel { get; set; } = new();

    private List<CreateGroupViewModel> Groups = new();

    private List<AdminGroupSelectableCategories> Categories = new();

    /// <summary>
    ///     The dialog selector component used to add tasks on the timesheet.
    /// </summary>
    private DialogSelector<AdminGroupSelectableCategories> categorieSelectorDialog = new();

    protected override void OnInitialized()
    {
        var arrayOfCat = Enum.GetValues(typeof(CategorieType)).Cast<CategorieType>().ToArray();
        for (int i = 0; i < arrayOfCat.Count(); i++)
        {
            var value = arrayOfCat[i];
            var newSelectableItem = new AdminGroupSelectableCategories { CategoryId = i + 1, CategoryName = value.ToString() };
            this.Categories.Add(newSelectableItem);
        }

        base.OnInitialized();
    }

    private void AddOrRemoveCategories(List<AdminGroupSelectableCategories> categoriesSelected)
    {
        foreach(var cat in categoriesSelected)
        {
            CategorieType category = (CategorieType)cat.CategoryId;
            this.CreateGroupModel.Categories.Add(category);
        }
    }

    private void ShowCategoriesDialog()
    {
        this.categorieSelectorDialog.ShowDialog();
    }

    private void OpenCreatingForm()
    {
        this.creatingInProgress = !this.creatingInProgress;
    }
    private async Task HandleSubmitAsync()
    {
        this.Groups.Add(this.CreateGroupModel);
        this.CreateGroupModel = new();
        await this.InvokeAsync(() => this.StateHasChanged());
    }
}
