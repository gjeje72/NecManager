namespace NecManager.Web.Areas.Admin.Settings.Students;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
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
    private Dialog? confirmDeleteDialog;
    private StudentCreationViewModel CreateStudentModel = new();
    private string GroupFilter = string.Empty;
    private List<StudentGroupViewModel> Groups { get; set; } = new();
    private List<StudentGroupViewModel> AvailableGroups
        => this.Groups.Where(
            group => !this.CreateStudentModel.Groups.Any(g => g.Id == group.Id)
            && (group.Title.ToUpperInvariant().Contains(this.GroupFilter.ToUpperInvariant())
            || group.Weapon.ToString().ToUpperInvariant().Contains(this.GroupFilter.ToUpperInvariant()))
            ).ToList();
    private StudentBaseViewModel underDeletionStudent = new();
    private bool isEditMode = false;
    private string ValidateButtonLabel => this.isEditMode ? "Mettre à jour" : "CREER";
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
                    Birthdate = s.Birthdate,
                    GroupIds = s.GroupIds,
                    GroupName = s.GroupName,
                    State = s.State,
                    Weapon = s.Weapon,
                    IsMaster = s.IsMaster,
                    EmailAddress = s.EmailAddress
                }).ToList() ?? new(),
            TotalElements = students?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableLessons.Items.ToList(), pageableLessons.TotalElements);
    }

    private async Task OnValidCreateOrUpdateFormAsync()
    {
        if (this.isEditMode)
        {
            await this.UpdateStudentAsync().ConfigureAwait(true);
            return;
        }

        await this.CreateStudentAsync().ConfigureAwait(true);
    }

    private async Task CreateStudentAsync()
    {
        if (this.CreateStudentModel.FirstName.Length > 100 || this.CreateStudentModel.Name.Length > 100)
            return;

        var studentToCreate = new StudentCreationInput
        {
            Name = this.CreateStudentModel.Name,
            FirstName = this.CreateStudentModel.FirstName,
            Birthdate = this.CreateStudentModel.Birthdate,
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

    private async Task UpdateStudentAsync()
    {
        if (this.CreateStudentModel.FirstName.Length > 100 || this.CreateStudentModel.Name.Length > 100)
            return;

        var studentToUpdate = new StudentUpdateInput
        {
            Id = this.CreateStudentModel.Id,
            Name = this.CreateStudentModel.Name,
            FirstName = this.CreateStudentModel.FirstName,
            Birthdate = this.CreateStudentModel.Birthdate,
            EmailAddress = this.CreateStudentModel.EmailAddress,
            PhoneNumber = this.CreateStudentModel.PhoneNumber,
            State = this.CreateStudentModel.State,
            IsMaster = this.CreateStudentModel.IsMaster,
            GroupIds = this.CreateStudentModel.Groups.Select(g => g.Id).ToList(),
        };

        var (state, errorMessage) = await this.StudentServices.UpdateStudentAsync(studentToUpdate).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
        {
            this.CreationMessage = $"Mise à jour réussie pour {this.CreateStudentModel.Name.ToUpperInvariant()} {this.CreateStudentModel.FirstName}";
            await this.studentsGrid.RefreshDataAsync();

            if(this.studentsCreationFormDialog is not null)
                this.studentsCreationFormDialog.CloseDialog();
        }
        else
        {
            this.CreationMessage = $"Erreur lors de la mise à jour : {errorMessage}";
        }
    }

    private void OnCreateNewStudentClickEventHandler()
    {
        this.isEditMode = false;
        this.CreateStudentModel.Name = string.Empty;
        this.CreateStudentModel.FirstName = string.Empty;
        this.CreateStudentModel.EmailAddress = string.Empty;
        this.CreateStudentModel.PhoneNumber = string.Empty;

        if (this.studentsCreationFormDialog is not null)
            this.studentsCreationFormDialog.ShowDialog();
    }

    private void OnUpdateStudentClickEventHandler(StudentBaseViewModel student)
    {
        this.isEditMode = true;

        this.CreateStudentModel.Id = student.Id;
        this.CreateStudentModel.Name = student.LastName;
        this.CreateStudentModel.FirstName = student.FirstName;
        this.CreateStudentModel.EmailAddress = student.EmailAddress;
        this.CreateStudentModel.IsMaster = student.IsMaster;
        this.CreateStudentModel.Birthdate = student.Birthdate;
        this.CreateStudentModel.State = student.State;
        this.CreateStudentModel.Groups = this.Groups.Where(g => student.GroupIds.Contains(g.Id)).ToList();

        if (this.studentsCreationFormDialog is not null)
            this.studentsCreationFormDialog.ShowDialog();
    }
    private void OnDeleteStudentClickEventHandler(StudentBaseViewModel student)
    {
        this.underDeletionStudent = student;

        if(this.confirmDeleteDialog is not null)
            this.confirmDeleteDialog.ShowDialog();
    }

    private async Task OnConfirmDeleteStudentClickEventHandler()
    {
        var result = await this.StudentServices.DeleteStudentAsync(this.underDeletionStudent.Id).ConfigureAwait(true);
        if (result.State == ServiceResultState.Success)
        {
            await this.studentsGrid.RefreshDataAsync();
            this.confirmDeleteDialog!.CloseDialog();
        }
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

    private void OnGroupFilterChangeEventHandler(ChangeEventArgs arg)
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
