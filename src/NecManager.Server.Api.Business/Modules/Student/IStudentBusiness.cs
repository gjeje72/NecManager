namespace NecManager.Server.Api.Business.Modules.Student;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Student.Models;

public interface IStudentBusiness
{
    Task<ApiResponse<PageableResult<StudentOutputBase>>> SearchStudents(ServiceMonitoringDefinition monitoringIds, StudentQueryInput query);
    Task<ApiResponse<StudentOutputBase>> GetStudentByIdAsync(ServiceMonitoringDefinition monitoringIds, int trainingId);
    Task<ApiResponseEmpty> CreateStudent(StudentCreationInput creationInput, ServiceMonitoringDefinition monitoringIds);
    Task<ApiResponseEmpty> UpdateStudentAsync(ServiceMonitoringDefinition monitoringIds, StudentUpdateInput input);
    Task<ApiResponseEmpty> DeleteStudentAsync(ServiceMonitoringDefinition monitoringIds, int studentId);
}
