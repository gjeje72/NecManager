namespace NecManager.Web.Service.ApiServices.Abstractions;

using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Query;

public interface ILessonServices
{
    Task<ServiceResult<PageableResult<LessonBase>>> GetAllLessonsAsync(LessonInputQuery orderQuery, CancellationToken cancellationToken = default);
}
