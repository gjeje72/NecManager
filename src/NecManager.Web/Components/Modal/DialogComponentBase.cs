namespace NecManager.Web.Components.Modal;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Class which represent a base component for modal.
/// </summary>
public class DialogComponentBase : ComponentBase
{
    /// <summary>
    ///     Gets or sets a value indicating whether the dialog is displayed.
    /// </summary>
    public bool ShowModal { get; set; }

    /// <summary>
    ///     Gets or sets the dialog title.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the content of the modal.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Gets or sets the content of the modal.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    ///     Gets or sets the cancel button text.
    /// </summary>
    [Parameter]
    public string CancelButtonText { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the event to raised on validation.
    /// </summary>
    [Parameter]
    public EventCallback OnValidate { get; set; }

    /// <summary>
    ///     Gets or sets the event to raised on cancellation.
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    /// <summary>
    ///     Gets or sets the method call when validation button is clicked.
    /// </summary>
    public Action? ValidateAction { get; set; }

    /// <summary>
    ///     Gets or sets the method call when cancel button is clicked.
    /// </summary>
    public Action? CancelAction { get; set; }

    /// <summary>
    ///     Gets or sets the method call when validation button is clicked.
    /// </summary>
    public Func<Task>? ValidateActionAsync { get; set; }

    /// <summary>
    ///     Gets or sets the method call when cancel button is clicked.
    /// </summary>
    public Func<Task>? CancelActionAsync { get; set; }

    /// <summary>
    ///     Method which raised on show event and which show the dialog.
    /// </summary>
    public void ShowDialog()
    {
        this.ShowModal = true;

        this.OnValidate = new(this, () =>
        {
            this.ValidateAction?.Invoke();

            this.CloseDialog();
        });

        this.OnCancel = new(this, () =>
        {
            this.CancelAction?.Invoke();

            this.CloseDialog();
        });

        this.StateHasChanged();
    }

    /// <summary>
    ///     Hide the dialog.
    /// </summary>
    public void CloseDialog() => this.ShowModal = false;

    /// <summary>
    ///     Method which raised on show event and which show the dialog.
    /// </summary>
    protected async void ShowDialogAsync()
    {
        this.ShowModal = true;

        this.OnValidate = new(this, async () =>
        {
            if (this.ValidateActionAsync is not null)
            {
                await this.ValidateActionAsync.Invoke();
            }

            this.CloseDialog();
        });

        this.OnCancel = new(this, async () =>
        {
            if (this.CancelActionAsync is not null)
            {
                await this.CancelActionAsync.Invoke();
            }

            this.CloseDialog();
        });

        await this.InvokeAsync(this.StateHasChanged);
    }
}
