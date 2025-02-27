using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Models;
using AutoMapper;

namespace AB_INBEV.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones.Select(p => p.Number)));

            CreateMap<EmployeeViewModel, Employee>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones.Select(p => new Phone { Number = p })));
        }
    }
}
