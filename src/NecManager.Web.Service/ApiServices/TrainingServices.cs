namespace NecManager.Web.Service.ApiServices;

using NecManager.Common;
using System.Threading.Tasks;
using System.Threading;

using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Provider;
using NecManager.Web.Service.Models.Trainings;
using NecManager.Web.Service.Extensions;
using System.Collections.Generic;

internal sealed class TrainingServices : ServiceBase, ITrainingServices
{
    public TrainingServices(RestHttpService restHttpService)
        : base(restHttpService, "TRAINING_S001")
    {
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<PageableResult<TrainingBase>>> GetAllTrainingsAsync(TrainingInputQuery trainingQuery, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.GetAsync($"{trainingQuery.AsQueryParams}", cancellationToken).ConfigureAwait(false);

        return await response.BuildDataServiceResultAsync<PageableResult<TrainingBase>>().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<TrainingDetails>> GetTrainingByIdAsync(int trainingId,  CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.GetAsync($"{trainingId}", cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<TrainingDetails>().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> CreateTrainingAsync(TrainingCreationInput creationInput, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.PostAsync(string.Empty, creationInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> CreateRangeTrainingsAsync(List<TrainingCreationInput> trainingCreationInputs, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.PostAsync("multiple", trainingCreationInputs.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> UpdateTrainingAsync(TrainingUpdateInput updateInput, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.PutAsync($"{updateInput.Id}", updateInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> DeleteTrainingAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.DeleteAsync($"{id}", cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }
}
