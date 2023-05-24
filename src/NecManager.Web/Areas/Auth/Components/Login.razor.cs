namespace NecManager.Web.Areas.Auth.Components;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Auth.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Identity;
using NecManager.Web.Service.Models.AuthModule;

using System.Threading.Tasks;


/// <summary>
///     Connection component logic.
/// </summary>
public partial class Login : ComponentBase
{
    /// <summary>
    ///     Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the authorization state provider.
    /// </summary>
    [Inject]
    private ICustomAuthentificationStateProvider AuthState { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the authorization service.
    /// </summary>
    [Inject]
    private IAuthService AuthService { get; set; } = null!;

    private LoginViewModel Model { get; set; } = new LoginViewModel();

    private bool ShowErrors { get; set; }

    private string Error { get; set; } = string.Empty;

    /// <summary>
    ///     Method to handle the login.
    /// </summary>
    private async Task HandleLogin()
    {
        this.ShowErrors = false;

        var loginDetails = new UserLoginDetails {
            Password = this.Model.Password,
            UserName = this.Model.Email
        };

        var (state, token, errorMessages) = await this.AuthService.LoginAsync(loginDetails).ConfigureAwait(false);

        if (state == ServiceResultState.Success && token is not null)
        {
            this.AuthState.SetUserLoggedIn(token.Token, token.ExpirationDate);
            this.Navigation.NavigateTo("/admin");
        }
        else
        {
            this.ShowErrors = true;
            this.Error = errorMessages[0];
        }
    }
}
