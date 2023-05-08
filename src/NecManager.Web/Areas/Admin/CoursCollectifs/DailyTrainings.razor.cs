namespace NecManager.Web.Areas.Admin.CoursCollectifs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.CoursCollectifs.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Trainings;

public partial class DailyTrainings
{
    [Inject]
    public ITrainingServices TrainingServices { get; set; } = null!;

    private List<TrainingBaseViewModel> Trainings = new();
    private TrainingDetailsViewModel CurrentTraining = new();
    private TrainingInputQuery trainingInputQuery = new();
    private string ValidateButtonText = "Valider";


    protected override async Task OnInitializedAsync()
    {
        this.trainingInputQuery.Date = DateTime.Now;
        await this.RefreshTrainingsListAsync().ConfigureAwait(true);
    }

    private async Task RefreshTrainingsListAsync()
    {
        var query = new TrainingInputQuery
        {
            CurrentPage = 1,
            PageSize = 10,
            Date = this.trainingInputQuery.Date,
            WeaponType = this.trainingInputQuery.WeaponType,
        };

        var (_, trainings, _) = await this.TrainingServices.GetAllTrainingsAsync(query).ConfigureAwait(true);
        this.Trainings = trainings?.Items?.Select(t =>
                new TrainingBaseViewModel
                {
                    Id = t.Id,
                    GroupId = t.GroupId,
                    LessonId = t.LessonId,
                    IsIndividual = t.IsIndividual,
                    GroupName = t.GroupName,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Weapon = t.Weapon,
                    LessonName = t.LessonName ?? string.Empty,
                    MasterName = t.MasterName ?? string.Empty,
                }).ToList() ?? new();
        await this.SetCurrentTrainingAsync(null).ConfigureAwait(true);
    }

    private async Task SetCurrentTrainingAsync(int? trainingId)
    {
        this.ValidateButtonText = "Valider";
        if (trainingId == null)
        {
            var nextTraining = this.Trainings.FirstOrDefault(t => t.IsIndividual == false && t.EndTime > DateTime.Now.Hour);
            if (nextTraining is null)
            {
                this.CurrentTraining = new();
                return;
            }

            trainingId = nextTraining.Id;
        }

        var (state, training, _) = await this.TrainingServices.GetTrainingByIdAsync((int)trainingId).ConfigureAwait(true);
        if (state == ServiceResultState.Success && training is not null)
        {
            this.CurrentTraining = new TrainingDetailsViewModel
            {
                Id = training.Id,
                Date = training.Date,
                StartTime = training.StartTime,
                EndTime = training.EndTime,
                GroupId = training.GroupId,
                GroupName = training.GroupName,
                GroupStudents = training.GroupStudents.Select(gs => new TrainingStudentViewModel { Id = gs.Id, Category = gs.Category, FirstName = gs.FirstName, Name = gs.Name }).ToList(),
                TrainingStudents = training.TrainingStudents.Select(ts => new TrainingStudentViewModel { Id = ts.Id, Category = ts.Category, FirstName = ts.FirstName, Name = ts.Name }).ToList(),
                IsIndividual = training.IsIndividual,
                Lesson = new() { Id = training.Lesson.Id, Content = training.Lesson.Content, Description = training.Lesson.Description, Difficulty = training.Lesson.Difficulty, Title = training.Lesson.Title, Weapon = training.Lesson.Weapon },
                Weapon = training.Weapon,
                MasterName = training.MasterName,
            };
        }
    }

    private async Task OnValidateTrainingStudentClickedEventHandlerAsync()
    {
        if (!this.CurrentTraining.TrainingStudents.Any())
            return;

        var updateStudentInput = new TrainingUpdateStudentInput
        {
            TrainingId = this.CurrentTraining.Id,
            IsIndividual = false,
            MasterName = this.CurrentTraining.MasterName,
            StudentsIds = this.CurrentTraining.TrainingStudents.Select(ts => ts.Id).Distinct().ToList(),
        };

        var (state, _) = await this.TrainingServices.AddStudentsInTrainingAsync(updateStudentInput).ConfigureAwait(true);
        if(state == ServiceResultState.Success)
        {
            this.ValidateButtonText = "✔️";
        }
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

        await this.RefreshTrainingsListAsync().ConfigureAwait(true);
    }

    private void AddTrainingStudent(TrainingStudentViewModel student, ChangeEventArgs arg)
    {
        _ = bool.TryParse(arg.Value?.ToString(), out var isSelected);
        this.ValidateButtonText = "Valider";

        if (isSelected)
            this.CurrentTraining.TrainingStudents.Add(student);
        else
        {
            var studentsToRemove = this.CurrentTraining.TrainingStudents.Where(x => x.Id == student.Id).ToList();
            foreach(var studentToRemove in studentsToRemove)
            {
                this.CurrentTraining.TrainingStudents.Remove(studentToRemove);
            }
        }
    }
}
