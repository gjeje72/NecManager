namespace NecManager.Web.Areas.Admin.Settings.Lessons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Lessons;
using NecManager.Web.Service.Models.Query;

public partial class SettingsLessons : ComponentBase
{
    private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    private GridItemsProvider<LessonBaseViewModel> lessonProviders = default!;
    private LessonCreationViewModel UnderCreationLesson = new();
    private QuickGrid<LessonBaseViewModel> lessonsGrid;
    private Dialog? lessonsCreationFormDialog;

    public IQueryable<LessonBaseViewModel> Lessons { get; set; } = new List<LessonBaseViewModel>().AsQueryable();

    [Inject]
    public ILessonServices LessonServices { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        this.lessonProviders = async req => await this.GetLessonsProviderAsync(req).ConfigureAwait(true);

        return base.OnInitializedAsync();
    }

    private async Task<GridItemsProviderResult<LessonBaseViewModel>> GetLessonsProviderAsync(GridItemsProviderRequest<LessonBaseViewModel> gridItemsProviderRequest)
    {
        var query = new LessonInputQuery
        {
            CurrentPage = (gridItemsProviderRequest.StartIndex / gridItemsProviderRequest.Count ?? 1) + 1,
            PageSize = this.pagination.ItemsPerPage,
        };

        var (_, lessons, _) = await this.LessonServices.GetAllLessonsAsync(query).ConfigureAwait(true);
        var pageableLessons = new PageableResult<LessonBaseViewModel>
        {
            Items = lessons?.Items?.Select(l =>
                new LessonBaseViewModel
                {
                    DifficultyType = l.Difficulty,
                    Title = l.Title,
                    Weapon = l.Weapon
                }).ToList() ?? new(),
            TotalElements = lessons?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableLessons.Items.ToList(), pageableLessons.TotalElements);
    }
    private void CreateLessonEventHandler()
    {
        if (this.lessonsCreationFormDialog is not null)
            this.lessonsCreationFormDialog.ShowDialog();
    }
    private async Task CreateLessonAsync()
    {
        if (string.IsNullOrWhiteSpace(this.UnderCreationLesson.Title)
            || string.IsNullOrWhiteSpace(this.UnderCreationLesson.Content)
            || string.IsNullOrWhiteSpace(this.UnderCreationLesson.Description))
            return;

        var result = await this.LessonServices.CreateLessonsAsync(new LessonCreationInput
        {
            Title = this.UnderCreationLesson.Title,
            Description = this.UnderCreationLesson.Description,
            Content = this.UnderCreationLesson.Content,
            Weapon = this.UnderCreationLesson.Weapon,
            Difficulty = this.UnderCreationLesson.Difficulty
        }).ConfigureAwait(true);

        if (result.State == ServiceResultState.Success)
        {
            await this.lessonsGrid.RefreshDataAsync().ConfigureAwait(true);
            if (this.lessonsCreationFormDialog is not null)
                this.lessonsCreationFormDialog.CloseDialog();
        }
    }

    private void WeaponSelectChangedEventHandler(ChangeEventArgs e)
    {
        _ = Enum.TryParse<WeaponType>(e.Value?.ToString(), out var weaponType);
        this.UnderCreationLesson.Weapon = weaponType;
    }

    private void DifficultySelectChangedEventHandler(ChangeEventArgs e)
    {
        _ = Enum.TryParse<DifficultyType>(e.Value?.ToString(), out var difficultyType);
        this.UnderCreationLesson.Difficulty = difficultyType;
    }
}
