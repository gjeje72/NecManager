namespace NecManager.Server.Api.Business.Modules.Training;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Training.Models;

public interface ITrainingBusiness
{
    Task<ApiResponse<PageableResult<TrainingBase>>> SearchAsync(ServiceMonitoringDefinition monitoringIds, TrainingQueryInput query);
}
