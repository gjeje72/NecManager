namespace NecManager.Web.Areas.Admin.Settings.Group;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Group.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Groups;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class MigrateStudent
{
    [Inject]
    public IGroupServices GroupServices { get; init; } = default!;

    [Inject]
    public IStudentServices StudentServices { get; init; } = default!;

    private List<GroupBaseViewModel> SourceGroups { get; set; } = new List<GroupBaseViewModel>();
    private List<GroupBaseViewModel> DestinationGroups
        => this.SourceGroups.Where(g => g.Id != this.SelectedSourceGroup.Id).ToList();
    private CreateGroupViewModel SelectedSourceGroup { get; set; } = new();
    private CreateGroupViewModel SelectedDestinationGroup { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await this.GetGroupsAsync().ConfigureAwait(true);
    }

    private async Task GetGroupsAsync()
    {
        var (groupSuccess, groups, _) = await this.GroupServices.GetAllGroups();
        if (groupSuccess == ServiceResultState.Success)
            this.SourceGroups = groups?.Select(g => new GroupBaseViewModel()
            {
                Id = g.Id,
                Weapon = g.Weapon,
                StudentCount = g.StudentCount,
                Categories = g.CategoriesIds?.Select(c => (CategoryType)c).ToList() ?? new(),
                Title = g.Title,
            }).ToList() ?? new List<GroupBaseViewModel>();
    }

    private async Task OnGroupSelectedChangedEventHandlerAsync(ChangeEventArgs arg, bool isSource)
    {
        _ = int.TryParse(arg.Value?.ToString(), out var selectedGroupId);
        var selectedGroup = this.SourceGroups.FirstOrDefault(g => g.Id == selectedGroupId);
        var (state, students, _) = await this.StudentServices.GetAllStudentsAsync(new() { GroupId = selectedGroup?.Id, IsPageable = false }).ConfigureAwait(true);
        if(state == ServiceResultState.Success && students != null)
        {
            var group = new CreateGroupViewModel()
            {
                Id = selectedGroup?.Id ?? 0,
                Categories = selectedGroup?.Categories ?? new(),
                Title = selectedGroup?.Title ?? string.Empty,
                Weapon = selectedGroup?.Weapon ?? WeaponType.None,
                Students = students.Items?.Select(s => new GroupStudentViewModel()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Birthdate = s.Birthdate,
                    Category = s.Categorie,
                    Weapon = s.Weapon
                }).ToList() ?? new(),
            };

            if (isSource)
            {
                this.SelectedSourceGroup = group;
                if (this.SelectedDestinationGroup.Id == group.Id)
                    this.SelectedDestinationGroup = new();
            }
            else
                this.SelectedDestinationGroup = group;

            await this.InvokeAsync(this.StateHasChanged);
        }
    }

    private async Task OnSaveClickedEventHandler()
    {
        var group1ToUpdate = new GroupUpdateInput
        {
            GroupId = this.SelectedSourceGroup.Id,
            Title = this.SelectedSourceGroup.Title,
            Weapon = this.SelectedSourceGroup.Weapon, 
            Categories = this.SelectedSourceGroup.Categories.Select(c => (int)c).ToList(),
            StudentsIds = this.SelectedSourceGroup.Students.Where(s => s.IsUnsavedMove == false).Select(s => s.Id).ToList(),
        };

        var group2ToUpdate = new GroupUpdateInput
        {
            GroupId = this.SelectedDestinationGroup.Id,
            Title = this.SelectedDestinationGroup.Title,
            Weapon = this.SelectedDestinationGroup.Weapon,
            Categories = this.SelectedDestinationGroup.Categories.Select(c => (int)c).ToList(),
            StudentsIds = this.SelectedDestinationGroup.Students.Select(s => s.Id).ToList(),
        };

        var (state, _) = await this.GroupServices.UpdateGroupAsync(group1ToUpdate).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
        {
            var (state2, _) = await this.GroupServices.UpdateGroupAsync(group2ToUpdate).ConfigureAwait(true);
            if (state2 == ServiceResultState.Success)
            {
                this.SelectedSourceGroup = new();
                this.SelectedDestinationGroup = new();
            }
        }
    }

    private void MoveStudentFromSourceToDestinationGroup(GroupStudentViewModel student)
    {
        if (this.SelectedDestinationGroup.Id == 0)
            return;
        student.IsUnsavedMove = true;
        this.SelectedDestinationGroup.Students.Add(student);
    }

    private void UndoStudentMove(GroupStudentViewModel student)
    {
        var sourceStudent = this.SelectedSourceGroup.Students.FirstOrDefault(s => s.Id == student.Id);
        if (sourceStudent == null)
            return;

        sourceStudent.IsUnsavedMove = false;
        this.SelectedDestinationGroup.Students.Remove(student);
    }

    private string GetCssForMovedStudent(bool isUnsavedMove)
    {
        return isUnsavedMove ? "moved" : string.Empty;
    }

}
