namespace NecManager.Server.Api.Business.Modules.Identity.ViewModel;

public class LoginIn
{
    /// <summary>
    ///     Gets or sets the login of the login viewModel.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the password of the login viewModel.
    /// </summary>
    public string Password { get; set; } = null!;
}
