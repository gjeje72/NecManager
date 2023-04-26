namespace NecManager.Web.Components.Modal;

using System;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Class which represent a dialog.
/// </summary>
public sealed partial class Dialog : DialogTemplate
{
    /// <summary>
    ///     Gets or sets a value indicating whether the validation button is disabled.
    /// </summary>
    [Parameter]
    public bool IsValidatedButtonDisabled { get; set; }

    /// <summary>
    ///     Gets or sets the validate button text.
    /// </summary>
    [Parameter]
    public string ValidateButtonText { get; set; } = string.Empty;

    /// <summary>
    ///     Method which raised on show event and which show the dialog.
    /// </summary>
    /// <param name="titleDialog">Title of dialog.</param>
    /// <param name="content">Content of dialog.</param>
    /// <param name="actionYesButton"><see cref="Action" /> on yes button click.</param>
    /// <param name="actionNoButton"><see cref="Action" /> on no button click.</param>
    public async void ShowDialog(string titleDialog, string content, Action actionYesButton, Action? actionNoButton = null)
    {
        this.Title = titleDialog;
        this.ChildContent = builder => builder.AddContent(0, content);
        this.CancelAction = actionNoButton ?? this.CloseDialog;
        this.ValidateAction = actionYesButton;

        this.ShowDialog();

        await this.InvokeAsync(this.StateHasChanged);
    }
}
