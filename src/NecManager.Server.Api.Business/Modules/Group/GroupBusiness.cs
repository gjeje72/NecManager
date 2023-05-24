namespace NecManager.Server.Api.Business.Modules.Group;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;
using NecManager.Common.Extensions;
using NecManager.Server.Api.Business.Modules.Group.Models;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

using static NecManager.Common.ApiResponseError;

internal sealed class GroupBusiness : IGroupBusiness
{
    private readonly ILogger<IGroupBusiness> logger;
    private readonly IGroupAccessLayer groupAccessLayer;
    private readonly IStudentAccessLayer studentAccessLayer;

    public GroupBusiness(ILogger<IGroupBusiness> logger, IGroupAccessLayer groupAccessLayer, IStudentAccessLayer studentAccessLayer)
    {
        this.logger = logger;
        this.groupAccessLayer = groupAccessLayer;
        this.studentAccessLayer = studentAccessLayer;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<GroupOutputBase>>> GetAllGroupsAsync(ServiceMonitoringDefinition monitoringIds)
    {
        try
        {
            var groups = await this.groupAccessLayer.GetCollection(null, false, x => x.Include(g => g.StudentGroups!).ThenInclude(sg => sg.Student!)).ToListAsync();

            if (groups is null)
                return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, "Error occurs while trying to retrieve all groups.")));

            var groupsToReturn = new List<GroupOutputBase>();

            foreach (var group in groups)
            {
                var groupOutputBase = this.ConvertGroupToGroupOutputBase(group);

                if (groupOutputBase != null)
                    groupsToReturn.Add(groupOutputBase);
            }
            return new(monitoringIds, new(ApiResponseResultState.Success, groupsToReturn));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, $"Error occurs while trying to retrieve all groups. Error : {ex.Message}")));
        }
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GroupOutputDetails>> GetGroupByIdAsync(ServiceMonitoringDefinition monitoringIds, int groupId)
    {
        try
        {
            var group = await this.groupAccessLayer.GetSingleAsync(
                x => x.Id == groupId,
                false,
                x => x.Include(g => g.StudentGroups!)
                                        .ThenInclude(sg => sg.Student!)
                                    .Include(g => g.Trainings!)
                                        .ThenInclude(t => t.Lesson!));

            if (group is null)
                return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, $"Error occurs while trying to retrieve a group. GroupId : {groupId}")));

            var groupOutputDetails = this.MapGroupToGroupOutputDetails(group);
            return new(monitoringIds, new(ApiResponseResultState.Success, groupOutputDetails));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, $"Error occurs while trying to retrieve a group. GroupId : {groupId} / Error : {ex.Message}")));
        }
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GroupOutputBase>> CreateGroupAsync(ServiceMonitoringDefinition monitoringIds, CreateGroupInput groupInput)
    {
        var dbGroup = new Group
        {
            Title = groupInput.Title,
            Weapon = groupInput.Weapon,
            Categories = groupInput.Categories is null ? null : string.Join(",", groupInput.Categories),
        };

        if (!await this.studentAccessLayer.ExistsRangeAsync(groupInput.Students.Select(s => s.Id)))
            return new(monitoringIds, new(ApiResponseResultState.NotFound, StudentApiErrors.StudentNotFound));

        foreach (var student in groupInput.Students)
        {
            dbGroup.StudentGroups.Add(new() { StudentId = student.Id, IsResponsiveMaster = student.Id == groupInput.MasterId });
        }

        try
        {
            var groupCreatedId = await this.groupAccessLayer.AddAsync(dbGroup).ConfigureAwait(false);
            return new(monitoringIds, new(ApiResponseResultState.Success, new GroupOutputBase { Id = groupCreatedId, Title = groupInput.Title }));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.CreateError, $"Error occurs during group creation. {ex.Message}")));
        }
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> UpdateGroupAsync(ServiceMonitoringDefinition monitoringIds, GroupUpdateInput input)
    {
        // ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingGroup = await this.groupAccessLayer.GetSingleAsync(x => x.Id == input.GroupId, true,x => x.Include(g => g.StudentGroups)).ConfigureAwait(false);
        if (matchingGroup is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, GroupApiErrors.GroupNotFound));

        try
        {
            matchingGroup.StudentGroups.Clear();
            if (input.StudentsIds.Count > 0)
            {
                if (!await this.studentAccessLayer.ExistsRangeAsync(input.StudentsIds))
                    return new(monitoringIds, new(ApiResponseResultState.NotFound, StudentApiErrors.StudentNotFound));

                foreach (var studentId in input.StudentsIds.Distinct())
                {
                    matchingGroup.StudentGroups.Add(new() { StudentId = studentId, IsResponsiveMaster = studentId == input.MasterId });
                }
            }

            matchingGroup.Title = input.Title;
            matchingGroup.Weapon = input.Weapon;
            matchingGroup.Categories = input.Categories is null ? null : string.Join(",", input.Categories);

            await this.groupAccessLayer.UpdateAsync(matchingGroup).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating group Id={groupId}", input.GroupId);
            return new(monitoringIds, new(GroupApiErrors.GroupUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> DeleteGroupAsync(ServiceMonitoringDefinition monitoringIds, int groupId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingGroup = await this.groupAccessLayer.GetSingleAsync(x => x.Id == groupId, true).ConfigureAwait(false);
        if (matchingGroup is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, GroupApiErrors.GroupNotFound));

        try
        {
            await this.groupAccessLayer.RemoveAsync(matchingGroup).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while trying to delete group Id={groupId}", groupId);
            return new(monitoringIds, new(GroupApiErrors.GroupDeletionFailure));
        }

        return new(monitoringIds, new());
    }


    private GroupOutputBase ConvertGroupToGroupOutputBase(Group group)
    {
        var categories = this.SetCategories(group);

        return new GroupOutputBase()
        {
            Id = group.Id,
            Title = group.Title,
            Weapon = group.Weapon,
            CategoriesIds = categories,
            StudentCount = group.StudentGroups?.Count() ?? 0
        };
    }

    private GroupOutputDetails MapGroupToGroupOutputDetails(Group group)
    {
        var categories = this.SetCategories(group);

        return new GroupOutputDetails()
        {
            Id = group.Id,
            Title = group.Title,
            Weapon = group.Weapon,
            CategoriesIds = categories,
            Students = group.StudentGroups?.Select(sg => sg.Student).Select(student =>
                student != null
                ? new StudentOutputBase
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.Name,
                    Categorie = student.BirthDate.ToCategoryType(),
                    Weapon = group.Weapon,
                    GroupName = group.Title ?? string.Empty,
                }
                : new()).ToList(),
            Trainings = group.Trainings.Select(t =>
            new TrainingDetails
            {
                Id = t.Id,
                Date = t.Date,
                Lesson = t.Lesson != null
                ? new()
                    {
                        Id = t.LessonId,
                        Difficulty = t.Lesson.Difficulty,
                        Content = t.Lesson.Content,
                        Title = t.Lesson.Title,
                        Description = t.Lesson.Description,
                        Weapon = t.Lesson.Weapon
                    }
                : new()
            }).ToList()
        };
    }

    private List<int> SetCategories(Group group)
    {
        var categories = new List<int>();
        var inputCategories = group.Categories;
        var cats = inputCategories?.Split(',').ToList() ?? new List<string>();

        foreach (var cat in cats)
        {
            var success = int.TryParse(cat, out int result);
            if (success)
                categories.Add(result);
        }

        return categories;
    }
}
