namespace NecManager.Web.Service.ApiServices;

using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models.Lessons;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Provider;

internal sealed class LessonServices : ServiceBase, ILessonServices
{
    public LessonServices(RestHttpService restHttpService)
        : base(restHttpService, "LESSON_S001")
    {
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<PageableResult<LessonBase>>> GetAllLessonsAsync(LessonInputQuery lessonQuery, CancellationToken cancellationToken = default)
    {
        var lessonClient = await this.RestHttpService.LessonClient.ConfigureAwait(false);
        var response = await lessonClient.GetAsync($"{lessonQuery.AsQueryParams}", cancellationToken).ConfigureAwait(false);

        return await response.BuildDataServiceResultAsync<PageableResult<LessonBase>>().ConfigureAwait(false);
    }

    public async Task<ServiceResult> CreateLessonsAsync(LessonCreationInput lessonCreation, CancellationToken cancellationToken = default)
    {
        var lessonClient = await this.RestHttpService.LessonClient.ConfigureAwait(false);
        var response = await lessonClient.PostAsync(string.Empty, lessonCreation.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    public async Task<ServiceResult> UpdateLessonAsync(LessonUpdateInput lessonUpdate, CancellationToken cancellationToken = default)
    {
        var lessonClient = await this.RestHttpService.LessonClient.ConfigureAwait(false);
        var response = await lessonClient.PutAsync($"{lessonUpdate.Id}", lessonUpdate.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    public async Task<ServiceResult> DeleteLessonsByIdAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        var lessonClient = await this.RestHttpService.LessonClient.ConfigureAwait(false);
        var response = await lessonClient.DeleteAsync($"{lessonId}", cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }
}
