namespace NecManager.Web.Areas.Admin.Settings.Trainings;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;
using NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;

public partial class SettingsTrainings
{
    [Inject]
    public ITrainingServices TrainingServices { get; set; } = null!;

    [Inject]
    public ILessonServices LessonServices { get; set; } = null!;

    private PaginationState pagination = new() { ItemsPerPage = 10 };
    private GridItemsProvider<TrainingBaseViewModel> trainingProviders = default!;
    private QuickGrid<TrainingBaseViewModel> trainingsGrid;
    private TrainingInputQuery trainingInputQuery = new();
    private Dialog? trainingCreationFormDialog;
    private TrainingCreationViewModel UnderCreationTraining { get; set; } = new();
    private LessonInputQuery LessonFilter = new();
    private List<LessonBaseViewModel> Lessons = new();

    protected override async Task OnInitializedAsync()
    {
        this.trainingProviders = async req => await this.GetTrainingsProviderAsync(req).ConfigureAwait(true);
    }

    private async Task<GridItemsProviderResult<TrainingBaseViewModel>> GetTrainingsProviderAsync(GridItemsProviderRequest<TrainingBaseViewModel> gridItemsProviderRequest)
    {
        var query = new TrainingInputQuery
        {
            CurrentPage = (gridItemsProviderRequest.StartIndex / gridItemsProviderRequest.Count ?? 1) + 1,
            PageSize = this.pagination.ItemsPerPage,
            WeaponType = this.trainingInputQuery.WeaponType,
            DifficultyType = this.trainingInputQuery.DifficultyType,
            Filter = this.trainingInputQuery.Filter,
            OnlyIndividual = this.trainingInputQuery.OnlyIndividual,
        };

        var (_, trainings, _) = await this.TrainingServices.GetAllTrainingsAsync(query).ConfigureAwait(true);
        var pageableTrainings = new PageableResult<TrainingBaseViewModel>
        {
            Items = trainings?.Items?.Select(t =>
                new TrainingBaseViewModel
                {
                    Id = t.Id,
                    IsIndividual = t.IsIndividual,
                    GroupName = t.GroupName,
                    Categories = t.Categories,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Weapon = t.Weapon,
                    LessonName = t.Lesson.Title,
                    MasterName = t.MasterName ?? string.Empty,
                }).ToList() ?? new(),
            TotalElements = trainings?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableTrainings.Items.ToList(), pageableTrainings.TotalElements);
    }

    private async Task OnWeaponFilterChangeEventHandlerAsync(WeaponType? weaponType)
    {
        if (weaponType is null || weaponType == WeaponType.None)
        {
            this.trainingInputQuery.WeaponType = null;
        }
        else
        {
            this.trainingInputQuery.WeaponType = weaponType;
        }

        await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
    }

    private async Task OnDifficultyFilterChangeEventHandlerAsync(DifficultyType? difficulty)
    {
        if (difficulty is null || difficulty == DifficultyType.None)
        {
            this.trainingInputQuery.DifficultyType = null;
        }
        else
        {
            this.trainingInputQuery.DifficultyType = difficulty;
        }

        await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
    }

    private async Task OnFilterChangeEventHandlerAsync(string? filter)
    {
        this.trainingInputQuery.Filter = filter;
        await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
    }

    private async Task OnOnlyIndiviualFilterChangeEventHandlerAsync(bool? onlyIndividual)
    {
        this.trainingInputQuery.OnlyIndividual = onlyIndividual ?? false;
        await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
    }

    private void CreateTrainingEventHandler()
    {
        if (this.trainingCreationFormDialog is not null)
            this.trainingCreationFormDialog.ShowDialog();
    }

    private void OnValidCreateFormAsync()
    {

    }
    private async Task RefreshLessonListAsync()
    {
       var (state, lessons, _) = await this.LessonServices.GetAllLessonsAsync(new() { DifficultyType = this.LessonFilter.DifficultyType, WeaponType = this.LessonFilter.WeaponType, IsPageable = false });
        if (state == ServiceResultState.Success && lessons is not null)
            this.Lessons = lessons.Items?.Select(l => new LessonBaseViewModel
            {
                Id = l.Id,
                Title = l.Title,
            }).ToList() ?? new();
    }
    private async Task DifficultySelectChangedEventHandler(DifficultyType? difficulty)
    {
        this.LessonFilter.DifficultyType = difficulty;
        await this.RefreshLessonListAsync().ConfigureAwait(true);
    }

    private async Task WeaponSelectChangedEventHandler(WeaponType? weapon)
    {
        this.LessonFilter.WeaponType = weapon;
        await this.RefreshLessonListAsync().ConfigureAwait(true);
    }

    private async Task LessonSelectChangedEventHandler(int? lessonId)
    {
        if(lessonId != null)
            this.UnderCreationTraining.LessonId = (int)lessonId;

    }
}
