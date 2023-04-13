namespace NecManager.Web.Service.ApiServices;

using NecManager.Common;
using System.Threading.Tasks;
using System.Threading;

using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Provider;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Extensions;

internal sealed class LessonServices : ServiceBase, ILessonServices
{
    public LessonServices(RestHttpService restHttpService)
        : base(restHttpService, "LESSON_S001")
    {
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<PageableResult<LessonBase>>> GetAllLessonsAsync(LessonInputQuery orderQuery, CancellationToken cancellationToken = default)
    {
        var lessonClient = await this.RestHttpService.LessonClient.ConfigureAwait(false);
        var response = await lessonClient.GetAsync($"{orderQuery.AsQueryParams}", cancellationToken).ConfigureAwait(false);

        return await response.BuildDataServiceResultAsync<PageableResult<LessonBase>>().ConfigureAwait(false);
    }
}
