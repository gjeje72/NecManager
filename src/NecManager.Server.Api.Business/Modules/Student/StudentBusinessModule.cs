namespace NecManager.Server.Api.Business.Modules.Student;

using Microsoft.EntityFrameworkCore;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;

internal sealed class StudentBusinessModule : IStudentBusinessModule
{
    private readonly IStudentAccessLayer studentAccessLayer;

    public StudentBusinessModule(IStudentAccessLayer studentAccessLayer)
    {
        this.studentAccessLayer = studentAccessLayer;
    }

    public async Task<ApiResponse<IEnumerable<StudentOutputBase>>> GetStudents(ServiceMonitoringDefinition monitoringIds)
        => new(monitoringIds, new (await this.studentAccessLayer.GetCollection(
            student => new StudentOutputBase
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.Name,
                Categorie = student.Categorie,
                Arme = student.Arme
            }).ToListAsync()));
}
