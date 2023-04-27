namespace NecManager.Web.Service.ApiServices.Abstractions;

using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Trainings;

public interface ITrainingServices
{
    Task<ServiceResult> CreateTrainingAsync(TrainingCreationInput creationInput, CancellationToken cancellationToken = default);
    Task<ServiceResult<PageableResult<TrainingBase>>> GetAllTrainingsAsync(TrainingInputQuery trainingQuery, CancellationToken cancellationToken = default);
}
