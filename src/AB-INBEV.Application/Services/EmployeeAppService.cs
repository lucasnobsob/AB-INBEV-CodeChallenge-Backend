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
                                  IEmployeeRepository employeeRepository,
                                  IMediatorHandler bus,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAll()
        {
            var employees = await _employeeRepository.GetAll();
            return _mapper.Map<List<EmployeeViewModel>>(employees);
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAll(int skip, int take)
        {
            var employees = await _employeeRepository.GetAll(new EmployeeFilterPaginatedSpecification(skip, take));
            return _mapper.Map<List<EmployeeViewModel>>(employees);
        }

        public async Task<EmployeeViewModel> GetById(Guid id)
        {
            var employee = await _employeeRepository.GetById(id);
            return _mapper.Map<EmployeeViewModel>(employee);
        }

        public async Task Register(EmployeeViewModel employeeViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewEmployeeCommand>(employeeViewModel);
            await Bus.SendCommand(registerCommand);
        }

        public async Task Update(EmployeeViewModel employeeViewModel)
        {
            var updateCommand = _mapper.Map<UpdateEmployeeCommand>(employeeViewModel);
            await Bus.SendCommand(updateCommand);
        }

        public async Task Remove(Guid id)
        {
            var removeCommand = new RemoveEmployeeCommand(id);
            await Bus.SendCommand(removeCommand);
        }

        public async Task<IList<EmployeeHistoryData>> GetAllHistory(Guid id)
        {
            var storedEvents = await _eventStoreRepository.All(id);
            return EmployeeHistory.ToJavaScriptCustomerHistory(storedEvents);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
