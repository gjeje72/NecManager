namespace NecManager.Web.Areas.Admin.Settings.Group;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Group.ViewModels;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Groups;

public partial class SettingsGroup
{

    [Inject]
    private IGroupServices GroupServices { get; set; } = null!;

    [Inject]
    private IStudentServices StudentServices { get; set; } = null!;

    private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    private IQueryable<GroupBaseViewModel> Groups { get; set; } = new List<GroupBaseViewModel>().AsQueryable();
    private QuickGrid<GroupBaseViewModel> groupsGrid = new();
    private CreateGroupViewModel ModelForm { get; set; } = new();
    private List<AdminGroupSelectableCategories> Categories = new();

    private List<GroupStudentViewModel> Students = new();
    private List<GroupStudentViewModel> AvailableStudents
        => this.Students.Where(s => !this.ModelForm.Students.Any(st => st.Id == s.Id)).ToList();
    private Dialog? groupsCreationFormDialog;
    private bool IsEditMode;
    private string ValidateButtonLabel
        => this.IsEditMode ? "Mettre à jour" : "CREER";
    private string ErrorMessage = string.Empty;


    /// <summary>
    ///     The dialog selector component used to add tasks on the timesheet.
    /// </summary>
    private DialogSelector<AdminGroupSelectableCategories> categorieSelectorDialog = new();
    private string? StudentFilter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await this.GetGroupsAsync().ConfigureAwait(true);
        this.InitCategories();
    }

    private async Task GetGroupsAsync()
    {
        var (groupSuccess, groups, _) = await this.GroupServices.GetAllGroups();
        if (groupSuccess == ServiceResultState.Success)
            this.Groups = groups?.Select(g => new GroupBaseViewModel()
            {
                Id = g.Id,
                Weapon = g.Weapon,
                StudentCount = g.StudentCount,
                Categories = g.CategoriesIds?.Select(c => (CategoryType)c).ToList() ?? new(),
                Title = g.Title,
            }).AsQueryable() ?? new List<GroupBaseViewModel>().AsQueryable();
    }

    private void InitCategories()
    {
        var arrayOfCat = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().ToArray();
        for (int i = 0; i < arrayOfCat.Count(); i++)
        {
            var value = arrayOfCat[i];
            var newSelectableItem = new AdminGroupSelectableCategories { CategoryId = i, CategoryName = value.ToString() };
            this.Categories.Add(newSelectableItem);
        }
    }

    private void CreateGroupEventHandler()
    {
        this.ResetEditMode();
        if (this.groupsCreationFormDialog is not null)
            this.groupsCreationFormDialog.ShowDialog();
    }

    private async Task UpdateGroupClickEventHandlerAsync(int groupId)
    {
        this.IsEditMode = true;
        var (state, group, _) = await this.GroupServices.GetGroupAsync(groupId);
        if (state == ServiceResultState.Success && group is not null)
        {
            this.ModelForm.Id = group.Id;
            this.ModelForm.Title = group.Title;
            this.ModelForm.Weapon = group.Weapon;
            this.ModelForm.Categories = group.Categories ?? new();
            this.ModelForm.Students = group.Students?.Select(s => new GroupStudentViewModel()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Category = s.Categorie,
                Weapon = s.Weapon
            }).ToList() ?? new();

            this.Categories.ForEach(x => x.IsSelected = false);
            this.Categories.Where(x => group.CategoriesIds?.Contains(x.CategoryId) ?? false).ToList()
                .ForEach(x => x.IsSelected = true);

            if (this.groupsCreationFormDialog is not null)
                this.groupsCreationFormDialog.ShowDialog();
        }
    }

    private async Task OnValidCreateOrUpdateFormAsync()
    {
        if (this.IsEditMode)
        {
            await this.UpdateGroupAsync().ConfigureAwait(true);
            return;
        }

        await this.CreateGroupAsync().ConfigureAwait(true);
    }

    private async Task CreateGroupAsync()
    {
        var categories = this.Categories.Where(x => x.IsSelected == true).Select(x => (CategoryType)x.CategoryId).ToList();
        var groupToCreate = new GroupDetails()
        {
            Title = this.ModelForm.Title,
            Weapon = this.ModelForm.Weapon,
            Categories = categories,
            Students = this.ModelForm.Students.Select(u => new StudentBase { Id = u.Id }).ToList(),
        };

        var (success, groupCreated, errorMessage) = await this.GroupServices.CreateGroupAsync(groupToCreate);

        if (success == ServiceResultState.Success)
        {
            if (this.groupsCreationFormDialog is not null)
                this.groupsCreationFormDialog.CloseDialog();
            await this.GetGroupsAsync().ConfigureAwait(true);
        }
    }

    private async Task UpdateGroupAsync()
    {
        var categories = this.Categories.Where(x => x.IsSelected == true).Select(x => (CategoryType)x.CategoryId).ToList();
        var groupToUpdate = new GroupUpdateInput()
        {
            GroupId = this.ModelForm.Id,
            Title = this.ModelForm.Title,
            Weapon = this.ModelForm.Weapon,
            Categories = categories.Select(c => (int)c).ToList(),
            StudentsIds = this.ModelForm.Students.Select(s =>  s.Id ).ToList(),
        };

        var (success, groupCreated, errorMessage) = await this.GroupServices.UpdateGroupAsync(groupToUpdate);

        if (success == ServiceResultState.Success)
        {
            if (this.groupsCreationFormDialog is not null)
                this.groupsCreationFormDialog.CloseDialog();
            await this.GetGroupsAsync().ConfigureAwait(true);
        }
    }

    private void AddOrRemoveCategories(List<AdminGroupSelectableCategories> categoriesSelected)
    {
        this.ModelForm.Categories = new();
        foreach (var cat in categoriesSelected)
        {
            CategoryType category = (CategoryType)cat.CategoryId;
            this.ModelForm.Categories.Add(category);
        }
    }

    private void ShowCategoriesDialog()
    {
        this.categorieSelectorDialog.ShowDialog();
    }

    private void ResetEditMode()
    {
        this.ModelForm = new();
        this.IsEditMode = false;
    }

    private async Task OnStudentFilterChangeEventHandler(ChangeEventArgs arg)
    {
        this.StudentFilter = arg.Value?.ToString() ?? string.Empty;
        if(!string.IsNullOrWhiteSpace(this.StudentFilter))
            await this.RefreshStudentListAsync().ConfigureAwait(true);
    }

    private async Task RefreshStudentListAsync()
    {
        var (state, students, _) = await this.StudentServices.GetAllStudentsAsync(new() { Filter = this.StudentFilter });
        if (state == ServiceResultState.Success && students is not null)
            this.Students = students.Items?.Select(s => new GroupStudentViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Category = s.Categorie,
                Weapon = s.Weapon
            }).ToList() ?? new();
    }

    private void OnStudentClickEventHandler(string studentId)
    {
        if (this.ModelForm.Students is not null && !this.ModelForm.Students.Any(u => u.Id == studentId))
        {
            var student = this.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
                this.ModelForm.Students.Add(student);
        }
    }

    private void OnStudentDeleteClickEventHandler(string studentId)
    {
        var student = this.ModelForm.Students.FirstOrDefault(s => s.Id == studentId);
        if (student is not null)
            this.ModelForm.Students.Remove(student);
    }

    private async Task OnDeleteGroupClickEventHandler(int groupId)
    {
        var (state, errorMessage) = await this.GroupServices.DeleteGroupAsync(groupId).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
            await this.GetGroupsAsync().ConfigureAwait(true);
        else
            this.ErrorMessage = "Impossible de supprimer un groupe avec des entrainements.";
    }
}
