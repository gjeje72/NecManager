namespace NecManager.Server.Api.Business.Modules.Lesson;

using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Lesson.Models;

public interface ILessonBusiness
{
    Task<ApiResponse<PageableResult<LessonBase>>> SearchAsync(ServiceMonitoringDefinition monitoringIds, LessonQueryInput query);

    Task<ApiResponse<LessonBase>> GetLessonByIdAsync(ServiceMonitoringDefinition monitoringIds, int lessonId);

    Task<ApiResponseEmpty> CreateLessonAsync(ServiceMonitoringDefinition monitoringIds, LessonCreationInput input);

    Task<ApiResponseEmpty> UpdateLessonAsync(ServiceMonitoringDefinition monitoringIds, int lessonId, LessonUpdateInput input);

    Task<ApiResponseEmpty> DeleteLessonAsync(ServiceMonitoringDefinition monitoringIds, int lessonId);
}
