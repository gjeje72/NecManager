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
using NecManager.Web.Service.Models;

public sealed partial class AdminGroup : ComponentBase
{
    private bool IsLoading;
    private bool IsEditMode;
    private bool creatingInProgress;
    private CreateGroupViewModel ModelForm { get; set; } = new();

    private List<GroupViewModel> Groups = new();

    private List<AdminGroupSelectableCategories> Categories = new();

    private List<AdminGroupSelectableStudent> Students = new();

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

    [Inject]
    private IGroupServices GroupServices { get; set; } = null!;


    protected override async Task OnInitializedAsync()
    {
        this.IsLoading = true;

        await this.GetGroupsAsync();

        this.InitCategories();

        await this.GetStudentsAsync();

        this.IsLoading = false;
        await this.InvokeAsync(this.StateHasChanged);
    }

    private async Task GetGroupsAsync()
    {
        var (groupSuccess, groups, _) = await this.GroupServices.GetAllGroups();
        if (groupSuccess == ServiceResultState.Success)
            this.Groups = this.Mapper.Map<List<GroupViewModel>>(groups);
    }

    private async Task GetStudentsAsync()
    {
        var (studentSuccess, studentsFound, _) = await this.StudentServices.GetAllStudentsAsync();
        if (studentSuccess == ServiceResultState.Success)
        {
            this.Students = this.Mapper.Map<List<AdminGroupSelectableStudent>>(studentsFound);
        }
    }

    private void InitCategories()
    {
        var arrayOfCat = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().ToArray();
        for (int i = 0; i < arrayOfCat.Count(); i++)
        {
            var value = arrayOfCat[i];
            var newSelectableItem = new AdminGroupSelectableCategories { CategoryId = i + 1, CategoryName = value.ToString() };
            this.Categories.Add(newSelectableItem);
        }
    }


    private void AddOrRemoveCategories(List<AdminGroupSelectableCategories> categoriesSelected)
    {
        this.ModelForm.Categories = new();
        foreach(var cat in categoriesSelected)
        {
            CategoryType category = (CategoryType)cat.CategoryId;
            this.ModelForm.Categories.Add(category);
        }
    }

    private void AddOrRemoveStudents(List<AdminGroupSelectableStudent> studentSelected)
    {
        this.ModelForm.UsersIds = new();

        foreach(var student in studentSelected)
        {
            this.ModelForm.UsersIds.Add(student.StudentId);
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
        if (this.IsEditMode)
            return;

        var categories = this.Categories.Where(x => x.IsSelected == true).Select(x => (CategoryType)x.CategoryId).ToList();
        var students = new List<StudentBase>(this.Students.Where(x => x.IsSelected == true).Select(x => new StudentBase() { Id = x.StudentId }));
        var groupToCreate = new GroupDetails()
        {
            Title = this.ModelForm.Title,
            Weapon = this.ModelForm.Weapon,
            Categories = categories,
            Students = students,
        };

        var (success, groupCreated, errorMessage) = await this.GroupServices.CreateGroupAsync(groupToCreate);

        if (success == ServiceResultState.Success)
        {
            var groupCreatedVM = this.Mapper.Map<GroupViewModel>(groupCreated);
            this.Groups.Add(groupCreatedVM);
            this.ModelForm = new();
        }

        await this.InvokeAsync(this.StateHasChanged);
    }

    private void ResetEditMode()
    {
        this.ModelForm = new();
        this.IsEditMode = false;
    }

    private async Task EditGroupAsync(GroupViewModel groupToUpdate)
    {
        this.IsEditMode = true;
        this.creatingInProgress = true;
        this.ModelForm = this.Mapper.Map<CreateGroupViewModel>(groupToUpdate);
        this.Categories.ForEach(x => x.IsSelected = false);
        this.Categories.Where(x => groupToUpdate.Categories.Contains((CategoryType)x.CategoryId)).ToList()
            .ForEach(x => x.IsSelected = true);


        var (state, groupFound, errorMessage) = await this.GroupServices.GetGroupAsync(groupToUpdate.Id).ConfigureAwait(true);
        if (state == ServiceResultState.Success && groupFound is not null)
        {
            this.Students.ForEach(x => x.IsSelected = false);
            if (groupFound.Students?.Any() is not null)
            this.Students.Where(student => groupFound.Students.Select(x => x.Id).ToList().Contains(student.StudentId)).ToList()
                .ForEach(s => s.IsSelected = true);
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}
