namespace NecManager.Common.Extensions;
using System;
using System.Globalization;

public static class DecimalExtension
{
    /// <summary>
    ///     Method to transform decimal into a time string.
    /// </summary>
    /// <param name="input">the decimal value to manage.</param>
    /// <returns>Returns a formated string.</returns>
    public static string ToTimeString(this decimal input)
    {
        TimeSpan timespan = TimeSpan.FromHours((double)input);
        return timespan.ToString("h\\:mm", CultureInfo.InvariantCulture);
    }
}
