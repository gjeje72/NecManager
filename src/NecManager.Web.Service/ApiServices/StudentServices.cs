﻿namespace NecManager.Web.Service.ApiServices;
using System.Threading;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Models.Query;
using NecManager.Web.Service.Models.Students;
using NecManager.Web.Service.Provider;

internal sealed class StudentServices : ServiceBase, IStudentServices
{
    public StudentServices(RestHttpService restHttpService)
        : base(restHttpService, "STUDENT_S001")
    {
    }

    public async Task<ServiceResult<PageableResult<StudentBase>>> GetAllStudentsAsync(StudentInputQuery query, CancellationToken cancellationToken = default)
    {
        var studentClient = await this.RestHttpService.StudentClient;
        var response = await studentClient.GetAsync($"{query.AsQueryParams}", cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<PageableResult<StudentBase>>().ConfigureAwait(false);
    }

    public async Task<ServiceResult> CreateStudentAsync(StudentCreationInput studentCreateInput, CancellationToken cancellationToken = default)
    {
        var studentClient = await this.RestHttpService.StudentClient;
        var response = await studentClient.PostAsync(string.Empty, studentCreateInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    public async Task<ServiceResult> UpdateStudentAsync(StudentUpdateInput studentUpdateInput, CancellationToken cancellationToken = default)
    {
        var studentClient = await this.RestHttpService.StudentClient;
        var response = await studentClient.PutAsync($"{studentUpdateInput.Id}", studentUpdateInput.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }

    public async Task<ServiceResult> DeleteStudentAsync(string studentId, CancellationToken cancellationToken = default)
    {
        var studentClient = await this.RestHttpService.StudentClient;
        var response = await studentClient.DeleteAsync($"{studentId}", cancellationToken).ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync().ConfigureAwait(false);
    }
}
