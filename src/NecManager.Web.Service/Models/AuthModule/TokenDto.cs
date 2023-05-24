namespace NecManager.Web.Service.Models.AuthModule;
using System;

public class TokenDto
{
    /// <summary>
    ///     Gets or sets the token of the tokenDto
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Gets or sets the expiration date
    /// </summary>
    public DateTime ExpirationDate { get; set; }
}
