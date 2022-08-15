namespace NecManager.Web.Areas.Admin.Student;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;

public sealed partial class AdminGroup
{
    private bool IsLoading;
    private bool creatingInProgress;
    private CreateGroupViewModel CreateGroupModel { get; set; } = new();

    private List<CreateGroupViewModel> Groups = new();

    private List<AdminGroupSelectableCategories> Categories = new();

    private List<AdminGroupSelectableStudent> Students = new();
    //{
    //    new AdminGroupSelectableStudent { StudentId = 2, StudentFullName = "David BUCQUET" },
    //    new AdminGroupSelectableStudent { StudentId = 3, StudentFullName = "Daniele GAROZZO" },
    //    new AdminGroupSelectableStudent { StudentId = 4, StudentFullName = "Alice VOLPI" },
    //};

    /// <summary>
    ///     The dialog selector component used to add tasks on the timesheet.
    /// </summary>
    private DialogSelector<AdminGroupSelectableCategories> categorieSelectorDialog = new();

    /// <summary>
    ///     The dialog selector component used to add tasks on the timesheet.
    /// </summary>
    private DialogSelector<AdminGroupSelectableStudent> studentSelectorDialog = new();

    [Inject]
    private IMapper Mapper { get; set; } = null!;

    [Inject]
    private IStudentServices StudentServices { get; set; } = null!;


    protected override async Task OnInitializedAsync()
    {
        this.IsLoading = true;
        var arrayOfCat = Enum.GetValues(typeof(CategorieType)).Cast<CategorieType>().ToArray();
        for (int i = 0; i < arrayOfCat.Count(); i++)
        {
            var value = arrayOfCat[i];
            var newSelectableItem = new AdminGroupSelectableCategories { CategoryId = i + 1, CategoryName = value.ToString() };
            this.Categories.Add(newSelectableItem);
        }

        var (success, studentsFound, _) = await this.StudentServices.GetAllStudentsAsync();
        if (success == ServiceResultState.Success)
        {
            this.Students = this.Mapper.Map<List<AdminGroupSelectableStudent>>(studentsFound);
        }

        this.IsLoading = false;
        await this.InvokeAsync(this.StateHasChanged);
    }

    private void AddOrRemoveCategories(List<AdminGroupSelectableCategories> categoriesSelected)
    {
        foreach(var cat in categoriesSelected)
        {
            CategorieType category = (CategorieType)cat.CategoryId;
            this.CreateGroupModel.Categories.Add(category);
        }
    }


    private void AddOrRemoveStudents(List<AdminGroupSelectableStudent> studentSelected)
    {
        foreach(var student in studentSelected)
        {
            this.CreateGroupModel.UsersIds.Add(student.StudentId);
        }
    }

    private void ShowCategoriesDialog()
    {
        this.categorieSelectorDialog.ShowDialog();
    }

    private void ShowStudentDialog()
    {
        this.studentSelectorDialog.ShowDialog();
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
