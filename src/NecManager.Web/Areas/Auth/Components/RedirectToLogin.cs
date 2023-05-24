namespace NecManager.Web.Areas.Auth.Components;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Component to force the redirection to signin page when a user is not authorized.
/// </summary>
public class RedirectToLogin : ComponentBase
{
    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        this.NavigationManager.NavigateTo("/");
        await base.OnInitializedAsync().ConfigureAwait(false);
    }
}
