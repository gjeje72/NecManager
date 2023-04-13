namespace NecManager.Web.Areas.Admin.Settings.Lessons;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;

public partial class SettingsLessons : ComponentBase
{
    private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    private GridItemsProvider<LessonBaseViewModel> lessonProviders = default!;
    public IQueryable<LessonBaseViewModel> Lessons { get; set; } = new List<LessonBaseViewModel>().AsQueryable();

    [Inject]
    private IMapper Mapper { get; set; } = null!;

    [Inject]
    public ILessonServices LessonServices { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        this.lessonProviders = async req => await this.GetLessonsProviderAsync(req).ConfigureAwait(true);

        return base.OnInitializedAsync();
    }

    private async Task<GridItemsProviderResult<LessonBaseViewModel>> GetLessonsProviderAsync(GridItemsProviderRequest<LessonBaseViewModel> gridItemsProviderRequest)
    {
        var query = new LessonInputQuery
        {
            CurrentPage = (gridItemsProviderRequest.StartIndex / gridItemsProviderRequest.Count ?? 1) + 1,
            PageSize = this.pagination.ItemsPerPage,
        };

        var (_, lessons, _) = await this.LessonServices.GetAllLessonsAsync(query).ConfigureAwait(true);
        var pageableLessons = new PageableResult<LessonBaseViewModel>
        {
            Items = lessons?.Items?.Select(l =>
                new LessonBaseViewModel
                {
                    DifficultyType = l.Difficulty,
                    Title = l.Title,
                    Weapon = l.Weapon
                }).ToList() ?? new(),
            TotalElements = lessons?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableLessons.Items.ToList(), pageableLessons.TotalElements);
    }
}
