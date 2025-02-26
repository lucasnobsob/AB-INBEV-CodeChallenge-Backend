using AB_INBEV.Application.Interfaces;
using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Commands;
using AB_INBEV.Domain.Specifications;
using AB_INBEV.Infra.Data.Repository.EventSourcing;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AB_INBEV.Application.EventSourcedNormalizers;
using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Domain.Core.Interfaces;

namespace AB_INBEV.Application.Services
{
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public EmployeeAppService(IMapper mapper,
                                  IEmployeeRepository customerRepository,
                                  IMediatorHandler bus,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _employeeRepository = customerRepository;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _employeeRepository.GetAll().ProjectTo<EmployeeViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<EmployeeViewModel> GetAll(int skip, int take)
        {
            return _employeeRepository.GetAll(new EmployeeFilterPaginatedSpecification(skip, take))
                .ProjectTo<EmployeeViewModel>(_mapper.ConfigurationProvider);
        }

        public EmployeeViewModel GetById(Guid id)
        {
            return _mapper.Map<EmployeeViewModel>(_employeeRepository.GetById(id));
        }

        public void Register(EmployeeViewModel customerViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewEmployeeCommand>(customerViewModel);
            Bus.SendCommand(registerCommand);
        }

        public void Update(EmployeeViewModel customerViewModel)
        {
            var updateCommand = _mapper.Map<UpdateEmployeeCommand>(customerViewModel);
            Bus.SendCommand(updateCommand);
        }

        public void Remove(Guid id)
        {
            var removeCommand = new RemoveEmployeeCommand(id);
            Bus.SendCommand(removeCommand);
        }

        public IList<EmployeeHistoryData> GetAllHistory(Guid id)
        {
            return EmployeeHistory.ToJavaScriptCustomerHistory(_eventStoreRepository.All(id));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
