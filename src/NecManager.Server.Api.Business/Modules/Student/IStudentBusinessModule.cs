namespace NecManager.Server.Api.Business.Modules.Student;
using System.Collections.Generic;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Student.Models;

public interface IStudentBusinessModule
{
    Task<ApiResponse<IEnumerable<StudentOutputBase>>> GetStudents(ServiceMonitoringDefinition monitoringIds);
}
