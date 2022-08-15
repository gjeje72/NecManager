namespace NecManager.Web.Service.ApiServices;
using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models;
using NecManager.Web.Service.Provider;

internal sealed class StudentServices : ServiceBase, IStudentServices
{
    public StudentServices(RestHttpService restHttpService)
        : base(restHttpService, "STUDENT_S001")
    {
    }

    public async Task<ServiceResult<List<StudentBase>>> GetAllStudentsAsync()
    {
        var studentClient = await this.RestHttpService.StudentClient;
        var response = await studentClient.GetAsync("").ConfigureAwait(false);
        return await response.BuildDataServiceResultAsync<List<StudentBase>>().ConfigureAwait(false);
    }

}
