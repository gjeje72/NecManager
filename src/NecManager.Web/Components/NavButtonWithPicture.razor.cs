namespace NecManager.Web.Components;

using Microsoft.AspNetCore.Components;

public partial class NavButtonWithPicture
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string ImgUrl { get; set; } = string.Empty;

    [Parameter]
    public string Url { get; set; } = string.Empty;
}
