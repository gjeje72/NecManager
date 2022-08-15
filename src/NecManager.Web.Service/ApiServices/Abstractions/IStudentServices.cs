namespace NecManager.Web.Service.ApiServices.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Web.Service.Models;

public interface IStudentServices
{
    /// <summary>
    ///     Method use to get all existing student basic info.
    /// </summary>
    /// <returns>A list of <see cref="StudentBase" /> representing the basic student info.</returns>
    Task<ServiceResult<List<StudentBase>>> GetAllStudentsAsync();
}
