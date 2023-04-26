namespace NecManager.Web.Areas.Admin.Settings.Trainings;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using NecManager.Common;
using NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;
using NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Models.Query;

public partial class SettingsTrainings
{
    [Inject]
    public ITrainingServices TrainingServices { get; set; } = null!;
    private PaginationState pagination = new() { ItemsPerPage = 10 };
    private GridItemsProvider<TrainingBaseViewModel> trainingProviders = default!;
    private QuickGrid<TrainingBaseViewModel> trainingsGrid;

    protected override async Task OnInitializedAsync()
    {
        this.trainingProviders = async req => await this.GetTrainingsProviderAsync(req).ConfigureAwait(true);
    }

    private async Task<GridItemsProviderResult<TrainingBaseViewModel>> GetTrainingsProviderAsync(GridItemsProviderRequest<TrainingBaseViewModel> gridItemsProviderRequest)
    {
        var query = new TrainingInputQuery
        {
            CurrentPage = (gridItemsProviderRequest.StartIndex / gridItemsProviderRequest.Count ?? 1) + 1,
            PageSize = this.pagination.ItemsPerPage,
        };

        var (_, trainings, _) = await this.TrainingServices.GetAllTrainingsAsync(query).ConfigureAwait(true);
        var pageableTrainings = new PageableResult<TrainingBaseViewModel>
        {
            Items = trainings?.Items?.Select(t =>
                new TrainingBaseViewModel
                {
                    Id = t.Id,
                    IsIndividual = t.IsIndividual,
                    Categories = t.Categories,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Weapon = t.Weapon
                }).ToList() ?? new(),
            TotalElements = trainings?.TotalElements ?? 0
        };
        return GridItemsProviderResult.From(pageableTrainings.Items.ToList(), pageableTrainings.TotalElements);
    }
}
