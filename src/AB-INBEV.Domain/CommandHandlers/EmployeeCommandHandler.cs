using AB_INBEV.Domain.Commands;
using AB_INBEV.Domain.Core.Interfaces;
using AB_INBEV.Domain.Core.Notifications;
using AB_INBEV.Domain.Events;
using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Domain.Models;
using MediatR;

namespace AB_INBEV.Domain.CommandHandlers
{
    public class EmployeeCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewEmployeeCommand, bool>,
        IRequestHandler<UpdateEmployeeCommand, bool>,
        IRequestHandler<RemoveEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMediatorHandler Bus;

        public EmployeeCommandHandler(IEmployeeRepository employeeRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _employeeRepository = employeeRepository;
            Bus = bus;
        }

        public async Task<bool> Handle(RegisterNewEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return await Task.FromResult(false);
            }

            var employee = new Employee(Guid.NewGuid(), message.FirstName, message.LastName, message.Email, message.Document, message.BirthDate, message.Phones);

            if (await _employeeRepository.GetByEmail(employee.Email) is not null)
            {
                await Bus.RaiseEvent(new DomainNotification(message.MessageType, "The employee e-mail has already been taken."));
                return await Task.FromResult(false);
            }

            await _employeeRepository.Add(employee);

            if (Commit())
            {
                await Bus.RaiseEvent(new EmployeeRegisteredEvent(employee.Id, employee.FirstName, employee.Email, employee.BirthDate));
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Handle(UpdateEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return await Task.FromResult(false);
            }

            var employee = new Employee(message.Id, message.FirstName, message.LastName, message.Email, message.Document, message.BirthDate, message.Phones);
            var existingCustomer = await _employeeRepository.GetByEmail(employee.Email);

            if (existingCustomer != null && existingCustomer.Id != employee.Id)
            {
                if (!existingCustomer.Equals(employee))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The employee e-mail has already been taken."));
                    return await Task.FromResult(false);
                }
            }

            _employeeRepository.Update(employee);

            if (Commit())
            {
                Bus.RaiseEvent(new EmployeeUpdatedEvent(employee.Id, employee.FirstName, employee.Email, employee.BirthDate));
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Handle(RemoveEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return await Task.FromResult(false);
            }

            await _employeeRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new EmployeeRemovedEvent(message.Id));
            }

            return await Task.FromResult(true);
        }

        public void Dispose()
        {
            _employeeRepository.Dispose();
        }
    }
}
