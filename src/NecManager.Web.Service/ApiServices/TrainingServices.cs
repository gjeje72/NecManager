namespace NecManager.Web.Service.ApiServices;

using NecManager.Common;
using System.Threading.Tasks;
using System.Threading;

using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Provider;
using NecManager.Web.Service.Models.Trainings;
using NecManager.Web.Service.Extensions;

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
    public async Task<ServiceResult> CreateTrainingAsync(TrainingCreationInput creationInput, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.PostAsync(string.Empty, creationInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    public async Task<ServiceResult> UpdateTrainingAsync(TrainingUpdateInput updateInput, CancellationToken cancellationToken = default)
    {
        var trainingClient = await this.RestHttpService.TrainingClient.ConfigureAwait(false);
        var response = await trainingClient.PutAsync($"{updateInput.Id}", updateInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }
}
