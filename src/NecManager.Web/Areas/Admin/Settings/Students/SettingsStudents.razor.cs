namespace NecManager.Web.Areas.Admin.Settings.Students;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Group.ViewModels;
using NecManager.Web.Areas.Admin.Settings.Students.ViewModels;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Students;

public partial class SettingsStudents
{
    [Inject]
    public IStudentServices StudentServices { get; set; } = null!;

    [Inject]
    public IGroupServices GroupServices { get; set; } = null!;

    private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    private GridItemsProvider<StudentBaseViewModel> studentProviders = default!;
    private QuickGrid<StudentBaseViewModel> studentsGrid = new();
    private StudentInputQuery StudentInputQuery { get; set; } = new();
    private Dialog? studentsCreationFormDialog;
    private StudentCreationViewModel CreateStudentModel = new();
    private string GroupFilter = string.Empty;
    private List<StudentGroupViewModel> Groups { get; set; } = new();
    private List<StudentGroupViewModel> AvailableGroups
        => this.Groups.Where(
            group => !this.CreateStudentModel.Groups.Any(g => g.Id == group.Id)
            && (group.Title.ToUpperInvariant().Contains(this.GroupFilter.ToUpperInvariant())
            || group.Weapon.ToString().ToUpperInvariant().Contains(this.GroupFilter.ToUpperInvariant()))
            ).ToList();
    private string ValidateButtonLabel = "CREER";
    private string CreationMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        this.studentProviders = async req => await this.GetStudentsProviderAsync(req).ConfigureAwait(true);
        await this.GetGroupListAsync().ConfigureAwait(true);
    }

    private async Task<GridItemsProviderResult<StudentBaseViewModel>> GetStudentsProviderAsync(GridItemsProviderRequest<StudentBaseViewModel> gridItemsProviderRequest)
    {
        var query = new StudentInputQuery
        {
            CurrentPage = (gridItemsProviderRequest.StartIndex / gridItemsProviderRequest.Count ?? 1) + 1,
            PageSize = this.pagination.ItemsPerPage,
            Filter = this.StudentInputQuery.Filter,
            StudentState = this.StudentInputQuery.StudentState,
            WeaponType = this.StudentInputQuery.WeaponType,
        };

        var (_, students, _) = await this.StudentServices.GetAllStudentsAsync(query).ConfigureAwait(true);
        var pageableLessons = new PageableResult<StudentBaseViewModel>
        {
            Items = students?.Items?.Select(s =>
                new StudentBaseViewModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Category = s.Categorie,
                    GroupIds = s.GroupIds,
                    GroupName = s.GroupName,
                    State = s.State,
                    Weapon = s.Weapon,
                }).ToList() ?? new(),
            TotalElements = students?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableLessons.Items.ToList(), pageableLessons.TotalElements);
    }

    private async Task OnValidCreateOrUpdateFormAsync()
    {
        if (this.CreateStudentModel.FirstName.Length > 100 || this.CreateStudentModel.Name.Length > 100)
            return;

        var studentToCreate = new StudentCreationInput
        {
            Name = this.CreateStudentModel.Name,
            FirstName = this.CreateStudentModel.FirstName,
            Category = this.CreateStudentModel.Category,
            EmailAddress = this.CreateStudentModel.EmailAddress,
            PhoneNumber = this.CreateStudentModel.PhoneNumber,
            State = this.CreateStudentModel.State,
            IsMaster = this.CreateStudentModel.IsMaster,
            GroupIds = this.CreateStudentModel.Groups.Select(g => g.Id).ToList(),
        };

        var (state, errorMessage) = await this.StudentServices.CreateStudentAsync(studentToCreate).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
        {
            this.CreationMessage = $"Création réussie pour {this.CreateStudentModel.Name.ToUpperInvariant()} {this.CreateStudentModel.FirstName}";
            await this.studentsGrid.RefreshDataAsync();
        }
        else
        {
            this.CreationMessage = $"Erreur lors de la création : {errorMessage}";
        }
    }

    private void OnCreateNewStudentClickEventHandler()
    {
        this.CreateStudentModel.Name = string.Empty;
        this.CreateStudentModel.FirstName = string.Empty;
        this.CreateStudentModel.EmailAddress = string.Empty;
        this.CreateStudentModel.PhoneNumber = string.Empty;

        if (this.studentsCreationFormDialog is not null)
            this.studentsCreationFormDialog.ShowDialog();
    }

    private async Task OnWeaponFilterChangeEventHandlerAsync(WeaponType? weaponType)
    {
        if (weaponType is null || weaponType == WeaponType.None)
        {
            this.StudentInputQuery.WeaponType = null;
        }
        else
        {
            this.StudentInputQuery.WeaponType = weaponType;
        }

        await this.studentsGrid.RefreshDataAsync();
    }

    private async Task OnStateFilterChangeEventHandlerAsync(StudentState? studentState)
    {
        if (studentState is null || studentState == StudentState.None)
        {
            this.StudentInputQuery.StudentState = null;
        }
        else
        {
            this.StudentInputQuery.StudentState = studentState;
        }

        await this.studentsGrid.RefreshDataAsync();
    }

    private async Task OnFilterChangeEventHandlerAsync(string? filter)
    {
        this.StudentInputQuery.Filter = filter;
        await this.studentsGrid.RefreshDataAsync();
    }

    private async Task OnGroupFilterChangeEventHandler(ChangeEventArgs arg)
        => this.GroupFilter = arg.Value?.ToString() ?? string.Empty;

    private async Task GetGroupListAsync()
    {
        var (state, groups, _) = await this.GroupServices.GetAllGroups();
        if (state == ServiceResultState.Success && groups is not null)
            this.Groups = groups.Select(s => new StudentGroupViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Weapon = s.Weapon
            }).ToList() ?? new();
    }

    private void OnGroupClickEventHandler(int groupId)
    {
        if (this.CreateStudentModel.Groups is not null && !this.CreateStudentModel.Groups.Any(u => u.Id == groupId))
        {
            var group = this.Groups.FirstOrDefault(s => s.Id == groupId);
            if (group != null)
                this.CreateStudentModel.Groups.Add(group);
        }
    }

    private void OnGroupDeleteClickEventHandler(int groupId)
    {
        var group = this.CreateStudentModel.Groups.FirstOrDefault(s => s.Id == groupId);
        if (group is not null)
            this.CreateStudentModel.Groups.Remove(group);
    }

    private string GetCreationStateCss()
        => this.CreationMessage.StartsWith("Erreur") ? "error" : "success";
}
