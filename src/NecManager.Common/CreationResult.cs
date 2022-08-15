namespace NecManager.Common;
using System;

/// <summary>
///     Record which represent the result creation of a model.
/// </summary>
public record CreationResult(int ResourceId)
{
    /// <summary>
    ///     Gets or sets the created uri of resource.
    /// </summary>
    public Uri? CreatedUri { get; set; }

    /// <summary>
    /// Method to deconstruct <see cref="CreationResult"/>.
    /// </summary>
    /// <param name="resourceId">The resource id.</param>
    /// <param name="createdUri">The creation uri.</param>
    public void Deconstruct(out int resourceId, out Uri? createdUri)
    {
        createdUri = this.CreatedUri;
        resourceId = this.ResourceId;
    }
}
