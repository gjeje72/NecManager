namespace NecManager.Web.Service.ApiServices;

using NecManager.Common;
using System.Threading.Tasks;
using System.Threading;

using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Provider;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models.Lessons;
using NecManager.Web.Service.Models;

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
}
