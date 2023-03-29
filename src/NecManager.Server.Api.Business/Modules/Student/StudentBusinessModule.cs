namespace NecManager.Server.Api.Business.Modules.Student;

using Microsoft.EntityFrameworkCore;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

internal sealed class StudentBusinessModule : IStudentBusinessModule
{
    private readonly IStudentAccessLayer studentAccessLayer;

    public StudentBusinessModule(IStudentAccessLayer studentAccessLayer)
    {
        this.studentAccessLayer = studentAccessLayer;
    }

    public ApiResponse<IEnumerable<StudentOutputBase>> GetStudents(ServiceMonitoringDefinition monitoringIds)
    {
        var student = this.studentAccessLayer.GetCollection(navigationProperties: x => x.Include(student => student.StudentGroups!).ThenInclude(sg => sg.Group!)).ToList();
        return new(monitoringIds, new(student.Select(
                student => new StudentOutputBase
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.Name,
                    Categorie = student.Category,
                })));
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateStudent(StudentCreationInput creationInput, ServiceMonitoringDefinition monitoringIds)
    {
        var studentToCreate = new Student
        {
            FirstName = creationInput.FirstName,
            Name = creationInput.Name,
            EmailAddress = creationInput.EmailAddress,
            Category = creationInput.Category,
            PhoneNumber = creationInput.PhoneNumber,
            IsMaster = creationInput.IsMaster,
        };
        if (creationInput.GroupIds.Any())
        {
            var studentGroups = creationInput.GroupIds.Select(g => new StudentGroup { GroupId = g }).ToList();
            studentGroups.ForEach(sg => studentToCreate.StudentGroups.Add(sg));
        }

        var result = await this.studentAccessLayer.AddAsync(studentToCreate).ConfigureAwait(false);
        return new(monitoringIds, new());
    }
}
