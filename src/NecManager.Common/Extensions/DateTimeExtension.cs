namespace NecManager.Common.Extensions;
using System;

using NecManager.Common.DataEnum;

public static class DateTimeExtension
{
    public static CategoryType ToCategoryType(this DateTime birthdate)
    {
        DateTime dateDeReference = new DateTime(DateTime.Now.Month < 9 ? DateTime.Now.Year -1 : DateTime.Now.Year, 9, 1);
        int age = dateDeReference.Year - birthdate.Year;

        return (age, age) switch
        {
            ( >= 4, < 5) => CategoryType.Eveil,
            ( >= 5, <= 6) => CategoryType.M7,
            ( >= 7, <= 8) => CategoryType.M9,
            ( >= 9, <= 10) => CategoryType.M11,
            ( >= 11, <= 12) => CategoryType.M13,
            ( >= 13, <= 14) => CategoryType.M15,
            ( >= 15, <= 16) => CategoryType.M17,
            ( >= 17, <= 19) => CategoryType.M20,
            ( >= 20, <= 22) => CategoryType.M23,
            ( >= 23, <= 38) => CategoryType.Senior,
            ( >= 39, _) => CategoryType.Veteran,
            (_, _) => CategoryType.None
        };
    }
}
