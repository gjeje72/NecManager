namespace NecManager.Web.Areas.Admin.Student;

using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Web.Areas.Admin.Student.ViewModel;

public partial class Student
{
    private bool creatingInProgress;

    private CreateUserViewModel CreateUserModel { get; set; } = new();

    private List<CreateUserViewModel> Users = new();
    private void OpenCreatingForm()
    {
        this.creatingInProgress = !this.creatingInProgress;
    }

    private async Task HandleSubmitAsync()
    {
        this.Users.Add(this.CreateUserModel);
        this.CreateUserModel = new();
        await this.InvokeAsync(() => this.StateHasChanged());
    }
}
