using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Commands;
using AutoMapper;

namespace AB_INBEV.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<EmployeeViewModel, RegisterNewEmployeeCommand>()
                .ConstructUsing(c => new RegisterNewEmployeeCommand(c.FirstName, c.LastName, c.Email, c.Document, c.BirthDate, c.Phones));

            CreateMap<EmployeeViewModel, UpdateEmployeeCommand>()
                .ConstructUsing(c => new UpdateEmployeeCommand(c.Id, c.FirstName, c.LastName, c.Email, c.Document, c.BirthDate, c.Phones));
        }
    }
}
