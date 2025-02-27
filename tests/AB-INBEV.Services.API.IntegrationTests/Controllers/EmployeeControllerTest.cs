using AB_INBEV.Application.EventSourcedNormalizers;
using AB_INBEV.Application.Interfaces;
using AB_INBEV.Application.ViewModels;
using AB_INBEV.Domain.Core.Interfaces;
using AB_INBEV.Domain.Core.Notifications;
using AB_INBEV.Domain.Models;
using AB_INBEV.Services.Api.Controllers;
using IdentityModel.OidcClient;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AB_INBEV.Services.Api.IntegrationTests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeAppService> _employeeAppServiceMock;
        private readonly Mock<DomainNotificationHandler> _notificationHandlerMock;
        private readonly Mock<ILogger<EmployeeController>> _loggerMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly EmployeeController _employeeController;

        public EmployeeControllerTests()
        {
            _employeeAppServiceMock = new Mock<IEmployeeAppService>();
            _notificationHandlerMock = new Mock<DomainNotificationHandler>(); // Correct mock type
            _loggerMock = new Mock<ILogger<EmployeeController>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();

            _employeeController = new EmployeeController(
                _employeeAppServiceMock.Object,
                _notificationHandlerMock.Object, // Correct mock type
                _loggerMock.Object,
                _mediatorHandlerMock.Object
            );
        }

        [Fact]
        public void Get_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" },
                new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "Jane Doe" }
            };

            _employeeAppServiceMock.Setup(service => service.GetAll())
                .Returns(employees);

            // Act
            var result = _employeeController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var employeeList = Assert.IsType<List<EmployeeViewModel>>(dataValue);
            Assert.Equal(2, employeeList.Count);
        }

        [Fact]
        public void Get_WithId_ShouldReturnEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new EmployeeViewModel { Id = employeeId, FirstName = "John Doe" };

            _employeeAppServiceMock.Setup(service => service.GetById(employeeId))
                .Returns(employee);

            // Act
            var result = _employeeController.Get(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var returnedEmployee = Assert.IsType<EmployeeViewModel>(dataValue);
            Assert.Equal(employeeId, returnedEmployee.Id);
        }

        [Fact]
        public void Post_WithValidModel_ShouldRegisterEmployee()
        {
            // Arrange
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" };

            _employeeAppServiceMock.Setup(service => service.Register(employeeViewModel));

            // Act
            var result = _employeeController.Post(employeeViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var returnedEmployee = Assert.IsType<EmployeeViewModel>(dataValue);
            Assert.Equal(employeeViewModel.Id, returnedEmployee.Id);
        }

        [Fact]
        public void Post_WithInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "" };
            var notificationList = new List<DomainNotification>() { new DomainNotification("", "Name is required") };
            _employeeController.ModelState.AddModelError("Name", "Name is required");
            _notificationHandlerMock.Setup(s => s.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(s => s.GetNotifications()).Returns(notificationList);

            // Act
            var result = _employeeController.Post(employeeViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var dataProperty = badRequestResult.Value.GetType().GetProperty("errors");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(badRequestResult.Value);
            Assert.NotNull(dataValue);
            Assert.Single((System.Collections.IEnumerable)dataValue);
        }

        [Fact]
        public void Put_WithValidModel_ShouldUpdateEmployee()
        {
            // Arrange
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" };

            _employeeAppServiceMock.Setup(service => service.Update(employeeViewModel));

            // Act
            var result = _employeeController.Put(employeeViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var returnedEmployee = Assert.IsType<EmployeeViewModel>(dataValue);
            Assert.Equal(employeeViewModel.Id, returnedEmployee.Id);
        }

        [Fact]
        public void Put_WithInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            var notificationList = new List<DomainNotification>() { new DomainNotification("", "Name is required") };
            var employeeViewModel = new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "" };
            _employeeController.ModelState.AddModelError("Name", "Name is required");
            _notificationHandlerMock.Setup(s => s.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(s => s.GetNotifications()).Returns(notificationList);

            // Act
            var result = _employeeController.Put(employeeViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var dataProperty = badRequestResult.Value.GetType().GetProperty("errors");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(badRequestResult.Value);
            Assert.NotNull(dataValue);
            Assert.Single((System.Collections.IEnumerable)dataValue);
        }

        [Fact]
        public void Delete_ShouldRemoveEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _employeeAppServiceMock.Setup(service => service.Remove(employeeId));

            // Act
            var result = _employeeController.Delete(employeeId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public void History_ShouldReturnEmployeeHistory()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var historyData = new List<EmployeeHistoryData>
            {
                new EmployeeHistoryData { Action = "Registered", When = DateTime.Now.ToString() }
            };

            _employeeAppServiceMock.Setup(service => service.GetAllHistory(employeeId))
                .Returns(historyData);

            // Act
            var result = _employeeController.History(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var returnedHistory = Assert.IsType<List<EmployeeHistoryData>>(dataValue);
            Assert.Single(returnedHistory);
        }

        [Fact]
        public void Pagination_ShouldReturnPaginatedEmployees()
        {
            // Arrange
            var employees = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "John Doe" },
                new EmployeeViewModel { Id = Guid.NewGuid(), FirstName = "Jane Doe" }
            };

            _employeeAppServiceMock.Setup(service => service.GetAll(0, 2))
                .Returns(employees);

            // Act
            var result = _employeeController.Pagination(0, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var dataProperty = okResult.Value.GetType().GetProperty("data");
            Assert.NotNull(dataProperty);
            var dataValue = dataProperty.GetValue(okResult.Value);
            var returnedEmployees = Assert.IsType<List<EmployeeViewModel>>(dataValue);
            Assert.Equal(2, returnedEmployees.Count);
        }
    }
}