namespace NecManager.Web.Components.Modal;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

/// <summary>
///     Logic of the dialog selector component.
/// </summary>
/// <typeparam name="TViewModel">Type of object manage by this component.</typeparam>
public partial class DialogSelector<TViewModel> where TViewModel : ISelectableItem
{
    /// <summary>
    ///     Gets or sets a value indicating whether the dialog is displayed.
    /// </summary>
    private bool ShowModal { get; set; }

    /// <summary>
    ///     Gets or sets the current selected items from <seealso cref="Items"/> collection.
    /// </summary>
    private List<TViewModel> SelectedItems => this.Items.Where(i => i.IsSelected).ToList();

    /// <summary>
    ///     Gets or sets the method call when validation button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<List<TViewModel>> ValidateAction { get; set; }

    /// <summary>
    ///     Gets or sets the method to call when the search text is changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> SearchValueChanged { get; set; }

    /// <summary>
    ///     Gets or sets the cancel button text.
    /// </summary>
    [Parameter]
    public string CancelButtonText { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the validate button text.
    /// </summary>
    [Parameter]
    public string ValidateButtonText { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the dialog title.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the dialog description.
    /// </summary>
    [Parameter]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the collection manage by this component.
    /// </summary>
    [Parameter]
    public List<TViewModel> Items { get; set; } = new();

    /// <summary>
    ///     Gets or sets the template used to display <seealso cref="Items"/>.
    /// </summary>
    [Parameter]
    public RenderFragment<TViewModel> ItemTemplate { get; set; }

    /// <summary>
    ///     Invokes the <seealso cref="ValidateAction"/> and close the dialog.
    /// </summary>
    /// <remarks>
    ///     Handle when the validate button is clicked.
    /// </remarks>
    private async Task OnValidateActionClicked()
    {
        await this.ValidateAction.InvokeAsync(this.SelectedItems).ConfigureAwait(false);
        this.CloseDialog();
    }

    /// <summary>
    ///     Invokes the <seealso cref="SearchValueChanged"./>
    /// </summary>
    /// <param name="filter">new filter value.</param>
    private async Task OnSearchValueChanged(string filter) => await this.SearchValueChanged.InvokeAsync(filter).ConfigureAwait(false);

    /// <summary>
    ///     Display the dialog.
    /// </summary>
    public void ShowDialog()
    {
        this.ShowModal = true;
        this.OnSearchValueChanged(string.Empty).ConfigureAwait(false);
    }

    /// <summary>
    ///     Hide the dialog.
    /// </summary>
    public void CloseDialog() => this.ShowModal = false;
}
