namespace NecManager.Web.Ressources.Keys;

using System.Diagnostics.CodeAnalysis;

/// <summary>
///     Contains all validation message keys.
/// </summary>
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
[SuppressMessage("Naming", "SA1600:", Justification = "Comment is not necessary due to explicit property naming")]
public class ValidationMessageKey
{
    public const string MandatoryLoginUsername = "MandatoryLoginUsername";
    public const string MandatoryLoginEmail = "MandatoryLoginEmail";
    public const string MandatoryLoginPassword = "MandatoryLoginPassword";
    public const string InvalidFormatPassword = "InvalidFormatPassword";
    public const string ConfirmationPasswordInvalid = "ConfirmationPasswordInvalid";
}
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
