namespace NecManager.Web.Service.ApiServices.Abstractions;
using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Students;

public interface IStudentServices
{
    Task<ServiceResult> CreateStudentAsync(StudentCreationInput studentCreateInput, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Method use to get all existing student basic info.
    /// </summary>
    /// <returns>A list of <see cref="StudentBase" /> representing the basic student info.</returns>
    Task<ServiceResult<PageableResult<StudentBase>>> GetAllStudentsAsync(StudentInputQuery query, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdateStudentAsync(StudentUpdateInput studentUpdateInput, CancellationToken cancellationToken = default);
}
