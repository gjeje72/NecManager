namespace NecManager.Common.Extensions;
using System;
using System.Collections.Generic;

using NecManager.Common.DataEnum;

public static class StringExtensions
{
    public static string ToCategories(this string input)
    {
        var splittedInput = input.Split(',');
        var categories = new List<string>();
        foreach (var c in splittedInput)
        {
            var success = Enum.TryParse(c, out CategoryType enumValue);
            if (success)
                categories.Add(Enum.GetName(enumValue) ?? string.Empty);
        }

        return string.Join(", ", categories);
    }
}
