namespace NecManager.Web.Areas.Admin.Settings.Trainings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Groups;
using NecManager.Web.Service.Models.Trainings;

public partial class InitSeasonTrainings
{
    [Inject]
    public IGroupServices GroupServices { get; set; } = null!;

    [Inject]
    public ITrainingServices TrainingServices { get; set; } = null!;

    private SeasonTrainingCreation SeasonTrainingCreation { get; set; } = new();
    private List<GroupBase> Groups = new();
    private string InititationStateMessage = "En attente d'initialisation.";

    protected override async Task OnInitializedAsync()
    {
        var (state, groups, _) = await this.GroupServices.GetAllGroups().ConfigureAwait(true);
        if (state == ServiceResultState.Success && groups is not null)
            this.Groups = groups;
    }

    private async Task CreateTrainingsAsync()
    {
        this.InititationStateMessage = "Création en cours...";
        var selectedDays = this.GetSelectedDayOfWeek();
        DateTime currentDate = this.SeasonTrainingCreation.StartDate;
        while (currentDate <= this.SeasonTrainingCreation.EndDate)
        {
            if (selectedDays.Contains(currentDate.DayOfWeek) && !this.SeasonTrainingCreation.ExcludedDates.Any(ed => ed.Month == currentDate.Month && ed.Day == currentDate.Day))
            {
                var startTime = currentDate.DayOfWeek switch
                {
                    DayOfWeek.Monday => this.SeasonTrainingCreation.MondayStartTime,
                    DayOfWeek.Tuesday => this.SeasonTrainingCreation.TuesdayStartTime,
                    DayOfWeek.Wednesday => this.SeasonTrainingCreation.WednesdayStartTime,
                    DayOfWeek.Thursday => this.SeasonTrainingCreation.ThursdayStartTime,
                    DayOfWeek.Friday => this.SeasonTrainingCreation.FridayStartTime,
                    DayOfWeek.Saturday => this.SeasonTrainingCreation.SaturdayStartTime,
                    DayOfWeek.Sunday => this.SeasonTrainingCreation.SundayStartTime,
                    _ => throw new NotImplementedException(),
                };
                var endTime = currentDate.DayOfWeek switch
                {
                    DayOfWeek.Monday => this.SeasonTrainingCreation.MondayEndTime,
                    DayOfWeek.Tuesday => this.SeasonTrainingCreation.TuesdayEndTime,
                    DayOfWeek.Wednesday => this.SeasonTrainingCreation.WednesdayEndTime,
                    DayOfWeek.Thursday => this.SeasonTrainingCreation.ThursdayEndTime,
                    DayOfWeek.Friday => this.SeasonTrainingCreation.FridayEndTime,
                    DayOfWeek.Saturday => this.SeasonTrainingCreation.SaturdayEndTime,
                    DayOfWeek.Sunday => this.SeasonTrainingCreation.SundayEndTime,
                    _ => throw new NotImplementedException(),
                };
                
                this.SeasonTrainingCreation.Trainings.Add(new()
                {
                    Date = currentDate,
                    StartTime = (decimal)startTime.TimeOfDay.TotalHours,
                    EndTime = (decimal)endTime.TimeOfDay.TotalHours,
                    IsIndividual = false,
                    LessonId = 1,
                    GroupId = this.SeasonTrainingCreation.GroupId,
                    MasterName = this.SeasonTrainingCreation.MasterName,
                });
            }
            currentDate = currentDate.AddDays(1);
        }

        var (state, errorMessage) = await this.TrainingServices.CreateRangeTrainingsAsync(this.SeasonTrainingCreation.Trainings).ConfigureAwait(true);
        if (state == ServiceResultState.Success)
        {
            this.InititationStateMessage = $"Création réussie pour le groupe {this.Groups.First(g => g.Id == this.SeasonTrainingCreation.GroupId).Title} !";
        }
        else
        {
            this.InititationStateMessage = $"Erreur lors de la création, vérifier vos saisies. {errorMessage}.";
        }
    }

    private void OnAddHolidayClickEventHandler()
    {
        if (this.SeasonTrainingCreation.HolidayStartDate > this.SeasonTrainingCreation.HolidayEndDate)
        {
            this.SeasonTrainingCreation.HolidayErrorMessage = "Dates saisies incorrects.";
            return;
        }
        var selectedDaysOfWeek = this.GetSelectedDayOfWeek();

        DateTime currentDate = this.SeasonTrainingCreation.HolidayStartDate;
        while (currentDate <= this.SeasonTrainingCreation.HolidayEndDate)
        {
            if (selectedDaysOfWeek.Any(day => day == currentDate.DayOfWeek) && !this.SeasonTrainingCreation.ExcludedDates.Contains(currentDate))
            {
                this.SeasonTrainingCreation.ExcludedDates.Add(currentDate);
            }
            currentDate = currentDate.AddDays(1);
        }
    }

    private void OnAddPublicHolidayClickEventHandler()
    {
        if (this.SeasonTrainingCreation.ExcludedDates.Contains(this.SeasonTrainingCreation.PublicHolidayDate))
            return;

        this.SeasonTrainingCreation.ExcludedDates.Add(this.SeasonTrainingCreation.PublicHolidayDate);
    }

    private List<DayOfWeek> GetSelectedDayOfWeek()
    {
        var selectedDayOfWeek = new List<DayOfWeek>();

        if (this.SeasonTrainingCreation.isMondaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Monday);

        if (this.SeasonTrainingCreation.isTuesdaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Tuesday);

        if (this.SeasonTrainingCreation.isWednesdaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Wednesday);

        if (this.SeasonTrainingCreation.isThursdaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Thursday);

        if (this.SeasonTrainingCreation.isFridaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Friday);

        if (this.SeasonTrainingCreation.isSaturdaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Saturday);

        if (this.SeasonTrainingCreation.isSundaySelected)
            selectedDayOfWeek.Add(DayOfWeek.Sunday);

        return selectedDayOfWeek;
    }
}
