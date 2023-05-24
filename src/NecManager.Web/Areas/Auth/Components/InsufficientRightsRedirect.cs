namespace NecManager.Web.Areas.Auth.Components;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Component to force the redirection to insufficient rights notification page when a user misses authrorization.
/// </summary>
public class InsufficientRightsRedirect : ComponentBase
{
    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        this.NavigationManager.NavigateTo($"/error/forbidden");
        await base.OnInitializedAsync().ConfigureAwait(false);
    }
}
