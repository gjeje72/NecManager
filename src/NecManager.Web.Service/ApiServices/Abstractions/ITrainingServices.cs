namespace NecManager.Web.Service.ApiServices.Abstractions;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Trainings;

public interface ITrainingServices
{
    Task<ServiceResult<PageableResult<TrainingBase>>> GetAllTrainingsAsync(TrainingInputQuery trainingQuery, CancellationToken cancellationToken = default);

    Task<ServiceResult<TrainingDetails>> GetTrainingByIdAsync(int trainingId, CancellationToken cancellationToken = default);

    Task<ServiceResult> CreateTrainingAsync(TrainingCreationInput creationInput, CancellationToken cancellationToken = default);

    Task<ServiceResult> CreateRangeTrainingsAsync(List<TrainingCreationInput> trainingCreationInputs, CancellationToken cancellationToken = default);

    Task<ServiceResult> AddStudentsInTrainingAsync(TrainingUpdateStudentInput updateInput, CancellationToken cancellationToken = default);

    Task<ServiceResult> UpdateTrainingAsync(TrainingUpdateInput updateInput, CancellationToken cancellationToken = default);

    Task<ServiceResult> DeleteTrainingAsync(int id, CancellationToken cancellationToken = default);
}
