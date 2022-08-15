namespace NecManager.Web.Areas.Admin.Student;

using AutoMapper;

using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Service.Models;

public sealed class StudentProfiles : Profile
{
    public StudentProfiles()
    {
        this.CreateMap<StudentBase, AdminGroupSelectableStudent>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName.ToUpperInvariant()}"))
            .ForMember(dest => dest.IsSelected, opt => opt.Ignore());
    }
}
