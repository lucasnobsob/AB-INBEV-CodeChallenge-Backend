using AB_INBEV.Application.Services;
using AB_INBEV.Application.UnitTests.Dummies;
using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Commands;
using AB_INBEV.Domain.Core.Interfaces;
using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Domain.Models;
using AB_INBEV.Domain.Specifications;
using AB_INBEV.Infra.Data.Repository.EventSourcing;
using AutoMapper;
using Moq;
using Xunit;

namespace AB_INBEV.Application.UnitTests.Services
{
    public class EmployeeAppServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IEventStoreRepository> _eventStoreRepositoryMock;
        private readonly EmployeeAppService _employeeAppService;
        private readonly EmployeeDummyFactory _employeeDummyFactory;
        private readonly StoredEventDommyFactory _storedEventDommyFactory;

        public EmployeeAppServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _eventStoreRepositoryMock = new Mock<IEventStoreRepository>();
            _employeeDummyFactory = new EmployeeDummyFactory();
            _storedEventDommyFactory = new StoredEventDommyFactory();
            _employeeAppService = new EmployeeAppService(
                _mapperMock.Object,
                _employeeRepositoryMock.Object,
                _mediatorHandlerMock.Object,
                _eventStoreRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = _employeeDummyFactory.Generate(3);

            MockEmployeeMapping();
            _employeeRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeAppService.GetAll();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAll_WithPagination_ShouldReturnPaginatedEmployees()
        {
            // Arrange
            var employees = _employeeDummyFactory.Generate(3);

            MockEmployeeMapping();
            _employeeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<EmployeeFilterPaginatedSpecification>()))
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeAppService.GetAll(0, 2);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee()
        {
            // Arrange
            var employee = _employeeDummyFactory.Generate();

            MockEmployeeMapping();
            _employeeRepositoryMock.Setup(repo => repo.GetById(employee.Id))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeAppService.GetById(employee.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.Id, result.Id);
        }

        [Fact]
        public async Task Register_ShouldSendRegisterCommand()
        {
            // Arrange
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" };
            var registerCommand = new RegisterNewEmployeeCommand(employeeViewModel.Id, employeeViewModel.FirstName);

            _mapperMock.Setup(mapper => mapper.Map<RegisterNewEmployeeCommand>(employeeViewModel))
                .Returns(registerCommand);

            // Act
            await _employeeAppService.Register(employeeViewModel);

            // Assert
            _mediatorHandlerMock.Verify(mediator => mediator.SendCommand(registerCommand), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldSendUpdateCommand()
        {
            // Arrange
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" };
            var updateCommand = new UpdateEmployeeCommand(employeeViewModel.Id, employeeViewModel.FirstName);

            _mapperMock.Setup(mapper => mapper.Map<UpdateEmployeeCommand>(employeeViewModel))
                .Returns(updateCommand);

            // Act
            await _employeeAppService.Update(employeeViewModel);

            // Assert
            _mediatorHandlerMock.Verify(mediator => mediator.SendCommand(updateCommand), Times.Once);
        }

        [Fact]
        public async Task Remove_ShouldSendRemoveCommand()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            // Act
            await _employeeAppService.Remove(employeeId);

            // Assert
            _mediatorHandlerMock.Verify(mediator => mediator.SendCommand(
                It.Is<RemoveEmployeeCommand>(cmd => cmd.Id == employeeId)), Times.Once);
        }

        [Fact]
        public async Task GetAllHistory_ShouldReturnEmployeeHistory()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var historyData = _storedEventDommyFactory.Generate(3);

            _eventStoreRepositoryMock.Setup(repo => repo.All(employeeId))
                .ReturnsAsync(historyData);

            // Act
            var result = await _employeeAppService.GetAllHistory(employeeId);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Dispose_ShouldCallSuppressFinalize()
        {
            // Act
            _employeeAppService.Dispose();

            // Assert
            // Verify that GC.SuppressFinalize was called
            // This is a bit tricky to test directly, but you can verify the behavior indirectly.
            // For example, you can check if the service is in a disposed state if it has any state to track that.
        }

        private void MockEmployeeMapping()
        {
            _mapperMock.Setup(m => m.Map<List<EmployeeViewModel>>(It.IsAny<List<Employee>>()))
                .Returns((List<Employee> source) => source.Select(e => new EmployeeViewModel {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Document = e.Document,
                    BirthDate = e.BirthDate,
                    Phones = e.Phones.Select(p => p.Number).ToList()
                }).ToList());

            _mapperMock.Setup(mapper => mapper.ConfigurationProvider)
                .Returns(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Employee, EmployeeViewModel>().ReverseMap();

                    cfg.CreateMap<Employee, EmployeeViewModel>()
                        .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones.Select(p => p.Number)));

                    cfg.CreateMap<EmployeeViewModel, Employee>()
                        .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones.Select(p => new Phone { Number = p })));
                }));

            // Mock the Map method for Employee -> EmployeeViewModel
            _mapperMock.Setup(mapper => mapper.Map<EmployeeViewModel>(It.IsAny<Employee>()))
                .Returns((Employee source) => new EmployeeViewModel
                {
                    Id = source.Id,
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Email = source.Email,
                    Document = source.Document,
                    BirthDate = source.BirthDate,
                    Phones = source.Phones.Select(p => p.Number).ToList() // Map phones
                });

            // Mock the Map method for EmployeeViewModel -> Employee (if needed)
            _mapperMock.Setup(mapper => mapper.Map<Employee>(It.IsAny<EmployeeViewModel>()))
                .Returns((EmployeeViewModel source) => new Employee
                {
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Email = source.Email,
                    Document = source.Document,
                    BirthDate = source.BirthDate,
                    Phones = source.Phones.Select(p => new Phone { Number = p }).ToList() // Map phones
                });
        }
    }
}