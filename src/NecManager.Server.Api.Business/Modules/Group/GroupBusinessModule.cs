namespace NecManager.Server.Api.Business.Modules.Group;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;
using NecManager.Server.Api.Business.Modules.Group.Models;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

internal sealed class GroupBusinessModule : IGroupBusinessModule
{
    private readonly IGroupAccessLayer groupAccessLayer;
    private readonly IStudentAccessLayer studentAccessLayer;

    public GroupBusinessModule(IGroupAccessLayer groupAccessLayer, IStudentAccessLayer studentAccessLayer)
    {
        this.groupAccessLayer = groupAccessLayer;
        this.studentAccessLayer = studentAccessLayer;
    }

    /// <inheritdoc />
    public ApiResponse<List<GroupOutputBase>> GetAllGroups(ServiceMonitoringDefinition monitoringIds)
    {
        try
        {
            var groups = this.groupAccessLayer.GetCollection(null, false, x => x.Include(g => g.StudentGroups!).ThenInclude(sg => sg.Student!));

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
            var group = await this.groupAccessLayer.GetSingleAsync(x => x.Id == groupId, false, x => x.Include(g => g.StudentGroups!).ThenInclude(sg => sg.Student!));

            if (group is null)
                return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, $"Error occurs while trying to retrieve a group. GroupId : {groupId}")));

            var groupOutputDetails = this.ConvertGroupToGroupOutputDetails(group);
            return new(monitoringIds, new(ApiResponseResultState.Success, groupOutputDetails));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.GetError, $"Error occurs while trying to retrieve a group. GroupId : {groupId} / Error : {ex.Message}")));
        }
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
            StudentCount = group.StudentGroups?.Where(sg => sg.GroupId == group.Id).Count() ?? 0
        };
    }

    private GroupOutputDetails ConvertGroupToGroupOutputDetails(Group group)
    {
        var categories = this.SetCategories(group);

        return new GroupOutputDetails()
        {
            Id = group.Id,
            Title = group.Title,
            Weapon = group.Weapon,
            CategoriesIds = categories,
            Students = group.StudentGroups?.Where(sg => sg.GroupId == group.Id).Select(
                sg => sg.Student).Select(student => new StudentOutputBase
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.Name,
                    Categorie = student.Category,
                    Arme = group.Weapon,
                    GroupName = group.Title ?? string.Empty,
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

    /// <inheritdoc />
    public async Task<ApiResponse<GroupOutputBase>> CreateGroupAsync(ServiceMonitoringDefinition monitoringIds, CreateGroupInput groupInput)
    {
        try
        {
            var groupCreatedId = await this.groupAccessLayer.AddAsync(
                 new Group
                 {
                     Title = groupInput.Title,
                     Weapon = groupInput.Weapon,
                     Categories = groupInput.Categories is null ? null : string.Join(",", groupInput.Categories)
                 });

            if (groupInput.Students != null)
            {
                foreach (var student in groupInput.Students)
                {
                    var studentFound = await this.studentAccessLayer.GetSingleAsync(x => x.Id == student.Id, true);
                    if (studentFound != null)
                    {
                        studentFound.StudentGroups.Add(new StudentGroup() { StudentId = studentFound.Id, GroupId = groupCreatedId });
                        await this.studentAccessLayer.UpdateAsync(studentFound);
                    }
                }
            }

            return new(monitoringIds, new(ApiResponseResultState.Success, new GroupOutputBase { Id = groupCreatedId, Title = groupInput.Title }));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.CreateError, $"Error occurs during group creation. {ex.Message}")));
        }
    }
}
