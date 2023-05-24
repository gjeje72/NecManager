namespace NecManager.Web.Areas.Admin.Settings.Group.ViewModels;

using System;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum;
using NecManager.Common.Extensions;

public sealed class GroupStudentViewModel
{
    public string Id { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public DateTime Birthdate { get; set; } = DateTime.MinValue;

    public CategoryType Category { get; set; }

    public CategoryType NextCategory => this.Birthdate.ToNextCategoryType();

    public string ShowCategoryWithNext => this.NextCategory == this.Category ? $"{this.Category}" : $"{this.Category}↗️{this.NextCategory}";

    public WeaponType Weapon { get; set; }

    public bool IsUnsavedMove { get; set; } = false;
}
