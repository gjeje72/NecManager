namespace NecManager.Web.Areas.Admin.Settings.Students;

using System.Collections.Generic;
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
    private List<StudentGroupViewModel> AvailableGroups { get; set; } = new();
    private string ValidateButtonLabel = "CREER";

    protected override Task OnInitializedAsync()
    {
        this.studentProviders = async req => await this.GetStudentsProviderAsync(req).ConfigureAwait(true);

        return base.OnInitializedAsync();
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

    private void OnValidCreateOrUpdateFormAsync()
    {

    }

    private void OnCreateNewStudentClickEventHandler()
    {
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
    {
        this.GroupFilter = arg.Value?.ToString() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(this.GroupFilter))
            await this.RefreshGroupListAsync().ConfigureAwait(true);
    }

    private async Task RefreshGroupListAsync()
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
}
