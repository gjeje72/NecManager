namespace NecManager.Server.Api.Business.Modules.Training;

using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Training.Models;

public interface ITrainingBusiness
{
    Task<ApiResponseEmpty> AddStudentsInTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingUpdateStudentInput input);
    Task<ApiResponseEmpty> CreateRangeTrainingAsync(ServiceMonitoringDefinition monitoringIds, List<TrainingCreationInput> inputs);
    Task<ApiResponseEmpty> CreateTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingCreationInput input);
    Task<ApiResponseEmpty> DeleteTrainingAsync(ServiceMonitoringDefinition monitoringIds, int trainingId);
    Task<ApiResponse<TrainingBase>> GetTrainingByIdAsync(ServiceMonitoringDefinition monitoringIds, int trainingId);
    Task<ApiResponse<PageableResult<TrainingBase>>> SearchAsync(ServiceMonitoringDefinition monitoringIds, TrainingQueryInput query);
    Task<ApiResponseEmpty> UpdateTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingUpdateInput input);
}
