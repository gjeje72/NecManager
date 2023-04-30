namespace NecManager.Web.Areas.Admin.Student;

using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Components;

using NecManager.Common.DataEnum.Internal;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Service.ApiServices.Abstractions;

public partial class Student : ComponentBase
{
    private bool creatingInProgress;
    private bool IsLoading;

    private CreateUserViewModel CreateUserModel { get; set; } = new();

    private List<CreateUserViewModel> Students = new();

    [Inject]
    private IMapper Mapper { get; set; } = null!;

    [Inject]
    private IStudentServices StudentServices { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        this.IsLoading = true;

        await this.GetStudentsAsync();

        this.IsLoading = false;
        await this.InvokeAsync(this.StateHasChanged);
    }

    private async Task GetStudentsAsync()
    {
        var (studentSuccess, studentsFound, _) = await this.StudentServices.GetAllStudentsAsync(new());
        if (studentSuccess == ServiceResultState.Success)
        {
            this.Students = this.Mapper.Map<List<CreateUserViewModel>>(studentsFound);
        }
    }

    private void OpenCreatingForm()
    {
        this.creatingInProgress = !this.creatingInProgress;
    }

    private async Task HandleSubmitAsync()
    {
        this.Students.Add(this.CreateUserModel);
        this.CreateUserModel = new();
        await this.InvokeAsync(() => this.StateHasChanged());
    }
}
