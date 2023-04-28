namespace NecManager.Web.Areas.Admin.Settings.Trainings;

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
using NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;
using NecManager.Web.Components.Modal;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Trainings;

public partial class SettingsTrainings
{
    [Inject]
    public ITrainingServices TrainingServices { get; set; } = null!;

    [Inject]
    public ILessonServices LessonServices { get; set; } = null!;

    [Inject]
    public IGroupServices GroupServices { get; set; } = null!;

    [Inject]
    public IStudentServices StudentServices { get; set; } = null!;

    private PaginationState pagination = new() { ItemsPerPage = 10 };
    private GridItemsProvider<TrainingBaseViewModel> trainingProviders = default!;
    private QuickGrid<TrainingBaseViewModel> trainingsGrid;
    private TrainingInputQuery trainingInputQuery = new();
    private Dialog? trainingCreationFormDialog;
    private Dialog? confirmDeleteDialog;
    private TrainingBaseViewModel underDeleteTraining = new();
    private TrainingCreationViewModel UnderCreationTraining { get; set; } = new() { Date = DateTime.Now };
    private LessonInputQuery LessonFilter = new();
    private List<LessonBaseViewModel> Lessons = new();
    private List<GroupBase> Groups = new();
    private List<TrainingStudentBaseViewModel> Students = new();

    private bool isUnderUpdate = false;
    private string ErrorMessage = string.Empty;
    private string ValidationButtonLabel
        => this.isUnderUpdate ? "Mettre à jour" : "Créer";
    private string CreateOrUpdateTrainingModalTitle
        => this.isUnderUpdate ? "Mettre à jour un entrainement" : "Création d'un entrainement";

    protected override async Task OnInitializedAsync()
    {
        this.trainingProviders = async req => await this.GetTrainingsProviderAsync(req).ConfigureAwait(true);
        var (state, groups, _) = await this.GroupServices.GetAllGroups().ConfigureAwait(true);
        if (state == ServiceResultState.Success && groups is not null)
            this.Groups = groups;

        await this.RefreshLessonListAsync().ConfigureAwait(true);
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
            MasterName = this.trainingInputQuery.MasterName,
        };

        var (_, trainings, _) = await this.TrainingServices.GetAllTrainingsAsync(query).ConfigureAwait(true);
        var pageableTrainings = new PageableResult<TrainingBaseViewModel>
        {
            Items = trainings?.Items?.Select(t =>
                new TrainingBaseViewModel
                {
                    Id = t.Id,
                    GroupId = t.GroupId,
                    LessonId = t.LessonId,
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

        await this.trainingsGrid.RefreshDataAsync();
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

        await this.trainingsGrid.RefreshDataAsync();
    }

    private async Task OnFilterChangeEventHandlerAsync(string? filter)
    {
        this.trainingInputQuery.Filter = filter;
        await this.trainingsGrid.RefreshDataAsync();
    }

    private async Task OnMasterNameChangeEventHandlerAsync(string? masterName)
    {
        this.trainingInputQuery.MasterName = masterName;
        await this.trainingsGrid.RefreshDataAsync();
    }

    private async Task OnOnlyIndiviualFilterChangeEventHandlerAsync(bool? onlyIndividual)
    {
        this.trainingInputQuery.OnlyIndividual = onlyIndividual ?? false;
        await this.trainingsGrid.RefreshDataAsync();
    }

    private async Task OnValidCreateOrUpdateFormAsync()
    {
        if (this.isUnderUpdate)
        {
            await this.UpdateTrainingAsync().ConfigureAwait(true);
            return;
        }

        await this.CreateTrainingAsync().ConfigureAwait(true);
    }

    private async Task OnCreateTrainingClickEventHandler()
    {
        this.isUnderUpdate = false;
        this.ErrorMessage = string.Empty;
        this.UnderCreationTraining = new();

        if (this.Students.Count == 0)
            await this.RefreshStudentListAsync().ConfigureAwait(true);

        if (this.trainingCreationFormDialog is not null)
            this.trainingCreationFormDialog.ShowDialog();
    }

    private async Task OnUpdateTrainingClickEventHandler(TrainingBaseViewModel training)
    {
        var hours = Convert.ToDouble(training.StartTime);
        var startTime = new DateTime().AddHours(hours);
        var endHours = Convert.ToDouble(training.EndTime);
        var endTime = new DateTime().AddHours(endHours);

        if (this.Students.Count == 0 && training.IsIndividual)
            await this.RefreshStudentListAsync().ConfigureAwait(true);

        this.isUnderUpdate = true;
        this.ErrorMessage = string.Empty;

        this.UnderCreationTraining.Id = training.Id;
        this.UnderCreationTraining.Date = training.Date;
        this.UnderCreationTraining.MasterName = training.MasterName;
        this.UnderCreationTraining.StartTime = startTime;
        this.UnderCreationTraining.EndTime = endTime;
        this.UnderCreationTraining.GroupId = training.GroupId;
        this.UnderCreationTraining.IsIndividual = training.IsIndividual;
        this.UnderCreationTraining.LessonId = training.LessonId;

        if (this.trainingCreationFormDialog is not null)
            this.trainingCreationFormDialog.ShowDialog();
    }

    private async Task CreateTrainingAsync()
    {
        var decimalStartTime = (decimal)this.UnderCreationTraining.StartTime.TimeOfDay.TotalHours;
        var decimalEndTime = (decimal)this.UnderCreationTraining.EndTime.TimeOfDay.TotalHours;

        var trainingToCreate = new TrainingCreationInput
        {
            Date = this.UnderCreationTraining.Date,
            StartTime = decimalStartTime,
            EndTime = decimalEndTime,
            IsIndividual = this.UnderCreationTraining.IsIndividual,
            LessonId = this.UnderCreationTraining.LessonId,
            MasterName = this.UnderCreationTraining.MasterName,
            GroupId = this.UnderCreationTraining.GroupId,
            Students = this.UnderCreationTraining.Students.Select(s => new TrainingStudentBase() { Id = s.Id, Category = s.Category, FirstName = s.FirstName, Name = s.Name }).ToList()
        };

        if (!trainingToCreate.IsIndividual && trainingToCreate.GroupId is null or 0
            || trainingToCreate.IsIndividual && trainingToCreate.Students.Count != 1)
        {
            this.ErrorMessage = "Vous devez choisir un groupe ou un élève si individuel.";
            return;
        }

        var (state, _) = await this.TrainingServices.CreateTrainingAsync(trainingToCreate).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
        {
            this.trainingCreationFormDialog!.CloseDialog();
            await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
        }
    }

    private async Task UpdateTrainingAsync()
    {
        var decimalStartTime = (decimal)this.UnderCreationTraining.StartTime.TimeOfDay.TotalHours;
        var decimalEndTime = (decimal)this.UnderCreationTraining.EndTime.TimeOfDay.TotalHours;

        var trainingToUpdate = new TrainingUpdateInput
        {
            Id = this.UnderCreationTraining.Id,
            Date = this.UnderCreationTraining.Date,
            StartTime = decimalStartTime,
            EndTime = decimalEndTime,
            MasterName = this.UnderCreationTraining.MasterName,
            IsIndividual = this.UnderCreationTraining.IsIndividual,
            LessonId = this.UnderCreationTraining.LessonId,
            GroupId = this.UnderCreationTraining.GroupId,
            Students = this.UnderCreationTraining.Students.Select(s => new TrainingStudentBase() { Id = s.Id, Category = s.Category, FirstName = s.FirstName, Name = s.Name }).ToList()
        };

        if (!trainingToUpdate.IsIndividual && trainingToUpdate.GroupId is null or 0
            || trainingToUpdate.IsIndividual && trainingToUpdate.Students.Count != 1)
        {
            this.ErrorMessage = "Vous devez choisir un groupe ou un élève si individuel.";
            return;
        }

        var (state, errorMessage) = await this.TrainingServices.UpdateTrainingAsync(trainingToUpdate).ConfigureAwait(true);
        if (state != ServiceResultState.Success)
        {
            this.ErrorMessage = errorMessage[0];
            return;
        }

        this.trainingCreationFormDialog!.CloseDialog();
        await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
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

    private async Task RefreshStudentListAsync()
    {
        var (state, students, _) = await this.StudentServices.GetAllStudentsAsync(new() { Filter = this.UnderCreationTraining.StudentFilter });
        if(state == ServiceResultState.Success && students is not null)
            this.Students = students.Items?.Select(s => new TrainingStudentBaseViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Categorie = s.Categorie,
                Weapon = s.Weapon
            }).ToList() ?? new();
    }

    private async Task DifficultySelectChangedEventHandler(DifficultyType? difficulty)
    {
        if (difficulty == DifficultyType.None)
            this.LessonFilter.DifficultyType = null;
        else
            this.LessonFilter.DifficultyType = difficulty;

        await this.RefreshLessonListAsync().ConfigureAwait(true);
    }

    private async Task WeaponSelectChangedEventHandler(WeaponType? weapon)
    {
        if (weapon == WeaponType.None)
            this.LessonFilter.WeaponType = null;
        else
            this.LessonFilter.WeaponType = weapon;

        await this.RefreshLessonListAsync().ConfigureAwait(true);
    }

    private void LessonSelectChangedEventHandler(int? lessonId)
    {
        if (lessonId != null)
            this.UnderCreationTraining.LessonId = (int)lessonId;
    }

    private void GroupSelectChangedEventHandler(int? groupId)
       => this.UnderCreationTraining.GroupId = groupId;

    private void StudentSelectChangedEventHandler(int? studentId)
    {
        if(studentId is not null)
            this.UnderCreationTraining.Students = new() { new() { Id = (int)studentId } };

        this.UnderCreationTraining.StudentId = studentId;
    }

    private async Task OnDeleteTrainingClickEventHandler(TrainingBaseViewModel training)
    {
        this.underDeleteTraining = training;
        if (this.confirmDeleteDialog != null)
            this.confirmDeleteDialog.ShowDialog();
    }

    private async Task OnConfirmDeleteTrainingClickEventHandler()
    {
        var result = await this.TrainingServices.DeleteTrainingAsync(this.underDeleteTraining.Id).ConfigureAwait(true);
        if (result.State == ServiceResultState.Success)
        {
            this.confirmDeleteDialog!.CloseDialog();
            await this.trainingsGrid.RefreshDataAsync().ConfigureAwait(true);
        }
    }

    private async Task OnStudentFilterChangeEventHandler(ChangeEventArgs arg)
    {
        this.UnderCreationTraining.StudentFilter = arg.Value?.ToString() ?? string.Empty;
        await this.RefreshStudentListAsync().ConfigureAwait(true);
    }
}
