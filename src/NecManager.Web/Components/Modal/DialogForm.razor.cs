namespace NecManager.Web.Components.Modal;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Class which represent a dialog.
/// </summary>
public sealed partial class DialogForm : DialogTemplate
{
    /// <summary>
    ///     Gets or sets a value indicating whether the validation button is disabled.
    /// </summary>
    [Parameter]
    public bool IsValidatedButtonDisabled { get; set; } = false;

    /// <summary>
    ///     Gets or sets the validate button text.
    /// </summary>
    [Parameter]
    public string ValidateButtonText { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the form id.
    /// </summary>
    /// <remarks>
    ///     If not null, validation button is a submit button.
    /// </remarks>
    [Parameter]
    public string? FormId { get; set; }
}
