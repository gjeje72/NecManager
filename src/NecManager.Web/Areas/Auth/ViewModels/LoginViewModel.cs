namespace NecManager.Web.Areas.Auth.ViewModels;

using System.ComponentModel.DataAnnotations;

using NecManager.Web.Ressources;
using NecManager.Web.Ressources.Keys;

/// <summary>
///     Model use to log in.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    ///     Email of the LoginViewModel.
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(ValidationMessage), ErrorMessageResourceName = ValidationMessageKey.MandatoryLoginUsername)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Password of the LoginViewModel.
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(ValidationMessage), ErrorMessageResourceName = ValidationMessageKey.MandatoryLoginPassword)]
    public string Password { get; set; } = string.Empty;
}
