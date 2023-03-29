namespace NecManager.Server.Api.Business.Modules.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Student.Models;

public interface IStudentBusinessModule
{
    Task<ApiResponseEmpty> CreateStudent(StudentCreationInput creationInput, ServiceMonitoringDefinition monitoringIds);

    ApiResponse<IEnumerable<StudentOutputBase>> GetStudents(ServiceMonitoringDefinition monitoringIds);
}
