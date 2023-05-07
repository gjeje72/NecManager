namespace NecManager.Server.Api.Business.Modules.Training;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

using static NecManager.Common.ApiResponseError;

internal class TrainingBusiness : ITrainingBusiness
{
    private readonly ILogger<ITrainingBusiness> logger;
    private readonly ITrainingAccessLayer trainingAccessLayer;
    private readonly ILessonAccessLayer lessonAccessLayer;
    private readonly IStudentAccessLayer studentAccessLayer;

    public TrainingBusiness(ILogger<ITrainingBusiness> logger, ITrainingAccessLayer trainingAccessLayer, ILessonAccessLayer lessonAccessLayer, IStudentAccessLayer studentAccessLayer)
    {
        this.logger = logger;
        this.trainingAccessLayer = trainingAccessLayer;
        this.lessonAccessLayer = lessonAccessLayer;
        this.studentAccessLayer = studentAccessLayer;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PageableResult<TrainingBase>>> SearchAsync(ServiceMonitoringDefinition monitoringIds, TrainingQueryInput query)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var (pageSize, currentPage, difficultyType, weaponType, groupId, date, season, studentId, filter, onlyIndividual, masterName) = query;
        var pageableResult = await this.trainingAccessLayer.GetPageableCollectionAsync(new(pageSize, currentPage, difficultyType, weaponType, groupId, date, season, studentId, filter, onlyIndividual, masterName), true);
        if (pageableResult.Items is not null)
        {
            var pageableLessons = new PageableResult<TrainingBase>
            {
                Items = pageableResult.Items.Select(t => this.MapTrainingToTrainingBase(t)),
                TotalElements = pageableResult.TotalElements,
            };
            return new ApiResponse<PageableResult<TrainingBase>>(monitoringIds, new(pageableLessons));
        }
        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TrainingDetails>> GetTrainingByIdAsync(ServiceMonitoringDefinition monitoringIds, int trainingId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == trainingId, false, x => x.Include(t => t.Group!).ThenInclude(g => g.StudentGroups).ThenInclude(sg => sg.Student).Include(t => t.PersonTrainings!).ThenInclude(pt => pt.Student).Include(t => t.Lesson!)).ConfigureAwait(false);
        return matchingTraining is null
            ? (new(monitoringIds, new(ApiResponseResultState.NotFound, ApiResponseError.LessonApiErrors.LessonNotFound)))
            : (new(monitoringIds, new(this.MapTrainingToTrainingDetails(matchingTraining))));
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingCreationInput input)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        if (input.StartTime >= input.EndTime || input.Date < new DateTime(2020, 1, 1))
            return new(monitoringIds, new(ApiResponseResultState.BadRequest, TrainingApiErrors.TrainingBadRequest));

        var dbLesson = new Training
        {
            Date = input.Date,
            StartTime = input.StartTime,
            EndTime = input.EndTime,
            GroupId = input.GroupId,
            LessonId = input.LessonId,
            MasterName = input.MasterName,
            PersonTrainings = input.Students.Select(s => new PersonTraining { IsIndividual = input.IsIndividual, StudentId = s.Id }).ToList()
        };

        try
        {
            await this.trainingAccessLayer.AddAsync(dbLesson).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while creating training. Date = {date}", input.Date);
            return new(monitoringIds, new(TrainingApiErrors.TrainingCreationFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateRangeTrainingAsync(ServiceMonitoringDefinition monitoringIds, List<TrainingCreationInput> inputs)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var dbTrainings = new List<Training>();

        foreach (var input in inputs)
        {
            if (input.StartTime >= input.EndTime || input.Date < new DateTime(2020, 1, 1))
                continue;

            var dbTraining = new Training
            {
                Date = input.Date,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                GroupId = input.GroupId,
                LessonId = input.LessonId,
                MasterName = input.MasterName,
                PersonTrainings = input.Students.Select(s => new PersonTraining { IsIndividual = input.IsIndividual, StudentId = s.Id }).ToList()
            };
            dbTrainings.Add(dbTraining);
        }
        try
        {
            await this.trainingAccessLayer.AddRangeAsync(dbTrainings).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while creating trainings.");
            return new(monitoringIds, new(TrainingApiErrors.TrainingsCreationFailure));
        }

        return new(monitoringIds, new());

    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> DeleteTrainingAsync(ServiceMonitoringDefinition monitoringIds, int trainingId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == trainingId, true).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        try
        {
            await this.trainingAccessLayer.RemoveAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while trying to delete training Id={trainingId}", trainingId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingDeletionFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> AddStudentsInTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingUpdateStudentInput input)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == input.TrainingId, true, x => x.Include(t => t.PersonTrainings)).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        if ((matchingTraining.PersonTrainings.Count == 1 && input.IsIndividual) || (input.StudentsIds.Count > 1 && input.IsIndividual))
            return new(monitoringIds, new(ApiResponseResultState.BadRequest, TrainingApiErrors.TrainingBadRequest));

        if (!await this.studentAccessLayer.ExistsRangeAsync(input.StudentsIds))
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        foreach (var studentId in input.StudentsIds)
        {
            matchingTraining.MasterName = input.MasterName;
            matchingTraining.PersonTrainings.Add(new PersonTraining
            {
                StudentId = studentId,
                TrainingId = matchingTraining.Id,
                IsIndividual = input.IsIndividual,
            });
        }
        try
        {
            await this.trainingAccessLayer.UpdateAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating training trainingId={trainingId} with students", input.TrainingId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> UpdateTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingUpdateInput input)
    {
        // ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == input.Id, true).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));


        try
        {
            if (input.LessonId > 0)
            {
                var matchingLesson = await this.lessonAccessLayer.GetSingleAsync(x => x.Id == input.LessonId, true).ConfigureAwait(false);
                if (matchingLesson is null)
                    return new(monitoringIds, new(ApiResponseResultState.NotFound, LessonApiErrors.LessonNotFound));

                matchingTraining.LessonId = input.LessonId;
            }

            matchingTraining.Date = input.Date;
            matchingTraining.StartTime = input.StartTime;
            matchingTraining.EndTime = input.EndTime;
            matchingTraining.MasterName = input.MasterName;

            await this.trainingAccessLayer.UpdateAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating training trainingId={trainingId} with lessonId = {lessonId}", input.Id, input.LessonId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    private TrainingBase MapTrainingToTrainingBase(Training matchingTraining)
        => new TrainingBase()
        {
            Id = matchingTraining.Id,
            GroupId = matchingTraining.GroupId,
            LessonId = matchingTraining.LessonId,
            Date = matchingTraining.Date,
            StartTime = matchingTraining.StartTime,
            EndTime = matchingTraining.EndTime,
            LessonName = matchingTraining.Lesson?.Title ?? string.Empty,
            GroupName = matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false
                        ? GetGroupNameIfIndividualTraining(matchingTraining)
                        : matchingTraining.Group?.Title,
            Weapon = matchingTraining.Lesson?.Weapon ?? WeaponType.None,
            IsIndividual = matchingTraining.PersonTrainings.Count() == 1 && (matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false),
            MasterName = matchingTraining.MasterName ?? string.Empty,
        };

    private TrainingDetails MapTrainingToTrainingDetails(Training matchingTraining)
        => new TrainingDetails()
        {
            Id = matchingTraining.Id,
            GroupId = matchingTraining.GroupId,
            LessonId = matchingTraining.LessonId,
            Date = matchingTraining.Date,
            StartTime = matchingTraining.StartTime,
            EndTime = matchingTraining.EndTime,
            LessonName = matchingTraining.Lesson?.Title ?? string.Empty,
            GroupName = matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false
                        ? GetGroupNameIfIndividualTraining(matchingTraining)
                        : matchingTraining.Group?.Title,
            Weapon = matchingTraining.Lesson?.Weapon ?? WeaponType.None,
            IsIndividual = matchingTraining.PersonTrainings.Count() == 1 && (matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false),
            MasterName = matchingTraining.MasterName ?? string.Empty,
            GroupStudents = matchingTraining.Group?.StudentGroups.Select(sg => new TrainingStudentBase() { Id = sg.StudentId, FirstName = sg.Student?.FirstName ?? string.Empty, Name = sg.Student?.Name ?? string.Empty, Category = sg.Student?.Category ?? CategoryType.None }).ToList() ?? new List<TrainingStudentBase>(),
            TrainingStudents = matchingTraining.PersonTrainings.Select(pt => new TrainingStudentBase() { Id = pt.StudentId, FirstName = pt.Student?.FirstName ?? string.Empty, Name = pt.Student?.Name ?? string.Empty, Category = pt.Student?.Category ?? CategoryType.None }).ToList(),
            Lesson = new()
            {
                Id = matchingTraining.Lesson?.Id ?? 0,
                Title = matchingTraining.Lesson?.Title ?? string.Empty,
                Description = matchingTraining.Lesson?.Description ?? string.Empty,
                Content = matchingTraining.Lesson?.Content ?? string.Empty,
                Difficulty = matchingTraining.Lesson?.Difficulty ?? DifficultyType.None,
                Weapon = matchingTraining.Lesson?.Weapon ?? WeaponType.None,
            },
        };

    private static string GetGroupNameIfIndividualTraining(Training matchingTraining)
        => $"{matchingTraining.PersonTrainings.FirstOrDefault()?.Student?.FirstName ?? string.Empty} {matchingTraining.PersonTrainings.FirstOrDefault()?.Student?.Name ?? string.Empty}";
}
