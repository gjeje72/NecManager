namespace NecManager.Web.Areas.Admin.Student;

using System;
using System.Collections.Generic;

using AutoMapper;

using NecManager.Common.DataEnum;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using NecManager.Web.Service.Models;

public sealed class StudentProfiles : Profile
{
    public StudentProfiles()
    {
        _ = this.CreateMap<StudentBase, CreateUserViewModel>();

        _ = this.CreateMap<StudentBase, AdminGroupSelectableStudent>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName.ToUpperInvariant()}"))
            .ForMember(dest => dest.IsSelected, opt => opt.Ignore());

        _ = this.CreateMap<GroupBase, GroupViewModel>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.CategoriesIds));

        _ = this.CreateMap<GroupViewModel, CreateGroupViewModel>();
    }
}
