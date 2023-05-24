namespace NecManager.Web.Components;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Component to force the redirection to LCS authenticated user's home page, instead of root page.
/// </summary>
public class UserHomePageRedirect : ComponentBase
{
    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        this.NavigationManager.NavigateTo("/admin");
        await base.OnInitializedAsync().ConfigureAwait(false);
    }
}
