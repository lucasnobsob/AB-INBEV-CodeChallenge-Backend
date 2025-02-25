using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Models;
using AutoMapper;

namespace AB_INBEV.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
