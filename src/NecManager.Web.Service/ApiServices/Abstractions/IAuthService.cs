namespace NecManager.Web.Service.ApiServices.Abstractions;
using System.Threading.Tasks;

using NecManager.Common;


using NecManager.Web.Service.Models.AuthModule;

/// <summary>
///     Interface of for the authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///      Method that call backend Rest API to log in the user.
    /// </summary>
    /// <param name="login">login of the service</param>
    /// <returns>Security token as <seealso cref="TokenDto"/>></returns>
    Task<ServiceResult<TokenDto>> LoginAsync(UserLoginDetails login);
}
