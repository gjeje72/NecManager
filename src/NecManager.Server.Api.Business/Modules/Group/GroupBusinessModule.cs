namespace NecManager.Server.Api.Business.Modules.Group;
using NecManager.Common;
using NecManager.Common.DataEnum.Internal;
using NecManager.Server.Api.Business.Modules.Group.Models;
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
    public async Task<ApiResponse<GroupOutputBase>> CreateGroupAsync(ServiceMonitoringDefinition monitoringIds, GroupInput groupInput)
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
                        studentFound.GroupId = groupCreatedId;
                        await this.studentAccessLayer.UpdateAsync(studentFound);
                    }
                }
            }

            return new(monitoringIds, new(new GroupOutputBase { Id = groupCreatedId, Title = groupInput.Title }));
        }
        catch (Exception ex)
        {
            return new(monitoringIds, new(new ApiResponseError(RestServiceErrorCode.CreateError, $"Error occurs during group creation. {ex.Message}")));
        }
    }
}
